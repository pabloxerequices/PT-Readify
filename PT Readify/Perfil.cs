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
    public partial class Perfil : Form
    {
        string nomeOriginal, emailOriginal, passOriginal, telefoneOriginal;
        bool modoEdicao = false;

        

        public Perfil()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Perfil_Load(object sender, EventArgs e)
        {
            // Supondo que você tem o ID do utilizador disponível, por exemplo, idUtilizador
            int idUtilizador = 1; // Substitua pelo valor correto do ID do utilizador
            DataTable dt = BLL.utilizador.queryUtilizadorById(idUtilizador);
            textBox1.Text = dt.Rows[0]["Nome"].ToString();
            textBox3.Text = dt.Rows[0]["Palavra_Passe"].ToString();
            textBox2.Text = dt.Rows[0]["Email"].ToString();
            
            textBox3.UseSystemPasswordChar = true;
            button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // GUARDAR OS VALORES ATUAIS (Antes de permitir a edição)
            // Isto serve para compararmos no botão de Guardar se algo realmente mudou
            nomeOriginal = textBox1.Text;
            emailOriginal = textBox2.Text;
            passOriginal = textBox3.Text;
            telefoneOriginal = textBox4.Text;

            // 1. Entrar no modo de edição
            modoEdicao = true;

            // 2. Gestão de Visibilidade (image_85e15b.jpg)
            // Esconde o botão de Editar e mostra o botão de Guardar (button3)
            button3.Visible = true;
            button1.Visible = false;

            // 3. Desbloquear os campos para escrita
            textBox1.ReadOnly = false; // Campo Nome
            textBox2.ReadOnly = false; // Campo Email
            textBox3.ReadOnly = false; // Campo Palavra-Passe
            textBox4.ReadOnly = false; // Campo Telefone

            // 4. Feedback visual (Opcional: mudar a cor de fundo para indicar que é editável)
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
            textBox3.BackColor = Color.White;
            textBox4.BackColor = Color.White;

            MessageBox.Show("Modo de edição ativado. Agora pode alterar os seus dados e clicar na foto de perfil.");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (modoEdicao)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Imagens|*.jpg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox6.Image = Image.FromFile(ofd.FileName);
                }
            }
            else
            {
                // Se clicar e não estiver em modo de edição, não faz nada ou avisa
                MessageBox.Show("Clique em 'Editar' para poder alterar a foto de perfil.");
            }
        }

        private void SalvarDadosNaBaseDeDados()
        {
            // Implemente aqui a lógica para salvar os dados nas TextBoxes no banco de dados.
            // Exemplo fictício:
            // string nome = textBox1.Text;
            // string email = textBox2.Text;
            // string telefone = textBox3.Text;
            // string endereco = textBox4.Text;
            // ... código para atualizar no banco de dados ...
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Verifica se o utilizador clicou no botão "Editar" primeiro
            if (modoEdicao)
            {
                // Se estiver escondida, mostra. Se estiver visível, esconde.
                if (textBox3.UseSystemPasswordChar == true)
                {
                    textBox3.UseSystemPasswordChar = false;
                    // Opcional: podes mudar o ícone para um olho aberto se tiveres a imagem
                    // pictureBox5.Image = Properties.Resources.eye_open; 
                }
                else
                {
                    textBox3.UseSystemPasswordChar = true;
                    // pictureBox5.Image = Properties.Resources.key_icon;
                }
            }
            else
            {
                // Mensagem caso o utilizador tente ver a pass sem estar a editar
                MessageBox.Show("Para visualizar ou alterar a palavra-passe, clique primeiro em 'Editar'.",
                                "Acesso Restrito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Verificação: Houve alguma alteração real nos dados?
            // Comparamos o texto atual com as variáveis que guardámos no button1_Click
            bool houveAlteracao = (textBox1.Text != nomeOriginal ||
                                   textBox2.Text != emailOriginal ||
                                   textBox3.Text != passOriginal ||
                                   textBox4.Text != telefoneOriginal);

            if (houveAlteracao)
            {
                // Se algo mudou, gravamos na base de dados (Referência: image_8665bc.png)
                SalvarDadosNaBaseDeDados();
                MessageBox.Show("Dados atualizados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Se nada mudou, apenas informamos o utilizador
                MessageBox.Show("Nenhuma alteração foi detetada.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // --- SAIR DO MODO DE EDIÇÃO (Executa sempre, quer mude ou não) ---

            // 2. Sair do modo de edição
            modoEdicao = false;

            // 3. Voltar a esconder a password por segurança (Referência: image_85e15b.jpg)
            textBox3.UseSystemPasswordChar = true;

            // 4. Bloquear as TextBoxes novamente
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;

            // 5. Inverter a visibilidade dos botões
            button3.Visible = false;
            button1.Visible = true;
        }
    }
}
