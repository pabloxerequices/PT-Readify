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
            static public int updateutilizadoradmin(int Id_Utilizador, bool Tipo_Utilizador, string Estado_Conta, string Email, string Nome, string Palavra_Passe, int prefixo_telefone, int numero_telefone , object Foto)
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

            static public DataTable ObterEstadosTabela()
            {
                DAL dal = new DAL();
                // Garantir valores únicos, sem espaços e ordenados
                return dal.executarReader(
                    "SELECT DISTINCT Id_Estado_Livro, LTRIM(RTRIM(estado)) AS estado FROM Estado_Livro ORDER BY estado",
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

            static public DataTable Pesquisar(string titulo, string autor, List<string> categorias, string estado)
            {
                DAL dal = new DAL();

                string sql = @"SELECT DISTINCT L.Id_Livro, L.Titulo, L.Autor, L.Estado_Livro AS Estado, L.Preço,
                         STUFF((
                         SELECT ', ' + G2.Categoria
                         FROM Livro_Genero LG2
                         INNER JOIN Genero G2 ON LG2.Id_Genero = G2.Id_Genero
                         WHERE LG2.Id_Livro = L.Id_Livro
                         FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)')
                         ,1,2,'') AS Categoria
                         FROM Livro L
                         LEFT JOIN Livro_Genero LG ON L.Id_Livro = LG.Id_Livro
                         LEFT JOIN Genero G ON LG.Id_Genero = G.Id_Genero ";

                var whereClauses = new List<string>();
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(titulo))
                {
                    whereClauses.Add("L.Titulo LIKE @titulo");
                    parameters.Add(new SqlParameter("@titulo", "%" + titulo + "%"));
                }

                if (!string.IsNullOrWhiteSpace(autor))
                {
                    whereClauses.Add("L.Autor LIKE @autor");
                    parameters.Add(new SqlParameter("@autor", "%" + autor + "%"));
                }

                if (categorias != null && categorias.Count > 0)
                {
                    var inParams = new List<string>();
                    for (int i = 0; i < categorias.Count; i++)
                    {
                        string pname = "@cat" + i;
                        inParams.Add(pname);
                        parameters.Add(new SqlParameter(pname, categorias[i]));
                    }
                    whereClauses.Add("G.Categoria IN (" + string.Join(",", inParams) + ")");
                }

                if (!string.IsNullOrWhiteSpace(estado) && estado != "Todos")
                {
                    whereClauses.Add("L.Estado_Livro = @estado");
                    parameters.Add(new SqlParameter("@estado", estado));
                }

                if (whereClauses.Count > 0)
                {
                    sql += " WHERE " + string.Join(" AND ", whereClauses);
                }

                sql += " GROUP BY L.Id_Livro, L.Titulo, L.Autor, L.Estado_Livro, L.Preço";

                try
                {
                    return dal.executarReader(sql, parameters.Count > 0 ? parameters.ToArray() : null);
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
        }
    }
}