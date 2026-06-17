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
            ClientSize = new Size(900, 550);

            var lblTitulo = new Label
            {
                Text = "Requisitar livros",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            txtPesquisa = new TextBox
            {
                Location = new Point(20, 55),
                Size = new Size(400, 23)
            };
            txtPesquisa.TextChanged += (s, e) => FiltrarLivros();

            gridLivros = new DataGridView
            {
                Location = new Point(20, 90),
                Size = new Size(840, 420),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            gridLivros.CellClick += GridLivros_CellClick;

            Controls.Add(lblTitulo);
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
                FillWeight = 10
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTitulo",
                HeaderText = "Título",
                DataPropertyName = "Titulo",
                FillWeight = 35
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAutor",
                HeaderText = "Autor",
                DataPropertyName = "Autor",
                FillWeight = 25
            });
            gridLivros.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colEstado",
                HeaderText = "Estado",
                DataPropertyName = "Estado_Livro",
                FillWeight = 15
            });
            gridLivros.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colRequisitar",
                HeaderText = "Ação",
                Text = "Requisitar",
                UseColumnTextForButtonValue = true,
                FillWeight = 15
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
            if (e.RowIndex < 0 || gridLivros.Columns[e.ColumnIndex].Name != "colRequisitar")
                return;

            try
            {
                int idLivro = Convert.ToInt32(gridLivros.Rows[e.RowIndex].Cells["colId"].Value);
                string titulo = gridLivros.Rows[e.RowIndex].Cells["colTitulo"].Value?.ToString() ?? "este livro";

                DialogResult confirmar = MessageBox.Show(
                    $"Deseja requisitar \"{titulo}\"?\n\nO livro será adicionado ao histórico de empréstimos.",
                    "Confirmar requisição",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmar != DialogResult.Yes)
                    return;

                BLL.Historicos.RegistrarEmprestimo(globais.id_utilizador, idLivro);

                MessageBox.Show(
                    "Livro requisitado com sucesso!\n\nConsulte o histórico em \"Histórico de Empréstimos\".",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao requisitar livro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
