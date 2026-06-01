using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Pesquisar_Livros : Form
    {
        private DataTable livrosTable;

        public Pesquisar_Livros()
        {
            InitializeComponent();
        }

        private void Pesquisar_Livros_Load(object sender, EventArgs e)
        {
            InicializarControles();
            CarregarDadosExemplo(); // ou carregue via BLL se preferir
            AplicarFiltro();

            // Garantir que os eventos estão ligados em runtime (evita dupla inscrição)
            dataGridViewLivros.CellDoubleClick -= DataGridViewLivros_CellDoubleClick;
            dataGridViewLivros.CellDoubleClick += DataGridViewLivros_CellDoubleClick;

            // Filtros: reagir a mudanças (registrar versões runtime é seguro)
            // Designer já liga alguns eventos, mas manter aqui garante comportamento consistente
            txtTitulo.TextChanged -= Filtro_TextChanged;
            txtTitulo.TextChanged += Filtro_TextChanged;

            txtAutor.TextChanged -= Filtro_TextChanged;
            txtAutor.TextChanged += Filtro_TextChanged;

            comboEstado.SelectedIndexChanged -= ComboEstado_SelectedIndexChanged;
            comboEstado.SelectedIndexChanged += ComboEstado_SelectedIndexChanged;

            // Usar o nome com maiúscula porque o Designer pode referenciar ClbCategoria_ItemCheck
            clbCategoria.ItemCheck -= ClbCategoria_ItemCheck;
            clbCategoria.ItemCheck += ClbCategoria_ItemCheck;
        }

        private void InicializarControles()
        {
            // Popular categorias a partir da tabela Genero
            try
            {
                clbCategoria.Items.Clear();
                clbCategoria.Items.Add("Todas"); // opção para não filtrar
                var generos = BusinessLogicLayer.BLL.Livros.ObterGeneros();
                foreach (var g in generos)
                {
                    clbCategoria.Items.Add(g);
                }
                // Por defeito selecionar "Todas"
                if (clbCategoria.Items.Count > 0)
                    clbCategoria.SetItemChecked(0, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar categorias: " + ex.Message);
            }

            // Popular estados a partir da tabela Estado_Livro
            try
            {
                comboEstado.Items.Clear();
                comboEstado.Items.Add("Todos"); // opção para não filtrar
                var estados = BusinessLogicLayer.BLL.Livros.ObterEstados();
                foreach (var e in estados)
                {
                    comboEstado.Items.Add(e);
                }
                comboEstado.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar estados: " + ex.Message);
            }
        }

        // Chamado por TextChanged em título/autor
        private void Filtro_TextChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void ComboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        // Handler legacy esperado pelo Designer (TextChanged / SelectedIndexChanged)
        private void Filtro_Changed(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        // Handler com o nome que o Designer pode referenciar (ItemCheck) — ItemCheck ocorre antes da alteração, usamos BeginInvoke
        private void ClbCategoria_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() => AplicarFiltro()));
        }

        // Mantém também o método com nome em lowercase (se existir referência em código antigo)
        private void clbCategoria_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new Action(() => AplicarFiltro()));
        }

        private void AplicarFiltro()
        {
            // Chama BLL que faz JOIN e IN com parâmetros
            var checkedItems = clbCategoria.CheckedItems.Cast<string>().ToList();
            List<string> categoriasParaFiltro = null;
            if (checkedItems.Count > 0 && !checkedItems.Contains("Todas"))
            {
                categoriasParaFiltro = checkedItems;
            }

            var estado = comboEstado.SelectedItem as string;
            DataTable resultado = BusinessLogicLayer.BLL.Livros.Pesquisar(
                txtTitulo.Text.Trim(),
                txtAutor.Text.Trim(),
                categoriasParaFiltro,
                estado);

            ConfigurarDataGrid(resultado);
        }

        private void ConfigurarDataGrid(DataTable resultado)
        {
            if (resultado == null) return;

            // Se a query retornar múltiplas linhas por livro (um género por linha),
            // agregamos aqui por Id do livro concatenando as categorias numa só célula.
            string idColName = null;
            if (resultado.Columns.Contains("Id_Livro")) idColName = "Id_Livro";
            else if (resultado.Columns.Contains("ID")) idColName = "ID";
            else if (resultado.Columns.Contains("Id")) idColName = "Id";

            string colCategoriaTextName = null;
            if (resultado.Columns.Contains("Categoria")) colCategoriaTextName = "Categoria";
            else if (resultado.Columns.Contains("Genero")) colCategoriaTextName = "Genero";
            else if (resultado.Columns.Contains("Gênero")) colCategoriaTextName = "Gênero";

            DataTable finalResult = resultado;

            if (!string.IsNullOrEmpty(idColName) && !string.IsNullOrEmpty(colCategoriaTextName))
            {
                var groups = resultado.AsEnumerable().GroupBy(r => r[idColName] ?? DBNull.Value);
                bool needsAggregation = groups.Any(g => g.Count() > 1);
                if (needsAggregation)
                {
                    finalResult = resultado.Clone(); // mantém schema
                    foreach (var g in groups)
                    {
                        var newRow = finalResult.NewRow();
                        foreach (DataColumn col in resultado.Columns)
                        {
                            if (col.ColumnName == colCategoriaTextName)
                            {
                                var vals = g.Select(r => r[colCategoriaTextName]?.ToString())
                                            .Where(s => !string.IsNullOrWhiteSpace(s))
                                            .Distinct();
                                newRow[colCategoriaTextName] = string.Join(", ", vals);
                            }
                            else
                            {
                                var first = g.Select(r => r[col.ColumnName]).FirstOrDefault(x => x != DBNull.Value);
                                newRow[col.ColumnName] = first ?? DBNull.Value;
                            }
                        }
                        finalResult.Rows.Add(newRow);
                    }
                }
            }

            dataGridViewLivros.AutoGenerateColumns = false;
            dataGridViewLivros.DataSource = null;
            dataGridViewLivros.Columns.Clear();

            // Detectar nomes possíveis das colunas (usar finalResult)
            string colTituloName = null;
            if (finalResult.Columns.Contains("Titulo")) colTituloName = "Titulo";
            else if (finalResult.Columns.Contains("Título")) colTituloName = "Título";
            else if (finalResult.Columns.Contains("Nome")) colTituloName = "Nome";

            string colAutorName = null;
            if (finalResult.Columns.Contains("Autor")) colAutorName = "Autor";
            else if (finalResult.Columns.Contains("Author")) colAutorName = "Author";

            if (string.IsNullOrEmpty(colCategoriaTextName))
            {
                if (finalResult.Columns.Contains("Categoria")) colCategoriaTextName = "Categoria";
                else if (finalResult.Columns.Contains("Genero")) colCategoriaTextName = "Genero";
                else if (finalResult.Columns.Contains("Gênero")) colCategoriaTextName = "Gênero";
            }

            string colEstadoTextName = null;
            if (finalResult.Columns.Contains("Estado")) colEstadoTextName = "Estado";

            string colEstadoIdName = null;
            if (finalResult.Columns.Contains("Id_Estado_Livro")) colEstadoIdName = "Id_Estado_Livro";
            else if (finalResult.Columns.Contains("Id_Estado")) colEstadoIdName = "Id_Estado";

            // Coluna Título / Nome
            if (colTituloName != null)
            {
                var tituloCol = new DataGridViewTextBoxColumn
                {
                    Name = "colTitulo",
                    HeaderText = "Título",
                    DataPropertyName = colTituloName,
                    Width = 70,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                };
                dataGridViewLivros.Columns.Add(tituloCol);
            }

            // Coluna Autor
            if (colAutorName != null)
            {
                var autorCol = new DataGridViewTextBoxColumn
                {
                    Name = "colAutor",
                    HeaderText = "Autor",
                    DataPropertyName = colAutorName,
                    Width = 70,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                };
                dataGridViewLivros.Columns.Add(autorCol);
            }

            // Coluna Categoria (texto)
            if (colCategoriaTextName != null)
            {
                var catCol = new DataGridViewTextBoxColumn
                {
                    Name = "colCategoria",
                    HeaderText = "Categoria",
                    DataPropertyName = colCategoriaTextName,
                    Width = 70,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                };
                dataGridViewLivros.Columns.Add(catCol);
            }

            // Coluna Estado (texto)
            if (colEstadoTextName != null)
            {
                var estadoCol = new DataGridViewTextBoxColumn
                {
                    Name = "colEstado",
                    HeaderText = "Estado",
                    DataPropertyName = colEstadoTextName,
                    Width = 70,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                };
                dataGridViewLivros.Columns.Add(estadoCol);
            }

            if (colEstadoIdName != null)
            {
                var idEst = new DataGridViewTextBoxColumn
                {
                    Name = "colIdEstado",
                    HeaderText = "Estado",
                    DataPropertyName = colEstadoIdName,
                    Width = 70,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                };
                dataGridViewLivros.Columns.Add(idEst);
            }

            // Propriedades gerais da grid
            dataGridViewLivros.ReadOnly = true;
            dataGridViewLivros.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewLivros.MultiSelect = false;
            dataGridViewLivros.AllowUserToAddRows = false;
            dataGridViewLivros.AllowUserToDeleteRows = false;

            // Associar o resultado (DataTable) como DataSource
            dataGridViewLivros.DataSource = finalResult;
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        // Limpa filtros e reaplica
        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtTitulo.Text = string.Empty;
            txtAutor.Text = string.Empty;

            for (int i = 0; i < clbCategoria.Items.Count; i++)
            {
                clbCategoria.SetItemChecked(i, false);
            }

            if (comboEstado.Items.Count > 0)
                comboEstado.SelectedIndex = 0;

            AplicarFiltro();
        }

        private void CarregarDadosExemplo()
        {
            // Exemplo de criação de DataTable fictício para testes
            livrosTable = new DataTable();
            livrosTable.Columns.Add("Titulo");
            livrosTable.Columns.Add("Autor");
            livrosTable.Columns.Add("Categoria");
            livrosTable.Columns.Add("Estado");

            livrosTable.Rows.Add("Dom Casmurro", "Machado de Assis", "Romance", "Disponível");
            livrosTable.Rows.Add("O Alquimista", "Paulo Coelho", "Ficção", "Emprestado");
            livrosTable.Rows.Add("Capitães da Areia", "Jorge Amado", "Drama", "Reservado");
        }

        private void dataGridViewLivros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Abre detalhes a partir do índice da linha
        private void AbrirDetalhesDaLinha(int rowIndex)
        {
            if (rowIndex < 0) return;

            DataRow rowToShow = null;

            // Tentar obter DataRowView (binding normal de DataTable)
            var drv = dataGridViewLivros.Rows[rowIndex].DataBoundItem as DataRowView;
            if (drv != null)
            {
                rowToShow = drv.Row;
            }
            else
            {
                // Fallback: construir DataTable temporário com valores visíveis na grid
                var dt = new DataTable("RowSnapshot");
                foreach (DataGridViewColumn col in dataGridViewLivros.Columns)
                {
                    // usar header text como nome de coluna temporária
                    dt.Columns.Add(col.HeaderText);
                }
                var newRow = dt.NewRow();
                for (int i = 0; i < dataGridViewLivros.Columns.Count; i++)
                {
                    var val = dataGridViewLivros.Rows[rowIndex].Cells[i].Value;
                    newRow[i] = val ?? DBNull.Value;
                }
                dt.Rows.Add(newRow);
                rowToShow = dt.Rows[0];
            }

            if (rowToShow != null)
            {
                using (var detalhe = new Detalhes_Livro(rowToShow))
                {
                    detalhe.ShowDialog(this);
                }
            }
        }

        // Handler de duplo clique na linha — abre o formulário de detalhes
        private void DataGridViewLivros_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            AbrirDetalhesDaLinha(e.RowIndex);
        }
    }
}
