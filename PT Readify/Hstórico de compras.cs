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
        public Hstórico_de_compras()
        {
            InitializeComponent();
        }

        private void dataGridViewCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Hstórico_de_compras_Load(object sender, EventArgs e)
        {
            dataGridViewHistorico_Compras.DataSource = BLL.Historicos.LoadHistoricoCompras();
        }

        private void btnLimparCarrinho_Click(object sender, EventArgs e)
        {
            Devolução_da_Compra devolução = new Devolução_da_Compra();
        }
    }
}
