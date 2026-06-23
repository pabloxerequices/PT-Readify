using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public partial class inserir_livros_rodrigo_admin_ : Form
    {
        byte[] fotoBytes = null;
        private NumericUpDown numStock;
        private Label lblStock;

        public inserir_livros_rodrigo_admin_()
        {
            InitializeComponent();
        }

        private void inserir_livros_rodrigo_admin__Load(object sender, EventArgs e)
        {
            CarregarEstados();
            CarregarGeneros();
            ConfigurarCampoStock();
        }

        private void ConfigurarCampoStock()
        {
            lblStock = new Label
            {
                Text = "Stock (exemplares):",
                Location = new Point(50, 505),
                AutoSize = true
            };
            numStock = new NumericUpDown
            {
                Location = new Point(240, 503),
                Size = new Size(80, 23),
                Minimum = 0,
                Maximum = 9999,
                Value = 1
            };
            Controls.Add(lblStock);
            Controls.Add(numStock);
        }

        private void CarregarEstados()
        {
            try
            {
                
                guna2ComboBox1.DataSource = new[] { "novo", "Usado", "Danificado", "Emprestado", "Indisponivel" };
                guna2ComboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar estados: " + ex.Message);
            }
        }

        private void CarregarGeneros()
        {
            try
            {
                var generos = BLL.Livros.ObterGeneros();
                guna2ComboBox2.DataSource = generos;
                guna2ComboBox2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar gêneros: " + ex.Message);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagens (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image = Image.FromFile(ofd.FileName);
                        fotoBytes = File.ReadAllBytes(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar imagem: " + ex.Message);
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // 1. Validação de campos vazios
            var campos = new[] {
                guna2TextBox1, guna2TextBox2, guna2TextBox3, guna2TextBox4,
                 guna2TextBox5, guna2TextBox7, guna2TextBox8, guna2TextBox9
            };

            if (campos.Any(t => string.IsNullOrWhiteSpace(t.Text)))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                return;
            }

            if (string.IsNullOrWhiteSpace(guna2ComboBox1.Text))
            {
                MessageBox.Show("Por favor, selecione o estado do livro.");
                return;
            }

            if (string.IsNullOrWhiteSpace(guna2ComboBox2.Text))
            {
                MessageBox.Show("Por favor, selecione um gênero.");
                return;
            }

            // 2. Validação de dados numéricos

            // Preço
            string precoLimpo = Regex.Replace(guna2TextBox4.Text, @"[^\d,.]", "");
            if (!decimal.TryParse(precoLimpo, out decimal precoDecimal))
            {
                MessageBox.Show("O campo 'Preço' deve conter um valor numérico válido.");
                return;
            }
            int preco = (int)(precoDecimal * 100);

            // Páginas
            if (!int.TryParse(guna2TextBox7.Text.Trim(), out int paginas))
            {
                MessageBox.Show("O campo 'Número de páginas' deve ser um número inteiro válido.");
                return;
            }

            // Ano de Publicação
            if (!int.TryParse(guna2TextBox9.Text.Trim(), out int ano))
            {
                MessageBox.Show("O campo 'Ano de Lançamento' deve ser um ano válido.");
                return;
            }

            // 3. Captura dos textos
            string titulo = guna2TextBox1.Text.Trim();
            string autor = guna2TextBox2.Text.Trim();
            string editora = guna2TextBox3.Text.Trim();
            string idioma = guna2TextBox5.Text.Trim();
            string biografia = guna2TextBox8.Text.Trim();
            string estado_livro = guna2ComboBox1.Text.Trim();
            string genero = guna2ComboBox2.Text.Trim();

            // Listas
            var generos = new List<string> { genero };

            // 4. Execução da BLL
            try
            {
                int stock = (int)numStock.Value;
                BLL.Livros.InserirLivro(
                    paginas,
                    titulo,
                    biografia,
                    preco,
                    ano,
                    autor,
                    estado_livro,
                    editora,
                    idioma,
                    fotoBytes,
                    generos,
                    stock
                );

                MessageBox.Show("Livro inserido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inserir livro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparFormulario()
        {
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            guna2TextBox3.Clear();
            guna2TextBox4.Clear();
            
            guna2TextBox5.Clear();
            guna2TextBox7.Clear();
            guna2TextBox8.Clear();
            guna2TextBox9.Clear();
            guna2ComboBox1.SelectedIndex = 0;
            guna2ComboBox2.SelectedIndex = 0;
            pictureBox1.Image = null;
            fotoBytes = null;
            if (numStock != null)
                numStock.Value = 1;
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(mStream);
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
