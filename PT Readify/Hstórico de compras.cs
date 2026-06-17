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
    public partial class Hstórico_de_compras : Form
    {
        public Hstórico_de_compras()
        {
            InitializeComponent();
        }

        private void dataGridViewCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void Hstórico_de_compras_Load(object sender, EventArgs e)
        {
            if (globais.id_utilizador <= 0)
            {
                MessageBox.Show("Inicie sessão para ver o histórico de compras.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            guna2DataGridView1Historico_Compras.DataSource = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
        }

        private void btnLimparCarrinho_Click(object sender, EventArgs e)
        {
            Devolução_da_Compra devolução = new Devolução_da_Compra();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            guna2Button2.Visible = false;
            guna2Button5.Visible = true;
            guna2Button4.Visible = true;
        }

        private void guna2DataGridView1Historico_Compras_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // 1. Obtém o DataTable correto das COMPRAS filtrado pelo ID do utilizador (por agora '1')
            DataTable historico = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);

            // 2. VALIDAÇÃO: Garante que a tabela não está vazia e que a coluna "Data_Compra" existe
            if (historico == null || historico.Columns.Count == 0 || !historico.Columns.Contains("Data_Compra"))
            {
                guna2DataGridView1Historico_Compras.DataSource = null;
                MessageBox.Show("Não foi possível ordenar: dados inválidos ou coluna não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Repõe a visibilidade caso falhe
                guna2Button2.Visible = true;
                guna2Button4.Visible = false;
                guna2Button5.Visible = false;
                return;
            }

            // 3. Ordena de forma Decrescente (Mais recente para o mais antigo)
            DataView view = historico.DefaultView;
            view.Sort = "Data_Compra DESC";
            guna2DataGridView1Historico_Compras.DataSource = view;

            // 4. Controlo de visibilidade: Mostra o botão principal e esconde as setas
            guna2Button2.Visible = true;
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            // 1. Obtém o DataTable correto das COMPRAS filtrado pelo ID do utilizador (por agora '1')
            DataTable historico = BLL.Historicos.LoadHistoricoComprasPorUtilizador(globais.id_utilizador);

            // 2. VALIDAÇÃO: Garante que a tabela não está vazia e que a coluna "Data_Compra" existe
            if (historico == null || historico.Columns.Count == 0 || !historico.Columns.Contains("Data_Compra"))
            {
                guna2DataGridView1Historico_Compras.DataSource = null;
                MessageBox.Show("Não foi possível ordenar: dados inválidos ou coluna não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Repõe a visibilidade caso falhe
                guna2Button2.Visible = true;
                guna2Button4.Visible = false;
                guna2Button5.Visible = false;
                return;
            }

            // 3. Ordena de forma Crescente (Mais antigo para o mais recente)
            DataView view = historico.DefaultView;
            view.Sort = "Data_Compra ASC";
            guna2DataGridView1Historico_Compras.DataSource = view;

            // 4. Controlo de visibilidade: Mostra o botão principal e esconde as setas
            guna2Button2.Visible = true;
            guna2Button4.Visible = false;
            guna2Button5.Visible = false;
        }
    }
}
