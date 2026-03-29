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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
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
                        if (Convert.ToBoolean(dt.Rows[0][1]) == true)
                        {
                            MessageBox.Show("Bem Vindo " + ("(ADMIN) ") + dt.Rows[0][3].ToString());
                            globais.iisAdmin = true;

                            globais.profileEmail = textBox1.Text;

                            globais.id_utilizador = Convert.ToInt32(dt.Rows[0][0]);

                            main_menu__admin_ main_menu_admin = new main_menu__admin_();
                            main_menu_admin.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Bem Vindo " + dt.Rows[0][3].ToString());

                            globais.profileEmail = textBox1.Text;

                            globais.id_utilizador = Convert.ToInt32(dt.Rows[0][0]);

                            main_menu main_menu = new main_menu();
                            main_menu.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Senha incorreta");
                    }

                }
                else
                {
                    MessageBox.Show("Email não encontrado");
                }
            }
            else
            {
                MessageBox.Show("Preencha os campos");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
