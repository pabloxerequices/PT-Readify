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
            // Categorias (CheckedListBox)
            clbCategoria.BeginUpdate();
            try
            {
                clbCategoria.Items.Clear();
                // Adiciona opção "Todas" e evita duplicados
                clbCategoria.Items.Add("Todas");

                DataTable dtCats = BusinessLogicLayer.BLL.Livro.ObterCategorias();
                foreach (DataRow r in dtCats.Rows)
                {
                    var val = r.Table.Columns.Contains("Nome") ? r["Nome"].ToString() : r[0].ToString();
                    if (!string.IsNullOrWhiteSpace(val) && !clbCategoria.Items.Contains(val))
                        clbCategoria.Items.Add(val);
                }

                // Ajustes visuais para evitar sobreposição
                clbCategoria.CheckOnClick = true;
                clbCategoria.IntegralHeight = true;
                clbCategoria.MultiColumn = false;

                // Marca "Todas" por defeito se não houver nenhuma específica marcada
                if (clbCategoria.Items.Count > 0)
                    clbCategoria.SetItemChecked(0, true);
            }
            finally
            {
                clbCategoria.EndUpdate();
            }

            // Estados (ComboBox) — removido "Todos" conforme solicitado
            comboEstado.BeginUpdate();
            try
            {
                comboEstado.Items.Clear();
                DataTable dtEstados = BusinessLogicLayer.BLL.Livro.ObterEstados();
                foreach (DataRow r in dtEstados.Rows)
                {
                    var val = r.Table.Columns.Contains("Nome") ? r["Nome"].ToString() : r[0].ToString();
                    if (!string.IsNullOrWhiteSpace(val) && !comboEstado.Items.Contains(val))
                        comboEstado.Items.Add(val);
                }

                comboEstado.SelectedIndex = -1; // nenhum selecionado por defeito
            }
            finally
            {
                comboEstado.EndUpdate();
            }

            // Liga handler para comportamento especial da opção "Todas"
            clbCategoria.ItemCheck -= ClbCategoria_ItemCheck;
            clbCategoria.ItemCheck += ClbCategoria_ItemCheck;
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
            DataTable resultado = BusinessLogicLayer.BLL.Livro.Pesquisar(
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
        private void ClbCategoria_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Se está a marcar "Todas" => desmarca todos os outros
            if (clbCategoria.Items[e.Index].ToString() == "Todas" && e.NewValue == CheckState.Checked)
            {
                for (int i = 1; i < clbCategoria.Items.Count; i++)
                    clbCategoria.SetItemChecked(i, false);
                return;
            }

            // Se está a marcar uma categoria específica => desmarca "Todas"
            if (clbCategoria.Items[e.Index].ToString() != "Todas" && e.NewValue == CheckState.Checked)
            {
                if (clbCategoria.Items.Contains("Todas"))
                    clbCategoria.SetItemChecked(0, false);
            }

            // Se, depois da alteração, não houver nenhuma específica marcada => marca "Todas"
            this.BeginInvoke((MethodInvoker)delegate
            {
                bool anyChecked = false;
                for (int i = 1; i < clbCategoria.Items.Count; i++)
                {
                    if (clbCategoria.GetItemChecked(i)) { anyChecked = true; break; }
                }
                if (!anyChecked && clbCategoria.Items.Count > 0)
                    clbCategoria.SetItemChecked(0, true);
            });
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
