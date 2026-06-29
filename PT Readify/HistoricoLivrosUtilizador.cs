using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;
using Guna.UI2.WinForms;

namespace PT_Readify
{
    public class HistoricoLivrosUtilizador : Form
    {
        private static readonly Color CorTop = Color.FromArgb(33, 41, 52);
        private static readonly Color CorContent = Color.FromArgb(240, 242, 245);
        private static readonly Color CorVerde = Color.FromArgb(46, 204, 113);
        private static readonly Color CorAzul = Color.FromArgb(52, 152, 219);

        private TabControl tabControl;
        private Guna2DataGridView gridCompras;
        private Guna2DataGridView gridEmprestimos;
        private Label lblResumo;
        private DataTable dadosCompras;
        private DataTable dadosEmprestimos;

        private Guna2Button btnOrdenarData;
        private Guna2Button btnDataDesc;
        private Guna2Button btnDataAsc;
        private Guna2Button btnOrdenarLivro;
        private Guna2Button btnLivroDesc;
        private Guna2Button btnLivroAsc;

        public HistoricoLivrosUtilizador()
        {
            Text = "Histórico de Livros";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1100, 600);
            BackColor = CorContent;

            var panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 65,
                BackColor = CorTop
            };

            var lblTitulo = new Label
            {
                Text = "📚 Histórico de Livros — Compras e Empréstimos",
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

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };

            var tabCompras = new TabPage("Compras");
            gridCompras = CriarGrid();
            gridCompras.Dock = DockStyle.Fill;
            gridCompras.DataError += (s, e) => e.ThrowException = false;
            tabCompras.Controls.Add(gridCompras);

            var tabEmprestimos = new TabPage("Empréstimos");
            gridEmprestimos = CriarGrid();
            gridEmprestimos.Dock = DockStyle.Fill;
            gridEmprestimos.CellFormatting += GridEmprestimos_CellFormatting;
            gridEmprestimos.DataError += (s, e) => e.ThrowException = false;
            tabEmprestimos.Controls.Add(gridEmprestimos);

            tabControl.TabPages.Add(tabCompras);
            tabControl.TabPages.Add(tabEmprestimos);
            tabControl.SelectedIndexChanged += (s, e) => AtualizarResumo();

            panelContent.Controls.Add(tabControl);

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
            btnOrdenarLivro = CriarBotao("Ordenar por ID Livro", 480, 40, CorAzul);
            btnLivroDesc = CriarBotao("ID ▼", 700, 40, CorAzul);
            btnLivroAsc = CriarBotao("ID ▲", 800, 40, CorAzul);

            btnDataDesc.Visible = false;
            btnDataAsc.Visible = false;
            btnLivroDesc.Visible = false;
            btnLivroAsc.Visible = false;

            btnOrdenarData.Click += (s, e) => MostrarBotoesOrdenacao("data");
            btnOrdenarLivro.Click += (s, e) => MostrarBotoesOrdenacao("livro");
            btnDataDesc.Click += (s, e) => AplicarOrdenacaoData(true);
            btnDataAsc.Click += (s, e) => AplicarOrdenacaoData(false);
            btnLivroDesc.Click += (s, e) => AplicarOrdenacaoLivro(true);
            btnLivroAsc.Click += (s, e) => AplicarOrdenacaoLivro(false);

            var btnAtualizar = CriarBotao("Atualizar", 980, 40, CorVerde);
            btnAtualizar.Size = new Size(100, 40);
            btnAtualizar.Click += (s, e) => CarregarDados();

            panelBottom.Controls.Add(lblResumo);
            panelBottom.Controls.Add(btnOrdenarData);
            panelBottom.Controls.Add(btnDataDesc);
            panelBottom.Controls.Add(btnDataAsc);
            panelBottom.Controls.Add(btnOrdenarLivro);
            panelBottom.Controls.Add(btnLivroDesc);
            panelBottom.Controls.Add(btnLivroAsc);
            panelBottom.Controls.Add(btnAtualizar);

