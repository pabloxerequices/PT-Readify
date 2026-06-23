using System;
using System.Data;
using BusinessLogicLayer;

namespace PT_Readify
{
    public static class CarrinhoService
    {
        private static DataTable _itens;
        private static bool _inicializado;

        public static event Action CarrinhoAlterado;

        public static DataTable Itens
        {
            get
            {
                GarantirInicializado();
                return _itens;
            }
        }

        public static int TotalItens
        {
            get
            {
                GarantirInicializado();
                int total = 0;
                foreach (DataRow row in _itens.Rows)
                {
                    if (row.RowState != DataRowState.Deleted)
                        total += Convert.ToInt32(row["Quantidade"]);
                }
                return total;
            }
        }

        public static decimal TotalPreco
        {
            get
            {
                GarantirInicializado();
                decimal total = 0;
                foreach (DataRow row in _itens.Rows)
                {
                    if (row.RowState != DataRowState.Deleted)
                        total += Convert.ToDecimal(row["Subtotal"]);
                }
                return total;
            }
        }

        private static void GarantirInicializado()
        {
            if (_inicializado)
                return;

            _itens = new DataTable();
            _itens.Columns.Add("Id_Livro", typeof(int));
            _itens.Columns.Add("Titulo", typeof(string));
            _itens.Columns.Add("Autor", typeof(string));
            _itens.Columns.Add("Preco", typeof(decimal));
            _itens.Columns.Add("Quantidade", typeof(int));
            _itens.Columns.Add("Subtotal", typeof(decimal));
            _itens.Columns.Add("Acao", typeof(string));

            _inicializado = true;
        }

        public static void AdicionarLivro(int idLivro)
        {
            DataTable dtLivro = BLL.Livros.ObterLivroPorId(idLivro);
            if (dtLivro == null || dtLivro.Rows.Count == 0)
                throw new Exception("Livro não encontrado.");

            DataRow livro = dtLivro.Rows[0];
            string titulo = livro["Titulo"]?.ToString() ?? "";
            string autor = livro["Autor"]?.ToString() ?? "";
            decimal preco = Convert.ToDecimal(livro["Preço"]) / 100m;

            AdicionarLivro(idLivro, titulo, autor, preco);
        }

        public static void AdicionarLivro(int idLivro, string titulo, string autor, decimal precoEuros)
        {
            GarantirInicializado();

            decimal preco = precoEuros;
            if (precoEuros >= 100)
                preco = precoEuros / 100m;

            DataRow[] existentes = _itens.Select("Id_Livro = " + idLivro);
            if (existentes.Length > 0)
            {
                int qtd = Convert.ToInt32(existentes[0]["Quantidade"]);
                existentes[0]["Quantidade"] = qtd + 1;
                AtualizarSubtotal(existentes[0]);
            }
            else
            {
                DataRow novaLinha = _itens.NewRow();
                novaLinha["Id_Livro"] = idLivro;
                novaLinha["Titulo"] = titulo;
                novaLinha["Autor"] = autor;
                novaLinha["Preco"] = preco;
                novaLinha["Quantidade"] = 1;
                novaLinha["Acao"] = "Comprar";
                AtualizarSubtotal(novaLinha);
                _itens.Rows.Add(novaLinha);
            }

            CarrinhoAlterado?.Invoke();
        }

        public static void RemoverLinha(int indice)
        {
            GarantirInicializado();
            if (indice >= 0 && indice < _itens.Rows.Count)
            {
                _itens.Rows[indice].Delete();
                _itens.AcceptChanges();
                CarrinhoAlterado?.Invoke();
            }
        }

        public static void Limpar()
        {
            GarantirInicializado();
            _itens.Clear();
            CarrinhoAlterado?.Invoke();
        }

        private static void AtualizarSubtotal(DataRow row)
        {
            decimal preco = Convert.ToDecimal(row["Preco"]);
            int quantidade = Convert.ToInt32(row["Quantidade"]);
            row["Subtotal"] = preco * quantidade;
        }

        public static void AtualizarQuantidade(int indice, int quantidade)
        {
            GarantirInicializado();
            if (indice < 0 || indice >= _itens.Rows.Count)
                return;

            DataRow row = _itens.Rows[indice];
            row["Quantidade"] = quantidade;
            AtualizarSubtotal(row);
            CarrinhoAlterado?.Invoke();
        }

        public static void ProcessarCarrinho()
        {
            GarantirInicializado();

            if (_itens.Rows.Count == 0)
                throw new InvalidOperationException("O carrinho está vazio.");

            if (globais.id_utilizador <= 0)
                throw new InvalidOperationException("Inicie sessão para finalizar o pedido.");

            foreach (DataRow row in _itens.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                int idLivro = Convert.ToInt32(row["Id_Livro"]);
                int quantidade = Convert.ToInt32(row["Quantidade"]);
                string acao = row["Acao"]?.ToString() ?? "Comprar";

                switch (acao)
                {
                    case "Comprar":
                        BLL.Historicos.RegistrarCompra(globais.id_utilizador, idLivro, quantidade);
                        break;
                    case "Reservar":
                        for (int i = 0; i < quantidade; i++)
                            BLL.Historicos.RegistrarReserva(globais.id_utilizador, idLivro);
                        break;
                    case "Emprestar":
                        for (int i = 0; i < quantidade; i++)
                            BLL.Historicos.RegistrarEmprestimo(globais.id_utilizador, idLivro);
                        break;
                }
            }

            Limpar();
        }
    }
}
