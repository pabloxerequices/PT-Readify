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
            static public int insertCompra(int id_cliente, int id_livro, DateTime data_compra)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id_cliente", id_cliente),
                new SqlParameter("@id_livro", id_livro),
                new SqlParameter("@data_compra", data_compra)
             };
                return dal.executarNonQuery("INSERT into Compra (Id_Cliente, Id_Livro, Data_Compra) VALUES(@id_cliente,@id_livro,@data_compra)", sqlParams);
            }

        }
        //---------------------------------------------------------------------------------------------------------------
        public class Historicos
        {
            static public int insertHistorico_de_compras(DateTime data_compra, string titulo, string autor, int preco, string estado_livro, int id_livro, int Id_Utilizador)
            {
                DAL dal = new DAL();

                // Criamos os parâmetros com todos os campos da tabela
                SqlParameter[] sqlParams = new SqlParameter[] {
                 new SqlParameter("@data_compra", data_compra),
                 new SqlParameter("@titulo", titulo),
                 new SqlParameter("@autor", autor),
                 new SqlParameter("@preco", preco),
                 new SqlParameter("@estado_livro", estado_livro),
                 new SqlParameter("@id_livro", id_livro),
                 new SqlParameter("@Id_Utilizador", Id_Utilizador),
                };

                // Executamos o INSERT mapeando cada coluna ao seu respetivo parâmetro
                return dal.executarNonQuery(
                    "INSERT into Historico_Compra ( Data_Compra, Titulo, Autor, Preço, Estado_Livro,Id_Livro,Id_Utilizador) " +
                    "VALUES (@data_compra, @titulo, @autor, @preco, @estado_livro, @id_livro, @Id_Utilizador)",
                    sqlParams);
            }


            // Renomeado para evitar duplicidade
            public static DataTable LoadHistoricoComprasPorUtilizador(int Id_Utilizador)
            {
                DAL dal = new DAL();

                // Criamos o parâmetro baseado no ID do cliente logado
                SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@Id_Utilizador", Id_Utilizador)
                };

                // CORREÇÃO: Mudado de Historico_do_compras para Historico_de_compras
                string query = "SELECT Id, Data_Compra, Titulo, Autor, Preço, Estado_Livro, Id_Livro " +
                               "FROM Historico_Compra WHERE Id_Utilizador = @Id_Utilizador";

                // Chamada ao método correto da tua DAL que preenche a DataTable
                DataTable dt = dal.executarReader(query, sqlParams);

                return dt;
            }

            static public int insertHistoricoEmp(string Estado_Emprestimo, DateTime Data_Entrega, DateTime Data_Prevista, DateTime Data_Levantamento, int Id_Livro, int Id_Utilizador)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@Estado_Emprestimo", Estado_Emprestimo),
                    new SqlParameter("@Data_Entrega", Data_Entrega),
                    new SqlParameter("@Data_Prevista", Data_Prevista),
                    new SqlParameter("@Data_Levantamento", Data_Levantamento),
                    new SqlParameter("@Id_Livro", Id_Livro),
                    new SqlParameter("@Id_Utilizador", Id_Utilizador)
                };
                return dal.executarNonQuery(
                    "INSERT into HistoricoEmp (Estado_Emprestimo, Data_Entrega, Data_Prevista, Data_Levantamento, Id_Livro, Id_Utilizador) VALUES(@Estado_Emprestimo, @Data_Entrega, @Data_Prevista, @Data_Levantamento, @Id_Livro, @Id_Utilizador)",
                    sqlParams);
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
                int preco = Convert.ToInt32(livro["Preço"]);
                string estado = livro["Estado_Livro"]?.ToString() ?? "";
                DateTime dataCompra = DateTime.Now;

                for (int i = 0; i < quantidade; i++)
                {
                    Compra.insertCompra(idUtilizador, idLivro, dataCompra);
                    insertHistorico_de_compras(dataCompra, titulo, autor, preco, estado, idLivro, idUtilizador);
                }
            }

            public static void RegistrarEmprestimo(int idUtilizador, int idLivro)
            {
                if (idUtilizador <= 0)
                    throw new InvalidOperationException("É necessário iniciar sessão para requisitar livros.");

                DataTable dtLivro = Livros.ObterLivroPorId(idLivro);
                if (dtLivro == null || dtLivro.Rows.Count == 0)
                    throw new Exception("Livro não encontrado.");

                DateTime levantamento = DateTime.Now;
                DateTime prevista = levantamento.AddDays(14);
                DateTime entregaPendente = new DateTime(1900, 1, 1);

                insertHistoricoEmp("Ativo", entregaPendente, prevista, levantamento, idLivro, idUtilizador);
            }

            public static DataTable LoadHistoricoEmpPorUtilizador(int idUtilizador)
            {
                DAL dal = new DAL();

                SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@Id_Utilizador", idUtilizador)
                };

                string query = "SELECT h.Estado_Emprestimo, h.Data_Entrega, h.Data_Prevista, h.Data_Levantamento, " +
                               "h.Id_Livro, h.Id_Utilizador, l.Titulo, l.Autor " +
                               "FROM HistoricoEmp h " +
                               "INNER JOIN Livro l ON h.Id_Livro = l.Id_Livro " +
                               "WHERE h.Id_Utilizador = @Id_Utilizador";

                return dal.executarReader(query, sqlParams);
            }
        }
    
        

        //---------------------------------------------------------------------------------------------------------------
        public class  Devolução
        {
            static public int insertDevoluçãoCompra (int id_cliente, int id_livro, DateTime data_devolução)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id_cliente", id_cliente),
                new SqlParameter("@id_livro", id_livro),
                new SqlParameter("@data_devolução", data_devolução)
             };
                return dal.executarNonQuery("INSERT into DevoluçãoCompra (Id_Cliente, Id_Livro, Data_Devolução) VALUES(@id_cliente,@id_livro,@data_devolução)", sqlParams);
            }
             public static DataTable Load()
            {
                DataTable dal = new DataTable();
                return dal;
            }


            static public int insertDevoluçãoEmp(int id_cliente, int id_livro, DateTime data_devolução)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@id_cliente", id_cliente),
                new SqlParameter("@id_livro", id_livro),
                new SqlParameter("@data_devolução", data_devolução)
             };
                return dal.executarNonQuery("INSERT into DevoluçãoEmp (Id_Cliente, Id_Livro, Data_Devolução) VALUES(@id_cliente,@id_livro,@data_devolução)", sqlParams);
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

            static public void InserirLivro(int paginas, string nome, string bio, int preço, int ano, string autor, string estado_livro, string editora, string idioma, object capa, List<string> generos)
            {
                DAL dal = new DAL();

                try
                {
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
            new SqlParameter("@Capa", capa != null ? (object)capa : DBNull.Value)
        };

                    // 2. Insere na tabela Livro e obtém o ID gerado
                    object resultado = dal.executarScalar(
                        "INSERT INTO Livro (Quantas_Paginas, Titulo, Bio, Preço, Ano, Autor, Estado_Livro, Editora, Idioma, Capa) " +
                        "OUTPUT INSERTED.Id_Livro " +
                        "VALUES (@Quantas_Paginas, @Titulo, @Bio, @Preço, @Ano, @Autor, @Estado_Livro, @Editora, @Idioma, @Capa)",
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
                        "SELECT l.Id_Livro, l.Titulo, l.Autor, l.Preço, l.Estado_Livro " +
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
                            new SqlParameter("@genero", string.IsNullOrWhiteSpace(genero) ? (object)DBNull.Value : genero),
                            new SqlParameter("@estado", string.IsNullOrWhiteSpace(estado) ? (object)DBNull.Value : estado)
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
                        "SELECT Id_Livro, Titulo, Autor, Preço, Estado_Livro FROM Livro WHERE Id_Livro = @idLivro",
                        new SqlParameter[] { new SqlParameter("@idLivro", idLivro) });
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao obter livro: " + ex.Message, ex);
                }
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