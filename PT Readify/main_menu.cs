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
    public partial class main_menu : Form
    {
        public main_menu()
        {
            InitializeComponent();
        }

        private void pesquisarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //-----aba-livros-----
            button6.Show();
            flowLayoutPanel1.Show();
            panel2.Show();
            label1.Show();
            button7.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            

        }

        private void main_menu_Load(object sender, EventArgs e)
        {
            //-----aba-livros-----
            button6.Hide();
            flowLayoutPanel1.Hide();
            panel2.Hide();
            label1.Hide();
            button7.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //-----aba-livros-----
            button6.Hide();
            flowLayoutPanel1.Hide();
            panel2.Hide();
            label1.Hide();
            button7.Hide();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //-----aba-livros-----
            button7.Hide();
            button6.Hide();
            flowLayoutPanel1.Hide();
            panel2.Hide();
            label1.Hide();
        }
    }
}
