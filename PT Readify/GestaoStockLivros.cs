using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public class GestaoStockLivros : Form
    {
        private static readonly Color ColorTeal = Color.FromArgb(45, 139, 150);
        private static readonly Color ColorSky = Color.FromArgb(146, 201, 217);
        private static readonly Color ColorAmber = Color.FromArgb(242, 192, 105);
        private static readonly Color ColorBrown = Color.FromArgb(166, 131, 100);
        private static readonly Color ColorText = Color.FromArgb(45, 55, 60);

        private Panel pnlHeader;
        private Label lblHeaderTitle;
        private Panel pnlBody;
        private Panel pnlList;
        private TextBox txtPesquisa;
        private DataGridView gridLivros;
        private Panel pnlDetail;
        private Panel pnlCoverFrame;
        private PictureBox pictureCapa;
        private Label lblTitulo;
        private Label lblAutor;
        private Label lblStockAtual;
        private Label lblEstado;
        private Label lblPreco;
        private Label lblAddTitle;
        private NumericUpDown numQuantidade;
        private Button btnAdicionar;
        private Button btnAtualizar;
        private Panel pnlFooter;
        private Button btnFechar;

        private DataTable _todosLivros;
        private int _livroSelecionadoId = -1;

        public GestaoStockLivros()
        {
            BuildLayout();
            Load += GestaoStockLivros_Load;
        }

        private void BuildLayout()
        {
            Text = "Gestão de Stock";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(960, 580);
            ClientSize = new Size(1020, 620);
            BackColor = Color.FromArgb(240, 240, 240);

            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 58,
                BackColor = ColorTeal,
                Padding = new Padding(20, 0, 20, 0)
            };
            lblHeaderTitle = new Label
            {
                Text = "Gestão de Stock de Livros",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 14)
            };
            pnlHeader.Controls.Add(lblHeaderTitle);

            pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            pnlList = new Panel
            {
                Dock = DockStyle.Left,
                Width = 420,
                Padding = new Padding(0, 0, 12, 0),
                BackColor = Color.FromArgb(240, 240, 240)
            };

            txtPesquisa = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = ColorText
            };
            txtPesquisa.TextChanged += (s, e) => FiltrarLivros();

            gridLivros = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 36
            };
            gridLivros.ColumnHeadersDefaultCellStyle.BackColor = ColorTeal;
            gridLivros.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridLivros.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gridLivros.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            gridLivros.DefaultCellStyle.ForeColor = ColorText;
            gridLivros.DefaultCellStyle.SelectionBackColor = ColorSky;
            gridLivros.DefaultCellStyle.SelectionForeColor = ColorText;
            gridLivros.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 251);
            gridLivros.SelectionChanged += GridLivros_SelectionChanged;
            gridLivros.CellFormatting += GridLivros_CellFormatting;

            pnlList.Controls.Add(gridLivros);
            pnlList.Controls.Add(txtPesquisa);

            pnlDetail = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(16)
            };

            pnlCoverFrame = new Panel
            {
                BackColor = ColorSky,
                Location = new Point(16, 16),
                Size = new Size(180, 240),
                Padding = new Padding(6)
            };
            pictureCapa = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            pnlCoverFrame.Controls.Add(pictureCapa);

            lblTitulo = CreateDetailLabel(220, 20, new Font("Segoe UI", 14F, FontStyle.Bold), ColorTeal);
            lblAutor = CreateDetailLabel(220, 52, new Font("Segoe UI", 10F, FontStyle.Italic), Color.FromArgb(100, 100, 100));
            lblStockAtual = CreateDetailLabel(220, 90, new Font("Segoe UI", 12F, FontStyle.Bold), ColorText);
            lblEstado = CreateDetailLabel(220, 120, new Font("Segoe UI", 10F), ColorText);
            lblPreco = CreateDetailLabel(220, 148, new Font("Segoe UI", 10F), ColorText);

            lblAddTitle = new Label
            {
                Text = "Adicionar unidades ao stock:",
                Location = new Point(16, 280),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ColorTeal
            };

            numQuantidade = new NumericUpDown
            {
                Location = new Point(16, 310),
                Size = new Size(120, 28),
                Minimum = 1,
                Maximum = 9999,
                Value = 1,
                Font = new Font("Segoe UI", 11F)
            };

            btnAdicionar = CreateActionButton("Adicionar Stock", 150, 306, ColorAmber, Color.FromArgb(60, 45, 30));
            btnAdicionar.Click += BtnAdicionar_Click;

            btnAtualizar = CreateActionButton("Atualizar lista", 16, 360, ColorTeal, Color.White);
            btnAtualizar.Size = new Size(140, 34);
            btnAtualizar.Click += (s, e) => CarregarLivros(manterSelecao: true);

            pnlDetail.Controls.AddRange(new Control[]
            {
                pnlCoverFrame, lblTitulo, lblAutor, lblStockAtual, lblEstado, lblPreco,
                lblAddTitle, numQuantidade, btnAdicionar, btnAtualizar
            });

            pnlBody.Controls.Add(pnlDetail);
            pnlBody.Controls.Add(pnlList);

            pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 54,
                BackColor = ColorBrown,
                Padding = new Padding(12, 10, 12, 10)
            };
            btnFechar = CreateActionButton("Fechar", 0, 0, ColorAmber, Color.FromArgb(60, 45, 30));
            btnFechar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnFechar.Click += (s, e) => Close();
            pnlFooter.Controls.Add(btnFechar);
            pnlFooter.Resize += (s, e) =>
            {
                btnFechar.Location = new Point(
                    pnlFooter.ClientSize.Width - btnFechar.Width - 12,
                    (pnlFooter.ClientSize.Height - btnFechar.Height) / 2);
            };

            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
        }

        private Label CreateDetailLabel(int x, int y, Font font, Color color)
        {
            return new Label
            {
                Location = new Point(x, y),
                Size = new Size(520, 24),
                Font = font,
                ForeColor = color,
                AutoEllipsis = true
            };
        }

        private Button CreateActionButton(string text, int x, int y, Color back, Color fore)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(150, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = fore,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private void GestaoStockLivros_Load(object sender, EventArgs e)
        {
            CarregarLivros();
        }

        private void CarregarLivros(bool manterSelecao = false)
        {
            int idAnterior = _livroSelecionadoId;
            try
            {
                _todosLivros = BLL.Livros.LoadStockResumo();
                FiltrarLivros();

                if (manterSelecao && idAnterior > 0)
                {
                    foreach (DataGridViewRow row in gridLivros.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["Id_Livro"].Value) == idAnterior)
                        {
                            row.Selected = true;
                            return;
                        }
                    }
                }

                LimparDetalhe();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar livros: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarLivros()
        {
            if (_todosLivros == null) return;

            string filtro = txtPesquisa.Text.Trim().ToLowerInvariant();
            DataView view = _todosLivros.DefaultView;
            if (string.IsNullOrEmpty(filtro))
                view.RowFilter = "";
            else
                view.RowFilter = $"Titulo LIKE '%{EscapeFilter(filtro)}%' OR Autor LIKE '%{EscapeFilter(filtro)}%'";

            gridLivros.DataSource = view.ToTable();

            if (gridLivros.Columns.Contains("Id_Livro"))
                gridLivros.Columns["Id_Livro"].HeaderText = "ID";
            if (gridLivros.Columns.Contains("Titulo"))
                gridLivros.Columns["Titulo"].HeaderText = "Título";
            if (gridLivros.Columns.Contains("Autor"))
                gridLivros.Columns["Autor"].HeaderText = "Autor";
            if (gridLivros.Columns.Contains("Stock"))
            {
                gridLivros.Columns["Stock"].HeaderText = "Stock";
                gridLivros.Columns["Stock"].FillWeight = 60;
            }
            if (gridLivros.Columns.Contains("Estado_Livro"))
                gridLivros.Columns["Estado_Livro"].HeaderText = "Estado";
            if (gridLivros.Columns.Contains("Preço"))
                gridLivros.Columns["Preço"].HeaderText = "Preço (€)";
        }

        private static string EscapeFilter(string value)
        {
            return value.Replace("'", "''");
        }

        private void GridLivros_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridLivros.Columns[e.ColumnIndex].Name == "Preço" && e.Value != null && e.Value != DBNull.Value)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal cents))
                    e.Value = (cents / 100m).ToString("F2") + " €";
                e.FormattingApplied = true;
            }
        }

        private void GridLivros_SelectionChanged(object sender, EventArgs e)
        {
            if (gridLivros.CurrentRow == null) return;
            var row = gridLivros.CurrentRow;
            if (row.Cells["Id_Livro"].Value == null) return;

            _livroSelecionadoId = Convert.ToInt32(row.Cells["Id_Livro"].Value);
            MostrarDetalhe(_livroSelecionadoId);
        }

        private void MostrarDetalhe(int idLivro)
        {
            try
            {
                DataTable dt = BLL.Livros.Load();
                DataRow livro = dt.AsEnumerable()
                    .FirstOrDefault(r => Convert.ToInt32(r["Id_Livro"]) == idLivro);

                if (livro == null)
                {
                    LimparDetalhe();
                    return;
                }

                lblTitulo.Text = livro["Titulo"]?.ToString() ?? "—";
                lblAutor.Text = livro["Autor"]?.ToString() ?? "—";
                int stock = BLL.Livros.ObterStock(idLivro);
                lblStockAtual.Text = stock > 0
                    ? $"Stock atual: {stock} unidade(s)"
                    : "Stock atual: Esgotado (0)";
                lblStockAtual.ForeColor = stock > 0 ? Color.FromArgb(39, 120, 80) : Color.FromArgb(192, 57, 43);
                lblEstado.Text = "Estado: " + (livro["Estado_Livro"]?.ToString() ?? "—");

                if (livro.Table.Columns.Contains("Preço") && livro["Preço"] != DBNull.Value)
                {
                    decimal preco = Convert.ToDecimal(livro["Preço"]) / 100m;
                    lblPreco.Text = $"Preço: € {preco:F2}";
                }
                else
                {
                    lblPreco.Text = "Preço: —";
                }

                CarregarCapa(livro);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar detalhe: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarCapa(DataRow livro)
        {
            if (pictureCapa.Image != null)
            {
                pictureCapa.Image.Dispose();
                pictureCapa.Image = null;
            }

            if (livro.Table.Columns.Contains("Capa") && livro["Capa"] != DBNull.Value && livro["Capa"] != null)
            {
                try
                {
                    byte[] bytes = livro["Capa"] as byte[];
                    if (bytes != null && bytes.Length > 0)
                    {
                        using (var ms = new MemoryStream(bytes))
                        using (var img = Image.FromStream(ms))
                        {
                            pictureCapa.Image = new Bitmap(img);
                        }
                        return;
                    }
                }
                catch { }
            }

            pictureCapa.BackColor = Color.FromArgb(248, 248, 248);
        }

        private void LimparDetalhe()
        {
            _livroSelecionadoId = -1;
            lblTitulo.Text = "Selecione um livro";
            lblAutor.Text = "";
            lblStockAtual.Text = "";
            lblEstado.Text = "";
            lblPreco.Text = "";
            if (pictureCapa.Image != null)
            {
                pictureCapa.Image.Dispose();
                pictureCapa.Image = null;
            }
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            if (_livroSelecionadoId <= 0)
            {
                MessageBox.Show("Selecione um livro da lista.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int qty = (int)numQuantidade.Value;
            try
            {
                BLL.Livros.AdicionarStock(_livroSelecionadoId, qty);
                MessageBox.Show(
                    $"{qty} unidade(s) adicionada(s) com sucesso.\nNovo stock: {BLL.Livros.ObterStock(_livroSelecionadoId)}",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CarregarLivros(manterSelecao: true);
                MostrarDetalhe(_livroSelecionadoId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar stock: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // GestaoStockLivros
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "GestaoStockLivros";
            this.Load += new System.EventHandler(this.GestaoStockLivros_Load_1);
            this.ResumeLayout(false);

        }

        private void GestaoStockLivros_Load_1(object sender, EventArgs e)
        {

        }
    }
}
