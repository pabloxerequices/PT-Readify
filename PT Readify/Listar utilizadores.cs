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
        string passwordOriginalHash = "";
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
                fotoBytes = (byte[])dataGridView1.CurrentRow.Cells["foto"].Value;
                fotoOriginal = fotoBytes;
                using (MemoryStream ms = new MemoryStream(fotoBytes))
                {
                    pictureBox6.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox6.Image = null; // Ou uma imagem padrão, se preferir
                fotoBytes = null;
                fotoOriginal = null;
            }
            textBox1.Text = dataGridView1.CurrentRow.Cells["nome"].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();
            passwordOriginalHash = dataGridView1.CurrentRow.Cells["palavra_passe"].Value.ToString();
            textBox3.Text = "";
            textBox4.Text = dataGridView1.CurrentRow.Cells["numero_telefone"].Value.ToString();
                comboBox1.Text = "+" + dataGridView1.CurrentRow.Cells["prefixo_telefone"].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells["Estado_Conta"].Value.ToString();
            checkBox1.Checked = (bool)dataGridView1.CurrentRow.Cells["tipo_utilizador"].Value;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            main_menu__admin_ mainMenuAdmin = new main_menu__admin_();
            mainMenuAdmin.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (modoEdicao)
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Imagens (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox6.Image = Image.FromFile(ofd.FileName);

                        // 2. AQUI ELA JÁ VAI SER RECONHECIDA SEM ERRO
                        fotoBytes = File.ReadAllBytes(ofd.FileName);
                    }
                }
            }
            else
            {
                MessageBox.Show("Clique em 'Editar' para poder alterar a foto de perfil.");
            }
        }

        private void Listar_utilizadores_Load(object sender, EventArgs e)
        {
            // Carregar os dados do utilizador selecionado na datagrifview para os TextBoxes
            dataGridView1.DataSource = BLL.utilizador.Load();

            comboBox1.Items.AddRange(globais.prefixosEuropa);
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
                                      !(fotoBytes ?? new byte[0]).SequenceEqual(fotoOriginal ?? new byte[0]));

            if (houveAlteracao)
            {
                bool passwordAlterada = !string.IsNullOrWhiteSpace(textBox3.Text);
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
                     if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Os campos Nome e Email não podem estar vazios.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                     if (!textBox2.Text.Contains("@") || !textBox2.Text.Contains("."))
                {
                    MessageBox.Show("Endereço de email inválido. Certifique-se de que contém '@' e '.'.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                     if (passwordAlterada && textBox3.Text.Length < 6)
                {
                    MessageBox.Show("A palavra-passe deve conter pelo menos 6 caracteres.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (passwordAlterada && (!textBox3.Text.Any(char.IsUpper) || !textBox3.Text.Any(char.IsLower) || !textBox3.Text.Any(char.IsDigit) || !textBox3.Text.Any(ch => !char.IsLetterOrDigit(ch))))
                {
                    MessageBox.Show("A password deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caracter especial");
                    return;
                }
                else if (passwordAlterada && textBox3.Text.Contains(" "))
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
                        passwordAlterada ? textBox3.Text : passwordOriginalHash, // Palavra_Passe


                    int.Parse(comboBox1.Text.Replace("+", "")), // Prefixo do telefone (removendo o "+")
                        int.Parse(textBox4.Text) // Número de telefone
                        , fotoBytes

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

            //recarregar os dados para atualizar a datagridview
            dataGridView1.DataSource = BLL.utilizador.Load();
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
            estadoContaOriginal = comboBox2.Text;
            tipoUtilizadorOriginal = checkBox1.Checked.ToString();

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
