using BusinessLogicLayer;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class Listar_utilizadores : Form
    {
        string nomeOriginal, emailOriginal, passOriginal, telefoneOriginal, prefixoOriginal, estadoContaOriginal, tipoUtilizadorOriginal;
        byte[] fotoOriginal = null; // Variável para armazenar a foto original em bytes
        bool modoEdicao = false;
        byte[] fotoBytes = null;
        
        public Listar_utilizadores()
        {
            InitializeComponent();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Carregar a foto de perfil
            if (dataGridView1.CurrentRow.Cells["foto"].Value != DBNull.Value)
            {
                byte[] fotoBytes = (byte[])dataGridView1.CurrentRow.Cells["foto"].Value;
                using (MemoryStream ms = new MemoryStream(fotoBytes))
                {
                    pictureBox6.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox6.Image = null; // Ou uma imagem padrão, se preferir
            }
            textBox1.Text = dataGridView1.CurrentRow.Cells["nome"].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells["palavra_passe"].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells["numero_telefone"].Value.ToString();
                comboBox1.Text = "+" + dataGridView1.CurrentRow.Cells["prefixo_telefone"].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells["Estado_Conta"].Value.ToString();
            checkBox1.Checked = (bool)dataGridView1.CurrentRow.Cells["tipo_utilizador"].Value;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void Listar_utilizadores_Load(object sender, EventArgs e)
        {
            // Carregar os dados do utilizador selecionado na datagrifview para os TextBoxes
            dataGridView1.DataSource = BLL.utilizador.Load();


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Verificação: Houve alguma alteração real nos dados?
            // Comparamos o texto atual com as variáveis que guardámos no button1_Click
            bool houveAlteracao = (textBox1.Text != nomeOriginal ||
                                   textBox2.Text != emailOriginal ||
                                   textBox3.Text != passOriginal ||
                                   textBox4.Text != telefoneOriginal ||
                                      comboBox1.Text != prefixoOriginal ||
                                        comboBox2.Text != estadoContaOriginal ||
                                        checkBox1.Checked.ToString() != tipoUtilizadorOriginal ||
                                      !fotoBytes.SequenceEqual(fotoOriginal ?? new byte[0]));

            if (houveAlteracao)
            {
                //verificar se o numero de telefone é válido (apenas dígitos) e se tem 9 dígitos
                if (!textBox4.Text.All(char.IsDigit))
                {
                    MessageBox.Show("Número de telefone inválido. Apenas dígitos são permitidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (textBox4.Text.Length != 9)
                {
                    MessageBox.Show("Número de telefone inválido. Deve conter exatamente 9 dígitos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                else
                     if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    MessageBox.Show("Os campos Nome, Email e Palavra-Passe não podem estar vazios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                     if (!textBox2.Text.Contains("@") || !textBox2.Text.Contains("."))
                {
                    MessageBox.Show("Endereço de email inválido. Certifique-se de que contém '@' e '.'.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                     if (textBox3.Text.Length < 6)
                {
                    MessageBox.Show("A palavra-passe deve conter pelo menos 6 caracteres.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (!textBox3.Text.Any(char.IsUpper) || !textBox3.Text.Any(char.IsLower) || !textBox3.Text.Any(char.IsDigit) || !textBox3.Text.Any(ch => !char.IsLetterOrDigit(ch)))
                {
                    MessageBox.Show("A password deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caracter especial");
                    return;
                }
                else if (textBox3.Text.Contains(" "))
                {
                    MessageBox.Show("A password não pode conter espaços em branco");
                    return;
                }
                else
                {
                    
                    BLL.utilizador.updateutilizadoradmin(int.Parse(dataGridView1.CurrentRow.Cells["Id_Utilizador"].Value.ToString()),
                        checkBox1.Checked, // Tipo_Utilizador
                        comboBox2.Text, // Estado_Conta
                        textBox2.Text, // Email
                        textBox1.Text, // Nome
                        textBox3.Text, // Palavra_Passe

                    int.Parse(comboBox1.Text.Replace("+", "")), // Prefixo do telefone (removendo o "+")
                        int.Parse(textBox4.Text) // Número de telefone
                        
                            );
                    MessageBox.Show("Dados atualizados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


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
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
                checkBox1.Enabled = false;


            // 5. Inverter a visibilidade dos botões
            button3.Visible = false;
            button1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // GUARDAR OS VALORES ATUAIS (Antes de permitir a edição)
            // Isto serve para compararmos no botão de Guardar se algo realmente mudou
            nomeOriginal = textBox1.Text;
             emailOriginal = textBox2.Text;
            passOriginal = textBox3.Text;
            telefoneOriginal = textBox4.Text;
            prefixoOriginal = comboBox1.Text;

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
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            checkBox1.Enabled = true;

            // 4. Feedback visual (Opcional: mudar a cor de fundo para indicar que é editável)
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
            textBox3.BackColor = Color.White;
            textBox4.BackColor = Color.White;
            comboBox1.BackColor = Color.White;


            MessageBox.Show("Modo de edição ativado. Agora pode alterar os seus dados e clicar na foto de perfil.");
        }
    }
}
