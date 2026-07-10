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
        private ToolTip _toolTipDevolucao;
        private Config _config;

        public Hstórico_de_compras()
        {
            InitializeComponent();
            _config = ConfigManager.Current;
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
            _config = ConfigManager.Current;
            ApplyConfig(_config);
            ApplyLanguage();

            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show(LanguageHelper.T("LoginToViewHistory", _config), LanguageHelper.T("ValidationWarning", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            guna2DataGridView1Historico_Compras.DataError += (s, ev) => ev.ThrowException = false;
            CarregarCompras();
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;

            _toolTipDevolucao = new ToolTip();
            _toolTipDevolucao.SetToolTip(guna2Button3, DevolucaoUiHelper.TextoPoliticaDevolucaoCompra());
        }

        private void CarregarCompras()
        {
            dadosComprasOriginais = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);
            guna2DataGridView1Historico_Compras.DataSource =
                GridDisplayHelper.FormatComprasParaExibicao(dadosComprasOriginais);
            AtualizarLabelTotal();
        }

        private void AtualizarLabelTotal()
        {
            int total = dadosComprasOriginais?.Rows.Count ?? 0;
            int devolvidas = 0;
            if (dadosComprasOriginais != null && dadosComprasOriginais.Columns.Contains("Estado_Compra"))
            {
                foreach (DataRow row in dadosComprasOriginais.Rows)
                {
                    if (row["Estado_Compra"]?.ToString() == "Devolvida")
                        devolvidas++;
                }
            }

            labelTotal.Text = total == 0
                ? LanguageHelper.T("NoPurchases", _config)
                : string.Format(LanguageHelper.T("PurchasesCount", _config), total) + (devolvidas > 0 ? " — " + string.Format(LanguageHelper.T("ReturnedCount", _config), devolvidas) : "");
        }

        private void CarregarHistorico()
        {
            dadosComprasOriginais = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);
            _sortHelper.DefinirDados(GridDisplayHelper.FormatComprasParaExibicao(dadosComprasOriginais));
            AtualizarLabelTotal();
        }

        private void OrdenarCompras(string sortExpression)
        {
            if (dadosComprasOriginais == null)
                dadosComprasOriginais = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);

            if (dadosComprasOriginais == null || dadosComprasOriginais.Columns.Count == 0 || !dadosComprasOriginais.Columns.Contains("Data_Compra"))
            {
                guna2DataGridView1Historico_Compras.DataSource = null;
                MessageBox.Show(LanguageHelper.T("SortError", _config), LanguageHelper.T("ValidationWarning", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show(LanguageHelper.T("LoginToReturn", _config), LanguageHelper.T("ValidationWarning", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Devolução_da_Compra devolução = new Devolução_da_Compra();
            devolução.FormClosed += (s, args) =>
            {
                CarteiraService.Recarregar();
                CarregarHistorico();
            };
            devolução.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e) => _sortHelper.MostrarOpcoesOrdenacao();

        private void ApplyLanguage()
        {
            if (_config == null) _config = ConfigManager.Current;
            this.Text = LanguageHelper.T("PurchaseHistoryTitle", _config);
            guna2Button3.Text = LanguageHelper.T("ReturnPurchase", _config);
        }

        public void ApplyConfig(Config cfg)
        {
            if (cfg == null) return;
            ConfigApplier.ApplyFont(this, cfg);
        }

        private void guna2DataGridView1Historico_Compras_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
