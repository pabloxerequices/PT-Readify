using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Criar_ultilizadores_admin_ : Form
    {
        byte[] fotoBytes = null;
        public Criar_ultilizadores_admin_()
        {
            InitializeComponent();
        }

        private void Criar_ultilizadores_admin__Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = BLL.utilizador.Load();
            comboBox1.Items.AddRange(globais.prefixosEuropa);
            textBox3.UseSystemPasswordChar = true;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagens (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox6.Image = Image.FromFile(ofd.FileName);

                    // 2. AQUI ELA JÁ VAI SER RECONHECIDA SEM ERRO
                    fotoBytes = File.ReadAllBytes(ofd.FileName);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            main_menu__admin_ main_Menu_Admin_ = new main_menu__admin_();
            main_Menu_Admin_.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //verificar se o numero de telefone é válido (apenas dígitos) e se tem 9 dígitos
            if (!textBox4.Text.All(char.IsDigit))
            {
                MessageBox.Show("Número de telefone inválido. Apenas dígitos são permitidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox4.Text.Length != 9)
            {
                MessageBox.Show("Número de telefone inválido. Deve conter exatamente 9 dígitos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
                 if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Os campos Nome, Email e Palavra-Passe não podem estar vazios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                 if (!textBox2.Text.Contains("@") || !textBox2.Text.Contains("."))
            {
                MessageBox.Show("Endereço de email inválido. Certifique-se de que contém '@' e '.'.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                 if (textBox3.Text.Length < 6)
            {
                MessageBox.Show("A palavra-passe deve conter pelo menos 6 caracteres.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!textBox3.Text.Any(char.IsUpper) || !textBox3.Text.Any(char.IsLower) || !textBox3.Text.Any(char.IsDigit) || !textBox3.Text.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("A password deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caracter especial");
                return;
            }
            else if (textBox3.Text.Contains(" "))
            {
                MessageBox.Show("A password não pode conter espaços em branco");
                return;
            }
            else if (!int.TryParse(new string(comboBox1.Text.Where(char.IsDigit).ToArray()), out int prefixo) || !int.TryParse(textBox4.Text, out int telefone))
            {
                MessageBox.Show("Por favor, insira um prefixo e um número de telefone válidos (apenas números).", "Erro de Formatação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Para a execução para não quebrar o código abaixo
            }
            else
            {
                BLL.utilizador.insertutilizadoradmin(checkBox1.Checked, "Ativa", textBox2.Text, textBox1.Text, textBox3.Text, int.Parse(new string(comboBox1.Text.Where(char.IsDigit).ToArray())), int.Parse(textBox4.Text), fotoBytes);
                MessageBox.Show("Utilizador criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Se estiver escondida, mostra. Se estiver visível, esconde.
            if (textBox3.UseSystemPasswordChar == true)
            {
                textBox3.UseSystemPasswordChar = false;
                // Opcional: podes mudar o ícone para um olho aberto se tiveres a imagem
                // pictureBox5.Image = Properties.Resources.eye_open; 
            }
            else
            {
                textBox3.UseSystemPasswordChar = true;
                // pictureBox5.Image = Properties.Resources.key_icon;
            }
        }
    }
}
