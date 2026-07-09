using System;
using System.Collections.Generic;
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
            _itens.Columns.Add("Editora", typeof(string));
            _itens.Columns.Add("Preco", typeof(decimal));
            _itens.Columns.Add("Quantidade", typeof(int));
            _itens.Columns.Add("Subtotal", typeof(decimal));
            _itens.Columns.Add("Acao", typeof(string));

            _inicializado = true;
        }

        private static void ValidarStockParaCompra(int idLivro, string titulo, int quantidadeDesejada)
        {
            int stock = BLL.Livros.ObterStock(idLivro);
            if (stock <= 0)
                throw new InvalidOperationException(
                    $"\"{titulo}\" está esgotado. Para reservar ou emprestar, vá a Requisições/Empréstimos.");

            if (quantidadeDesejada > stock)
                throw new InvalidOperationException(
                    $"Stock insuficiente para \"{titulo}\". Disponível: {stock}.");
        }

        public static void AdicionarLivro(int idLivro)
        {
            DataTable dtLivro = BLL.Livros.ObterLivroPorId(idLivro);
            if (dtLivro == null || dtLivro.Rows.Count == 0)
                throw new Exception("Livro não encontrado.");

            DataRow livro = dtLivro.Rows[0];
            string titulo = livro["Titulo"]?.ToString() ?? "";
            string autor = livro["Autor"]?.ToString() ?? "";
            string editora = livro["Editora"]?.ToString() ?? "";
            decimal preco = Convert.ToDecimal(livro["Preço"]) / 100m;

            AdicionarLivro(idLivro, titulo, autor, editora, preco);
        }

        public static void AdicionarLivro(int idLivro, string titulo, string autor, string editora, decimal precoEuros)
        {
            GarantirInicializado();

            DataRow[] existentes = _itens.Select("Id_Livro = " + idLivro);
            int novaQtd = existentes.Length > 0 ? Convert.ToInt32(existentes[0]["Quantidade"]) + 1 : 1;

            ValidarStockParaCompra(idLivro, titulo, novaQtd);

            if (existentes.Length > 0)
            {
                existentes[0]["Quantidade"] = novaQtd;
                AtualizarSubtotal(existentes[0]);
            }
            else
            {
                DataRow novaLinha = _itens.NewRow();
                novaLinha["Id_Livro"] = idLivro;
                novaLinha["Titulo"] = titulo;
                novaLinha["Autor"] = autor;
                novaLinha["Editora"] = editora;
                novaLinha["Preco"] = precoEuros;
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
            int idLivro = Convert.ToInt32(row["Id_Livro"]);
            string titulo = row["Titulo"]?.ToString() ?? "";

            ValidarStockParaCompra(idLivro, titulo, quantidade);

            row["Quantidade"] = quantidade;
            AtualizarSubtotal(row);
            CarrinhoAlterado?.Invoke();
        }

        public static ResultadoEnvioRecibo ProcessarCarrinho()
        {
            GarantirInicializado();

            if (_itens.Rows.Count == 0)
                throw new InvalidOperationException("O carrinho está vazio.");

            if (globais.id_utilizador <= 0)
                throw new InvalidOperationException("Inicie sessão para finalizar o pedido.");

            decimal totalCompra = TotalPreco;
            if (!CarteiraService.TemSaldoSuficiente(totalCompra))
                throw new InvalidOperationException(
                    $"Saldo insuficiente na carteira. Saldo: {CarteiraService.Saldo:C2}. Total do carrinho: {totalCompra:C2}.");

            var itensRecibo = new List<ItemReciboCompra>();
            DateTime dataCompra = DateTime.Now;
            int idCompra = 0;

            foreach (DataRow row in _itens.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                int idLivro = Convert.ToInt32(row["Id_Livro"]);
                int quantidade = Convert.ToInt32(row["Quantidade"]);
                string titulo = row["Titulo"]?.ToString() ?? "";

                ValidarStockParaCompra(idLivro, titulo, quantidade);
            }

            CarteiraService.Debitar(totalCompra);

            foreach (DataRow row in _itens.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                int idLivro = Convert.ToInt32(row["Id_Livro"]);
                int quantidade = Convert.ToInt32(row["Quantidade"]);
                string titulo = row["Titulo"]?.ToString() ?? "";
                string autor = row["Autor"]?.ToString() ?? "";
                string editora = row["Editora"]?.ToString() ?? "";
                decimal preco = Convert.ToDecimal(row["Preco"]);

                int novoIdCompra = BLL.Historicos.RegistrarCompra(globais.id_utilizador, idLivro, quantidade);
                if (idCompra == 0)
                    idCompra = novoIdCompra;

                itensRecibo.Add(new ItemReciboCompra
                {
                    Titulo = titulo,
                    Autor = autor,
                    Editora = editora,
                    PrecoUnitario = preco,
                    Quantidade = quantidade
                });
            }

            Limpar();

            // ==========================================
            // CORREÇÃO:
            // Em vez de chamar o ReciboCompraEmailService, 
            // devolvemos Sucesso = true diretamente!
            // ==========================================
            return new ResultadoEnvioRecibo
            {
                Sucesso = true,
                Mensagem = "Os detalhes da sua compra seguem abaixo.",
                IdCompra = idCompra
            };
        }
    }

}
