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
    public partial class Inserir_Livros : Form
    {
        public Inserir_Livros()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Preencha todos os campos!");
            }
            else
            {
                string titulo = textBox1.Text;
                string biografia = textBox2.Text;
                string autor = textBox3.Text;
                string editora = textBox4.Text;
                string preco = textBox5.Text;
                string genero = textBox6.Text;
                string idioma = textBox7.Text;
                int paginas = int.Parse(textBox8.Text);
                DateTime dataPublicacao = dateTimePicker1.Value;
                DateTime dataDescricao = dateTimePicker1.Value;


                // Aqui você pode adicionar o código para salvar os dados do livro em um banco de dados ou arquivo
                MessageBox.Show("Livro inserido com sucesso!");
            }
        }
    }
}