            Controls.Add(panelContent);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);

            Load += (s, e) => CarregarDados();
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
            btnOrdenarLivro.Visible = modo != "livro";
            btnDataDesc.Visible = modo == "data";
            btnDataAsc.Visible = modo == "data";
            btnLivroDesc.Visible = modo == "livro";
            btnLivroAsc.Visible = modo == "livro";
        }

        private void EsconderBotoesOrdenacao()
        {
            btnOrdenarData.Visible = true;
            btnOrdenarLivro.Visible = true;
            btnDataDesc.Visible = false;
            btnDataAsc.Visible = false;
            btnLivroDesc.Visible = false;
            btnLivroAsc.Visible = false;
        }

        private bool TabComprasAtiva => tabControl.SelectedIndex == 0;

        private void AplicarOrdenacaoData(bool descendente)
        {
            string colunaData = TabComprasAtiva ? "Data_Compra" : "Data_Levantamento";
            string direcao = descendente ? "DESC" : "ASC";
            AplicarOrdenacao(colunaData, direcao);
        }

        private void AplicarOrdenacaoLivro(bool descendente)
        {
            string direcao = descendente ? "DESC" : "ASC";
            AplicarOrdenacao("Id_Livro", direcao);
        }

        private void AplicarOrdenacao(string coluna, string direcao)
        {
            DataTable dados = TabComprasAtiva ? dadosCompras : dadosEmprestimos;

            if (dados == null || !dados.Columns.Contains(coluna))
            {
                MessageBox.Show("Não foi possível ordenar: dados inválidos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EsconderBotoesOrdenacao();
                return;
            }

            DataView view = dados.DefaultView;
            view.Sort = $"{coluna} {direcao}";

            if (TabComprasAtiva)
                AtualizarGridCompras();
            else
                AtualizarGridEmprestimos();

            EsconderBotoesOrdenacao();
        }

        private void AtualizarGridCompras()
        {
            if (dadosCompras == null)
                return;

            gridCompras.DataSource = GridDisplayHelper.FormatComprasParaExibicao(dadosCompras.DefaultView.ToTable());
        }

        private void AtualizarGridEmprestimos()
        {
            if (dadosEmprestimos == null)
                return;

            gridEmprestimos.DataSource = GridDisplayHelper.FormatEmprestimosParaExibicao(dadosEmprestimos.DefaultView.ToTable());
        }

        private void GridEmprestimos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridEmprestimos.Columns[e.ColumnIndex].Name == "Estado Multa" && e.Value != null && e.Value.ToString() != "—")
            {
                e.CellStyle.ForeColor = e.Value.ToString() == "Paga"
                    ? Color.FromArgb(46, 204, 113)
                    : Color.FromArgb(231, 76, 60);
            }
        }

        private void CarregarDados()
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para ver o histórico.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            try
            {
                dadosCompras = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);
                dadosEmprestimos = BLL.Historicos.LoadHistoricoEmpPorUtilizador(globais.id_utilizador);

                AtualizarGridCompras();
                AtualizarGridEmprestimos();

                AtualizarResumo();
                EsconderBotoesOrdenacao();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar histórico: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarResumo()
        {
            int compras = dadosCompras?.Rows.Count ?? 0;
            int emprestimos = dadosEmprestimos?.Rows.Count ?? 0;
            int ativos = 0;
            if (dadosEmprestimos != null)
            {
                foreach (DataRow row in dadosEmprestimos.Rows)
                {
                    if (row["Estado_Emprestimo"]?.ToString() == "Ativo")
                        ativos++;
                }
            }

            lblResumo.Text = TabComprasAtiva
                ? $"Utilizador #{globais.id_utilizador} | {compras} compra(s)"
                : $"Utilizador #{globais.id_utilizador} | {emprestimos} empréstimo(s) | {ativos} ativo(s)";
        }
    }
}
