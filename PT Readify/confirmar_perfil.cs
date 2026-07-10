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
            var cfg = ConfigManager.Current;
            ApplyConfig(cfg);
            textBox2.UseSystemPasswordChar = true;
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
                    string passwordGuardada = utilizador["Palavra_Passe"].ToString();

                    if (BLL.utilizador.ContaEstaBloqueada(utilizador) || BLL.utilizador.BloquearSeTiverTresMultas(idUtilizador))
                    {
                        MessageBox.Show("A sua conta está bloqueada por ter 3 multas. Contacte um administrador.");
                        return;
                    }

                    if (BLL.utilizador.VerificarPassword(passwordGuardada, textBox2.Text))
                    {
                        if (!BLL.utilizador.IsPasswordHash(passwordGuardada))
                        {
                            BLL.utilizador.AtualizarPasswordHash(idUtilizador, BLL.utilizador.HashPassword(textBox2.Text));
                        }

                        MessageBox.Show("Bem Vindo " + utilizador["Nome"].ToString());
                        globais.profileEmail = textBox1.Text;
                        globais.profilepassword = "";
                        globais.id_utilizador = idUtilizador;
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

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (textBox2.UseSystemPasswordChar == false)
            {
                textBox2.UseSystemPasswordChar = true;
            }
            else
            {
                textBox2.UseSystemPasswordChar = false;
            }
        }

        public void ApplyConfig(Config cfg)
        {
            if (cfg == null) return;
            ConfigApplier.ApplyFont(this, cfg);
        }
    }
}
