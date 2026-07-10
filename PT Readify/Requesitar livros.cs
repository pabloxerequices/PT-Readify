using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public partial class Requesitar_livros : Form
    {
        private DataGridView gridLivros;
        private TextBox txtPesquisa;
        private NumericUpDown numDias;
        private DataTable todosLivros;
        private Config _config;

        public Requesitar_livros()
        {
            InitializeComponent();
            _config = ConfigManager.Current;
            BuildUi();
            Load += Requesitar_livros_Load;
        }

        private void BuildUi()
        {
            _config = ConfigManager.Current;
            ApplyConfig(_config);
            ApplyLanguage();
            Text = LanguageHelper.T("RequestBooksTitle", _config);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(950, 580);

            var lblTitulo = new Label
            {
                Text = LanguageHelper.T("RequestBooks", _config),
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            var lblDias = new Label
            {
                Text = LanguageHelper.T("DaysToReturn", _config),
                Location = new Point(20, 55),
                AutoSize = true
            };

            numDias = new NumericUpDown
            {
                Location = new Point(200, 53),
                Size = new Size(60, 23),
                Minimum = 0,
                Maximum = BLL.Historicos.MaxDiasEmprestimo,
                Value = 14
            };

            var lblAviso = new Label
            {
                Text = LanguageHelper.T("FineWarning", _config),
                Location = new Point(280, 55),
                Size = new Size(550, 20),
                ForeColor = Color.DimGray
            };

            txtPesquisa = new TextBox
            {
                Location = new Point(20, 85),
                Size = new Size(400, 23)
            };
            txtPesquisa.TextChanged += (s, e) => FiltrarLivros();

            gridLivros = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(900, 420),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            gridLivros.CellClick += GridLivros_CellClick;

            Controls.Add(lblTitulo);
            Controls.Add(lblDias);
            Controls.Add(numDias);
            Controls.Add(lblAviso);
            Controls.Add(txtPesquisa);
            Controls.Add(gridLivros);
        }

        private void Requesitar_livros_Load(object sender, EventArgs e)
        {
            _config = ConfigManager.Current;
            ApplyLanguage();

            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show(LanguageHelper.T("LoginToRequest", _config), LanguageHelper.T("ValidationWarning", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            if (BLL.Historicos.TemEmprestimoAtivo(globais.id_utilizador))
            {
                MessageBox.Show(
                    LanguageHelper.T("ActiveLoanWarning", _config),
                    LanguageHelper.T("ActiveLoan", _config),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            try
            {
                todosLivros = BLL.Livros.Load();
                ConfigurarGrid();
                FiltrarLivros();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(LanguageHelper.T("ErrorLoadingBooks", _config), ex.Message), LanguageHelper.T("Error", _config), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrid()
        {
            gridLivros.AutoGenerateColumns = false;
            gridLivros.Columns.Clear();

            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = LanguageHelper.T("ID", _config),
                DataPropertyName = "Id_Livro",
                FillWeight = 8
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTitulo",
                HeaderText = LanguageHelper.T("Title", _config),
                DataPropertyName = "Titulo",
                FillWeight = 30
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAutor",
                HeaderText = LanguageHelper.T("Author", _config),
                DataPropertyName = "Autor",
                FillWeight = 22
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStock",
                HeaderText = LanguageHelper.T("Stock", _config),
                DataPropertyName = "Stock",
                FillWeight = 10
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEstado",
                HeaderText = LanguageHelper.T("Status", _config),
                DataPropertyName = "Estado_Livro",
                FillWeight = 12
            });
            gridLivros.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colRequisitar",
                HeaderText = LanguageHelper.T("Loan", _config),
                Text = LanguageHelper.T("Request", _config),
                UseColumnTextForButtonValue = true,
                FillWeight = 12
            });
            gridLivros.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colReservar",
                HeaderText = LanguageHelper.T("Reservation", _config),
                Text = LanguageHelper.T("Reserve", _config),
                UseColumnTextForButtonValue = true,
                FillWeight = 12
            });
        }

        private void FiltrarLivros()
        {
            if (todosLivros == null)
                return;

            string filtro = txtPesquisa.Text.Trim().ToLowerInvariant();
            DataTable filtrado = todosLivros.Clone();

            foreach (DataRow row in todosLivros.Rows)
            {
                string titulo = row["Titulo"]?.ToString().ToLowerInvariant() ?? "";
                string autor = row["Autor"]?.ToString().ToLowerInvariant() ?? "";

                if (string.IsNullOrEmpty(filtro) || titulo.Contains(filtro) || autor.Contains(filtro))
                    filtrado.ImportRow(row);
            }

            gridLivros.DataSource = filtrado;
        }

        private void GridLivros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string coluna = gridLivros.Columns[e.ColumnIndex].Name;
            if (coluna != "colRequisitar" && coluna != "colReservar")
                return;

            try
            {
                int idLivro = Convert.ToInt32(gridLivros.Rows[e.RowIndex].Cells["colId"].Value);
                string titulo = gridLivros.Rows[e.RowIndex].Cells["colTitulo"].Value?.ToString() ?? "este livro";
                int stock = Convert.ToInt32(gridLivros.Rows[e.RowIndex].Cells["colStock"].Value ?? 0);

                if (coluna == "colRequisitar")
                {
                    if (stock <= 0)
                    {
                        MessageBox.Show(LanguageHelper.T("BookOutOfStock", _config), LanguageHelper.T("NoStock", _config), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int dias = (int)numDias.Value;
                    DialogResult confirmar = MessageBox.Show(
                        string.Format(LanguageHelper.T("RequestConfirm", _config), titulo, dias),
                        LanguageHelper.T("ConfirmRequest", _config),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmar != DialogResult.Yes)
                        return;

                    BLL.Historicos.RegistrarEmprestimo(globais.id_utilizador, idLivro, dias);

                    MessageBox.Show(
                        string.Format(LanguageHelper.T("RequestSuccess", _config), dias),
                        LanguageHelper.T("Success", _config),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    if (stock > 0)
                    {
                        MessageBox.Show(LanguageHelper.T("BookAvailable", _config), LanguageHelper.T("StockAvailable", _config), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    DialogResult confirmar = MessageBox.Show(
                        string.Format(LanguageHelper.T("ReserveConfirm", _config), titulo),
                        LanguageHelper.T("ConfirmReservation", _config),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmar != DialogResult.Yes)
                        return;

                    BLL.Historicos.RegistrarReserva(globais.id_utilizador, idLivro);

                    MessageBox.Show(
                        LanguageHelper.T("ReservationSuccess", _config),
                        LanguageHelper.T("Success", _config),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                todosLivros = BLL.Livros.Load();
                FiltrarLivros();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(LanguageHelper.T("ErrorRequesting", _config), ex.Message), LanguageHelper.T("Error", _config), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyLanguage()
        {
            if (_config == null) _config = ConfigManager.Current;
            this.Text = LanguageHelper.T("RequestBooksTitle", _config);
        }

        public void ApplyConfig(Config cfg)
        {
            if (cfg == null) return;
            ConfigApplier.ApplyFont(this, cfg);
        }
    }
}
