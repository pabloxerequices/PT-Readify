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
    public partial class livros : Form
    {
        public livros()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Pesquisar_Livros().Show();
            Close();
        }

        private void livros_Load(object sender, EventArgs e)
        {
            foreach (DataRow livro in BLL.Livros.Load().Rows)
            {
                Panel card = new Panel();
                card.Size = new Size(150, 220);
                card.BackColor = Color.White; // Usar o branco gelo da sua paleta

                PictureBox capa = new PictureBox();
                capa.ImageLocation = livro.CaminhoCapa; // Caminho guardado na BD
                capa.SizeMode = PictureBoxSizeMode.Zoom;

                card.Controls.Add(capa);
                flowLayoutPanel1.Controls.Add(card);
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
