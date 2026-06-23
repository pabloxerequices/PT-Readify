using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public class EstatisticasAdmin : Form
    {
        private DataGridView gridResumo;
        private DataGridView gridTopLivros;

        public EstatisticasAdmin()
        {
            Text = "Estatísticas da Biblioteca";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(900, 600);

            var lblTitulo = new Label
            {
                Text = "Estatísticas Gerais",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            gridResumo = new DataGridView
            {
                Location = new Point(20, 55),
                Size = new Size(420, 320),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            var lblTop = new Label
            {
                Text = "Top livros emprestados",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(460, 55),
                AutoSize = true
            };

            gridTopLivros = new DataGridView
            {
                Location = new Point(460, 85),
                Size = new Size(420, 290),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            var btnAtualizar = new Button
            {
                Text = "Atualizar",
                Location = new Point(20, 390),
                Size = new Size(120, 35)
            };
            btnAtualizar.Click += (s, e) => CarregarDados();

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new Point(760, 390),
                Size = new Size(120, 35)
            };
            btnFechar.Click += (s, e) => Close();

            Controls.Add(lblTitulo);
            Controls.Add(gridResumo);
            Controls.Add(lblTop);
            Controls.Add(gridTopLivros);
            Controls.Add(btnAtualizar);
            Controls.Add(btnFechar);

            Load += (s, e) => CarregarDados();
        }

        private void CarregarDados()
        {
            try
            {
                gridResumo.DataSource = BLL.Estatisticas.ResumoGeral();
                gridTopLivros.DataSource = BLL.Estatisticas.TopLivrosEmprestados(10);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar estatísticas: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
