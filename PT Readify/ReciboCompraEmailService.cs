using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq; // Adicione esta linha no topo do arquivo, junto com os outros using
using System.Net;
using System.Net.Mail;
using System.Net.Mime; // Necessário para AlternateView e LinkedResource
using System.Text;
using BusinessLogicLayer;
using System.Xml.Linq;


namespace PT_Readify
{
    public class ItemReciboCompra
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal => PrecoUnitario * Quantidade;
        public byte[] FotoLivro { get; set; } // Adicionado para receber a imagem da Base de Dados
        public string CidImagem { get; set; } // Identificador interno para o HTML
    }

    public class ResultadoEnvioRecibo
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
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
            string assunto = "Recibo de Compra e Confirmação - PT Readify";

            // Gerar IDs únicos para as imagens de cada livro antes de construir o HTML
            for (int i = 0; i < itens.Count; i++)
            {
                itens[i].CidImagem = $"livro_{i}";
            }

            string corpoHtml = ConstruirCorpoRecibo(nomeUtilizador, dataCompra, itens);

            return EnviarEmailComImagens(destinatario, assunto, corpoHtml, itens);
        }

        private static ResultadoEnvioRecibo Falhar(string mensagem)
        {
            return new ResultadoEnvioRecibo { Sucesso = false, Mensagem = mensagem };
        }

        private static string ObterEmailUtilizador(int idUtilizador)
        {
            // Se não foi passado um id, tenta usar o utilizador autenticado na sessão
            int id = idUtilizador;
            if (id <= 0 && globais.id_utilizador > 0)
                id = globais.id_utilizador;

            if (id <= 0)
                return null;

            // Tenta obter o email diretamente da base de dados pelo Id do utilizador
            var dt = BLL.utilizador.LoadById(id);
            if (dt == null || dt.Rows.Count == 0)
                return null;

            string email = dt.Rows[0]["Email"]?.ToString();

            // Se o utilizador for o autenticado, guarda em globais.profileEmail para reutilizações
            if (globais.id_utilizador == id && string.IsNullOrWhiteSpace(globais.profileEmail))
                globais.profileEmail = email;

            return email;
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
            var linhasHtml = new StringBuilder();

            // Prazo de devolução: 30 dias após a data da compra (alterado de 14 para 30)
            DateTime dataLimiteDevolucao = dataCompra.AddDays(30);

            foreach (var item in itens)
            {
                totalGeral += item.Subtotal;

                string tagImagem = (item.FotoLivro != null && item.FotoLivro.Length > 0)
                    ? $"<img src='cid:{item.CidImagem}' alt='Capa' style='width: 50px; height: 75px; object-fit: cover; border-radius: 4px; margin-right: 10px; vertical-align: middle;' />"
                    : "<div style='width: 50px; height: 75px; background: #e0e0e0; display: inline-block; margin-right: 10px; vertical-align: middle; text-align: center; line-height: 75px; font-size: 10px; color: #7f8c8d;'>Sem Capa</div>";

                linhasHtml.AppendLine($@"
                    <tr>
                        <td style='padding: 10px; border-bottom: 1px solid #eee;'>
                            <div style='display: flex; align-items: center;'>
                                {tagImagem}
                                <div style='display: inline-block; vertical-align: middle;'>
                                    <strong style='color: #2c3e50;'>{item.Titulo}</strong><br>
                                    <span style='font-size: 12px; color: #7f8c8d;'>Autor: {item.Autor}</span>
                                </div>
                            </div>
                        </td>
                        <td style='padding: 10px; border-bottom: 1px solid #eee; text-align: center;'>{item.Quantidade}</td>
                        <td style='padding: 10px; border-bottom: 1px solid #eee; text-align: right;'>{item.PrecoUnitario.ToString("C2", cultura)}</td>
                        <td style='padding: 10px; border-bottom: 1px solid #eee; text-align: right; font-weight: bold;'>{item.Subtotal.ToString("C2", cultura)}</td>
                    </tr>");
            }

            var recibo = new StringBuilder();
            recibo.AppendLine($@"
                <html>
                <body style='font-family: ""Segoe UI"", Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 20px;'>
                    <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.05);'>
                        
                        <div style='background-color: #2c3e50; padding: 25px; text-align: center; color: #ffffff;'>
                            <h1 style='margin: 0; font-size: 24px; letter-spacing: 1px;'>PT READIFY</h1>
                            <p style='margin: 5px 0 0 0; opacity: 0.8; font-size: 14px;'>Recibo Oficial de Compra</p>
                        </div>

                        <div style='padding: 30px;'>
                            <p style='font-size: 16px; margin-top: 0;'>Olá <strong>{nomeUtilizador}</strong>,</p>
                            <p style='color: #555; line-height: 1.5;'>Agradecemos a sua compra na PT Readify. O seu pedido foi processado com sucesso.</p>
                            
                            <div style='background-color: #f8f9fa; padding: 15px; border-radius: 6px; margin: 20px 0; font-size: 14px; color: #333; border-left: 4px solid #2c3e50;'>
                                <strong>Data da Compra:</strong> {dataCompra.ToString("dd/MM/yyyy", cultura)}<br>
                                <strong>Hora da Compra:</strong> {dataCompra.ToString("HH:mm:ss", cultura)}<br>
                                <strong style='color: #c0392b;'>Prazo Limite de Devolução:</strong> {dataLimiteDevolucao.ToString("dd/MM/yyyy", cultura)} (Até 30 dias)
                            </div>

                            <table style='width: 100%; border-collapse: collapse; margin-top: 10px;'>
                                <tr style='border-bottom: 2px solid #2c3e50; text-align: left; font-size: 13px; color: #7f8c8d;'>
                                    <th style='padding: 10px; text-align: left;'>Livro</th>
                                    <th style='padding: 10px; text-align: center;'>Qtd</th>
                                    <th style='padding: 10px; text-align: right;'>Preço Unit.</th>
                                    <th style='padding: 10px; text-align: right;'>Subtotal</th>
                                </tr>
                                {linhasHtml}
                            </table>

                            <table style='width: 100%; margin-top: 20px;'>
                                <tr>
                                    <td style='text-align: right; font-size: 16px; padding: 10px 0;'><strong>Valor Total Pago:</strong></td>
                                    <td style='text-align: right; font-size: 20px; color: #27ae60; font-weight: bold; width: 130px; padding: 10px 0;'>
                                        {totalGeral.ToString("C2", cultura)}
                                    </td>
                                </tr>
                            </table>

                            <div style='margin-top: 25px; padding: 10px; background-color: #fffaf0; border: 1px dashed #f39c12; border-radius: 4px; font-size: 12px; color: #d35400;'>
                                * <strong>Nota sobre devoluções:</strong> Os livros comprados podem ser devolvidos até à data limite indicada acima (30 dias) mediante as políticas da loja.
                            </div>

                            <hr style='border: 0; border-top: 1px solid #eeeeee; margin: 30px 0;'>
                            
                            <p style='text-align: center; font-size: 14px; color: #2c3e50; margin: 0;'>
                                <strong>Boas leituras!</strong>
                            </p>
                        </div>

                        <div style='background-color: #f1f2f6; padding: 15px; text-align: center; font-size: 11px; color: #7f8c8d;'>
                            Este é um e-mail automático do sistema PT Readify. Por favor, não responda a esta mensagem.
                        </div>
                    </div>
                </body>
                </html>");

            return recibo.ToString();
        }

        private static ResultadoEnvioRecibo EnviarEmailComImagens(string destinatario, string assunto, string corpoHtml, IList<ItemReciboCompra> itens)
        {
            var config = CarregarConfiguracaoSmtp();
            string ficheiroConfig = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "smtp.config");

            // Validações detalhadas de configuração do servidor SMTP (retorna mensagem com chaves faltantes)
            var faltam = new List<string>();
            if (string.IsNullOrWhiteSpace(config.Host)) faltam.Add("host / SmtpHost");
            if (string.IsNullOrWhiteSpace(config.User)) faltam.Add("user / SmtpUser");
            if (string.IsNullOrWhiteSpace(config.Password)) faltam.Add("password / SmtpPassword");
            if (string.IsNullOrWhiteSpace(config.FromEmail)) faltam.Add("fromemail / SmtpFromEmail");

            if (faltam.Count > 0)
            {
                string msg = "Configurações SMTP incompletas: " + string.Join(", ", faltam) +
                             $". Verifique o ficheiro '{ficheiroConfig}' (ou appSettings em app.config).";
                return new ResultadoEnvioRecibo { Sucesso = false, Mensagem = msg };
            }

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
                    cliente.Credentials = new NetworkCredential(config.User, config.Password);
                    cliente.Timeout = 30000;

                    using (var mensagem = new MailMessage())
                    {
                        mensagem.From = new MailAddress(config.FromEmail, config.FromName);
                        mensagem.To.Add(destinatario);
                        mensagem.Subject = assunto;
                        mensagem.SubjectEncoding = Encoding.UTF8;

                        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(corpoHtml, Encoding.UTF8, MediaTypeNames.Text.Html);

                        foreach (var item in itens)
                        {
                            if (item.FotoLivro != null && item.FotoLivro.Length > 0)
                            {
                                MemoryStream ms = new MemoryStream(item.FotoLivro);
                                LinkedResource recursoImagem = new LinkedResource(ms, MediaTypeNames.Image.Jpeg)
                                {
                                    ContentId = item.CidImagem
                                };
                                htmlView.LinkedResources.Add(recursoImagem);
                            }
                        }

                        mensagem.AlternateViews.Add(htmlView);
                        cliente.Send(mensagem);
                    }
                }

                return new ResultadoEnvioRecibo { Sucesso = true, Mensagem = $"Recibo enviado para {destinatario}." };
            }
            catch (SmtpException sx)
            {
                return new ResultadoEnvioRecibo { Sucesso = false, Mensagem = $"Erro SMTP: {sx.StatusCode} - {sx.Message}" };
            }
            catch (Exception ex)
            {
                return new ResultadoEnvioRecibo { Sucesso = false, Mensagem = $"Erro ao enviar: {ex.Message}" };
            }
        }

        // Mantém-se igual o método CarregarConfiguracaoSmtp() e a classe SmtpSettings do teu código original...
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

            // Procura por possíveis ficheiros de configuração (prioriza ficheiro de texto não-XML)
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] candidatos = new[]
            {
                Path.Combine(baseDir, "smtp.config.txt"),
                Path.Combine(baseDir, "smtp.ini"),
                Path.Combine(baseDir, "smtp.config") // fallback
            };

            string ficheiroConfig = candidatos.FirstOrDefault(File.Exists);
            if (ficheiroConfig == null) return settings;

            string conteudo = File.ReadAllText(ficheiroConfig).Trim();
            // Se for XML, parseia como XML (para evitar erros do editor do VS)
            if (conteudo.StartsWith("<"))
            {
                try
                {
                    var doc = XDocument.Parse(conteudo);
                    XElement root = doc.Root;
                    if (root != null)
                    {
                        settings.Host = (string)root.Element("host") ?? settings.Host;
                        settings.PortTexto = (string)root.Element("port") ?? settings.PortTexto;
                        settings.User = (string)root.Element("user") ?? settings.User;
                        settings.Password = (string)root.Element("password") ?? settings.Password;
                        settings.FromEmail = (string)root.Element("fromemail") ?? settings.FromEmail;
                        settings.FromName = (string)root.Element("fromname") ?? settings.FromName;
                        settings.EnableSslTexto = (string)root.Element("enablessl") ?? settings.EnableSslTexto;
                    }
                }
                catch
                {
                    // Se XML inválido, ignora e tenta fallback para formato key=value
                }
            }

            // Se não foi XML ou XML inválido, tenta ler formato chave=valor
            if (!conteudo.StartsWith("<"))
            {
                foreach (string linha in File.ReadAllLines(ficheiroConfig))
                {
                    if (string.IsNullOrWhiteSpace(linha) || linha.TrimStart().StartsWith("#")) continue;
                    int separador = linha.IndexOf('=');
                    if (separador <= 0) continue;
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
            }

            return settings;
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
