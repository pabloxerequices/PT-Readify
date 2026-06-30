using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    internal static class DevolucaoUiHelper
    {
        private static readonly HashSet<string> ColunasOcultas = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Id", "Id_Livro", "Id_Utilizador", "Multa_Paga", "Duracao_Dias"
        };

        private static readonly Dictionary<string, string> Cabecalhos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Data_Compra", "Data da Compra" },
            { "Data_Devolução", "Data de Devolução" },
            { "Estado_Compra", "Estado" },
            { "Estado_Livro", "Condição" },
            { "Estado_Emprestimo", "Estado" },
            { "Data_Levantamento", "Levantamento" },
            { "Data_Prevista", "Devolver até" },
            { "Data_Entrega", "Entrega" },
            { "Valor_Multa", "Multa" },
            { "Preço", "Preço" }
        };

        public static string TextoPoliticaDevolucaoCompra()
        {
            return $"Só pode efetuar devoluções nos primeiros {BLL.Historicos.MaxDiasDevolucaoCompra} dias após a data de compra.";
        }

        public static void ConfigurarGrid(DataGridView grid)
        {
            grid.ReadOnly = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = Color.White;
            grid.DataBindingComplete += (s, e) => AplicarColunas(grid);
        }

        public static void AplicarColunas(DataGridView grid)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (ColunasOcultas.Contains(col.Name))
                    col.Visible = false;

                if (Cabecalhos.TryGetValue(col.Name, out string titulo))
                    col.HeaderText = titulo;
            }
        }

        public static void FormatarCelula(DataGridView grid, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null || e.Value == DBNull.Value)
                return;

            string coluna = grid.Columns[e.ColumnIndex].Name;

            if (coluna == "Preço" || coluna == "Valor_Multa")
            {
                if (e.Value is string)
                    return;

                if (coluna == "Valor_Multa" && int.TryParse(e.Value.ToString(), out int centimos))
                    e.Value = (centimos / 100m).ToString("C2");
                else if (decimal.TryParse(e.Value.ToString(), out decimal valor))
                    e.Value = (valor / 100m).ToString("C2");
                return;
            }

            if (coluna.StartsWith("Data_", StringComparison.Ordinal) && DateTime.TryParse(e.Value.ToString(), out DateTime data))
            {
                if (data.Year <= 1901)
                    e.Value = "—";
                else
                    e.Value = data.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public static void ColorirLinhaEmprestimo(DataGridView grid, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = grid.Rows[e.RowIndex];
            string estado = row.Cells["Estado_Emprestimo"]?.Value?.ToString();

            if (estado == "Devolvido")
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(127, 140, 141);
                return;
            }

            if (estado == "Ativo" && row.Cells["Data_Prevista"]?.Value != null &&
                DateTime.TryParse(row.Cells["Data_Prevista"].Value.ToString(), out DateTime prevista))
            {
                if (DateTime.Now.Date > prevista.Date)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235);
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(192, 57, 43);
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        public static void ColorirLinhaCompra(DataGridView grid, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || !grid.Columns.Contains("Estado_Compra"))
                return;

            var row = grid.Rows[e.RowIndex];
            if (row.Cells["Estado_Compra"]?.Value?.ToString() == "Devolvida")
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(127, 140, 141);
            }
        }

        public static string ConstruirConfirmacaoCompra(BusinessLogicLayer.BLL.Historicos.ResultadoDevolucao resumo)
        {
            string dataCompra = resumo.DataReferencia.HasValue
                ? resumo.DataReferencia.Value.ToString("dd/MM/yyyy")
                : "—";

            return $"Confirmar devolução de \"{resumo.Titulo}\"?\n\n" +
                   $"Autor: {resumo.Autor}\n" +
                   $"Comprado em: {dataCompra}\n" +
                   $"Reembolso: {resumo.ValorReembolso:C2}\n" +
                   $"Prazo restante: {resumo.DiasRestantesPrazo} dia(s)\n\n" +
                   $"{TextoPoliticaDevolucaoCompra()}\n\n" +
                   "O valor será creditado na sua carteira e o stock do livro será reposto.";
        }

        public static string ConstruirConfirmacaoEmprestimo(BusinessLogicLayer.BLL.Historicos.ResultadoDevolucao resumo)
        {
            string prazo = resumo.DataReferencia.HasValue
                ? resumo.DataReferencia.Value.ToString("dd/MM/yyyy")
                : "—";

            var msg = $"Confirmar devolução de \"{resumo.Titulo}\"?\n\n" +
                      $"Autor: {resumo.Autor}\n" +
                      $"Devolver até: {prazo}";

            if (resumo.DiasAtraso > 0)
                msg += $"\n\nAtenção: {resumo.DiasAtraso} dia(s) de atraso.\n" +
                       $"Multa estimada: {(resumo.MultaCentimos / 100m):C2} (2€ por semana).";
            else if (resumo.DiasRestantesPrazo > 0)
                msg += $"\n\nFaltam {resumo.DiasRestantesPrazo} dia(s) para o prazo.";

            return msg;
        }

        public static string ConstruirSucessoCompra(BusinessLogicLayer.BLL.Historicos.ResultadoDevolucao resultado)
        {
            return $"Compra devolvida com sucesso!\n\n" +
                   $"Livro: {resultado.Titulo}\n" +
                   $"Reembolso: {resultado.ValorReembolso:C2}\n" +
                   $"Creditado na carteira digital.\n" +
                   $"Data: {resultado.DataDevolucao:dd/MM/yyyy HH:mm}";
        }

        public static string ConstruirSucessoEmprestimo(BusinessLogicLayer.BLL.Historicos.ResultadoDevolucao resultado)
        {
            string msg = $"Livro devolvido com sucesso!\n\n" +
                         $"Livro: {resultado.Titulo}\n" +
                         $"Data: {resultado.DataDevolucao:dd/MM/yyyy HH:mm}";

            if (resultado.MultaCentimos > 0)
                msg += $"\n\nMulta aplicada: {(resultado.MultaCentimos / 100m):C2} (2€ por semana de atraso).";
            else
                msg += "\n\nSem multas — entrega dentro do prazo.";

            return msg;
        }
    }
}
