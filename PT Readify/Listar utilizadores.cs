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
    public partial class Listar_utilizadores : Form
    {
        public Listar_utilizadores()
        {
            InitializeComponent();
        }

        private void Listar_utilizadores_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource= BLL.utilizador.Load();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
