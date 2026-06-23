using System;
using System.Drawing;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public class NotificacoesUtilizador : Form
    {
        private DataGridView gridNotificacoes;

        public NotificacoesUtilizador()
        {
            Text = "Notificações";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(800, 450);

            var lblTitulo = new Label
            {
                Text = "Notificações",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            gridNotificacoes = new DataGridView
            {
                Location = new Point(20, 55),
                Size = new Size(760, 320),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            var btnMarcarLida = new Button
            {
                Text = "Marcar como lida",
                Location = new Point(20, 390),
                Size = new Size(140, 35)
            };
            btnMarcarLida.Click += BtnMarcarLida_Click;

            var btnMarcarTodas = new Button
            {
                Text = "Marcar todas como lidas",
                Location = new Point(180, 390),
                Size = new Size(180, 35)
            };
            btnMarcarTodas.Click += BtnMarcarTodas_Click;

            var btnFechar = new Button
            {
                Text = "Fechar",
                Location = new Point(660, 390),
                Size = new Size(120, 35)
            };
            btnFechar.Click += (s, e) => Close();

            Controls.Add(lblTitulo);
            Controls.Add(gridNotificacoes);
            Controls.Add(btnMarcarLida);
            Controls.Add(btnMarcarTodas);
            Controls.Add(btnFechar);

            Load += NotificacoesUtilizador_Load;
        }

        private void NotificacoesUtilizador_Load(object sender, EventArgs e)
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para ver notificações.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            CarregarNotificacoes();
        }

        private void CarregarNotificacoes()
        {
            try
            {
                gridNotificacoes.DataSource = BLL.Notificacoes.LoadNaoLidas(globais.id_utilizador);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar notificações: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMarcarLida_Click(object sender, EventArgs e)
        {
            if (gridNotificacoes.CurrentRow == null)
                return;

            try
            {
                int id = Convert.ToInt32(gridNotificacoes.CurrentRow.Cells["Id"].Value);
                BLL.Notificacoes.MarcarComoLida(id, globais.id_utilizador);
                CarregarNotificacoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMarcarTodas_Click(object sender, EventArgs e)
        {
            try
            {
                BLL.Notificacoes.MarcarTodasComoLidas(globais.id_utilizador);
                CarregarNotificacoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
