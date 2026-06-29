using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Detalhes_Livro : Form
    {
        private static readonly Color ColorBg = Color.FromArgb(240, 240, 240);
        private static readonly Color ColorTeal = Color.FromArgb(45, 139, 150);
        private static readonly Color ColorSky = Color.FromArgb(146, 201, 217);
        private static readonly Color ColorText = Color.FromArgb(45, 55, 60);

        private static readonly HashSet<string> HiddenColumns = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Capa", "CaminhoCapa", "CoverPath", "ImagemCapa", "Foto", "Imagem"
        };

        private static readonly Dictionary<string, string> FieldLabels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Id_Livro", "ID" },
            { "Titulo", "Título" },
            { "Autor", "Autor" },
            { "Editora", "Editora" },
            { "Quantas_Paginas", "Páginas" },
            { "Ano", "Ano" },
            { "Idioma", "Idioma" },
            { "Estado_Livro", "Estado" },
            { "Preço", "Preço" },
            { "Preco", "Preço" },
            { "Stock", "Stock" },
            { "Bio", "Descrição" },
            { "ISBN", "ISBN" }
        };

        private static readonly string[] PreferredOrder =
        {
            "Titulo", "Autor", "Editora", "Quantas_Paginas", "Ano", "Idioma",
            "Estado_Livro", "Stock", "Preço", "Preco", "ISBN", "Bio", "Id_Livro"
        };

        // --- VARIÁVEIS DE LIGAÇÃO COM CARTEIRA ---
        private string tituloLivro = string.Empty;
        private double precoLivro = 0.0;
        private Carteira carteiraAssociada = null;

        public Detalhes_Livro()
        {
            InitializeComponent();
        }

        public Detalhes_Livro(DataRow dados) : this()
        {
            PreencherDados(dados);
            FooterPanel_Resize(pnlFooter, EventArgs.Empty);
        }

        private void Detalhes_Livro_Load(object sender, EventArgs e)
        {
            ApplyUserSettings();
        }

        private void ApplyUserSettings()
        {
            var cfg = ConfigManager.Current;
            if (cfg == null) return;

            var readingFont = ConfigApplier.GetReadingFont(cfg);
            this.Font = readingFont;
            lblHeaderTitle.Font = new Font(readingFont.FontFamily, Math.Max(14, readingFont.Size), FontStyle.Bold);
            btnFechar.Font = new Font(readingFont.FontFamily, Math.Max(9, readingFont.Size - 2), FontStyle.Bold);
            btnFechar.Text = LanguageHelper.T("Close", cfg);
        }

        private void BtnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FooterPanel_Resize(object sender, EventArgs e)
        {
            if (btnFechar == null || pnlFooter == null) return;
            btnFechar.Location = new Point(
                pnlFooter.ClientSize.Width - btnFechar.Width - 12,
                (pnlFooter.ClientSize.Height - btnFechar.Height) / 2);
        }

        private void PreencherDados(DataRow row)
        {
            if (row == null) return;

            LoadCoverImage(row);

            string titulo = GetString(row, "Titulo");
            if (!string.IsNullOrWhiteSpace(titulo))
            {
                lblHeaderTitle.Text = titulo;
                Text = titulo;
            }

            flowDetails.SuspendLayout();
            flowDetails.Controls.Clear();

            var columns = row.Table.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .Where(name => !HiddenColumns.Contains(name)
                    && !name.Equals("Titulo", StringComparison.OrdinalIgnoreCase))
                .OrderBy(name =>
                {
                    int idx = Array.IndexOf(PreferredOrder, name);
                    return idx >= 0 ? idx : PreferredOrder.Length + name.GetHashCode();
                })
                .ToList();

            foreach (var colName in columns)
            {
                string value = FormatValue(row, colName);
                if (string.IsNullOrWhiteSpace(value)) continue;
                flowDetails.Controls.Add(CreateDetailRow(colName, value));
            }

            flowDetails.ResumeLayout(true);
        }

        private Panel CreateDetailRow(string columnName, string value)
        {
            var cfg = ConfigManager.Current;
            Font labelFont;
            Font valueFont;
            try
            {
                var baseFont = ConfigApplier.GetReadingFont(cfg);
                labelFont = new Font(baseFont.FontFamily, Math.Max(9, baseFont.Size - 2), FontStyle.Bold);
                valueFont = new Font(baseFont.FontFamily, Math.Max(9, baseFont.Size - 1), FontStyle.Regular);
            }
            catch
            {
                labelFont = new Font("Segoe UI", 10, FontStyle.Bold);
                valueFont = new Font("Segoe UI", 10, FontStyle.Regular);
            }

            string labelText = FieldLabels.TryGetValue(columnName, out var friendly)
                ? friendly
                : columnName.Replace("_", " ");

            var rowPanel = new Panel
            {
                Width = Math.Max(420, flowDetails.ClientSize.Width - 24),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.White
            };

            var lblName = new Label
            {
                Text = labelText,
                Font = labelFont,
                ForeColor = ColorTeal,
                AutoSize = false,
                Width = rowPanel.Width,
                Height = 22,
                Location = new Point(0, 0)
            };

            bool isLong = value.Length > 100 || value.Contains("\n");
            Control valueControl;

            if (isLong)
            {
                var tb = new TextBox
                {
                    Text = value,
                    ReadOnly = true,
                    Multiline = true,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.FromArgb(248, 250, 251),
                    ForeColor = ColorText,
                    Font = valueFont,
                    Width = rowPanel.Width - 16,
                    Height = Math.Min(160, 40 + value.Length / 3),
                    Location = new Point(0, 26),
                    ScrollBars = ScrollBars.Vertical
                };
                valueControl = tb;
            }
            else
            {
                var lblVal = new Label
                {
                    Text = value,
                    Font = valueFont,
                    ForeColor = ColorText,
                    AutoSize = false,
                    Width = rowPanel.Width,
                    MaximumSize = new Size(rowPanel.Width, 0),
                    Location = new Point(0, 26)
                };
                lblVal.Height = TextRenderer.MeasureText(value, valueFont, new Size(lblVal.Width, int.MaxValue),
                    TextFormatFlags.WordBreak).Height + 4;
                valueControl = lblVal;
            }

            var accent = new Panel
            {
                BackColor = ColorSky,
                Height = 2,
                Width = 48,
                Location = new Point(0, 22)
            };

            rowPanel.Controls.Add(lblName);
            rowPanel.Controls.Add(accent);
            rowPanel.Controls.Add(valueControl);

            int bottom = valueControl.Bottom + 8;
            rowPanel.Height = bottom;

            var separator = new Panel
            {
                BackColor = Color.FromArgb(230, 235, 238),
                Height = 1,
                Width = rowPanel.Width,
                Location = new Point(0, bottom - 1)
            };
            rowPanel.Controls.Add(separator);
            rowPanel.Height = bottom;

            return rowPanel;
        }

        private void LoadCoverImage(DataRow row)
        {
            pictureCapa.Image = null;

            if (row.Table.Columns.Contains("Capa") && row["Capa"] != DBNull.Value && row["Capa"] != null)
            {
                try
                {
                    byte[] imagemBytes = row["Capa"] as byte[];
                    if (imagemBytes != null && imagemBytes.Length > 0)
                    {
                        using (var ms = new MemoryStream(imagemBytes))
                        using (var img = Image.FromStream(ms))
                        {
                            pictureCapa.Image = new Bitmap(img);
                        }
                        return;
                    }
                }
                catch { }
            }

            string[] pathColumns = { "CaminhoCapa", "Capa", "CoverPath", "ImagemCapa" };
            foreach (var col in pathColumns)
            {
                if (!row.Table.Columns.Contains(col)) continue;
                var val = row[col];
                if (val == DBNull.Value || val == null) continue;

                var caminho = val.ToString();
                if (string.IsNullOrWhiteSpace(caminho) || caminho.Length > 260) continue;

                try
                {
                    if (File.Exists(caminho))
                    {
                        pictureCapa.Image = Image.FromFile(caminho);
                        return;
                    }
                }
                catch { }
            }

            pictureCapa.BackColor = Color.FromArgb(248, 248, 248);
        }

        private static string GetString(DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column)) return "";
            var val = row[column];
            return val == DBNull.Value || val == null ? "" : val.ToString();
        }

        private static string FormatValue(DataRow row, string column)
        {
            if (!row.Table.Columns.Contains(column)) return "";

            var val = row[column];
            if (val == DBNull.Value || val == null) return "";

            if (column.Equals("Preço", StringComparison.OrdinalIgnoreCase)
                || column.Equals("Preco", StringComparison.OrdinalIgnoreCase))
            {
                if (decimal.TryParse(val.ToString(), out decimal cents))
                    return $"€ {cents / 100m:F2}";
            }

            if (column.Equals("Capa", StringComparison.OrdinalIgnoreCase))
                return "";

            return val.ToString().Trim();
        }
    }
}
