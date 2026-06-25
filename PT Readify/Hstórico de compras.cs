using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Hstórico_de_compras : Form
    {
        private DataTable dadosComprasOriginais;

        public Hstórico_de_compras()
        {
            InitializeComponent();
        }

        private void dataGridViewCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
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
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Button2.Visible = false;
            guna2Button5.Visible = true;
            guna2Button4.Visible = true;
        }

        private void guna2DataGridView1Historico_Compras_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            OrdenarCompras("Data_Compra ASC");
        }
    }
}
