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
    public partial class confirmar_perfil : Form
    {
        public confirmar_perfil()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == globais.profileEmail && textBox2.Text == globais.profilepassword)
            {
                globais.confirmacao = true;
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email ou senha incorretos");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
