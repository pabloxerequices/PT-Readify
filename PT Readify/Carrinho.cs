using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Threading.Tasks;
using BusinessLogicLayer;
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

                // Regista a compra, desconta saldo/stock e envia o recibo para o email do utilizador.
                var resultadoRecibo = CarrinhoService.ProcessarCarrinho();
                this.Cursor = Cursors.Default;

                if (resultadoRecibo.Sucesso)
                {

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential("martimr480@gmail.com", "djni szgk juxn ludr");

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("martimr480@gmail.com", "Livraria");
                            mail.To.Add(BLL.Clientes.ObterEmailUtilizadorConectado(globais.id_utilizador)); // Destinatário do e-mail (e-mail do utilizador)
                            mail.Subject = "Recibo de Compra";

                            // Captura a data e hora exatas do momento da compra
                            DateTime dataHoraCompra = DateTime.Now;

                            // Monta o corpo do e-mail com o recibo original, data/hora e aviso de devolução
                            string corpoEmail = $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Recibo de Compra</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        .header {{
            border-bottom: 3px solid #007bff;
            padding-bottom: 15px;
            margin-bottom: 20px;
        }}
        .message {{
            color: #333;
            font-size: 16px;
            line-height: 1.6;
            margin-bottom: 20px;
        }}
        .info-section {{
            background-color: #f9f9f9;
            padding: 15px;
            border-left: 4px solid #007bff;
            border-radius: 4px;
            margin-bottom: 20px;
        }}
        .info-item {{
            margin-bottom: 10px;
        }}
        .label {{
            font-weight: 600;
            color: #007bff;
            display: inline-block;
            min-width: 200px;
        }}
        .value {{
            color: #555;
        }}
        .warning {{
            background-color: #fff3cd;
            border-left: 4px solid #ffc107;
            padding: 15px;
            border-radius: 4px;
            margin-top: 20px;
        }}
        .warning strong {{
            color: #856404;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h2 style=""color: #007bff; margin: 0;"">Seu Pedido foi Confirmado! ✓</h2>
        </div>
        
        <div class=""message"">
            $""{resultadoRecibo.Mensagem}
        </div>
        
        <div class=""info-section"">
            <div class=""info-item"">
                <span class=""label"">Data e Hora da Compra:</span>
                <span class=""value"">{{dataHoraCompra:dd/MM/yyyy HH:mm:ss}}</span>
            </div>
        </div>
        
        <div class=""warning"">
            <strong>ℹ️ Informação Importante:</strong><br>
            Dispõe de um prazo de 30 dias úteis para efetuar qualquer devolução.
        </div>
    </div>
</body>
</html> ";

                            mail.Body = corpoEmail;
                            mail.IsBodyHtml = true;

                            // Envia o e-mail (apenas uma vez para evitar duplicados)
                            smtp.Send(mail);
                        }
                    }

                    MessageBox.Show(
                        "Compra efetuada com sucesso!\n\n" + resultadoRecibo.Mensagem,
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    AtualizarTotalGeral();
                }
                else
                {
                    MessageBox.Show(
                        "A compra foi registada, mas ocorreu um problema a enviar o recibo por e-mail:\n\n" + resultadoRecibo.Mensagem,
                        "Aviso - E-mail",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    AtualizarTotalGeral();
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
