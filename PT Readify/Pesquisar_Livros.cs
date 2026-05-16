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

            dataGridViewLivros.AutoGenerateColumns = false;
            dataGridViewLivros.DataSource = null;
            dataGridViewLivros.Columns.Clear();

            // Detectar nomes possíveis das colunas
            string colTituloName = null;
            if (resultado.Columns.Contains("Titulo")) colTituloName = "Titulo";
            else if (resultado.Columns.Contains("Título")) colTituloName = "Título";
            else if (resultado.Columns.Contains("Nome")) colTituloName = "Nome";

            string colAutorName = null;
            if (resultado.Columns.Contains("Autor")) colAutorName = "Autor";
            else if (resultado.Columns.Contains("Author")) colAutorName = "Author";

            string colCategoriaTextName = null;
            if (resultado.Columns.Contains("Categoria")) colCategoriaTextName = "Categoria";
            else if (resultado.Columns.Contains("Genero")) colCategoriaTextName = "Genero";
            else if (resultado.Columns.Contains("Gênero")) colCategoriaTextName = "Gênero";

            string colCategoriaIdName = null;
            if (resultado.Columns.Contains("Id_Genero")) colCategoriaIdName = "Id_Genero";
            else if (resultado.Columns.Contains("Id_Categoria")) colCategoriaIdName = "Id_Categoria";

            string colEstadoTextName = null;
            if (resultado.Columns.Contains("Estado")) colEstadoTextName = "Estado";

            string colEstadoIdName = null;
            if (resultado.Columns.Contains("Id_Estado_Livro")) colEstadoIdName = "Id_Estado_Livro";
            else if (resultado.Columns.Contains("Id_Estado")) colEstadoIdName = "Id_Estado";

            // Coluna Título / Nome
            if (colTituloName != null)
            {
                var tituloCol = new DataGridViewTextBoxColumn
                {
                    Name = "colTitulo",
                    HeaderText = "Título",
                    DataPropertyName = colTituloName,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
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
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dataGridViewLivros.Columns.Add(autorCol);
            }

            // Coluna Categoria (Id -> Combo; texto -> TextBox)
            if (colCategoriaIdName != null)
            {
                try
                {
                    var generosDt = BusinessLogicLayer.BLL.Livros.ObterGeneros(); // pode lançar se não existir
                    var bsGen = new BindingSource { DataSource = generosDt };

                    var comboCat = new DataGridViewComboBoxColumn
                    {
                        Name = "Categoria",
                        HeaderText = "Categoria",
                        DataPropertyName = colCategoriaIdName,
                        DataSource = bsGen,
                        DisplayMember = "genero",
                       
                        FlatStyle = FlatStyle.Flat,
                        DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                    };
                    dataGridViewLivros.Columns.Add(comboCat);
                }
                catch
                {
                    // fallback para coluna de texto se não for possível carregar tabela de gêneros
                    var catTxt = new DataGridViewTextBoxColumn
                    {
                        Name = "Categoria",
                        HeaderText = "Categoria",
                        DataPropertyName = colCategoriaIdName,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    };
                    dataGridViewLivros.Columns.Add(catTxt);
                }
            }
            else if (colCategoriaTextName != null)
            {
                var catCol = new DataGridViewTextBoxColumn
                {
                    Name = "Categoria",
                    HeaderText = "Categoria",
                    DataPropertyName = colCategoriaTextName,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                };
                dataGridViewLivros.Columns.Add(catCol);
            }

            // Coluna Estado (Id -> Combo baseado em tabela; texto -> Combo com lista de estados)
            if (colEstadoIdName != null)
            {
                try
                {
                    var estadosDt = BusinessLogicLayer.BLL.Livros.ObterEstadosTabela();
                    var bs = new BindingSource { DataSource = estadosDt };

                    var comboCol = new DataGridViewComboBoxColumn
                    {
                        Name = "Estado",
                        HeaderText = "Estado",
                        DataPropertyName = colEstadoIdName,
                        DataSource = bs,
                        DisplayMember = "estado",
                        ValueMember = "Id_Estado_Livro",
                        ValueType = typeof(int),
                        FlatStyle = FlatStyle.Flat,
                        DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                    };

                    dataGridViewLivros.Columns.Add(comboCol);
                }
                catch
                {
                    // fallback: exibir id como texto
                    var estadoTxt = new DataGridViewTextBoxColumn
                    {
                        Name = "Estado",
                        HeaderText = "Estado",
                        DataPropertyName = colEstadoIdName,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    };
                    dataGridViewLivros.Columns.Add(estadoTxt);
                }
            }
            else if (colEstadoTextName != null)
            {
                try
                {
                    var estadosList = BusinessLogicLayer.BLL.Livros.ObterEstados().Select(s => s?.Trim()).Distinct().ToList();
                    var comboCol = new DataGridViewComboBoxColumn
                    {
                        Name = "Estado",
                        HeaderText = "Estado",
                        DataPropertyName = colEstadoTextName,
                        DataSource = estadosList,
                        FlatStyle = FlatStyle.Flat,
                        DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                    };
                    dataGridViewLivros.Columns.Add(comboCol);
                }
                catch
                {
                    // fallback para texto
                    var estadoCol = new DataGridViewTextBoxColumn
                    {
                        Name = "Estado",
                        HeaderText = "Estado",
                        DataPropertyName = colEstadoTextName,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    };
                    dataGridViewLivros.Columns.Add(estadoCol);
                }
            }

            // Se nenhuma coluna esperada foi adicionada, habilitar AutoGenerateColumns como fallback
            if (dataGridViewLivros.Columns.Count == 0)
            {
                dataGridViewLivros.AutoGenerateColumns = true;
            }

            dataGridViewLivros.DataSource = resultado;
            dataGridViewLivros.Refresh();
        }

        // Evento chamado quando se altera texto / estado
        private void Filtro_Changed(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        // ItemCheck acontece antes do estado ser atualizado, por isso usamos BeginInvoke
        private void clbCategoria_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)(() => AplicarFiltro()));
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtTitulo.Clear();
            txtAutor.Clear();
            // desmarcar tudo e marcar "Todas"
            for (int i = 0; i < clbCategoria.Items.Count; i++)
                clbCategoria.SetItemChecked(i, false);
            int idxTodas = clbCategoria.Items.IndexOf("Todas");
            if (idxTodas >= 0) clbCategoria.SetItemChecked(idxTodas, true);

            comboEstado.SelectedIndex = 0;
            AplicarFiltro();
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
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
    }
}
