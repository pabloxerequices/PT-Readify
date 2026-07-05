using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net; // Necessário para credenciais de rede
using System.Net.Mail; // Necessário para enviar o e-mail
using DataAccessLayer;
    
namespace PT_Readify
{
    public partial class Carrinho : Form
    {
        private DataTable carrinhoTable;

        public Carrinho()
        {
            InitializeComponent();
            carrinhoTable = CarrinhoService.Itens;
            ConfigurarDataGrid();
            ConfigurarBotoes();
            AtualizarTotalGeral();
        }

        private void ConfigurarBotoes()
        {
            btnReservar.Visible = false;
            btnEmprestar.Text = "Empréstimos";
            btnEmprestar.FillColor = Color.FromArgb(155, 89, 182);
            btnComprar.Visible = false;

            var lblInfo = new Label
            {
                Text = "O carrinho é apenas para compras. Para emprestar ou reservar, use Empréstimos.",
                ForeColor = Color.FromArgb(200, 200, 200),
                AutoSize = false,
                Size = new Size(340, 40),
                Location = new Point(140, 20),
                Font = new Font("Segoe UI", 8.5f)
            };
            panelBottom.Controls.Add(lblInfo);
        }

        private void ConfigurarDataGrid()
        {
            dataGridViewCarrinho.AutoGenerateColumns = false;
            dataGridViewCarrinho.DataSource = null;
            dataGridViewCarrinho.Columns.Clear();

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                DataPropertyName = "Id_Livro",
                Width = 50,
                ReadOnly = true
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTitulo",
                HeaderText = "Título",
                DataPropertyName = "Titulo",
                Width = 220,
                ReadOnly = true
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAutor",
                HeaderText = "Autor",
                DataPropertyName = "Autor",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPreco",
                HeaderText = "Preço",
                DataPropertyName = "Preco",
                Width = 70,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colQuantidade",
                HeaderText = "Qtd",
                DataPropertyName = "Quantidade",
                Width = 50,
                ReadOnly = false
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSubtotal",
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
                Width = 80,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colRemover",
                HeaderText = "",
                Text = "Remover",
                UseColumnTextForButtonValue = true,
                Width = 80
            });

            dataGridViewCarrinho.ReadOnly = false;
            dataGridViewCarrinho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCarrinho.MultiSelect = false;
            dataGridViewCarrinho.AllowUserToAddRows = false;
            dataGridViewCarrinho.AllowUserToDeleteRows = false;
            dataGridViewCarrinho.CellClick += DataGridViewCarrinho_CellClick;
            dataGridViewCarrinho.CellEndEdit += DataGridViewCarrinho_CellEndEdit;
            dataGridViewCarrinho.DataError += DataGridViewCarrinho_DataError;

