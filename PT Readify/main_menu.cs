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
            new Requesitar_livros().Show();
        }

        public void AbrirCarrinhoIntegrado()
        {
            using (var carrinho = new Carrinho())
            {
                carrinho.StartPosition = FormStartPosition.CenterParent;
                carrinho.ShowDialog(this);
            }

            formPesquisa?.RecarregarLivros();
            AtualizarTituloCarrinhoMenu();
        }

        public void AtualizarTituloCarrinhoMenu()
        {
            int total = CarrinhoService.TotalItens;
            button3.Text = total > 0 ? $"Livros ({total} no carrinho)" : "Livros";
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
            // Carrega configurações ao iniciar (aplica apenas o básico; outros forms devem ler ConfigManager.Current)
            var cfg = ConfigManager.Current;
            try
            {
                // Aplicar tema simples: altera BackColor
                if (cfg != null && cfg.Theme == "Escuro")
                {
                    this.BackColor = Color.FromArgb(45, 45, 48);
                    panel1.BackColor = Color.FromArgb(37, 37, 38);
                }
                else
                {
                    this.BackColor = Color.LightBlue;
                    panel1.BackColor = Color.WhiteSmoke;
                }

                // Aplicar fonte global do menu lateral (exemplo)
                if (cfg != null)
                {
                    try
                    {
                        var f = new Font(cfg.FontName, cfg.FontSize);
                        panel1.Font = f;
                        panel2.Font = f;
                    }
                    catch
                    {
                        // ignore if font invalid
                    }
                }
            }
            catch { }

            MostrarNotificacoesPendentes();
            panel_livros(new pesquisar_livros_rodrigo());
            AtualizarTituloCarrinhoMenu();
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
                        new NotificacoesUtilizador().Show();
                }
            }
            catch { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new Pesquisar_Livros().Show();
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
            new ReservasUtilizador().Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Perfil ().Show();
            this.Close();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
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
            Historico_de_Emprestimos historico = new Historico_de_Emprestimos();
            historico.Show();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Hstórico_de_compras historico = new Hstórico_de_compras();
            historico.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenUrl("https://siteptreadify.vercel.app/#sobre");
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            // Abre as configurações embutidas no panel2
            panel_livros(new Configuracoes());
        }

        // Adicione este método público à classe main_menu para aplicar tema/fonte de runtime.
        public void ApplyConfig(Config cfg)
        {
            if (cfg == null) return;

            if (cfg.Theme == "Escuro")
            {
                this.BackColor = Color.FromArgb(45, 45, 48);
                try { this.panel1.BackColor = Color.FromArgb(37, 37, 38); } catch { }
            }
            else
            {
                this.BackColor = Color.LightBlue;
                try { this.panel1.BackColor = Color.WhiteSmoke; } catch { }
            }

            try
            {
                var f1 = new Font(cfg.FontName, cfg.FontSize);
                this.panel1.Font = f1;
                this.panel2.Font = f1;
            }
            catch { /* ignora fontes inválidas */ }
        }
    }
}
