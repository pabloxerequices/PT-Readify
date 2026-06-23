using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Carrinho : Form
    {
        private DataTable carrinhoTable;

        public Carrinho()
        {
            InitializeComponent();
            carrinhoTable = CarrinhoService.Itens;
            ConfigurarDataGrid();
            AtualizarTotalGeral();
        }

        private void ConfigurarDataGrid()
        {
            dataGridViewCarrinho.AutoGenerateColumns = false;
            dataGridViewCarrinho.DataSource = null;
            dataGridViewCarrinho.Columns.Clear();

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                DataPropertyName = "Id_Livro",
                Width = 50,
                ReadOnly = true
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTitulo",
                HeaderText = "Título",
                DataPropertyName = "Titulo",
                Width = 220,
                ReadOnly = true
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAutor",
                HeaderText = "Autor",
                DataPropertyName = "Autor",
                Width = 120,
                ReadOnly = true
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPreco",
                HeaderText = "Preço",
                DataPropertyName = "Preco",
                Width = 70,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colQuantidade",
                HeaderText = "Qtd",
                DataPropertyName = "Quantidade",
                Width = 50,
                ReadOnly = false
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = "colAcao",
                HeaderText = "Ação",
                DataPropertyName = "Acao",
                Width = 110,
                FlatStyle = FlatStyle.Flat
            });
            var colAcao = (DataGridViewComboBoxColumn)dataGridViewCarrinho.Columns["colAcao"];
            colAcao.Items.AddRange("Comprar", "Reservar", "Emprestar");

            dataGridViewCarrinho.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSubtotal",
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
                Width = 80,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dataGridViewCarrinho.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "colRemover",
                HeaderText = "Ação",
                Text = "Remover",
                UseColumnTextForButtonValue = true,
                Width = 80
            });

            dataGridViewCarrinho.ReadOnly = false;
            dataGridViewCarrinho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCarrinho.MultiSelect = false;
            dataGridViewCarrinho.AllowUserToAddRows = false;
            dataGridViewCarrinho.AllowUserToDeleteRows = false;
            dataGridViewCarrinho.CellClick += DataGridViewCarrinho_CellClick;
            dataGridViewCarrinho.CellEndEdit += DataGridViewCarrinho_CellEndEdit;
            dataGridViewCarrinho.DataError += DataGridViewCarrinho_DataError;

            dataGridViewCarrinho.DataSource = carrinhoTable;
        }

        private void DataGridViewCarrinho_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void AtualizarTotalGeral()
        {
            labelTotal.Text = $"Total: {CarrinhoService.TotalPreco:C2} ({CarrinhoService.TotalItens} itens)";
        }

        private void DataGridViewCarrinho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dataGridViewCarrinho.Columns["colRemover"].Index)
            {
                DialogResult resultado = MessageBox.Show(
                    "Deseja remover este livro do carrinho?",
                    "Confirmação",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    CarrinhoService.RemoverLinha(e.RowIndex);
                    AtualizarTotalGeral();
                }
            }
        }

        private void DataGridViewCarrinho_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dataGridViewCarrinho.Columns["colQuantidade"].Index)
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

                    CarrinhoService.AtualizarQuantidade(e.RowIndex, novaQtd);
                    AtualizarTotalGeral();
                    dataGridViewCarrinho.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar quantidade: " + ex.Message);
                }
            }
        }

        private void ConfirmarPedido()
        {
            if (carrinhoTable.Rows.Count == 0)
            {
                MessageBox.Show("O carrinho está vazio!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para finalizar o pedido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult resultado = MessageBox.Show(
                $"Confirmar pedido de {CarrinhoService.TotalItens} item(ns)?\n\n" +
                "Cada livro será processado conforme a ação selecionada (Comprar, Reservar ou Emprestar).",
                "Confirmar pedido",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado != DialogResult.Yes)
                return;

            try
            {
                CarrinhoService.ProcessarCarrinho();
                AtualizarTotalGeral();
                MessageBox.Show(
                    "Pedido finalizado com sucesso!\n\n" +
                    "Compras → Histórico de Compras\n" +
                    "Reservas e Empréstimos → Histórico de Empréstimos",
                    "Sucesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao finalizar pedido: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MarcarTodosAcao(string acao)
        {
            foreach (DataRow row in carrinhoTable.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                    row["Acao"] = acao;
            }
            dataGridViewCarrinho.Refresh();
        }

        private void btnFinalizarCompra_Click(object sender, EventArgs e) => ConfirmarPedido();

        private void btnReservar_Click(object sender, EventArgs e)
        {
            MarcarTodosAcao("Reservar");
            ConfirmarPedido();
        }

        private void btnEmprestar_Click(object sender, EventArgs e)
        {
            MarcarTodosAcao("Emprestar");
            ConfirmarPedido();
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            MarcarTodosAcao("Comprar");
            ConfirmarPedido();
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
                CarrinhoService.Limpar();
                AtualizarTotalGeral();
            }
        }

        private void Carrinho_Load(object sender, EventArgs e)
        {
            ConfigurarDataGrid();
            AtualizarTotalGeral();
        }

        private void dataGridViewCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
