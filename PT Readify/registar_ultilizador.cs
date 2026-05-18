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
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && comboBox1.SelectedItem != null)
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
                                //numero de telefone com 9 digitos
                                if (textBox4.Text.Length != 9 || !textBox4.Text.All(char.IsDigit))
                                {
                                    MessageBox.Show("Número de telefone tem de ter 9 dígitos");
                                    return;
                                }
                                else
                                {
                                    //registar utilizador
                                    BLL.utilizador.insertutilizador(
                                        false,
                                        "ativa",
                                        textBox2.Text,
                                        textBox1.Text,
                                        textBox3.Text,
                                        "", // Foto (campo obrigatório na assinatura do método)
                                        int.Parse(comboBox1.SelectedItem.ToString().Split(' ')[0].Replace("+", "")),
                                        int.Parse(textBox4.Text)
                                    );
                                    MessageBox.Show("Utilizador (" + textBox1.Text + ") registado com sucesso!");
                                    this.Close();
                                }
                                    
                            }
                        }
                            
                    }
                        
                }
                else
                {
                    MessageBox.Show("Email já existe");
                }
            }
            else
            {
                MessageBox.Show("Preencha todos os campos");
            }
        }

        private void registar_ultilizador_Load(object sender, EventArgs e)
        {
            //-------------------NUMEROS DE TELEFONE---------------------

            // Limpa itens antigos 
            comboBox1.Items.Clear();

            // Adiciona a lista ao ComboBox
            comboBox1.Items.AddRange(globais.prefixosEuropa);

            // Define Portugal
            comboBox1.SelectedIndex = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
