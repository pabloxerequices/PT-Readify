using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    /// <summary>
    /// Formulário para recuperação de password em 3 etapas:
    /// 1. Inserir email e receber código
    /// 2. Validar código recebido
    /// 3. Definir nova password
    /// </summary>
    public partial class Recuperar_Palavra_passe : Form
    {
        // Variáveis para armazenar dados temporários do processo de recuperação
        private string emailUtilizador = string.Empty;
        private string codigoConfirmacao = string.Empty;
        private int idUtilizador = 0;
        private DateTime codigoExpiracao;

        public Recuperar_Palavra_passe()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento Load do formulário: inicializa o estado e carrega o logo
        /// </summary>
        private void Recuperar_Palavra_passe_Load(object sender, EventArgs e)
        {
            // Inicializar no passo 1 (email)
            panelEmail.Visible = true;
            panelCodigo.Visible = false;
            panelNovaPassword.Visible = false;
            AtualizarIndicadorPasso(1);
            
            // Carregar logo da Readify se disponível
            try
            {
                pictureBoxLogo.ImageLocation = "https://i.ibb.co/WWgWxxtx/image.png";
            }
            catch
            {
                // Se falhar, deixar vazio sem causar erro
            }
        }

        /// <summary>
        /// ETAPA 1: Enviar código de confirmação para o email
        /// </summary>
        private void BtnEnviarCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                emailUtilizador = txtEmail.Text.Trim();

                // Validação: campo não vazio
                if (string.IsNullOrWhiteSpace(emailUtilizador))
                {
                    MessageBox.Show("Por favor, introduza o seu email.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                // Validação: formato de email básico
                if (!emailUtilizador.Contains("@") || !emailUtilizador.Contains("."))
                {
                    MessageBox.Show("Por favor, introduza um email válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                // Verificar se o email existe na base de dados
                DataTable dt = BLL.utilizador.QueryutilizadorByemail(emailUtilizador);
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Não existe nenhuma conta com este email na Livraria Readify.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.Focus();
                    return;
                }

                // Armazenar ID do utilizador para uso posterior
                idUtilizador = Convert.ToInt32(dt.Rows[0]["Id_Utilizador"]);

                // Gerar código de 6 dígitos aleatório
                codigoConfirmacao = GerarCodigoConfirmacao();
                // Definir expiração do código (10 minutos)
                codigoExpiracao = DateTime.Now.AddMinutes(10);

                // Enviar email com o código
                EnviarEmailConfirmacao(emailUtilizador, codigoConfirmacao);

                MessageBox.Show("Código de confirmação enviado para o seu email!\nO código expira em 10 minutos.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Avançar para o passo 2 (verificar código)
                panelEmail.Visible = false;
                panelCodigo.Visible = true;
                panelNovaPassword.Visible = false;
                AtualizarIndicadorPasso(2);
                txtCodigo.Focus();
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show("Erro ao enviar email: Verifique a ligação à internet.\nDetalhes: " + smtpEx.Message, "Erro de Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Erro ao aceder à base de dados: " + sqlEx.Message, "Erro de Base de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ETAPA 2: Verificar o código de confirmação
        /// </summary>
        private void BtnVerificarCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                string codigoInserido = txtCodigo.Text.Trim();

                // Validação: campo não vazio
                if (string.IsNullOrWhiteSpace(codigoInserido))
                {
                    MessageBox.Show("Por favor, introduza o código de confirmação.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCodigo.Focus();
                    return;
                }

                // Validação: verificar expiração do código
                if (DateTime.Now > codigoExpiracao)
                {
                    MessageBox.Show("O código expirou. Por favor, solicite um novo código.", "Código Expirado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnReenviarCodigo_Click(sender, e);
                    return;
                }

                // Validação: comparar código inserido com o enviado
                if (codigoInserido == codigoConfirmacao)
                {
                    MessageBox.Show("Código validado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Avançar para o passo 3 (nova password)
                    panelEmail.Visible = false;
                    panelCodigo.Visible = false;
                    panelNovaPassword.Visible = true;
                    AtualizarIndicadorPasso(3);
                    txtNovaPassword.Focus();
                }
                else
                {
                    MessageBox.Show("Código incorreto. Por favor, tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCodigo.Clear();
                    txtCodigo.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar código: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Reenviar código de confirmação (disponível no passo 2)
        /// </summary>
        private void btnReenviarCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                // Gerar novo código
                codigoConfirmacao = GerarCodigoConfirmacao();
                codigoExpiracao = DateTime.Now.AddMinutes(10);

                // Reenviar email
                EnviarEmailConfirmacao(emailUtilizador, codigoConfirmacao);

                MessageBox.Show("Novo código enviado para o seu email!\nO código expira em 10 minutos.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtCodigo.Clear();
                txtCodigo.Focus();
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show("Erro ao enviar email: Verifique a ligação à internet.\nDetalhes: " + smtpEx.Message, "Erro de Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao reenviar código: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ETAPA 3: Alterar a password do utilizador
        /// </summary>
        private void BtnAlterarPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string novaPassword = txtNovaPassword.Text.Trim();
                string confirmarPassword = txtConfirmarPassword.Text.Trim();

                // Validação: campos não vazios
                if (string.IsNullOrWhiteSpace(novaPassword) || string.IsNullOrWhiteSpace(confirmarPassword))
                {
                    MessageBox.Show("Por favor, preencha ambos os campos de password.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validação: mínimo de 8 caracteres (requisito de segurança)
                if (novaPassword.Length < 8)
                {
                    MessageBox.Show("A password deve ter no mínimo 8 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNovaPassword.Focus();
                    return;
                }

                // Validação: máximo de 20 caracteres
                if (novaPassword.Length > 20)
                {
                    MessageBox.Show("A password deve ter no máximo 20 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNovaPassword.Focus();
                    return;
                }

                // Validação: passwords coincidem
                if (novaPassword != confirmarPassword)
                {
                    MessageBox.Show("As passwords não coincidem. Por favor, verifique.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtConfirmarPassword.Clear();
                    txtConfirmarPassword.Focus();
                    return;
                }

                // Encriptar a nova password usando BCrypt (método já existente no projeto)
                string passwordHash = BLL.utilizador.HashPassword(novaPassword);

                // Atualizar na base de dados
                BLL.utilizador.AtualizarPasswordHash(idUtilizador, passwordHash);

                MessageBox.Show("Password alterada com sucesso!\n\nPode agora fazer login com a nova password.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Fechar formulário
                this.Close();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Erro ao atualizar password na base de dados: " + sqlEx.Message, "Erro de Base de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao alterar password: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cancelar o processo e fechar o formulário
        /// </summary>
        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Voltar do passo 2 para o passo 1 (email)
        /// </summary>
        private void BtnVoltarEmail_Click(object sender, EventArgs e)
        {
            panelEmail.Visible = true;
            panelCodigo.Visible = false;
            panelNovaPassword.Visible = false;
            AtualizarIndicadorPasso(1);
            txtEmail.Focus();
        }

        /// <summary>
        /// Voltar do passo 3 para o passo 2 (código)
        /// </summary>
        private void BtnVoltarCodigo_Click(object sender, EventArgs e)
        {
            panelEmail.Visible = false;
            panelCodigo.Visible = true;
            panelNovaPassword.Visible = false;
            AtualizarIndicadorPasso(2);
            txtCodigo.Focus();
        }

        /// <summary>
        /// Atualiza o indicador visual do passo atual (1, 2 ou 3)
        /// </summary>
        private void AtualizarIndicadorPasso(int passo)
        {
            lblPassoAtual.Text = $"Passo {passo} de 3";
            
            // Atualizar cores dos indicadores
            lblPasso1.ForeColor = passo >= 1 ? Color.FromArgb(46, 204, 113) : Color.Gray;
            lblPasso2.ForeColor = passo >= 2 ? Color.FromArgb(46, 204, 113) : Color.Gray;
            lblPasso3.ForeColor = passo >= 3 ? Color.FromArgb(46, 204, 113) : Color.Gray;
        }

        /// <summary>
        /// Gera um código de confirmação aleatório de 6 dígitos usando criptografia segura
        /// </summary>
        private string GerarCodigoConfirmacao()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[6];
                rng.GetBytes(data);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i] % 10);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Envia email com o código de confirmação usando SMTP do Gmail
        /// </summary>
        private void EnviarEmailConfirmacao(string emailDestino, string codigo)
        {
            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("martimr480@gmail.com", "djniszgkjuxnludr");

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("martimr480@gmail.com", "Livraria Readify");
                    mail.To.Add(emailDestino);
                    mail.Subject = "Código de Confirmação - Recuperação de Password";

                    mail.Body = $@"
Olá,

Recebemos um pedido para recuperar a sua password na Livraria Readify.

O seu código de confirmação é: {codigo}

Este código expira em 10 minutos.

Se não solicitou esta recuperação, por favor ignore este email.

Atenciosamente,
Equipa Readify";

                    mail.IsBodyHtml = false;
                    smtp.Send(mail);
                }
            }
        }
    }
}
