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

        public Historico_de_Emprestimos()
        {
            InitializeComponent();
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
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para ver o histórico de empréstimos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            CarregarHistorico();
            guna2Button3.Text = "Devolver livro";
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
                labelTotal.Text = "Sem empréstimos ativos";
                labelTotal.ForeColor = Color.FromArgb(241, 196, 15);
            }
            else if (emAtraso > 0)
            {
                labelTotal.Text = $"{ativos} ativo(s) — {emAtraso} em atraso (multa: 2€/semana)";
                labelTotal.ForeColor = Color.FromArgb(231, 76, 60);
            }
            else
            {
                labelTotal.Text = $"{ativos} empréstimo(s) ativo(s)";
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
                MessageBox.Show("Selecione um empréstimo para devolver.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string estado = dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Estado_Emprestimo"]?.Value?.ToString();
                if (estado != "Ativo")
                {
                    MessageBox.Show("Selecione um empréstimo ativo para devolver.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idHistorico = Convert.ToInt32(dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Id"].Value);
                var resumo = BLL.Historicos.ObterResumoDevolucaoEmprestimo(idHistorico, globais.id_utilizador);

                DialogResult confirmar = MessageBox.Show(
                    DevolucaoUiHelper.ConstruirConfirmacaoEmprestimo(resumo),
                    "Devolução de Empréstimo",
                    MessageBoxButtons.YesNo,
                    resumo.DiasAtraso > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Question);

                if (confirmar != DialogResult.Yes)
                    return;

                var resultado = BLL.Historicos.DevolverEmprestimo(idHistorico, globais.id_utilizador);
                CarregarHistorico();
                MessageBox.Show(
                    DevolucaoUiHelper.ConstruirSucessoEmprestimo(resultado),
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao devolver: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Selecione um empréstimo rejeitado para pagar a multa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string estado = dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Estado_Emprestimo"].Value?.ToString();

                if (estado != "Rejeitado")
                {
                    MessageBox.Show("Apenas empréstimos rejeitados têm multa por livro estragado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int idHistorico = Convert.ToInt32(dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Id"].Value);
                int idLivro = Convert.ToInt32(dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Id_Livro"].Value);
                string titulo = dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Titulo"].Value.ToString();

                DialogResult confirmar = MessageBox.Show(
                    $"Pagar a multa por livro estragado de \"{titulo}\"?\n\n" +
                    $"Após o pagamento, o livro será devolvido.",
                    "Confirmar Pagamento",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmar != DialogResult.Yes)
                    return;

                BLL.Historicos.PagarMultaLivroEstragado(idHistorico, globais.id_utilizador, idLivro, titulo);
                CarregarHistorico();
                MessageBox.Show("Multa paga com sucesso! O livro foi devolvido.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao pagar multa: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
