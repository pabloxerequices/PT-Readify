using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessLayer;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.IO.Pipes;

namespace BusinessLogicLayer
{
    public class BLL
    {

        //--------------------------UTILIZADOR-------------------------
        public class utilizador
        {
            //load utilizador por id
            static public DataTable LoadById(int Id_Utilizador)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                    new SqlParameter("@Id_Utilizador", Id_Utilizador)
                };
                return dal.executarReader("select * from utilizador where Id_Utilizador=@Id_Utilizador", sqlParams);
            }
            //load utilizador
            static public DataTable Load()
            {
                DAL dal = new DAL();
                return dal.executarReader("select * from utilizador", null);
            }


            //login utilizador
            static public DataTable QueryutilizadorByemail(string Email)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@Email", Email)
                };
                return dal.executarReader("select * from utilizador where Email=@Email", sqlParams);
            }
            //registar utilizador
            static public int insertutilizador(bool Tipo_Utilizador, string Estado_Conta, string Email, string Nome, string Palavra_Passe, int prefixo_telefone, int numero_telefone)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                    new SqlParameter("@Estado_Conta", Estado_Conta),
                new SqlParameter("@Tipo_Utilizador", Tipo_Utilizador),
                new SqlParameter("@Email", Email),
                new SqlParameter("@Nome", Nome),
                new SqlParameter("@Palavra_Passe", Palavra_Passe),

                new SqlParameter("@prefixo_telefone", prefixo_telefone),
                new SqlParameter("@numero_telefone", numero_telefone)

                };
                return dal.executarNonQuery("INSERT into utilizador (Tipo_Utilizador, Estado_Conta, Email, Nome, Palavra_Passe, prefixo_telefone, numero_telefone) VALUES(@Tipo_Utilizador,@Estado_Conta,@Email,@Nome,@Palavra_Passe,@prefixo_telefone,@numero_telefone)", sqlParams);
            }
            //criar utilizador admin

            static public int insertutilizadoradmin(bool Tipo_Utilizador, string Estado_Conta, string Email, string Nome, string Palavra_Passe, int prefixo_telefone, int numero_telefone, object Foto)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                    new SqlParameter("@Estado_Conta", Estado_Conta),
                new SqlParameter("@Tipo_Utilizador", Tipo_Utilizador),
                new SqlParameter("@Email", Email),
                new SqlParameter("@Nome", Nome),
                new SqlParameter("@Palavra_Passe", Palavra_Passe),
                new SqlParameter("@prefixo_telefone", prefixo_telefone),
                new SqlParameter("@numero_telefone", numero_telefone),
                new SqlParameter("@Foto", SqlDbType.Image) { Value = Foto ?? (object)DBNull.Value }
                };
                return dal.executarNonQuery("INSERT into utilizador (Tipo_Utilizador, Estado_Conta, Email, Nome, Palavra_Passe, prefixo_telefone, numero_telefone, Foto) VALUES(@Tipo_Utilizador,@Estado_Conta,@Email,@Nome,@Palavra_Passe,@prefixo_telefone,@numero_telefone,@Foto)", sqlParams);
            }
            static public DataTable queryUtilizadorById(int Id_Utilizador)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
               new SqlParameter("@Id_Utilizador",Id_Utilizador)
                };
                return dal.executarReader("Select * from utilizador where Id_Utilizador=@Id_Utilizador", sqlParams);
            }
            //update utilizador (perfil editar)
            static public int updateUtilizador(int Id_Utilizador, string Email, string Nome, string Palavra_Passe, object Foto, int prefixo_telefone, int numero_telefone)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]
                {
        new SqlParameter("@Id_Utilizador", Id_Utilizador),
        new SqlParameter("@Email", Email),
        new SqlParameter("@Nome", Nome),
        new SqlParameter("@Palavra_Passe", Palavra_Passe),
        
        // Forçamos o ADO.NET a enviar o parâmetro com o tipo correto para a BD (Image)
        new SqlParameter("@Foto", SqlDbType.Image) { Value = Foto ?? (object)DBNull.Value },

        new SqlParameter("@prefixo_telefone", prefixo_telefone),
        new SqlParameter("@numero_telefone", numero_telefone)
                };

                // Executa a query de update que tens na linha 76
                return dal.executarNonQuery("update [utilizador] set [Email]=@Email, [Nome]=@Nome, [Palavra_Passe]=@Palavra_Passe, [Foto]=@Foto, [prefixo_telefone]=@prefixo_telefone, [numero_telefone]=@numero_telefone where Id_Utilizador=@Id_Utilizador", sqlParams);
            }
            static public int updateutilizadoradmin(int Id_Utilizador, bool Tipo_Utilizador, string Estado_Conta, string Email, string Nome, string Palavra_Passe, int prefixo_telefone, int numero_telefone, object Foto)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]
                { new SqlParameter("@Id_Utilizador", Id_Utilizador),
                new SqlParameter("@Tipo_Utilizador", Tipo_Utilizador),
                new SqlParameter("@Estado_Conta", Estado_Conta),
                new SqlParameter("@Email", Email),
                new SqlParameter("@Nome", Nome),
                new SqlParameter("@Palavra_Passe", Palavra_Passe),
                new SqlParameter("@prefixo_telefone", prefixo_telefone),
                new SqlParameter("@numero_telefone", numero_telefone),
                new SqlParameter("@Foto", SqlDbType.Image) { Value = Foto ?? (object)DBNull.Value }

                };
                return dal.executarNonQuery("update [utilizador] set [Tipo_Utilizador]=@Tipo_Utilizador, [Estado_Conta]=@Estado_Conta, [Email]=@Email, [Nome]=@Nome, [Palavra_Passe]=@Palavra_Passe, [prefixo_telefone]=@prefixo_telefone, [numero_telefone]=@numero_telefone, [Foto]=@Foto where Id_Utilizador=@Id_Utilizador", sqlParams);
            }


        }

        public class Carteira
        {
            public static void GarantirRegisto(int idUtilizador, DAL dal = null)
            {
                if (idUtilizador <= 0)
                    return;

                dal = dal ?? new DAL();
                dal.GarantirEsquema();

                object existe = dal.executarScalar(
                    "SELECT COUNT(*) FROM Carteira WHERE Id_Utilizador=@id",
                    new SqlParameter[] { new SqlParameter("@id", idUtilizador) });

                if (Convert.ToInt32(existe) == 0)
                {
                    dal.executarNonQuery(
                        "INSERT INTO Carteira (Id_Utilizador, Saldo) VALUES (@id, 0)",
                        new SqlParameter[] { new SqlParameter("@id", idUtilizador) });
                }
            }

            public static decimal ObterSaldo(int idUtilizador)
            {
                if (idUtilizador <= 0)
                    return 0m;

                var dal = new DAL();
                dal.GarantirEsquema();
                GarantirRegisto(idUtilizador, dal);

                object resultado = dal.executarScalar(
                    "SELECT Saldo FROM Carteira WHERE Id_Utilizador=@id",
                    new SqlParameter[] { new SqlParameter("@id", idUtilizador) });

                if (resultado == null || resultado == DBNull.Value)
                    return 0m;

                return Convert.ToDecimal(resultado);
            }

            public static void AtualizarSaldo(int idUtilizador, decimal saldo)
            {
                if (idUtilizador <= 0)
                    throw new InvalidOperationException("Utilizador inválido.");

                var dal = new DAL();
                dal.GarantirEsquema();
                GarantirRegisto(idUtilizador, dal);

                dal.executarNonQuery(
                    "UPDATE Carteira SET Saldo=@saldo WHERE Id_Utilizador=@id",
                    new SqlParameter[]
                    {
                        new SqlParameter("@id", idUtilizador),
                        new SqlParameter("@saldo", saldo)
                    });
            }

            public static void AdicionarSaldo(int idUtilizador, decimal valor)
            {
                if (idUtilizador <= 0)
                    throw new InvalidOperationException("Utilizador inválido.");

                if (valor <= 0)
                    throw new ArgumentOutOfRangeException(nameof(valor), "O valor a adicionar deve ser positivo.");

                AtualizarSaldo(idUtilizador, ObterSaldo(idUtilizador) + valor);
            }
        }

        //---------------------------------------------------------------

        public class Clientes
        {

            static public DataTable Load()
            {
                DAL dal = new DAL();
                return dal.executarReader("select * from Clientes", null);
            }
            static public int insertCliente(string nome, string morada, string telefone)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@nome", nome),
                new SqlParameter("@morada", morada),
                new SqlParameter("@telefone", telefone)
                };

                return dal.executarNonQuery("INSERT into Clientes (Nome,Morada,Telefone) VALUES(@nome,@morada,@telefone)", sqlParams);
            }
            static public DataTable queryClienteLike(String nome)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@nome", nome + "%")
                };
                return dal.executarReader("select * from Clientes where Nome like @nome", sqlParams);
            }
            static public DataTable queryClientePorId(int id)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id", id)
                };
                return dal.executarReader("select * from Clientes where ID=@id", sqlParams);
            }
            static public DataTable queryClientePorIdENome(int id, string nome)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id", id),
                 new SqlParameter("@Nome", nome)
                };
                return dal.executarReader("select * from Clientes where ID=@id and Nome=@nome", sqlParams);
            }
            static public int updateCliente(string id, string nome, string morada, string telefone)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id", id),
                new SqlParameter("@nome", nome),
                new SqlParameter("@morada", morada),
                new SqlParameter("@telefone", telefone)
            };
                return dal.executarNonQuery("update [Clientes] set [nome]=@nome, [morada]=@morada, [telefone]=@telefone where [id]=@id", sqlParams);
            }

            static public int deleteCliente(string id)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id", id),

            };
                return dal.executarNonQuery("Delete From Clientes WHERE[id]=@id", sqlParams);
            }

        }

        //---------------------------------------------------------------------------------------------------------------
        public class Compra
        {
            static public DataTable Load()
            {
                DAL dal = new DAL();
                return dal.executarReader("select * from Compra", null);
            }
            static public int insertCompra(int id, int idUtilizador, DateTime dataCompra, string titulo, string autor, decimal preco, string estadoLivro, int idLivro)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@Id", id),
                new SqlParameter("@Id_Utilizador", idUtilizador),
                new SqlParameter("@data_compra", dataCompra),
                new SqlParameter("@titulo", titulo),
                new SqlParameter("@autor", autor),
                new SqlParameter("@preco", preco),
                new SqlParameter("@estado_livro", estadoLivro),
                new SqlParameter("@id_livro", idLivro)
             };
                return dal.executarNonQuery(
                    "INSERT INTO Compra (Id, Data_Compra, Titulo, Autor, Preço, Estado_Livro, Id_Livro, Id_Utilizador) " +
                    "VALUES (@Id, @data_compra, @titulo, @autor, @preco, @estado_livro, @id_livro, @Id_Utilizador)",
                    sqlParams);
            }

        }
        //---------------------------------------------------------------------------------------------------------------
        public class Historicos
        {
            public const int MaxDiasEmprestimo = 21;
            public const int MaxDiasDevolucaoCompra = 30;
            public const int MultaSemanalCentimos = 200;
            private static readonly DateTime DataEntregaPendente = new DateTime(1900, 1, 1);

            public class ResultadoDevolucao
            {
                public string Titulo { get; set; }
                public string Autor { get; set; }
                public decimal ValorReembolso { get; set; }
                public int MultaCentimos { get; set; }
                public DateTime DataDevolucao { get; set; }
                public DateTime? DataReferencia { get; set; }
                public int DiasAtraso { get; set; }
                public int DiasRestantesPrazo { get; set; }
                public string Tipo { get; set; }
            }

            private static int ObterProximoId(DAL dal, string tabela)
            {
                object resultado = dal.executarScalar($"SELECT ISNULL(MAX(Id), 0) + 1 FROM [{tabela}]", null);
                return Convert.ToInt32(resultado);
            }

            private static decimal PrecoCentimosParaEuros(object precoDb)
            {
                if (precoDb == null || precoDb == DBNull.Value)
                    return 0m;

                return Convert.ToDecimal(precoDb) / 100m;
            }

            public static bool TemEmprestimoAtivo(int idUtilizador)
            {
                DAL dal = new DAL();
                object resultado = dal.executarScalar(
                    "SELECT COUNT(*) FROM HistoricoEmp WHERE Id_Utilizador=@id AND Estado_Emprestimo='Ativo' AND Data_Entrega=@pendente",
                    new SqlParameter[] {
                        new SqlParameter("@id", idUtilizador),
                        new SqlParameter("@pendente", DataEntregaPendente)
                    });
                return Convert.ToInt32(resultado) > 0;
            }

            public static int CalcularMultaCentimos(DateTime dataPrevista, DateTime? dataReferencia = null)
            {
                DateTime referencia = dataReferencia ?? DateTime.Now;
                if (referencia.Date <= dataPrevista.Date)
                    return 0;

                int diasAtraso = (referencia.Date - dataPrevista.Date).Days;
                int semanas = (int)Math.Ceiling(diasAtraso / 7.0);
                return semanas * MultaSemanalCentimos;
            }

            public static void AtualizarMultasEmAtraso()
            {
                DAL dal = new DAL();
                DataTable emAtraso = dal.executarReader(
                    "SELECT Id, Data_Prevista FROM HistoricoEmp WHERE Estado_Emprestimo='Ativo' AND Data_Entrega=@pendente AND Data_Prevista < GETDATE()",
                    new SqlParameter[] { new SqlParameter("@pendente", DataEntregaPendente) });

                foreach (DataRow row in emAtraso.Rows)
                {
                    int id = Convert.ToInt32(row["Id"]);
                    DateTime prevista = Convert.ToDateTime(row["Data_Prevista"]);
                    int multa = CalcularMultaCentimos(prevista);
                    dal.executarNonQuery(
                        "UPDATE HistoricoEmp SET Valor_Multa=@multa WHERE Id=@id",
                        new SqlParameter[] {
                            new SqlParameter("@multa", multa),
                            new SqlParameter("@id", id)
                        });
                }
            }

            static public int insertHistorico_de_compras(int id, DateTime data_compra, string titulo, string autor, decimal preco, string estado_livro, int id_livro, int Id_Utilizador)
            {
                DAL dal = new DAL();
                dal.GarantirEsquema();

                SqlParameter[] sqlParams = new SqlParameter[] {
                 new SqlParameter("@Id", id),
                 new SqlParameter("@data_compra", data_compra),
                 new SqlParameter("@titulo", titulo),
                 new SqlParameter("@autor", autor),
                 new SqlParameter("@preco", preco),
                 new SqlParameter("@estado_livro", estado_livro),
                 new SqlParameter("@id_livro", id_livro),
                 new SqlParameter("@Id_Utilizador", Id_Utilizador),
                };

                if (dal.ColunaExiste("Historico_Compra", "Estado_Compra"))
                {
                    return dal.executarNonQuery(
                        "INSERT INTO Historico_Compra (Id, Data_Compra, Titulo, Autor, Preço, Estado_Livro, Id_Livro, Id_Utilizador, Estado_Compra) " +
                        "VALUES (@Id, @data_compra, @titulo, @autor, @preco, @estado_livro, @id_livro, @Id_Utilizador, 'Ativa')",
                        sqlParams);
                }

                return dal.executarNonQuery(
                    "INSERT INTO Historico_Compra (Id, Data_Compra, Titulo, Autor, Preço, Estado_Livro, Id_Livro, Id_Utilizador) " +
                    "VALUES (@Id, @data_compra, @titulo, @autor, @preco, @estado_livro, @id_livro, @Id_Utilizador)",
                    sqlParams);
            }


            // Renomeado para evitar duplicidade
            public static DataTable LoadHistoricoComprasPorUtilizador(int Id_Utilizador)
            {
                DAL dal = new DAL();
                dal.GarantirEsquema();

                SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@Id_Utilizador", Id_Utilizador)
                };

                string estadoCol = dal.ColunaExiste("Historico_Compra", "Estado_Compra")
                    ? ", Estado_Compra"
                    : "";
                string dataDevCol = dal.ColunaExiste("Historico_Compra", "Data_Devolução")
                    ? ", Data_Devolução"
                    : "";

                string query = "SELECT Id, Data_Compra, Titulo, Autor, Preço, Estado_Livro, Id_Livro" + estadoCol + dataDevCol +
                               " FROM Historico_Compra WHERE Id_Utilizador = @Id_Utilizador ORDER BY Data_Compra DESC";

                return dal.executarReader(query, sqlParams);
            }

            public static DataTable LoadComprasDevolviveisPorUtilizador(int idUtilizador)
            {
                DAL dal = new DAL();
                dal.GarantirEsquema();

                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@Id_Utilizador", idUtilizador)
                };

                string filtroEstado = dal.ColunaExiste("Historico_Compra", "Estado_Compra")
                    ? " AND (Estado_Compra = 'Ativa' OR Estado_Compra IS NULL)"
                    : "";

                string query = "SELECT Id, Data_Compra, Titulo, Autor, Preço, Estado_Livro, Id_Livro " +
                               "FROM Historico_Compra WHERE Id_Utilizador = @Id_Utilizador" + filtroEstado +
                               $" AND Data_Compra >= DATEADD(day, -{MaxDiasDevolucaoCompra}, CAST(GETDATE() AS date)) " +
                               "ORDER BY Data_Compra DESC";

                return dal.executarReader(query, sqlParams);
            }

            public static ResultadoDevolucao ObterResumoDevolucaoCompra(int idHistorico, int idUtilizador)
            {
                DAL dal = new DAL();
                dal.GarantirEsquema();

                string estadoCol = dal.ColunaExiste("Historico_Compra", "Estado_Compra") ? ", Estado_Compra" : "";
                DataTable dt = dal.executarReader(
                    "SELECT Titulo, Autor, Preço, Data_Compra" + estadoCol +
                    " FROM Historico_Compra WHERE Id=@id AND Id_Utilizador=@user",
                    new SqlParameter[] {
                        new SqlParameter("@id", idHistorico),
                        new SqlParameter("@user", idUtilizador)
                    });

                if (dt == null || dt.Rows.Count == 0)
                    throw new Exception("Compra não encontrada.");

                DataRow row = dt.Rows[0];
                ValidarCompraDevolvivel(row, dal);

                DateTime dataCompra = Convert.ToDateTime(row["Data_Compra"]);
                int diasDesdeCompra = (DateTime.Now.Date - dataCompra.Date).Days;

                return new ResultadoDevolucao
                {
                    Titulo = row["Titulo"]?.ToString() ?? "",
                    Autor = row["Autor"]?.ToString() ?? "",
                    ValorReembolso = PrecoCentimosParaEuros(row["Preço"]),
                    DataDevolucao = DateTime.Now,
                    DataReferencia = dataCompra,
                    DiasRestantesPrazo = Math.Max(0, MaxDiasDevolucaoCompra - diasDesdeCompra),
                    Tipo = "Compra"
                };
            }

            public static ResultadoDevolucao ObterResumoDevolucaoEmprestimo(int idHistorico, int idUtilizador)
            {
                DAL dal = new DAL();
                dal.GarantirEsquema();
                AtualizarMultasEmAtraso();

                DataTable dt = dal.executarReader(
                    "SELECT h.Estado_Emprestimo, h.Data_Entrega, h.Data_Prevista, l.Titulo, l.Autor " +
                    "FROM HistoricoEmp h INNER JOIN Livro l ON h.Id_Livro = l.Id_Livro " +
                    "WHERE h.Id=@id AND h.Id_Utilizador=@user",
                    new SqlParameter[] {
                        new SqlParameter("@id", idHistorico),
                        new SqlParameter("@user", idUtilizador)
                    });

                if (dt == null || dt.Rows.Count == 0)
                    throw new Exception("Empréstimo não encontrado.");

                DataRow row = dt.Rows[0];
                ValidarEmprestimoDevolvivel(row);

                DateTime dataPrevista = Convert.ToDateTime(row["Data_Prevista"]);
                DateTime agora = DateTime.Now;
                int multa = CalcularMultaCentimos(dataPrevista, agora);
                int diasAtraso = agora.Date > dataPrevista.Date
                    ? (agora.Date - dataPrevista.Date).Days
                    : 0;

                return new ResultadoDevolucao
                {
                    Titulo = row["Titulo"]?.ToString() ?? "",
                    Autor = row["Autor"]?.ToString() ?? "",
                    MultaCentimos = multa,
                    DataDevolucao = agora,
                    DataReferencia = dataPrevista,
                    DiasAtraso = diasAtraso,
                    DiasRestantesPrazo = agora.Date <= dataPrevista.Date
                        ? (dataPrevista.Date - agora.Date).Days
                        : 0,
                    Tipo = "Emprestimo"
                };
            }

            public static ResultadoDevolucao DevolverCompra(int idHistorico, int idUtilizador)
            {
                if (idUtilizador <= 0)
                    throw new InvalidOperationException("É necessário iniciar sessão para devolver compras.");

                var resumo = ObterResumoDevolucaoCompra(idHistorico, idUtilizador);

                DAL dal = new DAL();
                dal.GarantirEsquema();

                DataTable dt = dal.executarReader(
                    "SELECT Id_Livro FROM Historico_Compra WHERE Id=@id AND Id_Utilizador=@user",
                    new SqlParameter[] {
                        new SqlParameter("@id", idHistorico),
                        new SqlParameter("@user", idUtilizador)
                    });

                int idLivro = Convert.ToInt32(dt.Rows[0]["Id_Livro"]);
                DateTime dataDevolucao = DateTime.Now;

                if (dal.ColunaExiste("Historico_Compra", "Estado_Compra"))
                {
                    dal.executarNonQuery(
                        "UPDATE Historico_Compra SET Estado_Compra='Devolvida' WHERE Id=@id",
                        new SqlParameter[] { new SqlParameter("@id", idHistorico) });
                }

                if (dal.ColunaExiste("Historico_Compra", "Data_Devolução"))
                {
                    dal.executarNonQuery(
                        "UPDATE Historico_Compra SET Data_Devolução=@data WHERE Id=@id",
                        new SqlParameter[] {
                            new SqlParameter("@data", dataDevolucao),
                            new SqlParameter("@id", idHistorico)
                        });
                }

                Devolução.insertDevoluçãoCompra(idUtilizador, idLivro, dataDevolucao);
                Livros.IncrementarStock(idLivro);
                Carteira.AdicionarSaldo(idUtilizador, resumo.ValorReembolso);

                Notificacoes.Criar(idUtilizador, idLivro,
                    $"Devolução da compra \"{resumo.Titulo}\" registada. Reembolso de {resumo.ValorReembolso:C2} creditado na carteira.");

                resumo.DataDevolucao = dataDevolucao;
                return resumo;
            }

            private static void ValidarCompraDevolvivel(DataRow row, DAL dal)
            {
                if (dal.ColunaExiste("Historico_Compra", "Estado_Compra"))
                {
                    string estado = row["Estado_Compra"]?.ToString() ?? "Ativa";
                    if (estado == "Devolvida")
                        throw new InvalidOperationException("Esta compra já foi devolvida.");
                }

                DateTime dataCompra = Convert.ToDateTime(row["Data_Compra"]);
                int diasDesdeCompra = (DateTime.Now.Date - dataCompra.Date).Days;
                if (diasDesdeCompra > MaxDiasDevolucaoCompra)
                    throw new InvalidOperationException(
                        $"O prazo de devolução expirou ({MaxDiasDevolucaoCompra} dias após a compra).");
            }

            private static void ValidarEmprestimoDevolvivel(DataRow row)
            {
                if (row["Estado_Emprestimo"]?.ToString() != "Ativo")
                    throw new InvalidOperationException("Apenas empréstimos ativos podem ser devolvidos.");

                if (Convert.ToDateTime(row["Data_Entrega"]) != DataEntregaPendente)
                    throw new InvalidOperationException("Este empréstimo já foi devolvido.");
            }

            static public int insertHistoricoEmp(string Estado_Emprestimo, DateTime Data_Entrega, DateTime Data_Prevista, DateTime Data_Levantamento, int Id_Livro, int Id_Utilizador, int duracaoDias = 14)
            {
                DAL dal = new DAL();
                int novoId = ObterProximoId(dal, "HistoricoEmp");

                SqlParameter[] sqlParamsBase = new SqlParameter[] {
                    new SqlParameter("@Id", SqlDbType.Int) { Value = novoId },
                    new SqlParameter("@Estado_Emprestimo", SqlDbType.NVarChar, 20) { Value = Estado_Emprestimo ?? "" },
                    new SqlParameter("@Data_Entrega", SqlDbType.DateTime) { Value = Data_Entrega },
                    new SqlParameter("@Data_Prevista", SqlDbType.DateTime) { Value = Data_Prevista },
                    new SqlParameter("@Data_Levantamento", SqlDbType.DateTime) { Value = Data_Levantamento },
                    new SqlParameter("@Id_Livro", SqlDbType.Int) { Value = Id_Livro },
                    new SqlParameter("@Id_Utilizador", SqlDbType.Int) { Value = Id_Utilizador }
                };

                if (dal.ColunaExiste("HistoricoEmp", "Duracao_Dias"))
                {
                    var parametros = new SqlParameter[sqlParamsBase.Length + 1];
                    sqlParamsBase.CopyTo(parametros, 0);
                    parametros[parametros.Length - 1] = new SqlParameter("@Duracao_Dias", SqlDbType.Int) { Value = duracaoDias };

                    return dal.executarNonQuery(
                        "INSERT INTO HistoricoEmp (Id, Estado_Emprestimo, Data_Entrega, Data_Prevista, Data_Levantamento, Id_Livro, Id_Utilizador, Duracao_Dias) " +
                        "VALUES (@Id, @Estado_Emprestimo, @Data_Entrega, @Data_Prevista, @Data_Levantamento, @Id_Livro, @Id_Utilizador, @Duracao_Dias)",
                        parametros);
                }

                return dal.executarNonQuery(
                    "INSERT INTO HistoricoEmp (Id, Estado_Emprestimo, Data_Entrega, Data_Prevista, Data_Levantamento, Id_Livro, Id_Utilizador) " +
                    "VALUES (@Id, @Estado_Emprestimo, @Data_Entrega, @Data_Prevista, @Data_Levantamento, @Id_Livro, @Id_Utilizador)",
                    sqlParamsBase);
            }

            public static void RegistrarCompra(int idUtilizador, int idLivro, int quantidade)
            {
                if (idUtilizador <= 0)
                    throw new InvalidOperationException("É necessário iniciar sessão para comprar.");

                if (quantidade <= 0)
                    return;

                DataTable dtLivro = Livros.ObterLivroPorId(idLivro);
                if (dtLivro == null || dtLivro.Rows.Count == 0)
                    throw new Exception("Livro não encontrado.");

                DataRow livro = dtLivro.Rows[0];
                string titulo = livro["Titulo"]?.ToString() ?? "";
                string autor = livro["Autor"]?.ToString() ?? "";
                decimal preco = Convert.ToDecimal(livro["Preço"]);
                string estado = livro["Estado_Livro"]?.ToString() ?? "";
                int stock = Livros.ObterStock(idLivro);

                if (stock <= 0)
                    throw new InvalidOperationException($"\"{titulo}\" está esgotado. Para reservar ou emprestar, use Requisições/Empréstimos.");

                if (quantidade > stock)
                    throw new InvalidOperationException($"Stock insuficiente para \"{titulo}\". Disponível: {stock}.");

                DateTime dataCompra = DateTime.Now;
                DAL dal = new DAL();
                for (int i = 0; i < quantidade; i++)
                {
                    int idCompra = ObterProximoId(dal, "Compra");
                    int idHistorico = ObterProximoId(dal, "Historico_Compra");
                    Compra.insertCompra(idCompra, idUtilizador, dataCompra, titulo, autor, preco, estado, idLivro);
                    insertHistorico_de_compras(idHistorico, dataCompra, titulo, autor, preco, estado, idLivro, idUtilizador);
                    Livros.DecrementarStock(idLivro);
                }
            }

            public static void RegistrarEmprestimo(int idUtilizador, int idLivro, int diasDevolucao = 14)
            {
                if (idUtilizador <= 0)
                    throw new InvalidOperationException("É necessário iniciar sessão para requisitar livros.");

                if (diasDevolucao < 0 || diasDevolucao > MaxDiasEmprestimo)
                    throw new InvalidOperationException($"O prazo de devolução deve ser entre 0 e {MaxDiasEmprestimo} dias.");

                if (TemEmprestimoAtivo(idUtilizador))
                    throw new InvalidOperationException("Só pode ter um empréstimo ativo. Devolva o livro atual antes de requisitar outro.");

                DataTable dtLivro = Livros.ObterLivroPorId(idLivro);
                if (dtLivro == null || dtLivro.Rows.Count == 0)
                    throw new Exception("Livro não encontrado.");

                int stock = Livros.ObterStock(idLivro);
                if (stock <= 0)
                    throw new InvalidOperationException("Este livro está esgotado. Pode reservá-lo e será notificado quando estiver disponível.");

                new DAL().GarantirEsquema();

                DateTime levantamento = DateTime.Now;
                DateTime prevista = levantamento.AddDays(diasDevolucao);

                insertHistoricoEmp("Ativo", DataEntregaPendente, prevista, levantamento, idLivro, idUtilizador, diasDevolucao);
                Livros.DecrementarStock(idLivro);
            }

            public static void RegistrarReserva(int idUtilizador, int idLivro)
            {
                if (idUtilizador <= 0)
                    throw new InvalidOperationException("É necessário iniciar sessão para reservar livros.");

                DataTable dtLivro = Livros.ObterLivroPorId(idLivro);
                if (dtLivro == null || dtLivro.Rows.Count == 0)
                    throw new Exception("Livro não encontrado.");

                int stock = Livros.ObterStock(idLivro);
                if (stock > 0)
                    throw new InvalidOperationException("Este livro está disponível em stock. Pode requisitá-lo diretamente.");

                if (TemReservaAtiva(idUtilizador, idLivro))
                    throw new InvalidOperationException("Já tem uma reserva ativa para este livro.");

                new DAL().GarantirEsquema();

                DateTime reserva = DateTime.Now;
                DateTime limiteLevantamento = reserva.AddDays(7);

                insertHistoricoEmp("Reservado", DataEntregaPendente, limiteLevantamento, reserva, idLivro, idUtilizador, 0);
            }

            public static bool TemReservaAtiva(int idUtilizador, int idLivro)
            {
                DAL dal = new DAL();
                object resultado = dal.executarScalar(
                    "SELECT COUNT(*) FROM HistoricoEmp WHERE Id_Utilizador=@id AND Id_Livro=@livro AND Estado_Emprestimo='Reservado' AND Data_Entrega=@pendente",
                    new SqlParameter[] {
                        new SqlParameter("@id", idUtilizador),
                        new SqlParameter("@livro", idLivro),
                        new SqlParameter("@pendente", DataEntregaPendente)
                    });
                return Convert.ToInt32(resultado) > 0;
            }

            public static DataTable LoadReservasPorUtilizador(int idUtilizador)
            {
                DAL dal = new DAL();
                return dal.executarReader(
                    "SELECT h.Id, h.Data_Levantamento, h.Data_Prevista, h.Id_Livro, l.Titulo, l.Autor, l.Stock " +
                    "FROM HistoricoEmp h INNER JOIN Livro l ON h.Id_Livro = l.Id_Livro " +
                    "WHERE h.Id_Utilizador=@id AND h.Estado_Emprestimo='Reservado' AND h.Data_Entrega=@pendente " +
                    "ORDER BY h.Data_Levantamento ASC",
                    new SqlParameter[] {
                        new SqlParameter("@id", idUtilizador),
                        new SqlParameter("@pendente", DataEntregaPendente)
                    });
            }

            public static ResultadoDevolucao DevolverEmprestimo(int idHistorico, int idUtilizador)
            {
                var resumo = ObterResumoDevolucaoEmprestimo(idHistorico, idUtilizador);

                DAL dal = new DAL();
                dal.GarantirEsquema();

                DataTable dt = dal.executarReader(
                    "SELECT Id_Livro, Data_Prevista FROM HistoricoEmp WHERE Id=@id AND Id_Utilizador=@user",
                    new SqlParameter[] {
                        new SqlParameter("@id", idHistorico),
                        new SqlParameter("@user", idUtilizador)
                    });

                int idLivro = Convert.ToInt32(dt.Rows[0]["Id_Livro"]);
                DateTime dataPrevista = Convert.ToDateTime(dt.Rows[0]["Data_Prevista"]);
                DateTime dataDevolucao = DateTime.Now;
                int multa = CalcularMultaCentimos(dataPrevista, dataDevolucao);

                dal.executarNonQuery(
                    "UPDATE HistoricoEmp SET Estado_Emprestimo='Devolvido', Data_Entrega=@entrega, Valor_Multa=@multa WHERE Id=@id",
                    new SqlParameter[] {
                        new SqlParameter("@entrega", dataDevolucao),
                        new SqlParameter("@multa", multa),
                        new SqlParameter("@id", idHistorico)
                    });

                Devolução.insertDevoluçãoEmp(idUtilizador, idLivro, dataDevolucao);
                Livros.IncrementarStock(idLivro);
                NotificarReservasDisponiveis(idLivro);

                string msgMulta = multa > 0
                    ? $" Multa de {(multa / 100m):C2} aplicada por atraso."
                    : "";
                Notificacoes.Criar(idUtilizador, idLivro,
                    $"Devolução de \"{resumo.Titulo}\" registada com sucesso.{msgMulta}");

                resumo.MultaCentimos = multa;
                resumo.DataDevolucao = dataDevolucao;
                return resumo;
            }

            private static void NotificarReservasDisponiveis(int idLivro)
            {
                if (Livros.ObterStock(idLivro) <= 0)
                    return;

                DAL dal = new DAL();
                DataTable reservas = dal.executarReader(
                    "SELECT TOP 1 h.Id_Utilizador, l.Titulo FROM HistoricoEmp h " +
                    "INNER JOIN Livro l ON h.Id_Livro = l.Id_Livro " +
                    "WHERE h.Id_Livro=@livro AND h.Estado_Emprestimo='Reservado' AND h.Data_Entrega=@pendente " +
                    "ORDER BY h.Data_Levantamento ASC",
                    new SqlParameter[] {
                        new SqlParameter("@livro", idLivro),
                        new SqlParameter("@pendente", DataEntregaPendente)
                    });

                if (reservas == null || reservas.Rows.Count == 0)
                    return;

                int idUtilizador = Convert.ToInt32(reservas.Rows[0]["Id_Utilizador"]);
                string titulo = reservas.Rows[0]["Titulo"]?.ToString() ?? "Livro";
                Notificacoes.Criar(idUtilizador, idLivro,
                    $"O livro \"{titulo}\" está novamente disponível! Pode requisitá-lo na biblioteca.");
            }

            public static DataTable LoadHistoricoEmpPorUtilizador(int idUtilizador)
            {
                AtualizarMultasEmAtraso();
                DAL dal = new DAL();

                SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@Id_Utilizador", idUtilizador)
                };

                string query = "SELECT h.Id, h.Estado_Emprestimo, h.Data_Entrega, h.Data_Prevista, h.Data_Levantamento, " +
                               "h.Id_Livro, h.Id_Utilizador, h.Duracao_Dias, h.Valor_Multa, h.Multa_Paga, l.Titulo, l.Autor " +
                               "FROM HistoricoEmp h " +
                               "INNER JOIN Livro l ON h.Id_Livro = l.Id_Livro " +
                               "WHERE h.Id_Utilizador = @Id_Utilizador";

                return dal.executarReader(query, sqlParams);
            }

            public static DataTable LoadHistoricoEmpTodos()
            {
                AtualizarMultasEmAtraso();
                DAL dal = new DAL();
                return dal.executarReader(
                    "SELECT h.Id, h.Estado_Emprestimo, h.Data_Entrega, h.Data_Prevista, h.Data_Levantamento, " +
                    "h.Duracao_Dias, h.Valor_Multa, h.Multa_Paga, h.Id_Livro, h.Id_Utilizador, u.Nome AS Utilizador, l.Titulo, l.Autor " +
                    "FROM HistoricoEmp h " +
                    "INNER JOIN Livro l ON h.Id_Livro = l.Id_Livro " +
                    "INNER JOIN utilizador u ON h.Id_Utilizador = u.Id_Utilizador " +
                    "WHERE h.Estado_Emprestimo IN ('Ativo','Devolvido') " +
                    "ORDER BY h.Data_Levantamento DESC",
                    null);
            }

            public static DataTable LoadRelatorioMultas()
            {
                AtualizarMultasEmAtraso();
                DAL dal = new DAL();
                return dal.executarReader(
                    "SELECT h.Id, h.Id_Utilizador, u.Nome AS Utilizador, l.Titulo, h.Data_Prevista, h.Data_Entrega, " +
                    "h.Valor_Multa, h.Multa_Paga, h.Estado_Emprestimo " +
                    "FROM HistoricoEmp h " +
                    "INNER JOIN Livro l ON h.Id_Livro = l.Id_Livro " +
                    "INNER JOIN utilizador u ON h.Id_Utilizador = u.Id_Utilizador " +
                    "WHERE h.Valor_Multa > 0 " +
                    "ORDER BY h.Valor_Multa DESC, h.Data_Prevista ASC",
                    null);
            }

            public static void MarcarMultaComoPaga(int idHistorico)
            {
                DAL dal = new DAL();
                dal.executarNonQuery(
                    "UPDATE HistoricoEmp SET Multa_Paga=1 WHERE Id=@id",
                    new SqlParameter[] { new SqlParameter("@id", idHistorico) });
            }
        }

        public class Notificacoes
        {
            private static int ObterProximoId(DAL dal)
            {
                object resultado = dal.executarScalar("SELECT ISNULL(MAX(Id), 0) + 1 FROM Notificacao", null);
                return Convert.ToInt32(resultado);
            }

            public static void Criar(int idUtilizador, int? idLivro, string mensagem)
            {
                DAL dal = new DAL();
                int id = ObterProximoId(dal);
                dal.executarNonQuery(
                    "INSERT INTO Notificacao (Id, Id_Utilizador, Id_Livro, Mensagem, Lida, Data_Criacao) " +
                    "VALUES (@id, @user, @livro, @msg, 0, GETDATE())",
                    new SqlParameter[] {
                        new SqlParameter("@id", id),
                        new SqlParameter("@user", idUtilizador),
                        new SqlParameter("@livro", (object)idLivro ?? DBNull.Value),
                        new SqlParameter("@msg", mensagem)
                    });
            }

            public static DataTable LoadNaoLidas(int idUtilizador)
            {
                DAL dal = new DAL();
                return dal.executarReader(
                    "SELECT Id, Mensagem, Data_Criacao, Id_Livro FROM Notificacao " +
                    "WHERE Id_Utilizador=@id AND Lida=0 ORDER BY Data_Criacao DESC",
                    new SqlParameter[] { new SqlParameter("@id", idUtilizador) });
            }

            public static int ContarNaoLidas(int idUtilizador)
            {
                DAL dal = new DAL();
                object resultado = dal.executarScalar(
                    "SELECT COUNT(*) FROM Notificacao WHERE Id_Utilizador=@id AND Lida=0",
                    new SqlParameter[] { new SqlParameter("@id", idUtilizador) });
                return Convert.ToInt32(resultado);
            }

            public static void MarcarComoLida(int idNotificacao, int idUtilizador)
            {
                DAL dal = new DAL();
                dal.executarNonQuery(
                    "UPDATE Notificacao SET Lida=1 WHERE Id=@id AND Id_Utilizador=@user",
                    new SqlParameter[] {
                        new SqlParameter("@id", idNotificacao),
                        new SqlParameter("@user", idUtilizador)
                    });
            }

            public static void MarcarTodasComoLidas(int idUtilizador)
            {
                DAL dal = new DAL();
                dal.executarNonQuery(
                    "UPDATE Notificacao SET Lida=1 WHERE Id_Utilizador=@id",
                    new SqlParameter[] { new SqlParameter("@id", idUtilizador) });
            }
        }

        public class Estatisticas
        {
            private static int ExecutarContagem(string sql, SqlParameter[] parametros = null)
            {
                DAL dal = new DAL();
                object resultado = dal.executarScalar(sql, parametros);
                return Convert.ToInt32(resultado);
            }

            public static int TotalUtilizadores() =>
                ExecutarContagem("SELECT COUNT(*) FROM utilizador");

            public static int TotalLivros() =>
                ExecutarContagem("SELECT COUNT(*) FROM Livro");

            public static int TotalStock() =>
                ExecutarContagem("SELECT ISNULL(SUM(Stock), 0) FROM Livro");

            public static int EmprestimosAtivos() =>
                ExecutarContagem("SELECT COUNT(*) FROM HistoricoEmp WHERE Estado_Emprestimo='Ativo' AND Data_Entrega='1900-01-01'");

            public static int EmprestimosEmAtraso()
            {
                BLL.Historicos.AtualizarMultasEmAtraso();
                return ExecutarContagem("SELECT COUNT(*) FROM HistoricoEmp WHERE Estado_Emprestimo='Ativo' AND Data_Entrega='1900-01-01' AND Data_Prevista < GETDATE()");
            }

            public static int ReservasPendentes() =>
                ExecutarContagem("SELECT COUNT(*) FROM HistoricoEmp WHERE Estado_Emprestimo='Reservado' AND Data_Entrega='1900-01-01'");

            public static int ComprasEsteMes() =>
                ExecutarContagem("SELECT COUNT(*) FROM Historico_Compra WHERE MONTH(Data_Compra)=MONTH(GETDATE()) AND YEAR(Data_Compra)=YEAR(GETDATE())");

            public static decimal ReceitaTotalCentimos()
            {
                DAL dal = new DAL();
                object resultado = dal.executarScalar("SELECT ISNULL(SUM(Preço), 0) FROM Historico_Compra", null);
                return Convert.ToDecimal(resultado);
            }

            public static decimal MultasPendentesCentimos()
            {
                BLL.Historicos.AtualizarMultasEmAtraso();
                DAL dal = new DAL();
                object resultado = dal.executarScalar(
                    "SELECT ISNULL(SUM(Valor_Multa), 0) FROM HistoricoEmp WHERE Valor_Multa > 0 AND Multa_Paga=0",
                    null);
                return Convert.ToDecimal(resultado);
            }

            public static DataTable TopLivrosEmprestados(int top = 5)
            {
                DAL dal = new DAL();
                return dal.executarReader(
                    $"SELECT TOP {top} l.Titulo, COUNT(*) AS TotalEmprestimos " +
                    "FROM HistoricoEmp h INNER JOIN Livro l ON h.Id_Livro = l.Id_Livro " +
                    "WHERE h.Estado_Emprestimo IN ('Ativo','Devolvido') " +
                    "GROUP BY l.Titulo ORDER BY TotalEmprestimos DESC",
                    null);
            }

            public static DataTable ResumoGeral()
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Indicador", typeof(string));
                dt.Columns.Add("Valor", typeof(string));

                dt.Rows.Add("Total de utilizadores", TotalUtilizadores().ToString());
                dt.Rows.Add("Total de livros", TotalLivros().ToString());
                dt.Rows.Add("Stock total disponível", TotalStock().ToString());
                dt.Rows.Add("Empréstimos ativos", EmprestimosAtivos().ToString());
                dt.Rows.Add("Empréstimos em atraso", EmprestimosEmAtraso().ToString());
                dt.Rows.Add("Reservas pendentes", ReservasPendentes().ToString());
                dt.Rows.Add("Compras este mês", ComprasEsteMes().ToString());
                dt.Rows.Add("Receita total", (ReceitaTotalCentimos() / 100m).ToString("C2"));
                dt.Rows.Add("Multas pendentes", (MultasPendentesCentimos() / 100m).ToString("C2"));

                return dt;
            }
        }
    
        

        //---------------------------------------------------------------------------------------------------------------
        public class  Devolução
        {
            static public int insertDevoluçãoCompra (int id_utilizador, int id_livro, DateTime data_devolução)
            {
                DAL dal = new DAL();
                dal.GarantirEsquema();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id_utilizador", id_utilizador),
                new SqlParameter("@id_livro", id_livro),
                new SqlParameter("@data_devolução", data_devolução)
             };
                return dal.executarNonQuery("INSERT INTO [DevoluçãoCompra] (Id_Utilizador, Id_Livro, Data_Devolução) VALUES(@id_utilizador,@id_livro,@data_devolução)", sqlParams);
            }
             public static DataTable Load()
            {
                DataTable dal = new DataTable();
                return dal;
            }


            static public int insertDevoluçãoEmp(int id_utilizador, int id_livro, DateTime data_devolução)
            {
                DAL dal = new DAL();
                dal.GarantirEsquema();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id_utilizador", id_utilizador),
                new SqlParameter("@id_livro", id_livro),
                new SqlParameter("@data_devolução", data_devolução)
             };
                return dal.executarNonQuery("INSERT INTO [DevoluçãoEmp] (Id_Utilizador, Id_Livro, Data_Devolução) VALUES(@id_utilizador,@id_livro,@data_devolução)", sqlParams);
            }
             public static DataTable LoadEmp()
            {
                DataTable dal = new DataTable();
                return dal;
            }

        }






        //---------------------------------------------------------------------------------------------------------------
       







        public class Imagem
        {
            static public object loadpic()
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id", 1),
             };
                return dal.executarScalar("select Img from Imagem where id=@id", sqlParams);

            }
            static public DataTable Load()
            {
                DAL dal = new DAL();
                return dal.executarReader("select * from Imagem", null);
            }

            static public int insertImagem(byte[] img)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@img", img),

           };

                return dal.executarNonQuery("INSERT into Imagem (Img) VALUES(@img)", sqlParams);
            }
        }



        //----------------------LIVROS---------------------------- 
        public class Livros
        {
            //listar livros
            static public DataTable Load()
            {
                DAL dal = new DAL();
                return dal.executarReader("select * from Livro", null);
            }
            // Método para inserir um novo livro com gêneros e tipos associados


            static public void InserirLivro(int paginas, string nome, string bio, int preço, int ano, string autor, string estado_livro, string editora, string idioma, object capa, List<string> generos, int stock = 1)
            {
                DAL dal = new DAL();

                try
                {
                    if (stock < 0)
                        throw new Exception("O stock não pode ser negativo.");

                    // 1. Parâmetros para a tabela Livro
                    SqlParameter[] sqlParams = new SqlParameter[]{
            new SqlParameter("@Quantas_Paginas", paginas),
            new SqlParameter("@Titulo", nome),
            new SqlParameter("@Bio", bio),
            new SqlParameter("@Preço", preço),
            new SqlParameter("@Ano", ano),
            new SqlParameter("@Autor", autor),
            new SqlParameter("@Estado_Livro", estado_livro),
            new SqlParameter("@Editora", editora),
            new SqlParameter("@Idioma", idioma),
            new SqlParameter("@Capa", capa != null ? (object)capa : DBNull.Value),
            new SqlParameter("@Stock", stock)
        };

                    // 2. Insere na tabela Livro e obtém o ID gerado
                    object resultado = dal.executarScalar(
                        "INSERT INTO Livro (Quantas_Paginas, Titulo, Bio, Preço, Ano, Autor, Estado_Livro, Editora, Idioma, Capa, Stock) " +
                        "OUTPUT INSERTED.Id_Livro " +
                        "VALUES (@Quantas_Paginas, @Titulo, @Bio, @Preço, @Ano, @Autor, @Estado_Livro, @Editora, @Idioma, @Capa, @Stock)",
                        sqlParams);

                    if (resultado == null || resultado == DBNull.Value)
                    {
                        throw new Exception("Falha ao obter o ID do livro inserido.");
                    }

                    int livroId = Convert.ToInt32(resultado);

                    // 3. Inserção na tabela de ligação Livro_Genero
                    if (generos != null && generos.Count > 0)
                    {
                        foreach (var genero in generos)
                        {
                            if (string.IsNullOrWhiteSpace(genero))
                                continue;

                            string generoLimpo = genero.Trim();

                            // Busca o Id_Genero pela Categoria
                            object objGeneroId = dal.executarScalar(
                                "SELECT Id_Genero FROM Genero WHERE Categoria = @categoria",
                                new SqlParameter[] { new SqlParameter("@categoria", generoLimpo) });

                            if (objGeneroId != null && objGeneroId != DBNull.Value)
                            {
                                int generoId = Convert.ToInt32(objGeneroId);

                                // Insere na tabela de ligação Livro_Genero
                                dal.executarNonQuery(
                                    "INSERT INTO Livro_Genero (Id_Livro, Id_Genero) VALUES (@livroId, @generoId)",
                                    new SqlParameter[] {
                            new SqlParameter("@livroId", livroId),
                            new SqlParameter("@generoId", generoId)
                        });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao inserir livro: " + ex.Message, ex);
                }
            }
            public static void AdicionarStock(int idLivro, int quantidade)
            {
                if (quantidade <= 0)
                    throw new ArgumentException("A quantidade deve ser maior que zero.");

                DAL dal = new DAL();
                int rows = dal.executarNonQuery(
                    "UPDATE Livro SET Stock = ISNULL(Stock, 0) + @qty WHERE Id_Livro=@id",
                    new SqlParameter[]
                    {
                        new SqlParameter("@id", idLivro),
                        new SqlParameter("@qty", quantidade)
                    });
            }

            public static DataTable LoadStockResumo()
            {
                DAL dal = new DAL();
                return dal.executarReader(
                    "SELECT Id_Livro, Titulo, Autor, ISNULL(Stock, 0) AS Stock, Estado_Livro, Preço FROM Livro ORDER BY Titulo",
                    null);
            }
            static public List<string> ObterGeneros()
            {
                DAL dal = new DAL();
                DataTable dt = dal.executarReader("SELECT Categoria FROM Genero ORDER BY Categoria", null);
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.AsEnumerable()
                             .Where(r => r["Categoria"] != DBNull.Value)
                             .Select(r => r["Categoria"].ToString().Trim())
                             .Distinct()
                             .ToList();
                }
                
                return new List<string>();
            }
            static public List<string> ObterEstados()
            {
                DAL dal = new DAL();
                DataTable dt = dal.executarReader("SELECT Estado_Livro FROM Livro GROUP BY Estado_Livro ORDER BY Estado_Livro", null);
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.AsEnumerable()
                             .Where(r => r["Estado_Livro"] != DBNull.Value)
                             .Select(r => r["Estado_Livro"].ToString().Trim())
                             .Distinct()
                             .ToList();
                }
                
                return new List<string>();
            }

            static public DataTable obterestadosTabela()
            {
                DAL dal = new DAL();
                // Garantir valores únicos, sem espaços e ordenados
                return dal.executarReader(
                    "SELECT DISTINCT Estado_Livro FROM Livro ORDER BY Estado_Livro",
                    null);
            }
            static public DataTable ObterGenerosTabela()
            {
                DAL dal = new DAL();
                // Garantir valores únicos, sem espaços e ordenados
                return dal.executarReader(
                    "SELECT DISTINCT Id_Genero, LTRIM(RTRIM(Categoria)) AS Categoria FROM Genero ORDER BY Categoria",
                    null);
            }

            static public DataTable pesquisarLivro(string titulo, string autor, string genero, string estado)
            {
                DAL dal = new DAL();
                try
                {
                    return dal.executarReader(
                        "SELECT DISTINCT l.Id_Livro, l.Titulo, l.Autor, l.Preço, l.Estado_Livro " +
                        "FROM Livro l " +
                        "LEFT JOIN Livro_Genero lg ON l.Id_Livro = lg.Id_Livro " +
                        "LEFT JOIN Genero g ON lg.Id_Genero = g.Id_Genero " +
                        "WHERE (@titulo IS NULL OR l.Titulo LIKE '%' + @titulo + '%') " +
                        "AND (@autor IS NULL OR l.Autor LIKE '%' + @autor + '%') " +
                        "AND (@genero IS NULL OR g.Categoria = @genero) " +
                        "AND (@estado IS NULL OR l.Estado_Livro = @estado)",
                        new SqlParameter[] {
                            new SqlParameter("@titulo", string.IsNullOrWhiteSpace(titulo) ? (object)DBNull.Value : titulo),
                            new SqlParameter("@autor", string.IsNullOrWhiteSpace(autor) ? (object)DBNull.Value : autor),
                            new SqlParameter("@genero", string.IsNullOrWhiteSpace(genero) || genero == "Todos" ? (object)DBNull.Value : genero),
                            new SqlParameter("@estado", string.IsNullOrWhiteSpace(estado) || estado == "Todos" ? (object)DBNull.Value : estado)
                        });
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao pesquisar livros: " + ex.Message, ex);
                }
            }

            static public DataTable ObterLivroPorId(int idLivro)
            {
                DAL dal = new DAL();
                try
                {
                    return dal.executarReader(
                        "SELECT Id_Livro, Titulo, Autor, Preço, Estado_Livro, Stock FROM Livro WHERE Id_Livro = @idLivro",
                        new SqlParameter[] { new SqlParameter("@idLivro", idLivro) });
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao obter livro: " + ex.Message, ex);
                }
            }

            public static int ObterStock(int idLivro)
            {
                DAL dal = new DAL();
                object resultado = dal.executarScalar(
                    "SELECT ISNULL(Stock, 0) FROM Livro WHERE Id_Livro=@id",
                    new SqlParameter[] { new SqlParameter("@id", idLivro) });
                return Convert.ToInt32(resultado);
            }

            public static void DecrementarStock(int idLivro)
            {
                DAL dal = new DAL();
                dal.executarNonQuery(
                    "UPDATE Livro SET Stock = Stock - 1 WHERE Id_Livro=@id AND Stock > 0",
                    new SqlParameter[] { new SqlParameter("@id", idLivro) });
            }

            public static void IncrementarStock(int idLivro)
            {
                DAL dal = new DAL();
                dal.executarNonQuery(
                    "UPDATE Livro SET Stock = Stock + 1 WHERE Id_Livro=@id",
                    new SqlParameter[] { new SqlParameter("@id", idLivro) });
            }

            static public List<string> ObterGenerosLivro(int idLivro)
            {
                DAL dal = new DAL();
                DataTable dt = dal.executarReader(
                    "SELECT g.Categoria FROM Genero g " +
                    "INNER JOIN Livro_Genero lg ON g.Id_Genero = lg.Id_Genero " +
                    "WHERE lg.Id_Livro = @idLivro " +
                    "ORDER BY g.Categoria",
                    new SqlParameter[] { new SqlParameter("@idLivro", idLivro) });
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.AsEnumerable()
                             .Where(r => r["Categoria"] != DBNull.Value)
                             .Select(r => r["Categoria"].ToString().Trim())
                             .Distinct()
                             .ToList();
                }
                
                return new List<string>();
            }
        }
    }
}