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
        private Config _config;

        public main_menu__admin_()
        {
            InitializeComponent();
            _config = ConfigManager.Current;
            Load += Main_menu__admin__Load;
        }

        private void Main_menu__admin__Load(object sender, EventArgs e)
        {
            _config = ConfigManager.Current;
            ApplyLanguage();

            try
            {
                lblEstatisticas.Text = string.Format(LanguageHelper.T("Statistics", _config), BLL.Estatisticas.EmprestimosAtivos(), BLL.Estatisticas.ReservasPendentes());
                lblFinanceiro.Text = string.Format(LanguageHelper.T("FinancialManagement", _config), BLL.Estatisticas.MultasPendentesCentimos() / 100m);
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
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Criar_ultilizadores_admin_().Show();
            
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

        private void button9_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new Aprovar_Devolucoes_Admin(), this);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new Aprovar_Devolucoes_Emp(), this);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            globais.id_utilizador = 0;
            CarteiraService.Limpar();
            
            // Reset fullscreen setting on logout to prevent it from persisting to next user
            var cfg = ConfigManager.Current;
            if (cfg != null)
            {
                cfg.FullscreenReading = false;
                ConfigManager.Save(cfg);
            }
            
            new Form1().Show();
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new Configuracoes(), this);
        }

        private void ApplyLanguage()
        {
            if (_config == null) _config = ConfigManager.Current;
            this.Text = LanguageHelper.T("AdminMenuTitle", _config);
        }
    }
}
