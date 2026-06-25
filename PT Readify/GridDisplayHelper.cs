using System;
using System.Data;

namespace PT_Readify
{
    public static class GridDisplayHelper
    {
        public static DataTable FormatMultasParaExibicao(DataTable origem)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Id_Utilizador", typeof(int));
            dt.Columns.Add("Utilizador", typeof(string));
            dt.Columns.Add("Titulo", typeof(string));
            dt.Columns.Add("Data_Prevista", typeof(DateTime));
            dt.Columns.Add("Data_Entrega", typeof(DateTime));
            dt.Columns.Add("Valor", typeof(string));
            dt.Columns.Add("Estado Multa", typeof(string));
            dt.Columns.Add("Estado_Emprestimo", typeof(string));

            if (origem == null)
                return dt;

            foreach (DataRow row in origem.Rows)
            {
                int centimos = row["Valor_Multa"] != DBNull.Value ? Convert.ToInt32(row["Valor_Multa"]) : 0;
                bool paga = row["Multa_Paga"] != DBNull.Value && Convert.ToBoolean(row["Multa_Paga"]);

                dt.Rows.Add(
                    row["Id"],
                    row["Id_Utilizador"],
                    row["Utilizador"]?.ToString() ?? "",
                    row["Titulo"]?.ToString() ?? "",
                    row["Data_Prevista"],
                    row["Data_Entrega"],
                    (centimos / 100m).ToString("C2"),
                    paga ? "Paga" : "Pendente",
                    row["Estado_Emprestimo"]?.ToString() ?? ""
                );
            }

            return dt;
        }

        public static DataTable FormatEmprestimosAdminParaExibicao(DataTable origem)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Id_Utilizador", typeof(int));
            dt.Columns.Add("Utilizador", typeof(string));
            dt.Columns.Add("Titulo", typeof(string));
            dt.Columns.Add("Autor", typeof(string));
            dt.Columns.Add("Estado_Emprestimo", typeof(string));
            dt.Columns.Add("Data_Levantamento", typeof(DateTime));
            dt.Columns.Add("Data_Prevista", typeof(DateTime));
            dt.Columns.Add("Data_Entrega", typeof(DateTime));
            dt.Columns.Add("Duracao_Dias", typeof(int));
            dt.Columns.Add("Valor Multa", typeof(string));
            dt.Columns.Add("Estado Multa", typeof(string));
            dt.Columns.Add("Id_Livro", typeof(int));

            if (origem == null)
                return dt;

            foreach (DataRow row in origem.Rows)
            {
                int centimos = row["Valor_Multa"] != DBNull.Value ? Convert.ToInt32(row["Valor_Multa"]) : 0;
                bool paga = row["Multa_Paga"] != DBNull.Value && Convert.ToBoolean(row["Multa_Paga"]);

                dt.Rows.Add(
                    row["Id"],
                    row["Id_Utilizador"],
                    row["Utilizador"]?.ToString() ?? "",
                    row["Titulo"]?.ToString() ?? "",
                    row["Autor"]?.ToString() ?? "",
                    row["Estado_Emprestimo"]?.ToString() ?? "",
                    row["Data_Levantamento"],
                    row["Data_Prevista"],
                    row["Data_Entrega"],
                    row["Duracao_Dias"] != DBNull.Value ? row["Duracao_Dias"] : 0,
                    centimos > 0 ? (centimos / 100m).ToString("C2") : "0,00 €",
                    centimos > 0 ? (paga ? "Paga" : "Pendente") : "—",
                    row["Id_Livro"]
                );
            }

            return dt;
        }

        public static DataTable FormatComprasParaExibicao(DataTable origem)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Data_Compra", typeof(DateTime));
            dt.Columns.Add("Titulo", typeof(string));
            dt.Columns.Add("Autor", typeof(string));
            dt.Columns.Add("Preço", typeof(string));
            dt.Columns.Add("Estado_Livro", typeof(string));
            dt.Columns.Add("Id_Livro", typeof(int));

            if (origem == null)
                return dt;

            foreach (DataRow row in origem.Rows)
            {
                int centimos = row["Preço"] != DBNull.Value ? Convert.ToInt32(row["Preço"]) : 0;

                dt.Rows.Add(
                    row["Id"],
                    row["Data_Compra"],
                    row["Titulo"]?.ToString() ?? "",
                    row["Autor"]?.ToString() ?? "",
                    (centimos / 100m).ToString("C2"),
                    row["Estado_Livro"]?.ToString() ?? "",
                    row["Id_Livro"]
                );
            }

            return dt;
        }

        public static DataTable FormatEmprestimosParaExibicao(DataTable origem)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Titulo", typeof(string));
            dt.Columns.Add("Autor", typeof(string));
            dt.Columns.Add("Estado_Emprestimo", typeof(string));
            dt.Columns.Add("Data_Levantamento", typeof(DateTime));
            dt.Columns.Add("Data_Prevista", typeof(DateTime));
            dt.Columns.Add("Data_Entrega", typeof(DateTime));
            dt.Columns.Add("Duracao_Dias", typeof(int));
            dt.Columns.Add("Valor Multa", typeof(string));
            dt.Columns.Add("Estado Multa", typeof(string));
            dt.Columns.Add("Id_Livro", typeof(int));

            if (origem == null)
                return dt;

            foreach (DataRow row in origem.Rows)
            {
                int centimos = row["Valor_Multa"] != DBNull.Value ? Convert.ToInt32(row["Valor_Multa"]) : 0;
                bool paga = row["Multa_Paga"] != DBNull.Value && Convert.ToBoolean(row["Multa_Paga"]);

                dt.Rows.Add(
                    row["Id"],
                    row["Titulo"]?.ToString() ?? "",
                    row["Autor"]?.ToString() ?? "",
                    row["Estado_Emprestimo"]?.ToString() ?? "",
                    row["Data_Levantamento"],
                    row["Data_Prevista"],
                    row["Data_Entrega"],
                    row["Duracao_Dias"] != DBNull.Value ? row["Duracao_Dias"] : 0,
                    centimos > 0 ? (centimos / 100m).ToString("C2") : "0,00 €",
                    centimos > 0 ? (paga ? "Paga" : "Pendente") : "—",
                    row["Id_Livro"]
                );
            }

            return dt;
        }
    }
}
