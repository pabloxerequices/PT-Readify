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

            dataGridViewLivros.AutoGenerateColumns = true;
            dataGridViewLivros.DataSource = resultado;

            // Ocultar coluna Id (se existir)
            string[] possibleIdNames = new[] { "Id_Livro", "ID", "Id" };
            foreach (var name in possibleIdNames)
            {
                if (dataGridViewLivros.Columns.Contains(name))
                {
                    dataGridViewLivros.Columns[name].Visible = false;
                    break;
                }
            }

            // Remover colunas antigas de estado para evitar duplicações/ligação incorreta
            if (dataGridViewLivros.Columns.Contains("Estado"))
                dataGridViewLivros.Columns.Remove("Estado");
            if (dataGridViewLivros.Columns.Contains("Id_Estado_Livro"))
                dataGridViewLivros.Columns.Remove("Id_Estado_Livro");

            // Inserir ComboBox ligado à tabela de estados (por id -> mostra nome)
            if (resultado.Columns.Contains("Id_Estado_Livro"))
            {
                try
                {
                    var estadosDt = BusinessLogicLayer.BLL.Livros.ObterEstadosTabela(); // DataTable limpo
                    var bs = new BindingSource { DataSource = estadosDt };

                    var comboCol = new DataGridViewComboBoxColumn
                    {
                        Name = "Estado",
                        HeaderText = "Estado",
                        DataPropertyName = "Id_Estado_Livro",
                        DataSource = bs,
                        DisplayMember = "estado",
                        ValueMember = "Id_Estado_Livro",
                        ValueType = typeof(int),
                        FlatStyle = FlatStyle.Flat,
                        DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                    };

                    dataGridViewLivros.Columns.Add(comboCol);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Não foi possível carregar estados por id: " + ex.Message);
                }
            }
            else if (resultado.Columns.Contains("Estado"))
            {
                // Se só existe texto 'Estado' no resultado: transformar em combo baseado em strings únicos
                try
                {
                    var estadosList = BusinessLogicLayer.BLL.Livros.ObterEstados().Select(s => s?.Trim()).Distinct().ToList();
                    var comboCol = new DataGridViewComboBoxColumn
                    {
                        Name = "Estado",
                        HeaderText = "Estado",
                        DataPropertyName = "Estado",
                        DataSource = estadosList,
                        FlatStyle = FlatStyle.Flat,
                        DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                    };
                    dataGridViewLivros.Columns.Add(comboCol);
                }
                catch { /* fallback: manter coluna de texto */ }
            }

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
