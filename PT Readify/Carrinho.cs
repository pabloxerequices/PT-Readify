using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public partial class Carrinho : Form
    {
        private DataTable carrinhoTable;
        private decimal totalGeral = 0;

        public Carrinho()
        {
            InitializeComponent();
            InicializarCarrinho();
        }

        private void InicializarCarrinho()
        {
            // Criar DataTable com a estrutura do carrinho
            carrinhoTable = new DataTable();
            carrinhoTable.Columns.Add("Id_Livro", typeof(int));
            carrinhoTable.Columns.Add("Titulo", typeof(string));
            carrinhoTable.Columns.Add("Autor", typeof(string));
            carrinhoTable.Columns.Add("Preco", typeof(decimal));
            carrinhoTable.Columns.Add("Quantidade", typeof(int));
            carrinhoTable.Columns.Add("Subtotal", typeof(decimal));

            ConfigurarDataGrid();
        }

        private void ConfigurarDataGrid()
        {
            dataGridViewCarrinho.AutoGenerateColumns = false;
            dataGridViewCarrinho.DataSource = null;
            dataGridViewCarrinho.Columns.Clear();

            // Coluna ID
            var idCol = new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                DataPropertyName = "Id_Livro",
                Width = 50,
                ReadOnly = true
            };
            dataGridViewCarrinho.Columns.Add(idCol);

            var tituloCol = new DataGridViewTextBoxColumn
            {
                Name = "colTitulo",
                HeaderText = "Título",
                DataPropertyName = "Titulo",
                Width = 280,
                ReadOnly = true
            };
            dataGridViewCarrinho.Columns.Add(tituloCol);

            var autorCol = new DataGridViewTextBoxColumn
            {
                Name = "colAutor",
                HeaderText = "Autor",
                DataPropertyName = "Autor",
                Width = 130,
                ReadOnly = true
            };
            dataGridViewCarrinho.Columns.Add(autorCol);

            var precoCol = new DataGridViewTextBoxColumn
            {
                Name = "colPreco",
                HeaderText = "Preço",
                DataPropertyName = "Preco",
                Width = 80,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            };
            dataGridViewCarrinho.Columns.Add(precoCol);

            var qtdCol = new DataGridViewTextBoxColumn
            {
                Name = "colQuantidade",
                HeaderText = "Qtd",
                DataPropertyName = "Quantidade",
                Width = 60,
                ReadOnly = false
            };
            dataGridViewCarrinho.Columns.Add(qtdCol);

            var subtotalCol = new DataGridViewTextBoxColumn
            {
                Name = "colSubtotal",
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
                Width = 90,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            };
            dataGridViewCarrinho.Columns.Add(subtotalCol);

            var btnRemover = new DataGridViewButtonColumn
            {
                Name = "colRemover",
                HeaderText = "Ação",
                Text = "Remover",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dataGridViewCarrinho.Columns.Add(btnRemover);

            dataGridViewCarrinho.ReadOnly = false;
            dataGridViewCarrinho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCarrinho.MultiSelect = false;
            dataGridViewCarrinho.AllowUserToAddRows = false;
            dataGridViewCarrinho.AllowUserToDeleteRows = false;
            dataGridViewCarrinho.CellClick += DataGridViewCarrinho_CellClick;
            dataGridViewCarrinho.CellEndEdit += DataGridViewCarrinho_CellEndEdit;

            dataGridViewCarrinho.DataSource = carrinhoTable;
        }

        private void btnAdicionarLivro_Click(object sender, EventArgs e)
        {
            try
            {
                PesquisarLivrosVisual pesquisar = new PesquisarLivrosVisual();
                pesquisar.labelCarrinho = labelTotal; // Passar referência da label
                
                if (pesquisar.ShowDialog() == DialogResult.OK)
                {
                    int idLivro = pesquisar.LivroSelecionado;
                    AdicionarLivroCarrinho(idLivro);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir pesquisa: " + ex.Message);
            }
        }

        private void AdicionarLivroCarrinho(int idLivro)
        {
            try
            {
                DataRow[] rows = carrinhoTable.Select("Id_Livro = " + idLivro);
                if (rows.Length > 0)
                {
                    int qtdAtual = Convert.ToInt32(rows[0]["Quantidade"]);
                    rows[0]["Quantidade"] = qtdAtual + 1;
                    AtualizarSubtotal(rows[0]);
                }
                else
                {
                    DataTable dtLivro = BLL.Livros.ObterLivroPorId(idLivro);
                    if (dtLivro != null && dtLivro.Rows.Count > 0)
                    {
                        DataRow livro = dtLivro.Rows[0];
                        DataRow novaLinha = carrinhoTable.NewRow();
                        novaLinha["Id_Livro"] = idLivro;
                        novaLinha["Titulo"] = livro["Titulo"].ToString();
                        novaLinha["Autor"] = livro["Autor"].ToString();
                        novaLinha["Preco"] = Convert.ToDecimal(livro["Preço"]) / 100;
                        novaLinha["Quantidade"] = 1;
                        AtualizarSubtotal(novaLinha);
                        carrinhoTable.Rows.Add(novaLinha);
                    }
                }

                AtualizarTotalGeral();
                MessageBox.Show("Livro adicionado com sucesso!", "Sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar livro: " + ex.Message);
            }
        }

        private void AtualizarSubtotal(DataRow row)
        {
            decimal preco = Convert.ToDecimal(row["Preco"]);
            int quantidade = Convert.ToInt32(row["Quantidade"]);
            row["Subtotal"] = preco * quantidade;
        }

        private void AtualizarTotalGeral()
        {
            totalGeral = 0;
            foreach (DataRow row in carrinhoTable.Rows)
            {
                totalGeral += Convert.ToDecimal(row["Subtotal"]);
            }

            labelTotal.Text = "Total: " + totalGeral.ToString("C2");
        }

        private void DataGridViewCarrinho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Botão Remover
            if (e.ColumnIndex == dataGridViewCarrinho.Columns["colRemover"].Index && e.RowIndex >= 0)
            {
                DialogResult resultado = MessageBox.Show(
                    "Deseja remover este livro do carrinho?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    carrinhoTable.Rows[e.RowIndex].Delete();
                    AtualizarTotalGeral();
                    MessageBox.Show("Livro removido do carrinho.", "Sucesso");
                }
            }
        }

        private void DataGridViewCarrinho_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Atualizar quantidade e subtotal
            if (e.ColumnIndex == dataGridViewCarrinho.Columns["colQuantidade"].Index && e.RowIndex >= 0)
            {
                try
                {
                    int novaQtd = Convert.ToInt32(dataGridViewCarrinho[e.ColumnIndex, e.RowIndex].Value);

                    if (novaQtd <= 0)
                    {
                        MessageBox.Show("A quantidade deve ser maior que 0.");
                        dataGridViewCarrinho[e.ColumnIndex, e.RowIndex].Value = 1;
                        return;
                    }

                    DataRow row = carrinhoTable.Rows[e.RowIndex];
                    row["Quantidade"] = novaQtd;
                    AtualizarSubtotal(row);
                    AtualizarTotalGeral();
                    dataGridViewCarrinho.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar quantidade: " + ex.Message);
                }
            }
        }

        private void btnFinalizarCompra_Click(object sender, EventArgs e)
        {
            if (carrinhoTable.Rows.Count == 0)
            {
                MessageBox.Show("O carrinho está vazio!", "Aviso");
                return;
            }

            DialogResult resultado = MessageBox.Show(
                "Total da compra: " + totalGeral.ToString("C2") + "\n\nDeseja finalizar a compra?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    // Aqui você pode adicionar a lógica de salvar a compra na BD
                    MessageBox.Show("Compra finalizada com sucesso!", "Sucesso");
                    carrinhoTable.Clear();
                    AtualizarTotalGeral();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao finalizar compra: " + ex.Message);
                }
            }
        }

        private void btnLimparCarrinho_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show(
                "Deseja limpar todo o carrinho?",
                "Confirmação",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                carrinhoTable.Clear();
                AtualizarTotalGeral();
                MessageBox.Show("Carrinho limpo.", "Sucesso");
            }
        }

        private void Carrinho_Load(object sender, EventArgs e)
        {
            ConfigurarDataGrid();
        }

        private void dataGridViewCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
