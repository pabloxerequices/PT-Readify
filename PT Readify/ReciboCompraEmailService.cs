using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using BusinessLogicLayer;

namespace PT_Readify
{
    public class ItemReciboCompra
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editora { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal => PrecoUnitario * Quantidade;
    }

    public class ResultadoEnvioRecibo
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public int IdCompra { get; set; }
    }

    public static class ReciboCompraEmailService
    {
        public static ResultadoEnvioRecibo EnviarRecibo(int idUtilizador, DateTime dataCompra, IList<ItemReciboCompra> itens)
        {
            if (itens == null || itens.Count == 0)
                return Falhar("Não existem itens para incluir no recibo.");

            string destinatario = ObterEmailUtilizador(idUtilizador);
            if (string.IsNullOrWhiteSpace(destinatario))
                return Falhar("Não foi possível obter o email do utilizador. Verifique o perfil.");

            string nomeUtilizador = ObterNomeUtilizador(idUtilizador);
            string assunto = "Recibo de Compra - PT Readify";
            string corpo = ConstruirCorpoRecibo(nomeUtilizador, dataCompra, itens);

            return EnviarEmail(destinatario, assunto, corpo);
        }

        private static ResultadoEnvioRecibo Falhar(string mensagem)
        {
            return new ResultadoEnvioRecibo { Sucesso = false, Mensagem = mensagem };
        }

        private static string ObterEmailUtilizador(int idUtilizador)
        {
            var dt = BLL.utilizador.LoadById(idUtilizador);
            if (dt != null && dt.Rows.Count > 0)
            {
                string emailBaseDados = dt.Rows[0]["Email"]?.ToString();
                if (!string.IsNullOrWhiteSpace(emailBaseDados))
                    return emailBaseDados;
            }

            if (!string.IsNullOrWhiteSpace(globais.profileEmail) && globais.id_utilizador == idUtilizador)
                return globais.profileEmail;

            return null;
        }

        private static string ObterNomeUtilizador(int idUtilizador)
        {
            var dt = BLL.utilizador.LoadById(idUtilizador);
            if (dt == null || dt.Rows.Count == 0)
                return "Cliente";

            return dt.Rows[0]["Nome"]?.ToString() ?? "Cliente";
        }

        private static string ConstruirCorpoRecibo(string nomeUtilizador, DateTime dataCompra, IList<ItemReciboCompra> itens)
        {
            var cultura = new CultureInfo("pt-PT");
            decimal totalGeral = 0;
            var linhas = new StringBuilder();

            foreach (var item in itens)
            {
                totalGeral += item.Subtotal;
                linhas.AppendLine("----------------------------------------");
                linhas.AppendLine($"Título:      {item.Titulo}");
                linhas.AppendLine($"Autor:       {item.Autor}");
                linhas.AppendLine($"Quantidade:  {item.Quantidade}");
                linhas.AppendLine($"Preço unit.: {item.PrecoUnitario.ToString("C2", cultura)}");
                linhas.AppendLine($"Subtotal:    {item.Subtotal.ToString("C2", cultura)}");
            }

            var recibo = new StringBuilder();
            recibo.AppendLine("PT READIFY");
            recibo.AppendLine("Recibo de Compra");
            recibo.AppendLine("========================================");
            recibo.AppendLine();
            recibo.AppendLine($"Olá {nomeUtilizador},");
            recibo.AppendLine();
            recibo.AppendLine("A sua compra foi registada com sucesso.");
            recibo.AppendLine();
            recibo.AppendLine($"Data:  {dataCompra.ToString("dd/MM/yyyy", cultura)}");
            recibo.AppendLine($"Hora:  {dataCompra.ToString("HH:mm:ss", cultura)}");
            recibo.AppendLine();
            recibo.AppendLine("Livros adquiridos:");
            recibo.AppendLine();
            recibo.Append(linhas);
            recibo.AppendLine("----------------------------------------");
            recibo.AppendLine($"TOTAL: {totalGeral.ToString("C2", cultura)}");
            recibo.AppendLine();
            recibo.AppendLine("Obrigado pela sua preferência!");
            recibo.AppendLine("PT Readify");

            return recibo.ToString();
        }

        private static SmtpSettings CarregarConfiguracaoSmtp()
        {
            var settings = new SmtpSettings
            {
                Host = ConfigurationManager.AppSettings["SmtpHost"],
                PortTexto = ConfigurationManager.AppSettings["SmtpPort"],
                User = ConfigurationManager.AppSettings["SmtpUser"],
                Password = ConfigurationManager.AppSettings["SmtpPassword"],
                FromEmail = ConfigurationManager.AppSettings["SmtpFromEmail"],
                FromName = ConfigurationManager.AppSettings["SmtpFromName"] ?? "PT Readify",
                EnableSslTexto = ConfigurationManager.AppSettings["SmtpEnableSsl"]
            };

            string ficheiroConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "smtp.config");
            if (!File.Exists(ficheiroConfig))
                return settings;

            foreach (string linha in File.ReadAllLines(ficheiroConfig))
            {
                if (string.IsNullOrWhiteSpace(linha) || linha.TrimStart().StartsWith("#"))
                    continue;

                int separador = linha.IndexOf('=');
                if (separador <= 0)
                    continue;

                string chave = linha.Substring(0, separador).Trim();
                string valor = linha.Substring(separador + 1).Trim();

                switch (chave.ToLowerInvariant())
                {
                    case "host": settings.Host = valor; break;
                    case "port": settings.PortTexto = valor; break;
                    case "user": settings.User = valor; break;
                    case "password": settings.Password = valor; break;
                    case "fromemail": settings.FromEmail = valor; break;
                    case "fromname": settings.FromName = valor; break;
                    case "enablessl": settings.EnableSslTexto = valor; break;
                }
            }

            return settings;
        }

        private static ResultadoEnvioRecibo EnviarEmail(string destinatario, string assunto, string corpo)
        {
            var config = CarregarConfiguracaoSmtp();

            if (string.IsNullOrWhiteSpace(config.Host))
                return Falhar("Servidor SMTP não configurado (SmtpHost).");

            if (string.IsNullOrWhiteSpace(config.FromEmail))
                return Falhar("Email de envio não configurado (SmtpFromEmail).");

            if (string.IsNullOrWhiteSpace(config.User))
                return Falhar("Utilizador SMTP não configurado (SmtpUser).");

            if (string.IsNullOrWhiteSpace(config.Password))
                return Falhar("Password SMTP não configurada (SmtpPassword).");

            if (!int.TryParse(config.PortTexto, out int smtpPort))
                smtpPort = 587;

            bool enableSsl = !bool.TryParse(config.EnableSslTexto, out bool ssl) || ssl;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var cliente = new SmtpClient(config.Host, smtpPort))
                {
                    cliente.EnableSsl = enableSsl;
                    cliente.UseDefaultCredentials = false;
                    cliente.DeliveryMethod = SmtpDeliveryMethod.Network;
                    cliente.Timeout = 30000;
                    cliente.Credentials = new NetworkCredential(config.User, config.Password);

                    using (var mensagem = new MailMessage())
                    {
                        mensagem.From = new MailAddress(config.FromEmail, config.FromName);
                        mensagem.To.Add(destinatario);
                        mensagem.Subject = assunto;
                        mensagem.Body = corpo;
                        mensagem.IsBodyHtml = false;
                        mensagem.BodyEncoding = Encoding.UTF8;
                        mensagem.SubjectEncoding = Encoding.UTF8;

                        cliente.Send(mensagem);
                    }
                }

                return new ResultadoEnvioRecibo
                {
                    Sucesso = true,
                    Mensagem = $"Recibo enviado para {destinatario}."
                };
            }
            catch (SmtpException ex)
            {
                return Falhar($"Erro SMTP: {ex.Message}\n\nVerifique smtp.config ou App.config (host, port, user, password).");
            }
            catch (Exception ex)
            {
                return Falhar($"Erro ao enviar email: {ex.Message}");
            }
        }

        private sealed class SmtpSettings
        {
            public string Host { get; set; }
            public string PortTexto { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public string FromEmail { get; set; }
            public string FromName { get; set; }
            public string EnableSslTexto { get; set; }
        }
    }
}
