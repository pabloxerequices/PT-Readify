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
            globais.id_utilizador = 0;
            CarteiraService.Limpar();
            textBox2.UseSystemPasswordChar = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Chat_Bot().Show();
        }

        private void linkRecuperarPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Recuperar_Palavra_passe recuperar_Palavra_Passe = new Recuperar_Palavra_passe();
            recuperar_Palavra_Passe.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                DataTable dt = BLL.utilizador.QueryutilizadorByemail(textBox1.Text);
                if (dt.Rows.Count != 0)
                {
                    DataRow utilizador = dt.Rows[0];
                    int idUtilizador = Convert.ToInt32(utilizador["Id_Utilizador"]);

                    BLL.Historicos.AtualizarMultasEmAtraso();
                    if (BLL.utilizador.BloquearSeTiverTresMultas(idUtilizador) || BLL.utilizador.ContaEstaBloqueada(utilizador))
                    {
                        MessageBox.Show("A sua conta está bloqueada por ter 3 multas. Contacte um administrador.");
                        return;
                    }

                    string passwordGuardada = utilizador["Palavra_Passe"].ToString();
                    if (BLL.utilizador.VerificarPassword(passwordGuardada, textBox2.Text))
                    {
                        if (!BLL.utilizador.IsPasswordHash(passwordGuardada))
                        {
                            BLL.utilizador.AtualizarPasswordHash(idUtilizador, BLL.utilizador.HashPassword(textBox2.Text));
                        }

                        if (Convert.ToBoolean(utilizador["Tipo_Utilizador"]) == true)
                        {
                            MessageBox.Show("Bem Vindo " + ("(ADMIN) ") + utilizador["Nome"].ToString());
                            globais.iisAdmin = true;

                            globais.profileEmail = textBox1.Text;
                            globais.profilepassword = "";

                            globais.id_utilizador = idUtilizador;
                            CarteiraService.CarregarParaUtilizador(globais.id_utilizador);

                            main_menu__admin_ main_menu_admin = new main_menu__admin_();
                            main_menu_admin.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Bem Vindo " + utilizador["Nome"].ToString());

                            globais.profileEmail = textBox1.Text;
                            globais.profilepassword = "";

                            globais.id_utilizador = idUtilizador;
                            CarteiraService.CarregarParaUtilizador(globais.id_utilizador);

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

        private void button1_Click(object sender, EventArgs e)
        {
            registar_ultilizador registar_Ultilizador = new registar_ultilizador();
            registar_Ultilizador.Show();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Se estiver escondida, mostra. Se estiver visível, esconde.
            if (textBox2.UseSystemPasswordChar == true)
            {
                textBox2.UseSystemPasswordChar = false;
                // Opcional: podes mudar o ícone para um olho aberto se tiveres a imagem
                // pictureBox5.Image = Properties.Resources.eye_open; 
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
                // pictureBox5.Image = Properties.Resources.key_icon;
            }
        }
    }
    
}
