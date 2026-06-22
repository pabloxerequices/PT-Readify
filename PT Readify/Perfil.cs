using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PT_Readify; // Certifique-se de que este using está presente e correto

namespace PT_Readify
{
    public partial class Perfil : Form
    {
        string nomeOriginal, emailOriginal, passOriginal, telefoneOriginal, prefixoOriginal;
        byte[] fotoOriginal = null; // Variável para armazenar a foto original em bytes
        bool modoEdicao = false;
        byte[] fotoBytes = null;



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
            new main_menu().Show();
            this.Close();

        }

        private void Perfil_Load(object sender, EventArgs e)
        {
            // Adiciona a lista ao ComboBox
            comboBox1.Items.AddRange(globais.prefixosEuropa);

            // Carregar os dados do utilizador com base no ID armazenado em globais.id_utilizador depois do login
            int idUtilizador = globais.id_utilizador; 
            DataTable dt = BLL.utilizador.queryUtilizadorById(idUtilizador);
            textBox1.Text = dt.Rows[0]["Nome"].ToString();
            textBox3.Text = dt.Rows[0]["Palavra_Passe"].ToString();
            textBox4.Text = dt.Rows[0]["numero_telefone"].ToString();
            textBox2.Text = dt.Rows[0]["Email"].ToString();
            comboBox1.Text = dt.Rows[0]["prefixo_telefone"] + "+".ToString();

            // Carregar a foto de perfil
            if (dt.Rows[0]["Foto"] != DBNull.Value)
            {
                fotoBytes = (byte[])dt.Rows[0]["Foto"];
                using (MemoryStream ms = new MemoryStream(fotoBytes))
                {
                    pictureBox6.Image = Image.FromStream(ms);
                }
            }

            textBox3.UseSystemPasswordChar = true;
            button3.Visible = false;
            comboBox1.Items.AddRange(globais.prefixosEuropa);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (globais.confirmacao == false)
            {
                MessageBox.Show("É necessário confirmar a sua identidade para ativar o modo de edição.", "Confirmação Necessária", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                new confirmar_perfil().ShowDialog(); // Mostrar a janela de confirmação novamente
                // Resetar a confirmação para evitar que o modo de edição seja ativado sem querer
            }
            else
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
                comboBox1.Enabled = true; // Campo Prefixo de Telefone

                // 4. Feedback visual (Opcional: mudar a cor de fundo para indicar que é editável)
                textBox1.BackColor = Color.White;
                textBox2.BackColor = Color.White;
                textBox3.BackColor = Color.White;
                textBox4.BackColor = Color.White;
                comboBox1.BackColor = Color.White;


                MessageBox.Show("Modo de edição ativado. Agora pode alterar os seus dados e clicar na foto de perfil.");
                globais.confirmacao = false; // Resetar a confirmação para evitar que o modo de edição seja ativado sem querer no futuro
            }
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //botao de olho para esconder a password por segurança
            if (modoEdicao) {
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
                                   textBox4.Text != telefoneOriginal ||
                                      comboBox1.Text != prefixoOriginal ||
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
                    // Se tudo estiver válido, podemos atualizar os dados do utilizador
                    // Se algo mudou, gravamos na base de dados (Referência: image_8665bc.png)
                    BLL.utilizador.updateUtilizador(
                                                    globais.id_utilizador,
                                                    textBox2.Text,
                                                    textBox1.Text,
                                                    textBox3.Text,
                                                    fotoBytes, // <--- SUBSTÍTUIDO: Agora passa os bytes da foto (ou null se ele não escolheu nenhuma)
                                                    int.Parse(comboBox1.Text.ToString().Split(' ')[0].Replace("+", "")),
                                                    int.Parse(textBox4.Text)
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
            

            // 5. Inverter a visibilidade dos botões
            button3.Visible = false;
            button1.Visible = true;
        }
    }
}
