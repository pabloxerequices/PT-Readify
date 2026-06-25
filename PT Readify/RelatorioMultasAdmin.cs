using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;
using Guna.UI2.WinForms;

namespace PT_Readify
{
    public class RelatorioMultasAdmin : Form
    {
        private static readonly Color CorTop = Color.FromArgb(33, 41, 52);
        private static readonly Color CorContent = Color.FromArgb(240, 242, 245);
        private static readonly Color CorVerde = Color.FromArgb(46, 204, 113);
        private static readonly Color CorAzul = Color.FromArgb(52, 152, 219);
        private static readonly Color CorVermelho = Color.FromArgb(231, 76, 60);

        private Guna2DataGridView gridMultas;
        private Label lblResumo;
        private DataTable dadosOriginais;

        private Guna2Button btnOrdenarData;
        private Guna2Button btnDataDesc;
        private Guna2Button btnDataAsc;
        private Guna2Button btnOrdenarUtilizador;
        private Guna2Button btnUtilizadorDesc;
        private Guna2Button btnUtilizadorAsc;

        public RelatorioMultasAdmin()
        {
            Text = "Relatório de Multas";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1100, 600);
            BackColor = CorContent;

            var panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = CorTop
            };

            var lblTitulo = new Label
            {
                Text = "€ Relatório de Multas",
                Font = new Font("Segoe UI", 22F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 15),
                AutoSize = true
            };

            var lblInfo = new Label
            {
                Text = "Multa de 2,00 € por cada semana de atraso após a data prevista de devolução.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(200, 210, 220),
                Location = new Point(18, 48),
                AutoSize = true
            };

            panelTop.Controls.Add(lblTitulo);
            panelTop.Controls.Add(lblInfo);

