using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

public class Email
{
    private string Provider { get; set; }
    private string Username { get; set; }
    private string Password { get; set; }

    // Construtor da classe
    public Email(string provider, string username, string password)
    {
        Provider = provider;
        Username = username;
        Password = password;
    }

    // Método principal público para enviar o e-mail
    public void SendEmail(List<string> emailsTo, string subject, string body, List<string> attachments)
    {
        MailMessage message = PrepareMessage(emailsTo, subject, body, attachments);
        SendEmailBySmtp(message);
    }

    // Método privado para preparar os dados do e-mail (Clean Code)
    private MailMessage PrepareMessage(List<string> emailsTo, string subject, string body, List<string> attachments)
    {
        var mail = new MailMessage();

        // Configura quem está enviando
        mail.From = new MailAddress(Username);

        // Adiciona e valida os destinatários da lista
        foreach (var email in emailsTo)
        {
            if (ValidateEmail(email))
            {
                mail.To.Add(email);
            }
        }

        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true; // Permite usar tags HTML no corpo do e-mail

        // Processa e adiciona a lista de anexos
        foreach (var path in attachments)
        {
            var fileInfo = new FileInfo(path);

            // Cria o anexo com base no ficheiro e tipo de dados (sintaxe padrão Microsoft)
            Attachment data = new Attachment(path, System.Net.Mime.MediaTypeNames.Application.Octet);

            // Define as propriedades de tempo de criação e modificação do arquivo
            System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = File.GetCreationTime(path);
            disposition.ModificationDate = File.GetLastWriteTime(path);
            disposition.ReadDate = File.GetLastAccessTime(path);

            mail.Attachments.Add(data);
        }

        return mail;
    }

    // Método privado que configura o cliente SMTP e faz o disparo do e-mail
    private void SendEmailBySmtp(MailMessage message)
    {
        var smtp = new SmtpClient();

        smtp.Host = Provider;
        smtp.Port = 587;
        smtp.EnableSsl = true;
        smtp.Timeout = 5000; // Define um limite de tempo de 5 segundos
        smtp.UseDefaultCredentials = false; // Indica que usará credenciais próprias
        smtp.Credentials = new NetworkCredential(Username, Password);

        smtp.Send(message);
        smtp.Dispose(); // Fecha a conexão para libertar recursos
    }

    // Validador de expressões regulares para checar se o e-mail inserido é válido
    private bool ValidateEmail(string email)
    {
        // Expressão regular padrão para validação de e-mail obtida na internet
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
}