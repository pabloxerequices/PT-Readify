using BusinessLogicLayer;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Devolução_da_Compra : Form
    {
        private HistoricoSortHelper _sortHelper;

        public Devolução_da_Compra()
        {
            InitializeComponent();
            DevolucaoUiHelper.ConfigurarGrid(guna2DataGridView1);
            guna2DataGridView1.CellFormatting += Grid_CellFormatting;

            _sortHelper = new HistoricoSortHelper(
                guna2DataGridView1,
                guna2Button2,
                guna2Button4,
                guna2Button5,
                "Data_Compra");
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DevolucaoUiHelper.FormatarCelula(guna2DataGridView1, e);
        }

        private void Devolução_da_Compra_Load(object sender, EventArgs e)
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para ver o histórico de compras.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            guna2Button1.Visible = false;
            panel1.Visible = false;

            guna2Button2.Visible = true;
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
            guna2Button2.Parent = panelBottom;
            guna2Button4.Parent = panelBottom;
            guna2Button5.Parent = panelBottom;

            guna2Button2.Location = new System.Drawing.Point(250, 20);
            guna2Button4.Location = new System.Drawing.Point(480, 20);
            guna2Button5.Location = new System.Drawing.Point(710, 20);

            panelBottom.Controls.Add(guna2Button2);
            panelBottom.Controls.Add(guna2Button4);
            panelBottom.Controls.Add(guna2Button5);
            guna2Button2.BringToFront();
            guna2Button4.BringToFront();
            guna2Button5.BringToFront();

            guna2Button3.Text = "Devolver compra";
            guna2DataGridView1.DataError += (s, ev) => ev.ThrowException = false;
            CarregarCompras();
        }

        private void CarregarCompras()
        {
            DataTable compras = BLL.Historicos.LoadComprasDevolviveisPorUtilizador(globais.id_utilizador);
            _sortHelper.DefinirDados(GridDisplayHelper.FormatComprasParaExibicao(compras));

            int total = compras?.Rows.Count ?? 0;
            guna2Button3.Enabled = total > 0;

            if (total == 0)
            {
                labelTotal.Text = $"Sem compras elegíveis (prazo: {BLL.Historicos.MaxDiasDevolucaoCompra} dias)";
                labelTotal.ForeColor = Color.FromArgb(241, 196, 15);
            }
            else
            {
                labelTotal.Text = $"{total} compra(s) elegível(eis) para devolução";
                labelTotal.ForeColor = Color.White;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e) => _sortHelper.MostrarOpcoesOrdenacao();

        private void guna2Button4_Click(object sender, EventArgs e) => _sortHelper.OrdenarDecrescente();

        private void guna2Button5_Click(object sender, EventArgs e) => _sortHelper.OrdenarCrescente();

        private void guna2Button1_Click(object sender, EventArgs e) => DevolverCompraSelecionada();

        private void guna2Button3_Click(object sender, EventArgs e) => DevolverCompraSelecionada();

        private void DevolverCompraSelecionada()
        {
            if (guna2DataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma compra para devolver.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idHistorico = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["Id"].Value);
                var resumo = BLL.Historicos.ObterResumoDevolucaoCompra(idHistorico, globais.id_utilizador);

                DialogResult confirmar = MessageBox.Show(
                    DevolucaoUiHelper.ConstruirConfirmacaoCompra(resumo),
                    "Devolução da Compra",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmar != DialogResult.Yes)
                    return;

                var resultado = BLL.Historicos.DevolverCompra(idHistorico, globais.id_utilizador);
                CarteiraService.CarregarParaUtilizador(globais.id_utilizador);
                CarregarCompras();
                MessageBox.Show(
                    DevolucaoUiHelper.ConstruirSucessoCompra(resultado),
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao devolver: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
