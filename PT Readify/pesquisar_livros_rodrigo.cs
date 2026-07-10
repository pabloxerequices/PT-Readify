using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public partial class pesquisar_livros_rodrigo : Form
    {
        private DataTable todosLivros;
        private Dictionary<int, HashSet<string>> generosPorLivro = new Dictionary<int, HashSet<string>>();
        private bool atualizandoCombos;

        public pesquisar_livros_rodrigo()
        {
            InitializeComponent();
        }

        private void pesquisar_livros_rodrigo_Load(object sender, EventArgs e)
        {
            CarregarTodosLivros();
            ConfigurarEventos();
            CarrinhoService.CarrinhoAlterado += OnCarrinhoAlterado;
            AtualizarContadorCarrinho();
        }

        private void OnCarrinhoAlterado()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnCarrinhoAlterado));
                return;
            }
            AtualizarContadorCarrinho();
        }

        public void RecarregarLivros()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RecarregarLivros));
                return;
            }
            CarregarTodosLivros();
            AtualizarContadorCarrinho();
        }

        private void CarregarTodosLivros()
        {
            try
            {
                todosLivros = BLL.Livros.Load();

                // Preencher ComboBox de Idiomas
                var idiomas = new HashSet<string> { "Todos" };
                var generos = new HashSet<string> { "Todos" };
                
                if (todosLivros != null)
                {
                    foreach (DataRow row in todosLivros.Rows)
                    {
                        string idioma = row["Idioma"]?.ToString();
                        if (!string.IsNullOrEmpty(idioma))
                        {
                            idiomas.Add(idioma);
                        }
                    }
                }

                // Obter gêneros da tabela Genero
                List<string> generosLista = BLL.Livros.ObterGeneros();
                foreach (var genero in generosLista)
                {
                    if (!string.IsNullOrEmpty(genero))
                    {
                        generos.Add(genero);
                    }
                }

                combobox1.Items.Clear();
                combobox1.Items.Add("Todos");
                foreach (var idioma in idiomas.Where(i => i != "Todos").OrderBy(x => x))
                    combobox1.Items.Add(idioma);

                comboBox2.Items.Clear();
                comboBox2.Items.Add("Todos");
                foreach (var genero in generos.Where(g => g != "Todos").OrderBy(x => x))
                    comboBox2.Items.Add(genero);

                atualizandoCombos = true;
                try
                {
                    combobox1.SelectedIndex = 0;
                    comboBox2.SelectedIndex = 0;
                }
                finally
                {
                    atualizandoCombos = false;
                }

                CarregarGenerosPorLivro();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar livros: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarGenerosPorLivro()
        {
            generosPorLivro.Clear();
            if (todosLivros == null)
                return;

            foreach (DataRow row in todosLivros.Rows)
            {
                int idLivro = Convert.ToInt32(row["Id_Livro"]);
                try
                {
                    List<string> generosLivro = BLL.Livros.ObterGenerosLivro(idLivro);
                    generosPorLivro[idLivro] = new HashSet<string>(generosLivro, StringComparer.OrdinalIgnoreCase);
                }
                catch
                {
                    generosPorLivro[idLivro] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                }
            }
        }

        private void ConfigurarEventos()
        {
            textbox1.TextChanged += (s, e) => AplicarFiltros();
            textbox2.TextChanged += (s, e) => AplicarFiltros();
            textbox3.TextChanged += (s, e) => AplicarFiltros();
            textbox4.TextChanged += (s, e) => AplicarFiltros();
            textbox5.TextChanged += (s, e) => AplicarFiltros();
            combobox1.SelectedIndexChanged += (s, e) => AplicarFiltros();
            comboBox2.SelectedIndexChanged += (s, e) => AplicarFiltros();

            button1.Click += (s, e) => LimparFiltros();
            button2.Click += (s, e) => AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            if (todosLivros == null || atualizandoCombos)
                return;

            string titulo = textbox1.Text.ToLower().Trim();
            string editora = textbox2.Text.ToLower().Trim();
            string autor = textbox3.Text.ToLower().Trim();
            string idioma = combobox1.SelectedItem?.ToString() ?? "Todos";
            string generoSelecionado = comboBox2.SelectedItem?.ToString() ?? "Todos";
            bool precoMinValido = decimal.TryParse(textbox4.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal precoMin);
            bool precoMaxValido = decimal.TryParse(textbox5.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal precoMax);

            if (!precoMinValido) precoMin = 0;
            if (!precoMaxValido) precoMax = 1000000;

            DataTable filtrado = todosLivros.Clone();

            foreach (DataRow row in todosLivros.Rows)
            {
                string rowTitulo = row["Titulo"]?.ToString().ToLower() ?? "";
                string rowEditora = row["Editora"]?.ToString().ToLower() ?? "";
                string rowAutor = row["Autor"]?.ToString().ToLower() ?? "";
                string rowIdioma = row["Idioma"]?.ToString() ?? "";
                decimal rowPreco = Convert.ToDecimal(row["Preço"] ?? 0) / 100m;
                int idLivro = Convert.ToInt32(row["Id_Livro"]);

                bool cumpreTitulo = string.IsNullOrEmpty(titulo) || rowTitulo.Contains(titulo);
                bool cumpreEditora = string.IsNullOrEmpty(editora) || rowEditora.Contains(editora);
                bool cumpreAutor = string.IsNullOrEmpty(autor) || rowAutor.Contains(autor);
                bool cumpreIdioma = idioma == "Todos" || string.Equals(rowIdioma, idioma, StringComparison.OrdinalIgnoreCase);
                bool cumprePreco = rowPreco >= precoMin && rowPreco <= precoMax;

                bool cumpreGenero = generoSelecionado == "Todos" || VerificaGeneroLivro(idLivro, generoSelecionado);

                if (cumpreTitulo && cumpreEditora && cumpreAutor && cumpreIdioma && cumpreGenero && cumprePreco)
                {
                    filtrado.ImportRow(row);
                }
            }

            ExibirLivros(filtrado);
        }

        private bool VerificaGeneroLivro(int idLivro, string genero)
        {
            if (generosPorLivro.TryGetValue(idLivro, out HashSet<string> generosLivro))
                return generosLivro.Contains(genero);

            return false;
        }

        private void LimparFiltros()
        {
            textbox1.Text = "";
            textbox2.Text = "";
            textbox3.Text = "";
            textbox4.Text = "0";
            textbox5.Text = "1000";

            atualizandoCombos = true;
            try
            {
                combobox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
            }
            finally
            {
                atualizandoCombos = false;
            }

            AplicarFiltros();
        }

        private void ExibirLivros(DataTable livros)
        {
            flowLayoutPanel1.Controls.Clear();

            if (livros == null || livros.Rows.Count == 0)
            {
                Label lblVazio = new Label
                {
                    Text = "Nenhum livro encontrado",
                    Font = new Font("Segoe UI", 14, FontStyle.Regular),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    AutoSize = true,
                    Padding = new Padding(20)
                };
                flowLayoutPanel1.Controls.Add(lblVazio);
                return;
            }

            foreach (DataRow row in livros.Rows)
            {
                Panel card = CriarCardLivro(row);
                flowLayoutPanel1.Controls.Add(card);
            }
        }

        private Panel CriarCardLivro(DataRow livro)
        {
            var cfg = ConfigManager.Current;
            Font cardFont;
            try
            {
                cardFont = new Font(cfg?.FontName ?? "Segoe UI", Math.Max(8, Math.Min(24, cfg?.FontSize ?? 15)));
            }
            catch
            {
                cardFont = new Font("Segoe UI", 9);
            }

            int idLivro = Convert.ToInt32(livro["Id_Livro"]);
            string titulo = livro["Titulo"]?.ToString() ?? "Sem título";
            string autor = livro["Autor"]?.ToString() ?? "Autor desconhecido";
            string editora = livro["Editora"]?.ToString() ?? "Editora desconhecida"; // <-- Adicionado aqui
            decimal preco = Convert.ToDecimal(livro["Preço"] ?? 0) / 100m;
            int stock = livro.Table.Columns.Contains("Stock") ? Convert.ToInt32(livro["Stock"] ?? 0) : BLL.Livros.ObterStock(idLivro);
            object capaObj = livro["Capa"];

            Panel card = new Panel
            {
                Size = new Size(200, stock > 0 ? 410 : 420),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };

            // Desenhar borda do card
            card.Paint += (s, e) =>
            {
                e.Graphics.Clear(Color.White);
                using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            // PictureBox para a capa
            PictureBox picCapa = new PictureBox
            {
                Size = new Size(180, 240),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand,
                Tag = livro
            };

            if (capaObj != null && capaObj != System.DBNull.Value)
            {
                try
                {
                    byte[] imagemBytes = (byte[])capaObj;
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imagemBytes))
                    {
                        picCapa.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    picCapa.BackColor = Color.FromArgb(220, 220, 220);
                }
            }

            picCapa.Click += (s, e) => MostrarDetalhesLivro(idLivro, livro);
            card.Controls.Add(picCapa);

            // Label Título
            Label lblTitulo = new Label
            {
                Location = new Point(10, 260),
                Size = new Size(180, 30),
                Text = titulo,
                Font = new Font(cardFont.FontFamily, cardFont.Size, FontStyle.Bold),
                AutoEllipsis = true,
                ForeColor = Color.FromArgb(50, 50, 50)
            };
            card.Controls.Add(lblTitulo);

            // Label Preço
            Label lblPreco = new Label
            {
                Location = new Point(10, 300),
                Size = new Size(180, 25),
                Text = $"€ {preco:F2}",
                Font = new Font(cardFont.FontFamily, cardFont.Size + 1, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 168, 83)
            };
            card.Controls.Add(lblPreco);

            Label lblStock = new Label
            {
                Location = new Point(10, 322),
                Size = new Size(180, 18),
                Text = stock > 0 ? $"Stock: {stock}" : "Esgotado — reservar",
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = stock > 0 ? Color.FromArgb(52, 168, 83) : Color.FromArgb(231, 76, 60)
            };
            card.Controls.Add(lblStock);

            if (stock > 0)
            {
                Button btnAdicionar = new Button
                {
                    Location = new Point(10, 345),
                    Size = new Size(180, 28),
                    Text = "+ Carrinho (comprar)",
                    Font = new Font("Segoe UI", 9, FontStyle.Regular),
                    BackColor = Color.FromArgb(52, 168, 83),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Tag = idLivro
                };
                btnAdicionar.Click += (s, e) => AdicionarAoCarrinho(idLivro, titulo, autor, editora, preco);
                card.Controls.Add(btnAdicionar);

                Button btnEmprestimo = new Button
                {
                    Location = new Point(10, 375),
                    Size = new Size(180, 28),
                    Text = "Requesitar",
                    Font = new Font("Segoe UI", 8, FontStyle.Regular),
                    BackColor = Color.FromArgb(155, 89, 182),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnEmprestimo.Click += (s, e) => IrParaEmprestimos();
                card.Controls.Add(btnEmprestimo);
            }
            else
            {
                Label lblEsgotado = new Label
                {
                    Location = new Point(10, 345),
                    Size = new Size(180, 36),
                    Text = "Sem stock para compra.\nUse Empréstimos para reservar.",
                    Font = new Font("Segoe UI", 8, FontStyle.Italic),
                    ForeColor = Color.FromArgb(231, 76, 60)
                };
                card.Controls.Add(lblEsgotado);

                Button btnEmprestimo = new Button
                {
                    Location = new Point(10, 385),
                    Size = new Size(180, 28),
                    Text = "Reservar",
                    Font = new Font("Segoe UI", 8, FontStyle.Regular),
                    BackColor = Color.FromArgb(155, 89, 182),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                btnEmprestimo.Click += (s, e) => IrParaEmprestimos();
                card.Controls.Add(btnEmprestimo);
            }

            return card;
        }

        private void MostrarDetalhesLivro(int idLivro, DataRow livro)
        {
            var detalhes = new Detalhes_Livro(livro);
            FormLaunchHelper.ShowDialog(detalhes, this);
        }

        private void AdicionarAoCarrinho(int idLivro, string titulo, string autor, string editora, decimal preco)
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para adicionar livros ao carrinho.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                CarrinhoService.AdicionarLivro(idLivro, titulo, autor, editora, preco);
                AtualizarContadorCarrinho();
                NotificarMenuPrincipal();

                MessageBox.Show(
                    $"'{titulo}' adicionado ao carrinho!\nPreço: {preco:C2}\n\nO carrinho é apenas para compras.",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Não foi possível adicionar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void NotificarMenuPrincipal()
        {
            Form menu = FindForm();
            if (menu is main_menu mainMenu)
                mainMenu.AtualizarTituloCarrinhoMenu();
        }

        private void IrParaEmprestimos()
        {
            Form menu = FindForm();
            if (menu is main_menu mainMenu)
            {
                mainMenu.AbrirEmprestimos();
                return;
            }
            FormLaunchHelper.Show(new Requesitar_livros(), FindForm());
        }

        private void AtualizarContadorCarrinho()
        {
            int total = CarrinhoService.TotalItens;
            button3.Text = total > 0 ? $"🛒 Carrinho ({total})" : "🛒 Carrinho";
        }

        private void AbrirCarrinho()
        {
            Form menu = FindForm();
            if (menu is main_menu mainMenu)
            {
                mainMenu.AbrirCarrinhoIntegrado();
                return;
            }

            using (var carrinho = new Carrinho())
            {
                FormLaunchHelper.ShowDialog(carrinho, FindForm() ?? this);
            }
            RecarregarLivros();
        }

        private void textbox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirCarrinho();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            CarregarTodosLivros();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //refresh dos livros
                AplicarFiltros();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LimparFiltros();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CarrinhoService.CarrinhoAlterado -= OnCarrinhoAlterado;
            base.OnFormClosed(e);
        }
    }
}
