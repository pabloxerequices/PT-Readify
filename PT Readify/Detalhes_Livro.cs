using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Detalhes_Livro : Form
    {
        public Detalhes_Livro(DataRow dados)
        {
            InitializeComponent();
            this.Text = "Detalhes do Livro";
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(640, 360);
            PreencherDados(dados);

            // Garantir posição correta do botão na carga inicial
            BottomPanel_Resize(this.bottomPanel, EventArgs.Empty);
        }

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Handler nomeado usado pelo Designer
        private void BottomPanel_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.btnFechar != null && this.bottomPanel != null)
                {
                    this.btnFechar.Location = new Point(this.bottomPanel.ClientSize.Width - this.btnFechar.Width - 12, 8);
                }
            }
            catch
            {
                // Não lançar no Designer; falhas de layout silenciosas são melhores aqui
            }
        }

        private void PreencherDados(DataRow row)
        {
            if (row == null) return;

            // Tentar carregar capa por nomes comuns de coluna
            string[] possiveisNomesCapa = { "CaminhoCapa", "Capa", "CoverPath", "ImagemCapa" };
            foreach (var nome in possiveisNomesCapa)
            {
                if (row.Table.Columns.Contains(nome))
                {
                    var val = row[nome];
                    if (val != DBNull.Value && val != null)
                    {
                        var caminho = val.ToString();
                        try
                        {
                            if (File.Exists(caminho))
                            {
                                if (pictureCapa.Image != null)
                                {
                                    pictureCapa.Image.Dispose();
                                    pictureCapa.Image = null;
                                }
                                pictureCapa.Image = Image.FromFile(caminho);
                                break;
                            }
                        }
                        catch
                        {
                            // ignora falha ao carregar imagem
                        }
                    }
                }
            }

            // Montar linhas com todas as colunas do DataRow
            table.SuspendLayout();
            table.Controls.Clear();
            table.RowStyles.Clear();
            table.RowCount = 0;

            foreach (DataColumn col in row.Table.Columns)
            {
                var nomeCol = col.ColumnName;
                var valorObj = row[col] ?? DBNull.Value;
                string valorTexto = valorObj == DBNull.Value ? string.Empty : valorObj.ToString();

                // Label de nome
                var lblNome = new Label
                {
                    Text = nomeCol,
                    AutoSize = true,
                    Font = new Font(FontFamily.GenericSansSerif, 9.0f, FontStyle.Bold),
                    Margin = new Padding(6),
                    Anchor = AnchorStyles.Left | AnchorStyles.Top
                };

                // Control de valor — usar TextBox multiline para textos longos
                Control valorControl;
                if (valorTexto.Length > 120 || valorTexto.Contains("\n"))
                {
                    var tb = new TextBox
                    {
                        Multiline = true,
                        ReadOnly = true,
                        ScrollBars = ScrollBars.Vertical,
                        Text = valorTexto,
                        Dock = DockStyle.Fill,
                        BackColor = SystemColors.Window,
                        BorderStyle = BorderStyle.None,
                        Margin = new Padding(6)
                    };
                    tb.Height = 120;
                    valorControl = tb;
                }
                else
                {
                    var lblVal = new Label
                    {
                        Text = valorTexto,
                        AutoSize = true,
                        Margin = new Padding(6),
                        Anchor = AnchorStyles.Left | AnchorStyles.Top
                    };
                    valorControl = lblVal;
                }

                // adicionar linha
                table.RowCount += 1;
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                table.Controls.Add(lblNome, 0, table.RowCount - 1);
                table.Controls.Add(valorControl, 1, table.RowCount - 1);
            }

            table.ResumeLayout();
        }
    }
}
