using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Inserir_Livros : Form
    {
        private ComboBox comboBox1;

        public Inserir_Livros()
        {
            InitializeComponent();

            if (comboBox1 == null)
            {
                comboBox1 = new ComboBox();
                comboBox1.Name = "comboBox1";
                comboBox1.Items.AddRange(new string[] { "Novo", "Usado", "Raro" });
                comboBox1.SelectedIndex = 0;
                this.Controls.Add(comboBox1);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Inserir_Livros_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(guna2TextBox1.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox8.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox4.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox5.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox6.Text) ||
                string.IsNullOrWhiteSpace(guna2TextBox7.Text))
            {
                MessageBox.Show("Preencha todos os campos!");
                return;
            }

            string titulo = guna2TextBox1.Text.Trim();
            string biografia = guna2TextBox8.Text.Trim();
            string autor = guna2TextBox2.Text.Trim();
            string editora = guna2TextBox3.Text.Trim();

            // Preço: aceitar formatos com símbolo (ex: "12€", "€12,00", "12.00")
            string precoRaw = guna2TextBox4.Text.Trim();
            decimal precoDecimal;
            int precoInt;
            if (decimal.TryParse(precoRaw, NumberStyles.Currency | NumberStyles.Number, CultureInfo.GetCultureInfo("pt-PT"), out precoDecimal) ||
                decimal.TryParse(precoRaw, NumberStyles.Currency | NumberStyles.Number, CultureInfo.InvariantCulture, out precoDecimal))
            {
                precoInt = (int)Math.Round(precoDecimal);
            }
            else
            {
                // fallback: remover todos os caracteres não-dígitos
                string onlyDigits = Regex.Replace(precoRaw, @"[^\d\-]", "");
                if (!int.TryParse(onlyDigits, out precoInt))
                {
                    MessageBox.Show("Preço inválido. Introduza um número inteiro ou valor monetário válido.");
                    return;
                }
            }

            // Páginas (campo correto: guna2TextBox7)
            if (!int.TryParse(guna2TextBox7.Text.Trim(), out int paginas))
            {
                MessageBox.Show("Número de páginas inválido. Introduza um número inteiro.");
                return;
            }

            // Ano de publicação (campo correto: guna2TextBox9)
            if (!int.TryParse(guna2TextBox9.Text.Trim(), out int dataPublicacao))
            {
                MessageBox.Show("Ano de Lançamento inválido. Introduza um ano válido.");
                return;
            }

            List<string> generos = new List<string> { guna2TextBox5.Text.Trim() };
            string idioma = guna2TextBox6.Text.Trim();
            string estadoLivro = guna2ComboBox1.Text;
            Image capa = null;
            List<string> tipos = new List<string>();

            BLL.Livros.InserirLivro(
                paginas,
                titulo,
                biografia,
                precoInt,
                dataPublicacao,
                autor,
                estadoLivro,
                editora,
                idioma,
                capa,
                generos,
                tipos
            );

            MessageBox.Show("Livro inserido com sucesso!");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imagens|*.jpg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }
    }
}