            var panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CorContent,
                Padding = new Padding(15)
            };

            gridMultas = CriarGrid();
            gridMultas.Dock = DockStyle.Fill;
            gridMultas.CellFormatting += GridMultas_CellFormatting;
            gridMultas.DataError += (s, e) => e.ThrowException = false;
            panelContent.Controls.Add(gridMultas);

            var panelBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 90,
                BackColor = CorTop
            };

            lblResumo = new Label
            {
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 10),
                AutoSize = true
            };

            btnOrdenarData = CriarBotao("Ordenar por Data", 15, 40, CorVerde);
            btnDataDesc = CriarBotao("Data ▼", 230, 40, CorVerde);
            btnDataAsc = CriarBotao("Data ▲", 350, 40, CorVerde);
            btnOrdenarUtilizador = CriarBotao("Ordenar por ID Utilizador", 480, 40, CorAzul);
            btnUtilizadorDesc = CriarBotao("ID ▼", 720, 40, CorAzul);
            btnUtilizadorAsc = CriarBotao("ID ▲", 820, 40, CorAzul);

            btnDataDesc.Visible = false;
            btnDataAsc.Visible = false;
            btnUtilizadorDesc.Visible = false;
            btnUtilizadorAsc.Visible = false;

            btnOrdenarData.Click += (s, e) => MostrarBotoesOrdenacao("data");
            btnOrdenarUtilizador.Click += (s, e) => MostrarBotoesOrdenacao("utilizador");
            btnDataDesc.Click += (s, e) => AplicarOrdenacao("Data_Prevista DESC", "data");
            btnDataAsc.Click += (s, e) => AplicarOrdenacao("Data_Prevista ASC", "data");
            btnUtilizadorDesc.Click += (s, e) => AplicarOrdenacao("Id_Utilizador DESC", "utilizador");
            btnUtilizadorAsc.Click += (s, e) => AplicarOrdenacao("Id_Utilizador ASC", "utilizador");

            var btnMarcarPaga = CriarBotao("Marcar como paga", 930, 40, CorVermelho);
            btnMarcarPaga.Size = new Size(150, 40);
            btnMarcarPaga.Click += BtnMarcarPaga_Click;

            var btnAtualizar = CriarBotao("Atualizar", 930, 10, CorVerde);
            btnAtualizar.Size = new Size(150, 28);
            btnAtualizar.Click += (s, e) => CarregarMultas();

            panelBottom.Controls.Add(lblResumo);
            panelBottom.Controls.Add(btnOrdenarData);
            panelBottom.Controls.Add(btnDataDesc);
            panelBottom.Controls.Add(btnDataAsc);
            panelBottom.Controls.Add(btnOrdenarUtilizador);
            panelBottom.Controls.Add(btnUtilizadorDesc);
            panelBottom.Controls.Add(btnUtilizadorAsc);
            panelBottom.Controls.Add(btnMarcarPaga);
            panelBottom.Controls.Add(btnAtualizar);

            Controls.Add(panelContent);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);

            Load += (s, e) => CarregarMultas();
        }

        private static Guna2DataGridView CriarGrid()
        {
            var grid = new Guna2DataGridView
            {
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 35
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 88, 255);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
            return grid;
        }

        private static Guna2Button CriarBotao(string texto, int x, int y, Color cor)
        {
            return new Guna2Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(200, 40),
                BorderRadius = 6,
                FillColor = cor,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White
            };
        }

        private void MostrarBotoesOrdenacao(string modo)
        {
            btnOrdenarData.Visible = modo != "data";
            btnOrdenarUtilizador.Visible = modo != "utilizador";
            btnDataDesc.Visible = modo == "data";
            btnDataAsc.Visible = modo == "data";
            btnUtilizadorDesc.Visible = modo == "utilizador";
            btnUtilizadorAsc.Visible = modo == "utilizador";
        }

        private void EsconderBotoesOrdenacao()
        {
            btnOrdenarData.Visible = true;
            btnOrdenarUtilizador.Visible = true;
            btnDataDesc.Visible = false;
            btnDataAsc.Visible = false;
            btnUtilizadorDesc.Visible = false;
            btnUtilizadorAsc.Visible = false;
        }

        private void AplicarOrdenacao(string sortExpression, string modo)
        {
            if (dadosOriginais == null || dadosOriginais.Columns.Count == 0)
                return;

            string coluna = sortExpression.Split(' ')[0];
            if (!dadosOriginais.Columns.Contains(coluna))
            {
                MessageBox.Show("Não foi possível ordenar: coluna não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EsconderBotoesOrdenacao();
                return;
            }

            DataView view = dadosOriginais.DefaultView;
            view.Sort = sortExpression;
            AtualizarGrid();
            EsconderBotoesOrdenacao();
        }

        private void AtualizarGrid()
        {
            if (dadosOriginais == null)
                return;

            gridMultas.DataSource = GridDisplayHelper.FormatMultasParaExibicao(dadosOriginais.DefaultView.ToTable());
        }

        private void GridMultas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridMultas.Columns[e.ColumnIndex].Name == "Estado Multa" && e.Value != null)
            {
                e.CellStyle.ForeColor = e.Value.ToString() == "Paga"
                    ? Color.FromArgb(46, 204, 113)
                    : Color.FromArgb(231, 76, 60);
            }
        }

        private void CarregarMultas()
        {
            try
            {
                dadosOriginais = BLL.Historicos.LoadRelatorioMultas();
                AtualizarGrid();

                int totalPendente = 0;
                int countPendente = 0;
                foreach (DataRow row in dadosOriginais.Rows)
                {
                    if (!Convert.ToBoolean(row["Multa_Paga"]))
                    {
                        totalPendente += Convert.ToInt32(row["Valor_Multa"]);
                        countPendente++;
                    }
                }

                lblResumo.Text = $"{dadosOriginais.Rows.Count} multa(s) | {countPendente} pendente(s) | Total pendente: {(totalPendente / 100m):C2}";
                EsconderBotoesOrdenacao();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar multas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMarcarPaga_Click(object sender, EventArgs e)
        {
            if (gridMultas.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma multa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int id = Convert.ToInt32(gridMultas.CurrentRow.Cells["Id"].Value);
                BLL.Historicos.MarcarMultaComoPaga(id);
                CarregarMultas();
                MessageBox.Show("Multa marcada como paga.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
