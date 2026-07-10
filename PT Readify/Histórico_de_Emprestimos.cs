using BusinessLogicLayer;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Historico_de_Emprestimos : Form
    {
        private HistoricoSortHelper _sortHelper;
        private Config _config;

        public Historico_de_Emprestimos()
        {
            InitializeComponent();
            _config = ConfigManager.Current;
            dataGridViewHistorico_Emprestimos.DataError += (s, e) => e.ThrowException = false;
            DevolucaoUiHelper.ConfigurarGrid(dataGridViewHistorico_Emprestimos);
            dataGridViewHistorico_Emprestimos.CellFormatting += Grid_CellFormatting;
            dataGridViewHistorico_Emprestimos.RowPrePaint += Grid_RowPrePaint;

            _sortHelper = new HistoricoSortHelper(
                dataGridViewHistorico_Emprestimos,
                guna2Button2,
                guna2Button4,
                guna2Button5,
                "Data_Levantamento");
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewHistorico_Emprestimos.Columns[e.ColumnIndex].Name == "Estado Multa" && e.Value != null && e.Value.ToString() != "—")
            {
                e.CellStyle.ForeColor = e.Value.ToString() == "Paga"
                    ? Color.FromArgb(46, 204, 113)
                    : Color.FromArgb(231, 76, 60);
            }
            DevolucaoUiHelper.FormatarCelula(dataGridViewHistorico_Emprestimos, e);
        }

        private void Grid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DevolucaoUiHelper.ColorirLinhaEmprestimo(dataGridViewHistorico_Emprestimos, e);
        }

        private void Historico_de_Emprestimos_Load(object sender, EventArgs e)
        {
            _config = ConfigManager.Current;
            ApplyConfig(_config);
            ApplyLanguage();

            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show(LanguageHelper.T("LoginToViewLoans", _config), LanguageHelper.T("ValidationWarning", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            CarregarHistorico();
            guna2Button3.Text = LanguageHelper.T("ReturnBook", _config);
        }

        private DataTable dadosEmprestimosOriginais;

        private void CarregarHistorico()
        {
            dadosEmprestimosOriginais = BLL.Historicos.LoadHistoricoEmpPorUtilizador(globais.id_utilizador);
            dataGridViewHistorico_Emprestimos.DataSource =
                GridDisplayHelper.FormatEmprestimosParaExibicao(dadosEmprestimosOriginais);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Button2.Visible = false;
            guna2Button5.Visible = true;
            guna2Button4.Visible = true;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            OrdenarHistorico("Data_Levantamento DESC");

            var historico = BLL.Historicos.LoadHistoricoEmpPorUtilizador(globais.id_utilizador);
            _sortHelper.DefinirDados(historico);

            int ativos = historico?.AsEnumerable()
                .Count(r => r["Estado_Emprestimo"]?.ToString() == "Ativo") ?? 0;
            int emAtraso = historico?.AsEnumerable()
                .Count(r => r["Estado_Emprestimo"]?.ToString() == "Ativo" &&
                            r["Data_Prevista"] != DBNull.Value &&
                            Convert.ToDateTime(r["Data_Prevista"]).Date < DateTime.Now.Date) ?? 0;

            guna2Button3.Enabled = ativos > 0;

            if (ativos == 0)
            {
                labelTotal.Text = LanguageHelper.T("NoActiveLoans", _config);
                labelTotal.ForeColor = Color.FromArgb(241, 196, 15);
            }
            else if (emAtraso > 0)
            {
                labelTotal.Text = string.Format(LanguageHelper.T("ActiveAndOverdue", _config), ativos, emAtraso);
                labelTotal.ForeColor = Color.FromArgb(231, 76, 60);
            }
            else
            {
                labelTotal.Text = string.Format(LanguageHelper.T("ActiveLoans", _config), ativos);
                labelTotal.ForeColor = Color.White;
            }
        }

        private void OrdenarHistorico(string sortExpression)
        {
            if (dadosEmprestimosOriginais == null)
                dadosEmprestimosOriginais = BLL.Historicos.LoadHistoricoEmpPorUtilizador(globais.id_utilizador);

            if (dadosEmprestimosOriginais == null || dadosEmprestimosOriginais.Columns.Count == 0 || !dadosEmprestimosOriginais.Columns.Contains("Data_Levantamento"))
                return;

            DataView view = dadosEmprestimosOriginais.DefaultView;
            view.Sort = sortExpression;
            dataGridViewHistorico_Emprestimos.DataSource =
                GridDisplayHelper.FormatEmprestimosParaExibicao(dadosEmprestimosOriginais.DefaultView.ToTable());
        }

        private void guna2Button2_Click_ShowSortOptions(object sender, EventArgs e) => _sortHelper.MostrarOpcoesOrdenacao();

        private void guna2Button4_Click_ShowSortDesc(object sender, EventArgs e) => _sortHelper.OrdenarDecrescente();

        private void guna2Button5_Click(object sender, EventArgs e) => _sortHelper.OrdenarCrescente();

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (dataGridViewHistorico_Emprestimos.CurrentRow == null)
            {
                MessageBox.Show(LanguageHelper.T("SelectLoanToReturn", _config), LanguageHelper.T("ValidationWarning", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string estado = dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Estado_Emprestimo"]?.Value?.ToString();
                if (estado != "Ativo")
                {
                    MessageBox.Show(LanguageHelper.T("SelectActiveLoan", _config), LanguageHelper.T("ValidationWarning", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idHistorico = Convert.ToInt32(dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Id"].Value);
                var resumo = BLL.Historicos.ObterResumoDevolucaoEmprestimo(idHistorico, globais.id_utilizador);

                DialogResult confirmar = MessageBox.Show(
                    DevolucaoUiHelper.ConstruirConfirmacaoEmprestimo(resumo),
                    LanguageHelper.T("LoanReturn", _config),
                    MessageBoxButtons.YesNo,
                    resumo.DiasAtraso > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Question);

                if (confirmar != DialogResult.Yes)
                    return;

                var resultado = BLL.Historicos.DevolverEmprestimo(idHistorico, globais.id_utilizador);
                CarregarHistorico();
                MessageBox.Show(
                    DevolucaoUiHelper.ConstruirSucessoEmprestimo(resultado),
                    LanguageHelper.T("Success", _config),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(LanguageHelper.T("ErrorReturning", _config), ex.Message), LanguageHelper.T("Error", _config), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewHistorico_Emprestimos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridViewHistorico_Emprestimos_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            CarregarHistorico();
        }

        private void dataGridViewHistorico_Emprestimos_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnPagarMulta_Click(object sender, EventArgs e)
        {
            if (dataGridViewHistorico_Emprestimos.CurrentRow == null)
            {
                MessageBox.Show(LanguageHelper.T("SelectRejectedLoan", _config), LanguageHelper.T("ValidationWarning", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string estado = dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Estado_Emprestimo"].Value?.ToString();

                if (estado != "Rejeitado")
                {
                    MessageBox.Show(LanguageHelper.T("OnlyRejectedHaveFine", _config), LanguageHelper.T("ValidationWarning", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idHistorico = Convert.ToInt32(dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Id"].Value);
                int idLivro = Convert.ToInt32(dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Id_Livro"].Value);
                string titulo = dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Titulo"].Value.ToString();

                DialogResult confirmar = MessageBox.Show(
                    string.Format(LanguageHelper.T("PayFineConfirm", _config), titulo),
                    LanguageHelper.T("ConfirmFinePayment", _config),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmar != DialogResult.Yes)
                    return;

                BLL.Historicos.PagarMultaLivroEstragado(idHistorico, globais.id_utilizador, idLivro, titulo);
                CarregarHistorico();
                MessageBox.Show(LanguageHelper.T("FinePaid", _config), LanguageHelper.T("Success", _config), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(LanguageHelper.T("ErrorPayingFine", _config), ex.Message), LanguageHelper.T("Error", _config), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyLanguage()
        {
            if (_config == null) _config = ConfigManager.Current;
            this.Text = LanguageHelper.T("LoanHistoryTitle", _config);
            guna2Button3.Text = LanguageHelper.T("ReturnBook", _config);
            btnPagarMulta.Text = LanguageHelper.T("ConfirmFinePayment", _config);
        }

        public void ApplyConfig(Config cfg)
        {
            if (cfg == null) return;
            ConfigApplier.ApplyFont(this, cfg);
        }
    }
}
