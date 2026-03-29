using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class registar_ultilizador : Form
    {
        public registar_ultilizador()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //verificar se os campos estão preenchidos
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" )
            {
                //verificar se o email já existe
                DataTable dt =  BLL.utilizador.QueryutilizadorByemail(textBox1.Text);
                if (dt.Rows.Count == 0)
                {
                    //verificar se a password tem mais de 6 caracteres
                    if (textBox3.Text.Length < 6)
                    {
                        MessageBox.Show("A password deve ter mais de 6 caracteres");
                        return;
                    }
                    else
                    {
                        //verificar se o email é válido
                        if (!textBox2.Text.Contains("@") || !textBox2.Text.Contains("."))
                        {
                            MessageBox.Show("Email inválido");
                            return;
                        }
                        else
                        {
                            //verificar a palavrapasse tem pelo menos um número e uma letra mauscula e uma letra minuscula e um caracter especial
                            if (!textBox3.Text.Any(char.IsUpper) || !textBox3.Text.Any(char.IsLower) || !textBox3.Text.Any(char.IsDigit) || !textBox3.Text.Any(ch => !char.IsLetterOrDigit(ch)))
                            {
                                MessageBox.Show("A password deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caracter especial");
                                return;
                            }
                            else
                            {
                                //registar utilizador
                                BLL.utilizador.insertutilizador(false,"ativa", textBox2.Text, textBox1.Text, textBox3.Text);
                                MessageBox.Show("Utilizador ("+ textBox1 + ") registado com sucesso!");
                                this.Close();
                            }
                        }
                            
                    }
                        
                }
                else
                {
                    MessageBox.Show("Email já existe");
                }
            }
        }
    }
}
