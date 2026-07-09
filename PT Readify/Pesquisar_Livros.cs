using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public partial class Pesquisar_Livros : Form
    {
        private DataTable livrosTable;
        public int LivroSelecionado { get; set; }
        private FlowLayoutPanel flowPanel;
        private int quantidadeCarrinho = 0;

        public Pesquisar_Livros()
        {
            InitializeComponent();
        }

        public Label LabelCarrinho { get; set; }

        private void Pesquisar_Livros_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            InicializarControles();
            CriarInterfaceVisual();
        }

        private void CriarInterfaceVisual()
        {
            this.Controls.Clear();

            // Panel Superior com Filtros
            Panel panelFiltros = new Panel();
            panelFiltros.BackColor = Color.FromArgb(33, 41, 52);
            panelFiltros.Dock = DockStyle.Top;
            panelFiltros.Height = 100;
            panelFiltros.Padding = new Padding(10);

            Label lblTitulo = new Label();
            lblTitulo.Text = "Título:";
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(10, 10);
            lblTitulo.AutoSize = true;

            TextBox txtTitulo = new TextBox();
            txtTitulo.Name = "txtTitulo";
            txtTitulo.Location = new Point(60, 10);
            txtTitulo.Width = 200;
            txtTitulo.TextChanged += (s, e) => AplicarFiltro();

            Label lblAutor = new Label();
            lblAutor.Text = "Autor:";
            lblAutor.ForeColor = Color.White;
            lblAutor.Location = new Point(280, 10);
            lblAutor.AutoSize = true;

            TextBox txtAutor = new TextBox();
            txtAutor.Name = "txtAutor";
            txtAutor.Location = new Point(330, 10);
            txtAutor.Width = 200;
            txtAutor.TextChanged += (s, e) => AplicarFiltro();

            Label lblEstado = new Label();
            lblEstado.Text = "Estado:";
            lblEstado.ForeColor = Color.White;
            lblEstado.Location = new Point(10, 40);
            lblEstado.AutoSize = true;

            ComboBox comboEstado = new ComboBox();
            comboEstado.Name = "comboEstado";
            comboEstado.Location = new Point(60, 40);
            comboEstado.Width = 200;
            comboEstado.SelectedIndexChanged += (s, e) => AplicarFiltro();

            try
            {
                comboEstado.Items.Add("Todos");
                var estados = BLL.Livros.ObterEstados();
                foreach (var est in estados)
                    comboEstado.Items.Add(est);
                comboEstado.SelectedIndex = 0;
            }
            catch { }

            Label lblGenero = new Label();
            lblGenero.Text = "Gênero:";
            lblGenero.ForeColor = Color.White;
            lblGenero.Location = new Point(280, 40);
            lblGenero.AutoSize = true;

            ComboBox comboGenero = new ComboBox();
            comboGenero.Name = "comboGenero";
            comboGenero.Location = new Point(330, 40);
            comboGenero.Width = 200;
            comboGenero.SelectedIndexChanged += (s, e) => AplicarFiltro();

            try
            {
                comboGenero.Items.Add("Todos");
                var generos = BLL.Livros.ObterGeneros();
                foreach (var g in generos)
                    comboGenero.Items.Add(g);
                comboGenero.SelectedIndex = 0;
            }
            catch { }

            panelFiltros.Controls.Add(lblTitulo);
            panelFiltros.Controls.Add(txtTitulo);
            panelFiltros.Controls.Add(lblAutor);
            panelFiltros.Controls.Add(txtAutor);
            panelFiltros.Controls.Add(lblEstado);
            panelFiltros.Controls.Add(comboEstado);
            panelFiltros.Controls.Add(lblGenero);
            panelFiltros.Controls.Add(comboGenero);

            this.Controls.Add(panelFiltros);

            // FlowLayoutPanel para Cards
            flowPanel = new FlowLayoutPanel();
            flowPanel.Name = "flowPanel";
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.BackColor = Color.FromArgb(240, 242, 245);
            flowPanel.AutoScroll = true;
            flowPanel.Padding = new Padding(10);

            this.Controls.Add(flowPanel);

            AplicarFiltro();
        }

        private void InicializarControles()
        {
            try
            {
                var generos = BLL.Livros.ObterGeneros();
                var estados = BLL.Livros.ObterEstados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }

        private void AplicarFiltro()
        {
            try
            {
                string titulo = this.Controls.Find("txtTitulo", true).FirstOrDefault()?.Text ?? "";
                string autor = this.Controls.Find("txtAutor", true).FirstOrDefault()?.Text ?? "";
                string estado = (this.Controls.Find("comboEstado", true).FirstOrDefault() as ComboBox)?.SelectedItem?.ToString() ?? "Todos";
                string genero = (this.Controls.Find("comboGenero", true).FirstOrDefault() as ComboBox)?.SelectedItem?.ToString() ?? "Todos";

                string generoParam = (genero == "Todos" || string.IsNullOrEmpty(genero)) ? null : genero;
                string estadoParam = (estado == "Todos" || string.IsNullOrEmpty(estado)) ? null : estado;

                livrosTable = BLL.Livros.pesquisarLivro(titulo, autor, generoParam, estadoParam);

                AtualizarCards();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao filtrar: " + ex.Message);
            }
        }

        private void AtualizarCards()
        {
            if (flowPanel == null || livrosTable == null) return;

            flowPanel.Controls.Clear();

            foreach (DataRow row in livrosTable.Rows)
            {
                Panel card = CriarCard(row);
                flowPanel.Controls.Add(card);
            }
        }

        private Panel CriarCard(DataRow row)
        {
            Panel card = new Panel();
            card.Width = 200;
            card.Height = 320;
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Margin = new Padding(10);
            card.Cursor = Cursors.Hand;

            // PictureBox para Capa
            PictureBox pic = new PictureBox();
            pic.Width = 180;
            pic.Height = 200;
            pic.Location = new Point(10, 10);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.BackColor = Color.LightGray;
            pic.Cursor = Cursors.Hand;
            
            try
            {
                if (row["Capa"] != DBNull.Value && row["Capa"] != null)
                {
                    byte[] imageData = (byte[])row["Capa"];
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image img = Image.FromStream(ms);
                        pic.Image = new Bitmap(img);
                    }
                }
                else
                {
                    Bitmap defaultImg = new Bitmap(180, 200);
                    using (Graphics g = Graphics.FromImage(defaultImg))
                    {
                        g.Clear(Color.LightGray);
                        g.DrawString("Sem Capa", new Font("Arial", 12), Brushes.Gray, 40, 90);
                    }
                    pic.Image = defaultImg;
                }
            }
            catch
            {
                Bitmap errorImg = new Bitmap(180, 200);
                using (Graphics g = Graphics.FromImage(errorImg))
                {
                    g.Clear(Color.LightGray);
                    g.DrawString("Erro Capa", new Font("Arial", 10), Brushes.Red, 50, 90);
                }
                pic.Image = errorImg;
            }

            // Label Título
            Label lblTitulo = new Label();
            lblTitulo.Text = row["Titulo"].ToString();
            lblTitulo.Width = 180;
            lblTitulo.Height = 40;
            lblTitulo.Location = new Point(10, 215);
            lblTitulo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblTitulo.AutoSize = false;
            lblTitulo.TextAlign = ContentAlignment.TopLeft;
            lblTitulo.Cursor = Cursors.Hand;

            // Label Preço
            Label lblPreco = new Label();
            decimal preco = Convert.ToDecimal(row["Preço"]) / 100;
            lblPreco.Text = preco.ToString("C2");
            lblPreco.Width = 180;
            lblPreco.Height = 30;
            lblPreco.Location = new Point(10, 260);
            lblPreco.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblPreco.ForeColor = Color.FromArgb(46, 204, 113);
            lblPreco.TextAlign = ContentAlignment.MiddleLeft;
            lblPreco.Cursor = Cursors.Hand;

            card.Controls.Add(pic);
            card.Controls.Add(lblTitulo);
            card.Controls.Add(lblPreco);

            int idLivro = Convert.ToInt32(row["Id_Livro"]);

            // Evento de clique simples - mostra detalhes
            card.Click += (s, e) => MostrarDetalhes(row);
            pic.Click += (s, e) => MostrarDetalhes(row);
            lblTitulo.Click += (s, e) => MostrarDetalhes(row);
            lblPreco.Click += (s, e) => MostrarDetalhes(row);

            // Evento de duplo clique - adiciona ao carrinho
            card.DoubleClick += (s, e) => AdicionarAoCarrinho(idLivro);
            pic.DoubleClick += (s, e) => AdicionarAoCarrinho(idLivro);
            lblTitulo.DoubleClick += (s, e) => AdicionarAoCarrinho(idLivro);
            lblPreco.DoubleClick += (s, e) => AdicionarAoCarrinho(idLivro);

            return card;
        }

        private void MostrarDetalhes(DataRow row)
        {
            Form detalhes = new Form();
            detalhes.Text = "Detalhes do Livro";
            detalhes.Width = 600;
            detalhes.Height = 500;
            detalhes.StartPosition = FormStartPosition.CenterParent;
            detalhes.BackColor = Color.FromArgb(240, 242, 245);

            Panel panelDetalhes = new Panel();
            panelDetalhes.Dock = DockStyle.Fill;
            panelDetalhes.BackColor = Color.White;
            panelDetalhes.Padding = new Padding(20);
            panelDetalhes.AutoScroll = true;

            PictureBox pic = new PictureBox();
            pic.Width = 150;
            pic.Height = 200;
            pic.Location = new Point(20, 20);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;

            try
            {
                if (row["Capa"] != DBNull.Value && row["Capa"] != null)
                {
                    byte[] imageData = (byte[])row["Capa"];
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image img = Image.FromStream(ms);
                        pic.Image = new Bitmap(img);
                    }
                }
            }
            catch { }

            int yPos = 20;
            foreach (DataColumn col in row.Table.Columns)
            {
                if (col.ColumnName == "Capa") continue;

                Label lbl = new Label();
                string texto = col.ColumnName + ": " + row[col].ToString();
                
                if (texto.Length > 60)
                {
                    lbl.Text = texto;
                    lbl.Location = new Point(180, yPos);
                    lbl.Width = 380;
                    lbl.Height = 60;
                    lbl.AutoSize = false;
                }
                else
                {
                    lbl.Text = texto;
                    lbl.Location = new Point(180, yPos);
                    lbl.AutoSize = true;
                }

                panelDetalhes.Controls.Add(lbl);
                yPos += 35;
            }

            Button btnAdicionar = new Button();
            btnAdicionar.Text = "✓ Adicionar ao Carrinho";
            btnAdicionar.Location = new Point(200, yPos + 20);
            btnAdicionar.Width = 200;
            btnAdicionar.Height = 40;
            btnAdicionar.BackColor = Color.FromArgb(46, 204, 113);
            btnAdicionar.ForeColor = Color.White;
            btnAdicionar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdicionar.Click += (s, e) =>
            {
                AdicionarAoCarrinho(Convert.ToInt32(row["Id_Livro"]));
                detalhes.Close();
            };

            panelDetalhes.Controls.Add(pic);
            panelDetalhes.Controls.Add(btnAdicionar);

            detalhes.Controls.Add(panelDetalhes);
            FormLaunchHelper.ShowDialog(detalhes, this);
        }

        private void AdicionarAoCarrinho(int idLivro)
        {
            try
            {
                CarrinhoService.AdicionarLivro(idLivro);
                MessageBox.Show("Livro adicionado ao carrinho!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        // Métodos de handler obrigatórios do Designer
        private void Filtro_Changed(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void clbCategoria_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            AplicarFiltro();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            comboEstado.SelectedIndex = 0;
            
            for (int i = 0; i < clbCategoria.Items.Count; i++)
            {
                clbCategoria.SetItemChecked(i, false);
            }
            
            AplicarFiltro();
        }

        private void dataGridViewLivros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            
            if (dataGridViewLivros.Rows[e.RowIndex].DataBoundItem is DataRowView rowView)
            {
                MostrarDetalhes(rowView.Row);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Implementar ação do botão de imagem se necessário
        }
    }
}
