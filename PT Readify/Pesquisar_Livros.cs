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
            // Preencher estados (igual ao anterior)
            comboEstado.Items.Clear();
            comboEstado.Items.Add("Todos");
            comboEstado.Items.Add("Disponível");
            comboEstado.Items.Add("Emprestado");
            comboEstado.Items.Add("Reservado");
            comboEstado.SelectedIndex = 0;

            // Preencher categorias a partir do BLL -> Genero
            clbCategoria.Items.Clear();
            clbCategoria.Items.Add("Todas"); // opção para tratar como sem filtro
            try
            {
                var generos = BusinessLogicLayer.BLL.Livros.ObterGeneros();
                foreach (var g in generos)
                    clbCategoria.Items.Add(g);
            }
            catch
            {
                // fallback: se ocorrer erro, pode adicionar itens fixos
            }
            clbCategoria.SetItemChecked(0, true); // "Todas" por defeito
        }

        private void AplicarFiltro()
        {
            if (livrosTable == null) return;

            // Recolher categorias selecionadas
            var checkedItems = clbCategoria.CheckedItems.Cast<string>().ToList();
            List<string> categoriasParaFiltro = null;
            if (checkedItems.Count > 0 && !checkedItems.Contains("Todas"))
            {
                categoriasParaFiltro = checkedItems;
            }
            // Se "Todas" estiver marcada ou nenhuma selecionada => categoriasParaFiltro permanece NULL (sem filtro)

            // Chama BLL que faz JOIN e IN com parâmetros
            var estado = comboEstado.SelectedItem as string;
            DataTable resultado = BusinessLogicLayer.BLL.Livros.Pesquisar(
                txtTitulo.Text.Trim(),
                txtAutor.Text.Trim(),
                categoriasParaFiltro,
                estado);

            dataGridViewLivros.DataSource = resultado;
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
            // Chame o método de filtro ou lógica de pesquisa já existente
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
    }
}
