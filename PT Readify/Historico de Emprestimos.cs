using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
           dataGridViewHistorico_Emprestimos.DataSource = BLL.Historicos.LoadHistoricoEmp();
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
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
            // Obtém o DataTable do histórico
            var historico = BLL.Historicos.LoadHistoricoEmp();

            // CORRIGIDO: Valida se é nulo, vazio ou se NÃO (!) contém a coluna correta
            if (historico == null || historico.Columns.Count == 0 || !historico.Columns.Contains("Data_Emprestimo"))
            {
                // Log / mostrar mensagens / definir DataSource vazio
                dataGridViewHistorico_Emprestimos.DataSource = null;
                MessageBox.Show("Não foi possível ordenar: dados inválidos ou coluna não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataView view = historico.DefaultView;
            view.Sort = "Data_Emprestimo DESC";
            dataGridViewHistorico_Emprestimos.DataSource = view; // usar view em vez de view.ToTable() evita cópia gráfica lenta

            // Mantém a tua lógica dos botões Guna
            guna2Button2.Visible = true;
            guna2Button5.Visible = false;
            guna2Button4.Visible = false;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // Obtém o DataTable do histórico
            var historico = BLL.Historicos.LoadHistoricoEmp();

            // CORRIGIDO: Valida se é nulo, vazio ou se NÃO (!) contém a coluna correta
            if (historico == null || historico.Columns.Count == 0 || !historico.Columns.Contains("Data_Emprestimo"))
            {
                // Log ou mensagens ao usuário
                dataGridViewHistorico_Emprestimos.DataSource = null;
                MessageBox.Show("Não foi possível ordenar: dados inválidos ou coluna não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var view = historico.DefaultView;
            view.Sort = "Data_Emprestimo ASC";
            dataGridViewHistorico_Emprestimos.DataSource = view; // mostrar a view ordenada sem criar cópia

            // Mantém a tua lógica dos botões Guna
            guna2Button2.Visible = true;
            guna2Button5.Visible = false;
            guna2Button4.Visible = false;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Devolução_da_Compra devolucaoForm = new Devolução_da_Compra();
            devolucaoForm.Show();
        }

        private void dataGridViewHistorico_Emprestimos_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewHistorico_Emprestimos.DataSource = BLL.Historicos.LoadHistoricoEmp();
        }
    }
}
