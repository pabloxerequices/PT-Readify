using BusinessLogicLayer;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Hstórico_de_compras : Form
    {
        private HistoricoSortHelper _sortHelper;

        public Hstórico_de_compras()
        {
            InitializeComponent();
            DevolucaoUiHelper.ConfigurarGrid(guna2DataGridView1Historico_Compras);
            guna2DataGridView1Historico_Compras.CellFormatting += Grid_CellFormatting;
            guna2DataGridView1Historico_Compras.RowPrePaint += Grid_RowPrePaint;

            _sortHelper = new HistoricoSortHelper(
                guna2DataGridView1Historico_Compras,
                guna2Button2,
                guna2Button4,
                guna2Button5,
                "Data_Compra");
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DevolucaoUiHelper.FormatarCelula(guna2DataGridView1Historico_Compras, e);
        }

        private void Grid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DevolucaoUiHelper.ColorirLinhaCompra(guna2DataGridView1Historico_Compras, e);
        }

        private void Hstórico_de_compras_Load(object sender, EventArgs e)
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para ver o histórico de compras.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            CarregarHistorico();
        }

        private void CarregarHistorico()
        {
            var historico = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);
            _sortHelper.DefinirDados(historico);

            int total = historico?.Rows.Count ?? 0;
            int devolvidas = 0;
            if (historico != null && historico.Columns.Contains("Estado_Compra"))
            {
                foreach (DataRow row in historico.Rows)
                {
                    if (row["Estado_Compra"]?.ToString() == "Devolvida")
                        devolvidas++;
                }
            }

            labelTotal.Text = total == 0
                ? "Sem compras registadas"
                : $"{total} compra(s)" + (devolvidas > 0 ? $" — {devolvidas} devolvida(s)" : "");
        }

        private void btnLimparCarrinho_Click(object sender, EventArgs e)
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para devolver compras.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Devolução_da_Compra devolução = new Devolução_da_Compra();
            devolução.FormClosed += (s, args) => CarregarHistorico();
            devolução.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e) => _sortHelper.MostrarOpcoesOrdenacao();

        private void guna2Button4_Click(object sender, EventArgs e) => _sortHelper.OrdenarDecrescente();

        private void guna2Button5_Click(object sender, EventArgs e) => _sortHelper.OrdenarCrescente();

        private void guna2DataGridView1Historico_Compras_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
