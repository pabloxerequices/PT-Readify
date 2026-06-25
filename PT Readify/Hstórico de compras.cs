using BusinessLogicLayer;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Hstórico_de_compras : Form
    {
        private DataTable dadosComprasOriginais;
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

            guna2DataGridView1Historico_Compras.DataError += (s, ev) => ev.ThrowException = false;
            CarregarCompras();
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
        }

        private void CarregarCompras()
        {
            dadosComprasOriginais = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);
            guna2DataGridView1Historico_Compras.DataSource =
                GridDisplayHelper.FormatComprasParaExibicao(dadosComprasOriginais);
        }

        private void btnLimparCarrinho_Click(object sender, EventArgs e)
        {
            Devolução_da_Compra devolução = new Devolução_da_Compra();
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

        private void OrdenarCompras(string sortExpression)
        {
            if (dadosComprasOriginais == null)
                dadosComprasOriginais = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);

            if (dadosComprasOriginais == null || dadosComprasOriginais.Columns.Count == 0 || !dadosComprasOriginais.Columns.Contains("Data_Compra"))
            {
                guna2DataGridView1Historico_Compras.DataSource = null;
                MessageBox.Show("Não foi possível ordenar: dados inválidos ou coluna não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2Button2.Visible = true;
                guna2Button4.Visible = false;
                guna2Button5.Visible = false;
                return;
            }

            DataView view = dadosComprasOriginais.DefaultView;
            view.Sort = sortExpression;
            guna2DataGridView1Historico_Compras.DataSource =
                GridDisplayHelper.FormatComprasParaExibicao(dadosComprasOriginais.DefaultView.ToTable());

            guna2Button2.Visible = true;
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            OrdenarCompras("Data_Compra DESC");
        }

        // Renomeado para evitar duplicidade
        private void guna2Button4_OrdenarDecrescente_Click(object sender, EventArgs e) => _sortHelper.OrdenarDecrescente();

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            OrdenarCompras("Data_Compra ASC");
        }

        // Renomeado para evitar duplicidade
        private void btnLimparCarrinho_Devolucao_Click(object sender, EventArgs e)
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
        
        //private void guna2Button4_Click(object sender, EventArgs e) => _sortHelper.OrdenarDecrescente();
        
       // private void guna2Button5_Click(object sender, EventArgs e) => _sortHelper.OrdenarCrescente();

        private void guna2DataGridView1Historico_Compras_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
