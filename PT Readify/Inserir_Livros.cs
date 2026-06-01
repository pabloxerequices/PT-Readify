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
        private byte[] ConverterImagemParaBytes(Image imagem)
        {
            if (imagem == null) return null;

            using (var ms = new System.IO.MemoryStream())
            {
                // Salva a imagem no stream usando o formato original dela
                imagem.Save(ms, imagem.RawFormat);
                return ms.ToArray();
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
            // 1. Array explícito com todos os campos obrigatórios para garantir que nenhum falha
            var campos = new[] {
        guna2TextBox1, guna2TextBox2, guna2TextBox3, guna2TextBox4,
        guna2TextBox5, guna2TextBox6, guna2TextBox7, guna2TextBox9, guna2TextBox8
    };

            if (campos.Any(t => string.IsNullOrWhiteSpace(t.Text)))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                return;
            }

            // 2. Extração segura de dados numéricos (Evita que o programa vá para o 'catch')

            // Preço
            string precoLimpo = Regex.Replace(guna2TextBox4.Text, @"[^\d]", "");
            if (!int.TryParse(precoLimpo, out int precoInt))
            {
                MessageBox.Show("O campo 'Preço' deve conter um valor numérico válido.");
                return;
            }

            // Páginas
            if (!int.TryParse(guna2TextBox7.Text.Trim(), out int paginas))
            {
                MessageBox.Show("O campo 'Número de páginas' deve ser um número inteiro válido.");
                return;
            }

            // Ano de Publicação
            if (!int.TryParse(guna2TextBox9.Text.Trim(), out int dataPublicacao))
            {
                MessageBox.Show("O campo 'Ano de Lançamento' deve ser um ano válido.");
                return;
            }

            // 3. Captura dos restantes textos
            string titulo = guna2TextBox1.Text.Trim();
            string biografia = guna2TextBox8.Text.Trim();
            string autor = guna2TextBox2.Text.Trim();
            string editora = guna2TextBox3.Text.Trim();
            string idioma = guna2TextBox6.Text.Trim();
            string estadoLivro = guna2ComboBox1.Text;

            // Listas e Imagem
            var generos = new List<string> { guna2TextBox5.Text.Trim() };
            var tipos = new List<string>();


            // Se quiser guardar a imagem selecionada da PictureBox para a BD:
            Image imagemCapa = pictureBox1.Image;
            byte[] Capa = ConverterImagemParaBytes(imagemCapa);

            // 4. Execução da BLL protegida contra falhas de conexão ou BD
            try
            {
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
                    Capa,
                    generos,
                    tipos
                );

                MessageBox.Show("Livro inserido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Exibe o erro REAL que a Base de Dados ou a BLL devolver para sabermos o que falhou
                MessageBox.Show($"Erro ao salvar na Base de Dados: {ex.Message}", "Erro BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
