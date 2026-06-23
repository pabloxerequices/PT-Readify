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

        public Requesitar_livros()
        {
            InitializeComponent();
            BuildUi();
            Load += Requesitar_livros_Load;
        }

        private void BuildUi()
        {
            Text = "Requisições / Empréstimos";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(950, 580);

            var lblTitulo = new Label
            {
                Text = "Requisitar livros",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            var lblDias = new Label
            {
                Text = "Dias para devolver (0-21):",
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
                Text = "Multa de 2€ por semana após o prazo. Apenas 1 empréstimo ativo por utilizador.",
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
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para requisitar livros.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            if (BLL.Historicos.TemEmprestimoAtivo(globais.id_utilizador))
            {
                MessageBox.Show(
                    "Já tem um empréstimo ativo. Devolva o livro no histórico de empréstimos antes de requisitar outro.",
                    "Empréstimo ativo",
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
                MessageBox.Show("Erro ao carregar livros: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarGrid()
        {
            gridLivros.AutoGenerateColumns = false;
            gridLivros.Columns.Clear();

            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                DataPropertyName = "Id_Livro",
                FillWeight = 8
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTitulo",
                HeaderText = "Título",
                DataPropertyName = "Titulo",
                FillWeight = 30
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAutor",
                HeaderText = "Autor",
                DataPropertyName = "Autor",
                FillWeight = 22
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStock",
                HeaderText = "Stock",
                DataPropertyName = "Stock",
                FillWeight = 10
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEstado",
                HeaderText = "Estado",
                DataPropertyName = "Estado_Livro",
                FillWeight = 12
            });
            gridLivros.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colRequisitar",
                HeaderText = "Empréstimo",
                Text = "Requisitar",
                UseColumnTextForButtonValue = true,
                FillWeight = 12
            });
            gridLivros.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colReservar",
                HeaderText = "Reserva",
                Text = "Reservar",
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
                        MessageBox.Show("Este livro está esgotado. Use a opção Reservar.", "Sem stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int dias = (int)numDias.Value;
                    DialogResult confirmar = MessageBox.Show(
                        $"Deseja requisitar \"{titulo}\"?\n\nPrazo de devolução: {dias} dias.\nMulta de 2€ por semana após o prazo.",
                        "Confirmar requisição",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmar != DialogResult.Yes)
                        return;

                    BLL.Historicos.RegistrarEmprestimo(globais.id_utilizador, idLivro, dias);

                    MessageBox.Show(
                        $"Livro requisitado com sucesso!\nDevolução prevista em {dias} dias.\n\nConsulte o histórico em \"Histórico de Empréstimos\".",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    if (stock > 0)
                    {
                        MessageBox.Show("Este livro está disponível. Use Requisitar em vez de Reservar.", "Stock disponível", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    DialogResult confirmar = MessageBox.Show(
                        $"Deseja reservar \"{titulo}\"?\n\nSerá notificado quando o livro estiver disponível.",
                        "Confirmar reserva",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmar != DialogResult.Yes)
                        return;

                    BLL.Historicos.RegistrarReserva(globais.id_utilizador, idLivro);

                    MessageBox.Show(
                        "Reserva efetuada com sucesso!\nSerá notificado quando o livro estiver em stock.",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                todosLivros = BLL.Livros.Load();
                FiltrarLivros();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
