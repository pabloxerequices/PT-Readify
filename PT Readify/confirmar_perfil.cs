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

        private void confirmar_perfil_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                DataTable dt = BLL.utilizador.QueryutilizadorByemail(textBox1.Text);
                if (dt.Rows.Count != 0)
                {
                    if (dt.Rows[0][5].ToString() == textBox2.Text)
                    {
                        MessageBox.Show("Bem Vindo " + dt.Rows[0][3].ToString());
                        globais.profileEmail = textBox1.Text;
                        globais.profilepassword = textBox2.Text;
                        globais.id_utilizador = Convert.ToInt32(dt.Rows[0][0]);
                        CarteiraService.CarregarParaUtilizador(globais.id_utilizador);
                        globais.confirmacao = true;
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Password incorreta");
                    }
                }
                else
                {
                    MessageBox.Show("Email não encontrado");
                }
            }
            else
            {
                MessageBox.Show("Preencha todos os campos");
            }
        }
    }
}
