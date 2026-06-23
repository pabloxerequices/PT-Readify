using System;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public class HistoricoEmprestimosAdmin : Form
    {
        private DataGridView gridHistorico;

        public HistoricoEmprestimosAdmin()
        {
            Text = "Histórico de Empréstimos (Admin)";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1100, 550);

            var lblTitulo = new Label
            {
                Text = "Histórico de Empréstimos — Todos os utilizadores",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            gridHistorico = new DataGridView
            {
                Location = new Point(20, 55),
                Size = new Size(1060, 430),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            gridHistorico.CellFormatting += GridHistorico_CellFormatting;

            var btnAtualizar = new Button
            {
                Text = "Atualizar",
                Location = new Point(20, 495),
                Size = new Size(120, 35)
            };
            btnAtualizar.Click += (s, e) => CarregarHistorico();

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new Point(960, 495),
                Size = new Size(120, 35)
            };
            btnFechar.Click += (s, e) => Close();

            Controls.Add(lblTitulo);
            Controls.Add(gridHistorico);
            Controls.Add(btnAtualizar);
            Controls.Add(btnFechar);

            Load += (s, e) => CarregarHistorico();
        }

        private void GridHistorico_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridHistorico.Columns[e.ColumnIndex].Name == "Valor_Multa" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int centimos))
                    e.Value = (centimos / 100m).ToString("C2");
            }
        }

        private void CarregarHistorico()
        {
            try
            {
                gridHistorico.DataSource = BLL.Historicos.LoadHistoricoEmpTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar histórico: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
