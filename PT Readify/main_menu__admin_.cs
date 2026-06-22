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
    public partial class main_menu__admin_ : Form
    {
        public main_menu__admin_()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new inserir_livros_rodrigo_admin_().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new livros().Show();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Listar_utilizadores().Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Criar_ultilizadores_admin_().Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
    }
}
