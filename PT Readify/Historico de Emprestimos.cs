using BusinessLogicLayer;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Historico_de_Emprestimos : Form
    {
        public Historico_de_Emprestimos()
        {
            InitializeComponent();
            dataGridViewHistorico_Emprestimos.CellFormatting += DataGridViewHistorico_Emprestimos_CellFormatting;
        }

        private void DataGridViewHistorico_Emprestimos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewHistorico_Emprestimos.Columns[e.ColumnIndex].Name == "Valor_Multa" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int centimos))
                    e.Value = (centimos / 100m).ToString("C2");
            }
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
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
            guna2Button3.Text = "Devolver livro";
        }

        private void CarregarHistorico()
        {
            dataGridViewHistorico_Emprestimos.DataSource = BLL.Historicos.LoadHistoricoEmpPorUtilizador(globais.id_utilizador);
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
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            OrdenarHistorico("Data_Levantamento ASC");
        }

        private void OrdenarHistorico(string sortExpression)
        {
            var historico = BLL.Historicos.LoadHistoricoEmpPorUtilizador(globais.id_utilizador);

            if (historico == null || historico.Columns.Count == 0 || !historico.Columns.Contains("Data_Levantamento"))
            {
                dataGridViewHistorico_Emprestimos.DataSource = null;
                MessageBox.Show("Não foi possível ordenar: dados inválidos ou coluna não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2Button2.Visible = true;
                guna2Button4.Visible = false;
                guna2Button5.Visible = false;
                return;
            }

            DataView view = historico.DefaultView;
            view.Sort = sortExpression;
            dataGridViewHistorico_Emprestimos.DataSource = view;

            guna2Button2.Visible = true;
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
        }

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
                string titulo = dataGridViewHistorico_Emprestimos.CurrentRow.Cells["Titulo"]?.Value?.ToString() ?? "livro";

                DialogResult confirmar = MessageBox.Show(
                    $"Confirmar devolução de \"{titulo}\"?",
                    "Devolução",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmar != DialogResult.Yes)
                    return;

                BLL.Historicos.DevolverEmprestimo(idHistorico, globais.id_utilizador);

                var historicoAtualizado = BLL.Historicos.LoadHistoricoEmpPorUtilizador(globais.id_utilizador);
                DataRow[] linha = historicoAtualizado.Select($"Id = {idHistorico}");
                string msgMulta = "";
                if (linha.Length > 0)
                {
                    int multa = Convert.ToInt32(linha[0]["Valor_Multa"]);
                    if (multa > 0)
                        msgMulta = $"\n\nMulta aplicada: {(multa / 100m):C2} (2€ por semana de atraso).";
                }

                CarregarHistorico();
                MessageBox.Show("Livro devolvido com sucesso!" + msgMulta, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
