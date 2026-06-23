using System;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public class RelatorioMultasAdmin : Form
    {
        private DataGridView gridMultas;

        public RelatorioMultasAdmin()
        {
            Text = "Relatório de Multas";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 550);

            var lblTitulo = new Label
            {
                Text = "Relatório de Multas",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            var lblInfo = new Label
            {
                Text = "Multa de 2,00 € por cada semana de atraso após a data prevista de devolução.",
                Location = new Point(20, 50),
                Size = new Size(900, 20),
                ForeColor = Color.DimGray
            };

            gridMultas = new DataGridView
            {
                Location = new Point(20, 80),
                Size = new Size(960, 390),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };
            gridMultas.CellFormatting += GridMultas_CellFormatting;

            var btnMarcarPaga = new Button
            {
                Text = "Marcar multa como paga",
                Location = new Point(20, 485),
                Size = new Size(180, 35)
            };
            btnMarcarPaga.Click += BtnMarcarPaga_Click;

            var btnAtualizar = new Button
            {
                Text = "Atualizar",
                Location = new Point(220, 485),
                Size = new Size(120, 35)
            };
            btnAtualizar.Click += (s, e) => CarregarMultas();

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new Point(860, 485),
                Size = new Size(120, 35)
            };
            btnFechar.Click += (s, e) => Close();

            Controls.Add(lblTitulo);
            Controls.Add(lblInfo);
            Controls.Add(gridMultas);
            Controls.Add(btnMarcarPaga);
            Controls.Add(btnAtualizar);
            Controls.Add(btnFechar);

            Load += (s, e) => CarregarMultas();
        }

        private void GridMultas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridMultas.Columns[e.ColumnIndex].Name == "Valor_Multa" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int centimos))
                    e.Value = (centimos / 100m).ToString("C2");
            }
        }

        private void CarregarMultas()
        {
            try
            {
                gridMultas.DataSource = BLL.Historicos.LoadRelatorioMultas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar multas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMarcarPaga_Click(object sender, EventArgs e)
        {
            if (gridMultas.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma multa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int id = Convert.ToInt32(gridMultas.CurrentRow.Cells["Id"].Value);
                BLL.Historicos.MarcarMultaComoPaga(id);
                CarregarMultas();
                MessageBox.Show("Multa marcada como paga.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
