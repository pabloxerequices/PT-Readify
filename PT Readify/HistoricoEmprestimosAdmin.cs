using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;
using Guna.UI2.WinForms;

namespace PT_Readify
{
    public class HistoricoEmprestimosAdmin : Form
    {
        private static readonly Color CorTop = Color.FromArgb(33, 41, 52);
        private static readonly Color CorContent = Color.FromArgb(240, 242, 245);
        private static readonly Color CorVerde = Color.FromArgb(46, 204, 113);
        private static readonly Color CorAzul = Color.FromArgb(52, 152, 219);

        private Guna2DataGridView gridHistorico;
        private Label lblResumo;
        private DataTable dadosOriginais;

        private Guna2Button btnOrdenarData;
        private Guna2Button btnDataDesc;
        private Guna2Button btnDataAsc;
        private Guna2Button btnOrdenarUtilizador;
        private Guna2Button btnUtilizadorDesc;
        private Guna2Button btnUtilizadorAsc;

        public HistoricoEmprestimosAdmin()
        {
            Text = "Histórico de Empréstimos (Admin)";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1150, 600);
            BackColor = CorContent;

            var panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = CorTop
            };

            var lblTitulo = new Label
            {
                Text = "⟳ Histórico de Empréstimos — Todos os utilizadores",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 15),
                AutoSize = true
            };
            panelTop.Controls.Add(lblTitulo);

            var panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CorContent,
                Padding = new Padding(15)
            };

            gridHistorico = CriarGrid();
            gridHistorico.Dock = DockStyle.Fill;
            gridHistorico.CellFormatting += GridHistorico_CellFormatting;
            gridHistorico.DataError += (s, e) => e.ThrowException = false;
            panelContent.Controls.Add(gridHistorico);

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
            btnDataDesc.Click += (s, e) => AplicarOrdenacao("Data_Levantamento DESC", "data");
            btnDataAsc.Click += (s, e) => AplicarOrdenacao("Data_Levantamento ASC", "data");
            btnUtilizadorDesc.Click += (s, e) => AplicarOrdenacao("Id_Utilizador DESC", "utilizador");
            btnUtilizadorAsc.Click += (s, e) => AplicarOrdenacao("Id_Utilizador ASC", "utilizador");

            var btnAtualizar = CriarBotao("Atualizar", 980, 40, CorVerde);
            btnAtualizar.Size = new Size(140, 40);
            btnAtualizar.Click += (s, e) => CarregarHistorico();

            panelBottom.Controls.Add(lblResumo);
            panelBottom.Controls.Add(btnOrdenarData);
            panelBottom.Controls.Add(btnDataDesc);
            panelBottom.Controls.Add(btnDataAsc);
            panelBottom.Controls.Add(btnOrdenarUtilizador);
            panelBottom.Controls.Add(btnUtilizadorDesc);
            panelBottom.Controls.Add(btnUtilizadorAsc);
            panelBottom.Controls.Add(btnAtualizar);

            Controls.Add(panelContent);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);

            Load += (s, e) => CarregarHistorico();
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

            gridHistorico.DataSource = GridDisplayHelper.FormatEmprestimosAdminParaExibicao(dadosOriginais.DefaultView.ToTable());
        }

        private void GridHistorico_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridHistorico.Columns[e.ColumnIndex].Name == "Estado Multa" && e.Value != null && e.Value.ToString() != "—")
            {
                e.CellStyle.ForeColor = e.Value.ToString() == "Paga"
                    ? Color.FromArgb(46, 204, 113)
                    : Color.FromArgb(231, 76, 60);
            }
        }

        private void CarregarHistorico()
        {
            try
            {
                dadosOriginais = BLL.Historicos.LoadHistoricoEmpTodos();
                AtualizarGrid();

                int ativos = 0;
                foreach (DataRow row in dadosOriginais.Rows)
                {
                    if (row["Estado_Emprestimo"]?.ToString() == "Ativo")
                        ativos++;
                }

                lblResumo.Text = $"{dadosOriginais.Rows.Count} registo(s) | {ativos} empréstimo(s) ativo(s)";
                EsconderBotoesOrdenacao();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar histórico: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
