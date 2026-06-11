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
    public partial class Historico_de_Emprestimos : Form
    {
        public Historico_de_Emprestimos()
        {
            InitializeComponent();
        }

        private void Historico_de_Emprestimos_Load(object sender, EventArgs e)
        {
            dataGridViewHistorico_Emprestimos.DataSource = BLL.Historicos.LoadHistoricoEmp();
        }

        private void dataGridViewHistorico_Emprestimos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
