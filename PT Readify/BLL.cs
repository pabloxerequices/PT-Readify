using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccessLayer;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Diagnostics;

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
        public class Livros
        {
            static public List<string> ObterGeneros()
            {
                DAL dal = new DAL();
                DataTable dt = dal.executarReader("SELECT Nome FROM Genero", null);
                return dt.AsEnumerable().Select(r => r["Nome"].ToString()).ToList();
            }

            static public DataTable Pesquisar(string titulo, string autor, List<string> categorias, string estado)
            {
                DAL dal = new DAL();

                string sql = "SELECT DISTINCT L.* FROM Livros L " +
                             "LEFT JOIN LivroGenero LG ON L.ID = LG.LivroID " +
                             "LEFT JOIN Genero G ON LG.GeneroID = G.ID";

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

                    if (inParams.Count > 0)
                        whereClauses.Add("G.Nome IN (" + string.Join(", ", inParams) + ")");
                }

                if (!string.IsNullOrWhiteSpace(estado) && estado != "Todos")
                {
                    whereClauses.Add("L.Estado = @estado");
                    parameters.Add(new SqlParameter("@estado", estado));
                }

                if (whereClauses.Count > 0)
                    sql += " WHERE " + string.Join(" AND ", whereClauses);

                return dal.executarReader(sql, parameters.Count > 0 ? parameters.ToArray() : null);
            }
        }
    }
}