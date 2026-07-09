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
                Name = "colEditora",
                HeaderText = "Editora",
                DataPropertyName = "Editora",
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
        MessageBox.Show($"Saldo insuficiente na carteira. Total: {totalCompra:C2}", "Saldo insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    try
    {
        this.Cursor = Cursors.WaitCursor;

        // Gerar as linhas da tabela dinamicamente
        string itensHtml = "";
        foreach (DataRow row in carrinhoTable.Rows)
        {
            itensHtml += $@"
            <tr style=""border-bottom: 1px solid #eee;"">
                <td style=""padding: 10px;"">{row["Titulo"]}</td>
                <td style=""padding: 10px;"">{row["Autor"]}</td>
                <td style=""padding: 10px;"">{row["Editora"]}</td>
                <td style=""padding: 10px; text-align: center;"">{row["Quantidade"]}</td>
                <td style=""padding: 10px; text-align: right;"">{Convert.ToDecimal(row["Preco"]):C2}</td>
            </tr>";
        }

        var resultadoRecibo = CarrinhoService.ProcessarCarrinho();
        this.Cursor = Cursors.Default;

        if (resultadoRecibo.Sucesso)
        {
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("martimr480@gmail.com", "djniszgkjuxnludr");

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("martimr480@gmail.com", "Livraria Readify");
                    mail.To.Add(BLL.Clientes.ObterEmailUtilizadorConectado(globais.id_utilizador));
                    mail.Subject = "Recibo de Compra - Livraria Readify";
                    mail.IsBodyHtml = true;

                    DateTime dataHoraCompra = DateTime.Now;

                    string corpoEmail = $@"
                    <!DOCTYPE html>
                    <html lang=""pt-pt"">
                    <head>
                        <meta charset=""UTF-8"">
                        <style>
                            body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f4f7f6; margin: 0; padding: 40px 20px; }}
                            .container {{ max-width: 500px; margin: 0 auto; background-color: #ffffff; padding: 40px; border-radius: 12px; box-shadow: 0 10px 25px rgba(0,0,0,0.05); border-top: 6px solid #2ecc71; }}
                            .logo {{ text-align: center; margin-bottom: 20px; }}
                            .logo img {{ width: 120px; height: auto; }}
                            .header h2 {{ color: #2c3e50; margin-top: 0; font-size: 24px; text-align: center; }}
                            .purchase-id {{ text-align: center; background: #e8f8f5; padding: 12px; border-radius: 6px; color: #27ae60; font-size: 16px; font-weight: bold; margin-bottom: 20px; }}
                            table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
                            th {{ background-color: #f8f9fa; color: #2c3e50; padding: 10px; text-align: left; border-bottom: 2px solid #eee; }}
                            .total {{ font-weight: bold; font-size: 18px; color: #2c3e50; text-align: right; padding-top: 10px; border-top: 2px solid #2ecc71; }}
                            .timestamp {{ text-align: center; background: #f8f9fa; padding: 10px; border-radius: 6px; color: #7f8c8d; font-size: 14px; margin-bottom: 25px; }}
                            .warning {{ margin-top: 30px; font-size: 13px; color: #95a5a6; text-align: center; border-top: 1px solid #eee; padding-top: 20px; }}
                        </style>
                    </head>
                    <body>
                        <div class=""container"">
                            <div class=""logo""><img src=""https://i.ibb.co/WWgWxxtx/image.png"" alt=""Logo Readify""></div>
                            <div class=""header""><h2>Pedido Confirmado na Readify!</h2></div>
                            <div class=""purchase-id"">ID da Compra: #{resultadoRecibo.IdCompra}</div>
                            <div class=""timestamp""><strong>Data da compra:</strong> {dataHoraCompra:dd/MM/yyyy HH:mm:ss}</div>
                            
                            <table>
                                <thead><tr><th>Livro</th><th>Autor</th><th>Editora</th><th>Qtd</th><th>Preço</th></tr></thead>
                                <tbody>{itensHtml}</tbody>
                            </table>
                            
                            <div class=""total"">Total Pago: {totalCompra:C2}</div>
                            
                            <div class=""warning"">
                                Obrigado por comprar na nossa Livraria Readify.<br>
                                Dispõe de 30 dias úteis para trocas ou devoluções.<br><br>
                                <strong>ID da Compra: #{resultadoRecibo.IdCompra}</strong>
                            </div>
                        </div>
                    </body>
                    </html>";

                    mail.Body = corpoEmail;
                    smtp.Send(mail);
                }
            }

            MessageBox.Show("Compra efetuada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AtualizarTotalGeral();
        }
        else
        {
            MessageBox.Show("Erro ao enviar recibo: " + resultadoRecibo.Mensagem, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
