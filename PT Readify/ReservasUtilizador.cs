using System;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public class ReservasUtilizador : Form
    {
        private DataGridView gridReservas;

        public ReservasUtilizador()
        {
            Text = "As minhas reservas";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(900, 450);

            var lblTitulo = new Label
            {
                Text = "Reservas ativas",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            var lblInfo = new Label
            {
                Text = "Só pode reservar livros esgotados. Será notificado quando o stock estiver disponível.",
                Location = new Point(20, 48),
                Size = new Size(860, 20),
                ForeColor = Color.DimGray
            };

            gridReservas = new DataGridView
            {
                Location = new Point(20, 80),
                Size = new Size(860, 300),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new Point(760, 395),
                Size = new Size(120, 35)
            };
            btnFechar.Click += (s, e) => Close();

            Controls.Add(lblTitulo);
            Controls.Add(lblInfo);
            Controls.Add(gridReservas);
            Controls.Add(btnFechar);

            Load += ReservasUtilizador_Load;
        }

        private void ReservasUtilizador_Load(object sender, EventArgs e)
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para ver as reservas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            CarregarReservas();
        }

        private void CarregarReservas()
        {
            try
            {
                gridReservas.DataSource = BLL.Historicos.LoadReservasPorUtilizador(globais.id_utilizador);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar reservas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
