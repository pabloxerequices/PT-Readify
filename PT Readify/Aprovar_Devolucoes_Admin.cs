using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public partial class Aprovar_Devolucoes_Admin : Form
    {
        private DataTable devolucoesPendentes;

        public Aprovar_Devolucoes_Admin()
        {
            InitializeComponent();
        }

        private void Aprovar_Devolucoes_Admin_Load(object sender, EventArgs e)
        {
            CarregarDevolucoesPendentes();
        }

        private void CarregarDevolucoesPendentes()
        {
            try
            {
                devolucoesPendentes = BLL.Historicos.CarregarDevolucoesPendentes();
                dataGridViewDevolucoes.DataSource = devolucoesPendentes;

                if (devolucoesPendentes == null || devolucoesPendentes.Rows.Count == 0)
                {
                    lblStatus.Text = "Nenhuma devolução pendente";
                    lblStatus.ForeColor = Color.FromArgb(52, 168, 83);
                    btnAprovar.Enabled = false;
                    btnRejeitar.Enabled = false;
                }
                else
                {
                    lblStatus.Text = $"{devolucoesPendentes.Rows.Count} devolução(ões) pendente(s)";
                    lblStatus.ForeColor = Color.FromArgb(241, 196, 15);
                    btnAprovar.Enabled = true;
                    btnRejeitar.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar devoluções: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAprovar_Click(object sender, EventArgs e)
        {
            if (dataGridViewDevolucoes.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma devolução para aprovar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idHistorico = Convert.ToInt32(dataGridViewDevolucoes.CurrentRow.Cells["Id"].Value);
                int idUtilizador = Convert.ToInt32(dataGridViewDevolucoes.CurrentRow.Cells["Id_Utilizador"].Value);
                int idLivro = Convert.ToInt32(dataGridViewDevolucoes.CurrentRow.Cells["Id_Livro"].Value);
                string titulo = dataGridViewDevolucoes.CurrentRow.Cells["Titulo"].Value.ToString();

                DialogResult confirmar = MessageBox.Show(
                    $"Aprovar devolução de \"{titulo}\"?\n\n" +
                    $"- Stock do livro será incrementado\n" +
                    $"- Reembolso será creditado na carteira do utilizador",
                    "Confirmar Aprovação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmar != DialogResult.Yes)
                    return;

                BLL.Historicos.AprovarDevolucaoCompra(idHistorico, idUtilizador, idLivro, titulo);
                MessageBox.Show("Devolução aprovada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarDevolucoesPendentes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aprovar devolução: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRejeitar_Click(object sender, EventArgs e)
        {
            if (dataGridViewDevolucoes.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma devolução para rejeitar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idHistorico = Convert.ToInt32(dataGridViewDevolucoes.CurrentRow.Cells["Id"].Value);
                string titulo = dataGridViewDevolucoes.CurrentRow.Cells["Titulo"].Value.ToString();

                DialogResult confirmar = MessageBox.Show(
                    $"Rejeitar devolução de \"{titulo}\"?\n\n" +
                    $"A compra permanecerá como ativa e não haverá reembolso.",
                    "Confirmar Rejeição",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmar != DialogResult.Yes)
                    return;

                BLL.Historicos.RejeitarDevolucaoCompra(idHistorico);
                MessageBox.Show("Devolução rejeitada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarregarDevolucoesPendentes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao rejeitar devolução: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarDevolucoesPendentes();
        }

        private void panelBottom_Paint(object sender, PaintEventArgs e)
        {
            // Event handler para o Paint do panelBottom
        }
    }
}
