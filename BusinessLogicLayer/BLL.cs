using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BusinessLogicLayer
{
    public class BLL
    {
        //--------------------------UTILIZADOR-------------------------
        public class utilizador
        {
            static public DataTable Load()
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                return dal.executarReader("SELECT * FROM utilizador", null);
            }

            static public DataTable QueryutilizadorByemail(string Email)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@Email", Email)
                };
                return dal.executarReader("SELECT * FROM utilizador WHERE Email = @Email", sqlParams);
            }

            static public int insertutilizador(bool Tipo_Utilizador, string Estado_Conta, string Email, string Nome, string Palavra_Passe)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@Tipo_Utilizador", Tipo_Utilizador),
                    new SqlParameter("@Estado_Conta", Estado_Conta),
                    new SqlParameter("@Email", Email),
                    new SqlParameter("@Nome", Nome),
                    new SqlParameter("@Palavra_Passe", Palavra_Passe)
                };
                return dal.executarNonQuery("INSERT INTO utilizador (Tipo_Utilizador, Estado_Conta, Email, Nome, Palavra_Passe) VALUES (@Tipo_Utilizador, @Estado_Conta, @Email, @Nome, @Palavra_Passe)", sqlParams);
            }
        }

        //---------------------------------------------------------------

        public class Clientes
        {
            static public DataTable Load()
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                return dal.executarReader("SELECT * FROM Clientes", null);
            }

            static public int insertCliente(string nome, string morada, string telefone)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@nome", nome),
                    new SqlParameter("@morada", morada),
                    new SqlParameter("@telefone", telefone)
                };
                return dal.executarNonQuery("INSERT INTO Clientes (Nome, Morada, Telefone) VALUES (@nome, @morada, @telefone)", sqlParams);
            }

            static public DataTable queryClienteLike(string nome)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@nome", nome + "%")
                };
                return dal.executarReader("SELECT * FROM Clientes WHERE Nome LIKE @nome", sqlParams);
            }

            static public DataTable queryClientePorId(int id)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@id", id)
                };
                return dal.executarReader("SELECT * FROM Clientes WHERE ID = @id", sqlParams);
            }

            static public DataTable queryClientePorIdENome(int id, string nome)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@id", id),
                    new SqlParameter("@Nome", nome)
                };
                return dal.executarReader("SELECT * FROM Clientes WHERE ID = @id AND Nome = @Nome", sqlParams);
            }

            static public int updateCliente(string id, string nome, string morada, string telefone)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@id", id),
                    new SqlParameter("@nome", nome),
                    new SqlParameter("@morada", morada),
                    new SqlParameter("@telefone", telefone)
                };
                return dal.executarNonQuery("UPDATE [Clientes] SET [Nome] = @nome, [Morada] = @morada, [Telefone] = @telefone WHERE [ID] = @id", sqlParams);
            }

            static public int deleteCliente(string id)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@id", id)
                };
                return dal.executarNonQuery("DELETE FROM Clientes WHERE [ID] = @id", sqlParams);
            }
        }

        //----------------------LIVROS---------------------------- 
        public class Livros
        {
            // listar livros (omitindo Capa para evitar DISTINCT sobre blob)
            static public DataTable Load()
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                string sql = "SELECT Id_Livro, Quantas_Paginas, Nome, Bio, Preço, Ano, Autor, Id_Estado_Livro, Editora, Idioma FROM Livro";
                return dal.executarReader(sql, null);
            }

            // Obter lista de categorias (coluna 'Categoria' na tabela Genero)
            static public List<string> ObterGeneros()
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                string sql = "SELECT Categoria FROM Genero";
                try
                {
                    DataTable dt = dal.executarReader(sql, null);
                    return dt.AsEnumerable()
                             .Where(r => r["Categoria"] != DBNull.Value)
                             .Select(r => r["Categoria"].ToString())
                             .ToList();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao executar SQL: " + sql + Environment.NewLine + "Exceção: " + ex.Message, "Depuração");
                    return new List<string>();
                }
            }

            // Obter lista de estados (coluna 'estado' na tabela Estado_Livro)
            static public List<string> ObterEstados()
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                DataTable dt = dal.executarReader("SELECT estado FROM Estado_Livro", null);
                return dt.AsEnumerable()
                         .Where(r => r["estado"] != DBNull.Value)
                         .Select(r => r["estado"].ToString())
                         .ToList();
            }

            // Inserir livro — retorna Id_Livro gerado. 'capa' recebe byte[] (varbinary)
            static public int InserirLivro(int paginas, string nome, string bio, decimal preco, DateTime ano, string autor, int idEstadoLivro, string editora, string idioma, byte[] capa, List<string> generos)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();

                SqlParameter[] sqlParams = new SqlParameter[] {
                    new SqlParameter("@Quantas_Paginas", paginas),
                    new SqlParameter("@Nome", nome ?? (object)DBNull.Value),
                    new SqlParameter("@Bio", bio ?? (object)DBNull.Value),
                    new SqlParameter("@Preço", preco),
                    new SqlParameter("@Ano", ano),
                    new SqlParameter("@Autor", autor ?? (object)DBNull.Value),
                    new SqlParameter("@Id_Estado_Livro", idEstadoLivro),
                    new SqlParameter("@Editora", editora ?? (object)DBNull.Value),
                    new SqlParameter("@Idioma", idioma ?? (object)DBNull.Value),
                    new SqlParameter("@Capa", capa != null ? (object)capa : DBNull.Value)
                };

                string insertSql = "INSERT INTO Livro (Quantas_Paginas, Nome, Bio, Preço, Ano, Autor, Id_Estado_Livro, Editora, Idioma, Capa) OUTPUT INSERTED.Id_Livro VALUES (@Quantas_Paginas, @Nome, @Bio, @Preço, @Ano, @Autor, @Id_Estado_Livro, @Editora, @Idioma, @Capa)";
                int livroId = Convert.ToInt32(dal.executarScalar(insertSql, sqlParams));

                if (generos != null)
                {
                    foreach (var genero in generos)
                    {
                        var generoIdObj = dal.executarScalar("SELECT Id_Genero FROM Genero WHERE Categoria = @categoria", new SqlParameter[] { new SqlParameter("@categoria", genero) });
                        if (generoIdObj != null && generoIdObj != DBNull.Value)
                        {
                            int generoId = Convert.ToInt32(generoIdObj);
                            dal.executarNonQuery("INSERT INTO Livro_Genero (Id_Livro, Id_Genero) VALUES (@Id_Livro, @Id_Genero)",
                                new SqlParameter[] {
                                    new SqlParameter("@Id_Livro", livroId),
                                    new SqlParameter("@Id_Genero", generoId)
                                });
                        }
                    }
                }

                return livroId;
            }

            // Pesquisar — seleciona colunas sem Capa (evita DISTINCT sobre blob)
            static public DataTable Pesquisar(string titulo, string autor, List<string> categorias, string estado)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();

                string sql = "SELECT DISTINCT L.Id_Livro, L.Quantas_Paginas, L.Nome, L.Bio, L.Preço, L.Ano, L.Autor, L.Id_Estado_Livro, L.Editora, L.Idioma " +
                             "FROM Livro L " +
                             "LEFT JOIN Livro_Genero LG ON L.Id_Livro = LG.Id_Livro " +
                             "LEFT JOIN Genero G ON LG.Id_Genero = G.Id_Genero " +
                             "LEFT JOIN Estado_Livro EL ON L.Id_Estado_Livro = EL.Id_Estado_Livro ";

                var whereClauses = new List<string>();
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(titulo))
                {
                    whereClauses.Add("L.Nome LIKE @titulo");
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

                    if (inParams.Count > 0)
                        whereClauses.Add("G.Categoria IN (" + string.Join(", ", inParams) + ")");
                }

                if (!string.IsNullOrWhiteSpace(estado) && estado != "Todos")
                {
                    whereClauses.Add("EL.estado = @estado");
                    parameters.Add(new SqlParameter("@estado", estado));
                }

                if (whereClauses.Count > 0)
                    sql += " WHERE " + string.Join(" AND ", whereClauses);

                return dal.executarReader(sql, parameters.Count > 0 ? parameters.ToArray() : null);
            }

            // Obter a capa (byte[]) separadamente
            static public byte[] ObterCapaPorId(int idLivro)
            {
                DataAccessLayer.DAL dal = new DataAccessLayer.DAL();
                object result = dal.executarScalar("SELECT Capa FROM Livro WHERE Id_Livro = @id", new SqlParameter[] { new SqlParameter("@id", idLivro) });
                return result == null || result == DBNull.Value ? null : (byte[])result;
            }
        }
    }
}