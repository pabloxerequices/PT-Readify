using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

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
                    $"Saldo insuficiente na carteira. Total do carrinho: {totalCompra:C2}\n\nAdicione saldo na Carteira Digital para concluir a compra.",
                    "Saldo insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultado = MessageBox.Show(
                $"Confirmar compra de {CarrinhoService.TotalItens} item(ns) por {totalCompra:C2}?\n\nSerá debitado {totalCompra:C2} da sua carteira (saldo atual: {CarteiraService.Saldo:C2}).",
                "Confirmar compra",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado != DialogResult.Yes)
                return;

            try
            {
                ResultadoEnvioRecibo resultadoRecibo = CarrinhoService.ProcessarCarrinho();
                AtualizarTotalGeral();

                MessageBox.Show(
                    "Compra finalizada com sucesso!\n\nConsulte o Histórico de Compras.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                if (resultadoRecibo.Sucesso)
                {
                    MessageBox.Show(
                        resultadoRecibo.Mensagem,
                        "Recibo enviado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "A compra foi registada, mas o recibo por email não foi enviado.\n\n" +
                        resultadoRecibo.Mensagem + "\n\n" +
                        "Configure o ficheiro smtp.config na pasta da aplicação (veja smtp.config.example).",
                        "Aviso - Email",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
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
