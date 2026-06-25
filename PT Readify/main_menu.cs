using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using BusinessLogicLayer;

namespace PT_Readify
{
    public partial class main_menu : Form
    {
        private pesquisar_livros_rodrigo formPesquisa;

        public main_menu()
        {
            InitializeComponent();
        }

        private void panel_livros(Form form)
        {
            panel2.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            panel2.Controls.Add(form);
            form.Show();

            formPesquisa = form as pesquisar_livros_rodrigo;
        }

        public void AbrirEmprestimos()
        {
            FormLaunchHelper.Show(new Requesitar_livros(), this);
        }

        public void AbrirCarrinhoIntegrado()
        {
            using (var carrinho = new Carrinho())
            {
                carrinho.StartPosition = FormStartPosition.CenterParent;
                FormLaunchHelper.ShowDialog(carrinho, this);
            }

            formPesquisa?.RecarregarLivros();
            AtualizarTituloCarrinhoMenu();
        }

        public void AtualizarTituloCarrinhoMenu()
        {
            int total = CarrinhoService.TotalItens;
            var cfg = ConfigManager.Current;
            string booksLabel = LanguageHelper.T("Books", cfg);
            button3.Text = total > 0
                ? string.Format(LanguageHelper.T("BooksInCart", cfg), total)
                : booksLabel;
        }

        private void ApplyMenuLanguage(Config cfg)
        {
            if (cfg == null) return;

            button1.Text = LanguageHelper.T("Logout", cfg);
            button2.Text = LanguageHelper.T("Profile", cfg);
            button4.Text = LanguageHelper.T("Loans", cfg);
            button5.Text = LanguageHelper.T("Reservations", cfg);
            button6.Text = LanguageHelper.T("Assistant", cfg);
            button7.Text = LanguageHelper.T("PurchaseHistory", cfg);
            button8.Text = LanguageHelper.T("LoanHistory", cfg);
            button9.Text = LanguageHelper.T("Help", cfg);
            buttonConfig.Text = LanguageHelper.T("SettingsTitle", cfg);
        }

        private void pesquisarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (formPesquisa == null || !formPesquisa.Visible)
                panel_livros(new pesquisar_livros_rodrigo());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            globais.id_utilizador = 0;
            new Form1().Show();
            this.Close();
        }

        private void main_menu_Load(object sender, EventArgs e)
        {
            var cfg = ConfigManager.Current;
            ApplyConfig(cfg);
            AutoLogoutManager.Attach(this);

            MostrarNotificacoesPendentes();
            panel_livros(new pesquisar_livros_rodrigo());
            AtualizarTituloCarrinhoMenu();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            AutoLogoutManager.Detach();
            base.OnFormClosed(e);
        }

        private void MostrarNotificacoesPendentes()
        {
            if (globais.id_utilizador <= 0)
                return;

            try
            {
                int total = BLL.Notificacoes.ContarNaoLidas(globais.id_utilizador);
                if (total > 0)
                {
                    var resultado = MessageBox.Show(
                        $"Tem {total} notificação(ões) nova(s) (ex.: livros reservados disponíveis).\n\nDeseja ver agora?",
                        "Notificações",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (resultado == DialogResult.Yes)
                        FormLaunchHelper.Show(new NotificacoesUtilizador(), this);
                }
            }
            catch { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new Pesquisar_Livros(), this);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirEmprestimos();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new ReservasUtilizador(), this);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new Perfil(), this);
            this.Close();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            // Chat mantém tamanho normal — sem fullscreen
            new Chat_Bot().Show();
        }

        private void OpenUrl(string url)
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível abrir o navegador: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new Historico_de_Emprestimos(), this);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            FormLaunchHelper.Show(new Hstórico_de_compras(), this);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenUrl("https://siteptreadify.vercel.app/#sobre");
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            panel_livros(new Configuracoes());
        }

        public void ShowBooksPanel()
        {
            panel_livros(new pesquisar_livros_rodrigo());
        }

        // Adicione este método público à classe main_menu para aplicar tema/fonte de runtime.
        public void ApplyConfig(Config cfg)
        {
            if (cfg == null) return;

            ConfigApplier.ApplyTheme(this, panel1, cfg);
            ConfigApplier.ApplyFont(panel1, cfg);
            ConfigApplier.ApplyFont(panel2, cfg);
            ApplyMenuLanguage(cfg);
            AtualizarTituloCarrinhoMenu();
        }
    }
}
