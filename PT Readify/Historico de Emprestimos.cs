using BusinessLogicLayer;
using System;
using System.Data;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Historico_de_Emprestimos : Form
    {
        public Historico_de_Emprestimos()
        {
            InitializeComponent();
        }

        private void Historico_de_Emprestimos_Load(object sender, EventArgs e)
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para ver o histórico de empréstimos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            CarregarHistorico();
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
        }

        private void CarregarHistorico()
        {
            dataGridViewHistorico_Emprestimos.DataSource = BLL.Historicos.LoadHistoricoEmpPorUtilizador(globais.id_utilizador);
        }

        private void dataGridViewHistorico_Emprestimos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Button2.Visible = false;
            guna2Button5.Visible = true;
            guna2Button4.Visible = true;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            OrdenarHistorico("Data_Levantamento DESC");
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            OrdenarHistorico("Data_Levantamento ASC");
        }

        private void OrdenarHistorico(string sortExpression)
        {
            var historico = BLL.Historicos.LoadHistoricoEmpPorUtilizador(globais.id_utilizador);

            if (historico == null || historico.Columns.Count == 0 || !historico.Columns.Contains("Data_Levantamento"))
            {
                dataGridViewHistorico_Emprestimos.DataSource = null;
                MessageBox.Show("Não foi possível ordenar: dados inválidos ou coluna não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                guna2Button2.Visible = true;
                guna2Button4.Visible = false;
                guna2Button5.Visible = false;
                return;
            }

            DataView view = historico.DefaultView;
            view.Sort = sortExpression;
            dataGridViewHistorico_Emprestimos.DataSource = view;

            guna2Button2.Visible = true;
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Devolução_da_Compra devolucaoForm = new Devolução_da_Compra();
            devolucaoForm.Show();
        }

        private void dataGridViewHistorico_Emprestimos_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            CarregarHistorico();
        }

        private void dataGridViewHistorico_Emprestimos_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
