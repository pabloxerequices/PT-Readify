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
                int preco = int.Parse(textBox5.Text);
                List<string> generos = new List<string> { textBox6.Text };
                string idioma = textBox7.Text;
                int paginas = int.Parse(textBox8.Text);
                DateTime dataPublicacao = dateTimePicker1.Value;
                string estadoLivro = comboBox1.Text; // Exemplo, ajuste conforme necessário
                Image capa = null; // Ajuste se houver seleção de imagem
                List<string> tipos = new List<string>(); // Ajuste conforme necessário

                 // Ordem e tipos corrigidos conforme assinatura do método
                BLL.Livros.InserirLivro(
                    paginas,          // int
                    titulo,           // string
                    biografia,        // string
                    preco,            // int
                    dataPublicacao,   // DateTime
                    autor,            // string
                    estadoLivro,      // string
                    editora,          // string
                    idioma,           // string
                    capa,             // Image
                    generos,          // List<string>
                    tipos             // List<string>
                );
                MessageBox.Show("Livro inserido com sucesso!");
            }
        }
    }
}
