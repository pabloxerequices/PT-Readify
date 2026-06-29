using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public partial class main_menu__admin_ : Form
    {
        public main_menu__admin_()
        {
            InitializeComponent();
            Load += Main_menu__admin__Load;
        }

        private void Main_menu__admin__Load(object sender, EventArgs e)
        {
            try
            {
                lblEstatisticas.Text = $"Estatísticas: {BLL.Estatisticas.EmprestimosAtivos()} ativos, {BLL.Estatisticas.ReservasPendentes()} reservas";
                lblFinanceiro.Text = $"Gestão Financeira: {(BLL.Estatisticas.MultasPendentesCentimos() / 100m):C2} em multas";
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new inserir_livros_rodrigo_admin_(), this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new pesquisar_livros_rodrigo(), this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new GestaoStockLivros(), this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Listar_utilizadores().Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Criar_ultilizadores_admin_().Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            new EstatisticasAdmin().Show();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            new HistoricoEmprestimosAdmin().Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new RelatorioMultasAdmin().Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            globais.id_utilizador = 0;
            new Form1().Show();
            this.Close();
        }
    }
}
