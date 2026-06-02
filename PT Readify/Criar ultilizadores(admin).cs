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
        public Criar_ultilizadores_admin_()
        {
            InitializeComponent();
        }

        private void Criar_ultilizadores_admin__Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = BLL.utilizador.Load();
            comboBox1.Items.AddRange(globais.prefixosEuropa);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

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
            if (dataGridView1.CurrentRow.Cells["foto"].Value != DBNull.Value)
            {
                byte[] fotoBytes = (byte[])dataGridView1.CurrentRow.Cells["foto"].Value;
                using (MemoryStream ms = new MemoryStream(fotoBytes))
                {
                    pictureBox6.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox6.Image = null; // Ou uma imagem padrão, se preferir
            }
            textBox1.Text = dataGridView1.CurrentRow.Cells["nome"].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells["palavra_passe"].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells["numero_telefone"].Value.ToString();
            comboBox1.Text = "+" + dataGridView1.CurrentRow.Cells["prefixo_telefone"].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells["Estado_Conta"].Value.ToString();
            checkBox1.Checked = (bool)dataGridView1.CurrentRow.Cells["tipo_utilizador"].Value;
        }
    }
}
