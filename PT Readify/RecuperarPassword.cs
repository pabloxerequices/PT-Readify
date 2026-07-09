using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using BusinessLogicLayer;

namespace PT_Readify
{
    public partial class RecuperarPassword : Form
    {
        private string emailUtilizador = string.Empty;
        private string codigoConfirmacao = string.Empty;
        private int idUtilizador = 0;

        public RecuperarPassword()
        {
            InitializeComponent();
        }

        private void RecuperarPassword_Load(object sender, EventArgs e)
        {
            panelEmail.Visible = true;
            panelCodigo.Visible = false;
            panelNovaPassword.Visible = false;
            
            // Load logo image if available
            try
            {
                pictureBoxLogo.ImageLocation = "https://i.ibb.co/WWgWxxtx/image.png";
            }
            catch
            {
                // If image fails to load, leave empty
            }
        }

        private void BtnEnviarCodigo_Click(object sender, EventArgs e)
        {
            emailUtilizador = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(emailUtilizador))
            {
                MessageBox.Show("Por favor, introduza o seu email.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = BLL.utilizador.QueryutilizadorByemail(emailUtilizador);
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Não existe nenhuma conta com este email.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            idUtilizador = Convert.ToInt32(dt.Rows[0]["Id_Utilizador"]);

            try
            {
                codigoConfirmacao = GerarCodigoConfirmacao();
                EnviarEmailConfirmacao(emailUtilizador, codigoConfirmacao);

                MessageBox.Show("Código de confirmação enviado para o seu email!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                panelEmail.Visible = false;
                panelCodigo.Visible = true;
                panelNovaPassword.Visible = false;
                txtCodigo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao enviar email: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVerificarCodigo_Click(object sender, EventArgs e)
        {
            string codigoInserido = txtCodigo.Text.Trim();

            if (string.IsNullOrWhiteSpace(codigoInserido))
            {
                MessageBox.Show("Por favor, introduza o código de confirmação.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (codigoInserido == codigoConfirmacao)
            {
                MessageBox.Show("Código validado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                panelEmail.Visible = false;
                panelCodigo.Visible = false;
                panelNovaPassword.Visible = true;
                txtNovaPassword.Focus();
            }
            else
            {
                MessageBox.Show("Código incorreto. Por favor, tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCodigo.Clear();
                txtCodigo.Focus();
            }
        }

        private void BtnAlterarPassword_Click(object sender, EventArgs e)
        {
            string novaPassword = txtNovaPassword.Text.Trim();
            string confirmarPassword = txtConfirmarPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(novaPassword) || string.IsNullOrWhiteSpace(confirmarPassword))
            {
                MessageBox.Show("Por favor, preencha ambos os campos de password.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (novaPassword.Length < 4)
            {
                MessageBox.Show("A password deve ter no mínimo 4 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (novaPassword.Length > 20)
            {
                MessageBox.Show("A password deve ter no máximo 20 caracteres.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (novaPassword != confirmarPassword)
            {
                MessageBox.Show("As passwords não coincidem.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string passwordHash = BLL.utilizador.HashPassword(novaPassword);
                BLL.utilizador.AtualizarPasswordHash(idUtilizador, passwordHash);

                MessageBox.Show("Password alterada com sucesso! Pode agora fazer login com a nova password.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao alterar password: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnVoltarEmail_Click(object sender, EventArgs e)
        {
            panelEmail.Visible = true;
            panelCodigo.Visible = false;
            panelNovaPassword.Visible = false;
            txtEmail.Focus();
        }

        private void BtnVoltarCodigo_Click(object sender, EventArgs e)
        {
            panelEmail.Visible = false;
            panelCodigo.Visible = true;
            panelNovaPassword.Visible = false;
            txtCodigo.Focus();
        }

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
