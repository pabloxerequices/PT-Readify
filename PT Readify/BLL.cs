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

namespace BusinessLogicLayer
{
    public class BLL
    {

        //--------------------------UTILIZADOR-------------------------
        public class utilizador
        {
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
            static public int insertutilizador(bool Tipo_Utilizador,string Estado_Conta, string Email, string Nome, string Palavra_Passe)
            {
                DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                    new SqlParameter("@Estado_Conta", Estado_Conta),
                new SqlParameter("@Tipo_Utilizador", Tipo_Utilizador),
                new SqlParameter("@Email", Email),
                new SqlParameter("@Nome", Nome),
                new SqlParameter("@Palavra_Passe", Palavra_Passe)
                };
                return dal.executarNonQuery("INSERT into utilizador (Tipo_Utilizador,Estado_Conta,Email,Nome,Palavra_Passe) VALUES(@Tipo_Utilizador,@Estado_Conta,@Email,@Nome,@Palavra_Passe)", sqlParams);
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



        // Nova classe para pesquisa de livros 
        public class Livro
        {
                public static DataTable ObterCategorias()
                {
                    DAL dal = new DAL();
                    string sql = "SELECT Categoria FROM Genero ORDER BY Categoria";
                    return dal.executarReader(sql, null);
                }

                public static DataTable ObterEstados()
                {
                DAL dal = new DAL();
                // Se a sua tabela Estado_Livro usar outro nome de coluna, altere "Nome" para o nome correto
                string sql = "SELECT estado FROM Estado_Livro ORDER BY estado";
                return dal.executarReader(sql, null);
                }
                    static public void InserirLivro(string titulo,string autor , string estado, List<string>generos, List<string> tipos,string biografia,string editora,int preço,DateTime ano,Image capa)
            {                 DAL dal = new DAL();
                SqlParameter[] sqlParams = new SqlParameter[]{
                    new SqlParameter("@titulo", titulo),
                    new SqlParameter("@autor", autor),
                    new SqlParameter("@estado", estado),
                    new SqlParameter("@biografia", biografia),
                    new SqlParameter("@editora", editora),
                    new SqlParameter("@preço", preço),
                    new SqlParameter("@ano", ano),
                    new SqlParameter("@capa", capa != null ? (object)capa : DBNull.Value)
                };
                // Inserir o livro e obter o ID gerado
                int livroId = Convert.ToInt32(dal.executarScalar("INSERT INTO Livro (Titulo, Autor, Estado, Biografia, Editora, Preço, Ano, Capa) OUTPUT INSERTED.ID VALUES (@titulo, @autor, @estado, @biografia, @editora, @preço, @ano, @capa)", sqlParams));
                // Inserir os gêneros associados
                if (generos != null)
                {
                    foreach (var genero in generos)
                    {
                        var generoId = Convert.ToInt32(dal.executarScalar(
                            "SELECT ID FROM Genero WHERE Nome = @nome",
                            new SqlParameter[] { new SqlParameter("@nome", genero) })); // Corrigido aqui
                        dal.executarNonQuery(
                            "INSERT INTO LivroGenero (LivroID, GeneroID) VALUES (@livroId, @generoId)",
                            new SqlParameter[] {
                                new SqlParameter("@livroId", livroId),
                                new SqlParameter("@generoId", generoId)
                            });
                    }
                }
                // Inserir os tipos associados
                if (tipos != null)
                {
                    foreach (var tipo in tipos)
                    {
                        var tipoId = Convert.ToInt32(dal.executarScalar(
                            "SELECT ID FROM Genero WHERE Categoria = @Categoria",
                            new SqlParameter[] { new SqlParameter("@Categoria", tipo) })); // Corrigido aqui
                        dal.executarNonQuery(
                            "INSERT INTO LivroTipo (LivroID, TipoID) VALUES (@livroId, @tipoId)",
                            new SqlParameter[] {
                                new SqlParameter("@livroId", livroId),
                                new SqlParameter("@tipoId", tipoId)
                            });
                    }
                }
            }
            static public List<string> ObterGeneros()
            {
                DAL dal = new DAL();
                DataTable dt = dal.executarReader("SELECT Nome FROM Genero", null);
                return dt.AsEnumerable().Select(r => r["Nome"].ToString()).ToList();
            }

            static public DataTable Pesquisar(string Nome, string autor, List<string> categorias, string estado)
            {
                DAL dal = new DAL();
                var whereClauses = new List<string>();
                var parameters = new List<SqlParameter>();
                string where = whereClauses.Count > 0 ? " WHERE " + string.Join(" AND ", whereClauses) : "";

                string joinClause = "FROM Livro L " +
                                    "LEFT JOIN Livro_Genero LG ON L.Id_Livro = LG.Id_Livro " +
                                    "LEFT JOIN Genero G ON LG.Id_Genero = G.Id_Genero " +
                                    "LEFT JOIN Estado_Livro E ON L.Id_Estado_Livro = E.Id_Estado_Livro ";

                string sql = "WITH DistinctIds AS (" +
                             "SELECT DISTINCT L.Id_Livro " + joinClause + where +
                             ") " +
                             "SELECT L.* FROM Livro L JOIN DistinctIds d ON L.Id_Livro = d.Id_Livro;";

                if (!string.IsNullOrWhiteSpace(Nome))
                {
                    whereClauses.Add("L.Nome LIKE @Nome");
                    parameters.Add(new SqlParameter("@Nome", "%" + Nome + "%"));
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
                        whereClauses.Add("G.Nome IN (" + string.Join(", ", inParams) + ")");
                }

                if (!string.IsNullOrWhiteSpace(estado) && estado != "Todos")
                {
                    whereClauses.Add("L.Estado = @estado");
                    parameters.Add(new SqlParameter("@estado", estado));
                }

                return dal.executarReader(sql, parameters.Count > 0 ? parameters.ToArray() : null);
            }
        }
    }
}