            dataGridViewCarrinho.DataSource = carrinhoTable;
        }

        private void DataGridViewCarrinho_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void AtualizarTotalGeral()
        {
            labelTotal.Text = $"Total: {CarrinhoService.TotalPreco:C2} ({CarrinhoService.TotalItens} itens)";
        }

        private void DataGridViewCarrinho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dataGridViewCarrinho.Columns["colRemover"].Index)
            {
                DialogResult resultado = MessageBox.Show(
                    "Deseja remover este livro do carrinho?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    CarrinhoService.RemoverLinha(e.RowIndex);
                    AtualizarTotalGeral();
                }
            }
        }

        private void DataGridViewCarrinho_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dataGridViewCarrinho.Columns["colQuantidade"].Index)
            {
                try
                {
                    int novaQtd = Convert.ToInt32(dataGridViewCarrinho[e.ColumnIndex, e.RowIndex].Value);

                    if (novaQtd <= 0)
                    {
                        MessageBox.Show("A quantidade deve ser maior que 0.");
                        dataGridViewCarrinho[e.ColumnIndex, e.RowIndex].Value = 1;
                        return;
                    }

                    CarrinhoService.AtualizarQuantidade(e.RowIndex, novaQtd);
                    AtualizarTotalGeral();
                    dataGridViewCarrinho.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dataGridViewCarrinho.Refresh();
                    AtualizarTotalGeral();
                }
            }
        }

        // ==========================================
        // MÉTODO PARA IR BUSCAR O E-MAIL À BD
        // ==========================================
        private string ObterEmailDoUtilizadorDaBD(int idUtilizador)
        {
            if (idUtilizador <= 0)
                return string.Empty;

            // Ajusta a Connection String para a tua
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True";

            string query = "SELECT email FROM utilizador WHERE id_utilizador = @id";
            string email = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idUtilizador);
                        conn.Open();
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            email = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aceder à base de dados para obter o e-mail: " + ex.Message);
            }

            return email;
        }

        // ==========================================
        // MÉTODO NOVO PARA ENVIAR O E-MAIL DIRETAMENTE
        // ==========================================
        private bool EnviarEmailRecibo(string destinatario, string assunto, string corpo)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("martimr480@gmail.com");
                mail.To.Add(destinatario);
                mail.Subject = assunto;
                mail.Body = corpo;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    // ATENÇÃO: Substitui "TUA_SENHA_DE_APP_AQUI" pela palavra-passe de app de 16 dígitos gerada na tua conta Google
                    smtp.Credentials = new NetworkCredential("martimr480@gmail.com", "TUA_SENHA_DE_APP_AQUI");

                    smtp.Send(mail);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar o e-mail de recibo: " + ex.Message, "Falha de E-mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // ==========================================
        // LÓGICA DE CONFIRMAÇÃO
        // ==========================================
        private void ConfirmarPedido()
        {
            if (carrinhoTable.Rows.Count == 0)
            {
                MessageBox.Show("O carrinho está vazio!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para finalizar a compra.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalCompra = CarrinhoService.TotalPreco;
            if (!CarteiraService.TemSaldoSuficiente(totalCompra))
            {
                MessageBox.Show(
                    $"Saldo insuficiente na carteira. Total do carrinho: {totalCompra:C2}\n\nAdicione saldo na Carteira.",
                    "Saldo insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;

                // 1. Obter E-mail
                string emailUsuario = ObterEmailDoUtilizadorDaBD(globais.id_utilizador);

                if (string.IsNullOrEmpty(emailUsuario))
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Não foi possível encontrar o e-mail do utilizador na Base de Dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Registar a compra na BD através do serviço (descontar saldo, atualizar stock, etc)
                var resultadoRecibo = CarrinhoService.ProcessarCarrinho();

                if (resultadoRecibo.Sucesso)
                {
                    // 3. Gerar os dados para o e-mail
                    string dataHoraAtual = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                    List<string> nomesLivros = new List<string>();
                    foreach (DataRow linha in carrinhoTable.Rows)
                    {
                        nomesLivros.Add($"{linha["Quantidade"]}x {linha["Titulo"]}");
                    }
                    string livrosComprados = string.Join(", ", nomesLivros);

                    string assuntoDinamico = $"Confirmação de Compra: {livrosComprados} - {dataHoraAtual}";

                    string corpoDinamico = $"Olá!\n\nA tua compra foi processada com sucesso.\n\n" +
                                           $"Livros adquiridos:\n{livrosComprados}\n\n" +
                                           $"Total pago: {totalCompra:C2}\n" +
                                           $"Data da transação: {dataHoraAtual}\n\n" +
                                           $"Obrigado por utilizares o PT_Readify!";

                    // 4. Disparar o e-mail
                    bool emailEnviado = EnviarEmailRecibo(emailUsuario, assuntoDinamico, corpoDinamico);

                    this.Cursor = Cursors.Default;

                    if (emailEnviado)
                    {
                        MessageBox.Show(
                            "Compra efetuada e recibo enviado com sucesso para o teu e-mail!",
                            "Sucesso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            "A compra foi registada, mas ocorreu um problema a enviar o e-mail.",
                            "Aviso - E-mail",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                    AtualizarTotalGeral();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(
                        "Falha ao registar a compra na base de dados:\n\n" + resultadoRecibo.Mensagem,
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("Erro ao finalizar compra: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void IrParaEmprestimos()
        {
            var menu = Owner as main_menu ?? FindForm() as main_menu;
            Close();
            if (menu != null)
                menu.AbrirEmprestimos();
            else
                new Requesitar_livros().Show();
        }

        private void btnFinalizarCompra_Click(object sender, EventArgs e) => ConfirmarPedido();

        private void btnEmprestar_Click(object sender, EventArgs e) => IrParaEmprestimos();

        private void btnComprar_Click(object sender, EventArgs e) => ConfirmarPedido();

        private void btnReservar_Click(object sender, EventArgs e) => IrParaEmprestimos();

        private void btnLimparCarrinho_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "Deseja limpar todo o carrinho?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                CarrinhoService.Limpar();
                AtualizarTotalGeral();
            }
        }

        private void Carrinho_Load(object sender, EventArgs e)
        {
            ConfigurarDataGrid();
            AtualizarTotalGeral();
        }

        private void dataGridViewCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}