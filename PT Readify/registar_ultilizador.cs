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
        private Config _config;

        public registar_ultilizador()
        {
            InitializeComponent();
            _config = ConfigManager.Current;
            ApplyConfig(_config);
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
                DataTable dt =  BLL.utilizador.QueryutilizadorByemail(textBox2.Text);
                if (dt.Rows.Count == 0)
                {
                    //verificar se a password tem mais de 6 caracteres
                    if (textBox3.Text.Length < 6)
                    {
                        MessageBox.Show(LanguageHelper.T("PasswordMinCharsRegister", _config));
                        return;
                    }
                    else
                    {
                        //verificar se o email é válido
                        if (!textBox2.Text.Contains("@") || !textBox2.Text.Contains("."))
                        {
                            MessageBox.Show(LanguageHelper.T("InvalidEmailRegister", _config));
                            return;
                        }
                        else
                        {
                            //verificar a palavrapasse tem pelo menos um número e uma letra mauscula e uma letra minuscula e um caracter especial
                            if (!textBox3.Text.Any(char.IsUpper) || !textBox3.Text.Any(char.IsLower) || !textBox3.Text.Any(char.IsDigit) || !textBox3.Text.Any(ch => !char.IsLetterOrDigit(ch)))
                            {
                                MessageBox.Show(LanguageHelper.T("PasswordComplexityRegister", _config));
                                return;
                            }
                            else
                            {
                                //numero de telefone com 9 digitos
                                if (textBox4.Text.Length != 9 || !textBox4.Text.All(char.IsDigit))
                                {
                                    MessageBox.Show(LanguageHelper.T("Phone9Digits", _config));
                                    return;
                                }
                                else
                                {
                                    //verificar se o numero de telefone já existe
                                    if (BLL.utilizador.Load().Select("prefixo_telefone = " + comboBox1.SelectedItem.ToString().Split(' ')[0].Replace("+", "") + " AND numero_telefone = " + textBox4.Text).Length > 0)
                                    {
                                        MessageBox.Show(LanguageHelper.T("PhoneExists", _config));
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

                                                int.Parse(comboBox1.SelectedItem.ToString().Split(' ')[0].Replace("+", "")),
                                                int.Parse(textBox4.Text)
                                            );
                                            MessageBox.Show(string.Format(LanguageHelper.T("UserRegistered", _config), textBox1.Text));
                                            this.Close();
                                        
                                    }
                                    
                                }
                                    
                            }
                        }
                            
                    }
                        
                }
                else
                {
                    MessageBox.Show(LanguageHelper.T("EmailExists", _config));
                }
            }
            else
            {
                MessageBox.Show(LanguageHelper.T("FillAllFields", _config));
            }
        }

        private void registar_ultilizador_Load(object sender, EventArgs e)
        {
            _config = ConfigManager.Current;
            ApplyLanguage();

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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (textBox3.UseSystemPasswordChar == false)
            {
                textBox3.UseSystemPasswordChar = true;
            }
            else
            {
                textBox3.UseSystemPasswordChar = false;
            }
        }

        private void ApplyLanguage()
        {
            if (_config == null) _config = ConfigManager.Current;
            this.Text = LanguageHelper.T("RegisterTitle", _config);
        }

        public void ApplyConfig(Config cfg)
        {
            if (cfg == null) return;
            ConfigApplier.ApplyFont(this, cfg);
        }
    }
}
