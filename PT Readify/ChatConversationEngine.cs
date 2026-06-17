using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BusinessLogicLayer;

namespace PT_Readify
{
    internal enum ChatState
    {
        Idle,
        WaitingBookQuery,
        AfterBookResults
    }

    internal class ChatConversationEngine
    {
        private ChatState _state = ChatState.Idle;
        private readonly List<(string Role, string Text)> _history = new List<(string, string)>();
        private DataTable _lastSearchResults;
        private string _lastTopic;
        private readonly string _userName;

        public bool ShouldEndConversation { get; private set; }

        public ChatConversationEngine()
        {
            _userName = ResolveUserName();
        }

        public void Reset()
        {
            _state = ChatState.Idle;
            _history.Clear();
            _lastSearchResults = null;
            _lastTopic = null;
            ShouldEndConversation = false;
        }

        public string GetOpeningMessage()
        {
            var name = string.IsNullOrEmpty(_userName) ? "" : $" {_userName}";
            return $"Olá{name}! Sou o assistente da PT Readify. Podemos conversar à vontade — pergunte sobre livros, empréstimos, compras, ou diga o que precisa.\n\nComo posso ajudar hoje?";
        }

        public string Reply(string input)
        {
            input = input?.Trim() ?? "";
            if (string.IsNullOrEmpty(input))
                return "Não ouvi nada. Pode escrever outra vez?";

            _history.Add(("user", input));
            if (_history.Count > 20)
                _history.RemoveAt(0);

            var response = BuildResponse(input);
            _history.Add(("bot", response));
            return response;
        }

        private string BuildResponse(string input)
        {
            var s = input.ToLowerInvariant().Trim();

            if (IsGoodbye(s) || ContainsAny(s, "terminar conversa", "fechar chat", "sair do chat"))
            {
                _state = ChatState.Idle;
                ShouldEndConversation = true;
                return Pick(
                    "Até breve! Foi um prazer conversar consigo. Até à próxima!",
                    "Adeus! Volte quando quiser — estarei por aqui.",
                    "Até logo! Boa leitura.");
            }

            if (IsThanks(s))
                return Pick(
                    "De nada! Quer continuar a conversar ou precisa de mais alguma coisa?",
                    "Por nada! Diga-me se precisar de mais informação.");

            if (_state == ChatState.WaitingBookQuery)
                return HandleBookQuery(input);

            if (_state == ChatState.AfterBookResults)
            {
                if (IsAffirmative(s))
                    return DescribeBookAtIndex(0);
                if (IsNegative(s))
                {
                    _state = ChatState.Idle;
                    return "Sem problema. Quer pesquisar outro livro ou falar sobre outro assunto?";
                }
                if (ContainsAny(s, "primeiro", "1", "um"))
                    return DescribeBookAtIndex(0);
                if (ContainsAny(s, "segundo", "2", "dois"))
                    return DescribeBookAtIndex(1);
                if (ContainsAny(s, "terceiro", "3", "três", "tres"))
                    return DescribeBookAtIndex(2);
                if (ContainsAny(s, "outro", "mais", "nova pesquisa"))
                {
                    _state = ChatState.WaitingBookQuery;
                    return "Claro! Diga-me o título ou autor do livro que procura.";
                }
            }

            if (IsAffirmative(s) && _history.Count >= 2)
            {
                var lastBot = _history.LastOrDefault(h => h.Role == "bot").Text ?? "";
                if (lastBot.Contains("Quer saber mais") || lastBot.Contains("lista completa"))
                    return DescribeBookAtIndex(0);
                if (lastBot.Contains("Quer pesquisar"))
                {
                    _state = ChatState.WaitingBookQuery;
                    return "Perfeito! Qual é o título ou autor?";
                }
            }

            if (IsNegative(s))
            {
                _state = ChatState.Idle;
                return "Está bem. Em que mais posso ajudar?";
            }

            if (ContainsAny(s, "olá", "ola", "oi", "hey", "bom dia", "boa tarde", "boa noite"))
                return GetGreeting();

            if (ContainsAny(s, "como estás", "como estas", "tudo bem", "como vai"))
                return "Estou bem, obrigado por perguntar! Em que posso ajudar?";

            if (ContainsAny(s, "como te chamas", "qual é o teu nome", "quem és"))
                return "Chamo-me Assistente PT Readify. Estou aqui para conversar e ajudar com livros, empréstimos e compras.";

            if (ContainsAny(s, "piada", "conta uma piada"))
                return "Porque é que o livro foi ao médico? Porque tinha problemas de lombada!";

            if (ContainsAny(s, "ajuda", "help", "o que sabes", "o que podes fazer"))
                return GetHelpText();

            if (ContainsAny(s, "horas", "que horas"))
                return $"Agora são {DateTime.Now:HH:mm}. Mais alguma coisa?";

            if (ContainsAny(s, "site", "website", "página", "pagina"))
                return "O nosso site é https://siteptreadify.vercel.app/";

            if (ContainsAny(s, "género", "genero", "géneros", "generos", "categorias"))
                return GetGenerosResponse();

            if (ContainsAny(s, "emprest", "requis", "reserva", "levantar livro"))
            {
                _lastTopic = "emprestimo";
                return GetEmprestimoHelp() + "\n\nQuer que eu explique algum passo com mais detalhe?";
            }

            if (ContainsAny(s, "compra", "carrinho", "comprar", "pagar"))
            {
                _lastTopic = "compras";
                return GetComprasHelp() + "\n\nTem alguma dúvida sobre o processo de compra?";
            }

            if (ContainsAny(s, "histórico", "historico", "compras anteriores", "emprestimos anteriores"))
                return GetHistoricoHelp();

            if (ContainsAny(s, "perfil", "conta", "password", "palavra-passe"))
                return "Pode gerir o perfil no botão \"Perfil\" no menu principal.";

            if (ContainsAny(s, "logout", "sair", "terminar sessão", "terminar sessao"))
                return "Use o botão \"Logout\" no menu para terminar sessão.";

            if (WantsBookSearch(s) || (ContainsAny(s, "livro", "livros", "autor", "título", "titulo", "pesquisar", "procurar") && HasSearchableContent(input)))
            {
                if (HasSearchableContent(input))
                    return HandleBookQuery(input);
                _state = ChatState.WaitingBookQuery;
                return "Com prazer! Diga-me o título ou autor do livro.";
            }

            if (!IsAffirmative(s) && _state == ChatState.Idle && input.Length >= 2 &&
                !ContainsAny(s, "como", "quem", "onde", "quando", "porquê", "porque"))
            {
                var guess = TrySearchAsBook(input);
                if (guess != null)
                    return guess;
            }

            return GetContextualFallback();
        }

        private string HandleBookQuery(string input)
        {
            _state = ChatState.AfterBookResults;

            try
            {
                string titulo = ExtractSearchTerm(input, "livro", "livros", "título", "titulo", "pesquisar", "procurar", "quero", "preciso");
                string autor = ExtractSearchTerm(input, "autor", "de", "do", "da", "escrito por", "por");

                if (string.IsNullOrWhiteSpace(titulo) && string.IsNullOrWhiteSpace(autor))
                {
                    var cleaned = input.Trim(' ', '?', '!', '.', ',');
                    if (cleaned.Length >= 2)
                        titulo = cleaned;
                }

                if (string.IsNullOrWhiteSpace(titulo) && string.IsNullOrWhiteSpace(autor))
                {
                    _state = ChatState.WaitingBookQuery;
                    return "Não apanhei o nome do livro. Pode dizer-me o título ou autor?";
                }

                var results = BLL.Livros.pesquisarLivro(titulo, autor, null, null);
                _lastSearchResults = results;

                if (results == null || results.Rows.Count == 0)
                {
                    _state = ChatState.WaitingBookQuery;
                    return $"Não encontrei nada com \"{titulo ?? autor}\". Quer tentar outro título ou autor?";
                }

                var sb = new StringBuilder();
                sb.AppendLine($"Encontrei {results.Rows.Count} livro(s):");
                int count = 0;
                foreach (DataRow row in results.Rows)
                {
                    if (count >= 5)
                    {
                        sb.AppendLine($"... e mais {results.Rows.Count - 5}. Diga \"primeiro\", \"segundo\", etc.");
                        break;
                    }
                    sb.AppendLine($"{count + 1}. {row["Titulo"]} — {row["Autor"]} ({row["Preço"]}€)");
                    count++;
                }
                sb.AppendLine();
                sb.Append("Quer saber mais sobre algum? Diga \"sim\", \"primeiro\"… ou peça outra pesquisa.");
                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                _state = ChatState.Idle;
                return "Tive um problema ao pesquisar. Tente outra vez ou use o menu \"Livros\".\n(" + ex.Message + ")";
            }
        }

        private string DescribeBookAtIndex(int index)
        {
            if (_lastSearchResults == null || _lastSearchResults.Rows.Count == 0)
            {
                _state = ChatState.WaitingBookQuery;
                return "Já não tenho essa pesquisa em memória. Qual livro quer que procure?";
            }

            if (index >= _lastSearchResults.Rows.Count)
                return $"Só tenho {_lastSearchResults.Rows.Count} resultado(s). Escolha entre 1 e {_lastSearchResults.Rows.Count}.";

            var row = _lastSearchResults.Rows[index];
            var sb = new StringBuilder();
            sb.AppendLine($"Sobre \"{row["Titulo"]}\":");
            sb.AppendLine($"• Autor: {row["Autor"]}");
            sb.AppendLine($"• Preço: {row["Preço"]}€");
            sb.AppendLine($"• Estado: {row["Estado_Livro"]}");
            sb.AppendLine();
            sb.Append("Para requisitar, vá a \"Requisições/Empréstimos\". Para comprar, use \"Livros\" e o carrinho.");
            _state = ChatState.Idle;
            return sb.ToString().TrimEnd();
        }

        private string TrySearchAsBook(string input)
        {
            if (input.Split(' ').Length > 6)
                return null;

            try
            {
                var results = BLL.Livros.pesquisarLivro(input, null, null, null);
                if (results == null || results.Rows.Count == 0)
                    return null;

                _lastSearchResults = results;
                _state = ChatState.AfterBookResults;

                if (results.Rows.Count == 1)
                    return $"Acho que se refere a \"{results.Rows[0]["Titulo"]}\" de {results.Rows[0]["Autor"]}. Quer saber mais detalhes?";

                return $"Encontrei {results.Rows.Count} livros relacionados com \"{input}\". Quer que mostre a lista completa?";
            }
            catch
            {
                return null;
            }
        }

        private string GetContextualFallback()
        {
            if (_lastTopic == "emprestimo")
                return GetEmprestimoHelp();
            if (_lastTopic == "compras")
                return GetComprasHelp();

            return Pick(
                "Interessante. Pode explicar um pouco mais? Ou diga \"Ajuda\".",
                "Não percebi bem. Quer falar de livros, empréstimos ou compras?",
                "Experimente \"Quero um livro\" ou \"Como empresto?\"");
        }

        private string GetGreeting()
        {
            var name = string.IsNullOrEmpty(_userName) ? "" : $", {_userName}";
            if (globais.id_utilizador > 0)
                return $"Olá{name}! Sobre o que quer conversar — livros, empréstimos, compras?";
            return $"Olá{name}! Bem-vindo(a) à PT Readify. Por onde começamos?";
        }

        private string GetHelpText()
        {
            return "Podemos conversar naturalmente. Exemplos:\n" +
                   "• \"Quero um livro de fantasia\"\n" +
                   "• \"Como empresto um livro?\"\n" +
                   "• \"sim\" / \"não\" — respondo ao que acabámos de falar\n" +
                   "• \"obrigado\" / \"adeus\" — encerra a conversa\n\nExperimente — estou à escuta!";
        }

        private string GetEmprestimoHelp()
        {
            return "Para requisitar um livro:\n" +
                   "1. Menu \"Requisições/Empréstimos\"\n" +
                   "2. Escolha o livro\n" +
                   "3. Confirme a requisição\n\n" +
                   "O histórico fica em \"Histórico de Empréstimos\".";
        }

        private string GetComprasHelp()
        {
            return "Para comprar:\n" +
                   "1. Menu \"Livros\" → escolha o livro\n" +
                   "2. Adicione ao carrinho\n" +
                   "3. Conclua no carrinho\n\n" +
                   "Histórico em \"Histórico de Compras\".";
        }

        private string GetHistoricoHelp()
        {
            if (globais.id_utilizador <= 0)
                return "Para ver histórico precisa de login. Depois use \"Histórico de Compras\" ou \"Histórico de Empréstimos\".";
            return "No menu tem \"Histórico de Compras\" e \"Histórico de Empréstimos\".";
        }

        private string GetGenerosResponse()
        {
            try
            {
                var generos = BLL.Livros.ObterGeneros();
                if (generos == null || generos.Count == 0)
                    return "Ainda não há géneros registados.";
                return "Géneros:\n• " + string.Join("\n• ", generos.Take(12)) +
                       (generos.Count > 12 ? $"\n... e mais {generos.Count - 12}." : "");
            }
            catch (Exception ex)
            {
                return "Não consegui listar géneros. (" + ex.Message + ")";
            }
        }

        private static string ResolveUserName()
        {
            if (globais.id_utilizador <= 0) return null;
            try
            {
                var dt = BLL.utilizador.LoadById(globais.id_utilizador);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["Nome"]?.ToString()?.Split(' ').FirstOrDefault();
            }
            catch { }
            return null;
        }

        private static bool WantsBookSearch(string s) =>
            ContainsAny(s, "quero um livro", "quero livro", "procurar livro", "pesquisar livro", "recomenda");

        private static bool HasSearchableContent(string input)
        {
            var cleaned = ExtractSearchTerm(input, "livro", "livros", "título", "titulo", "pesquisar", "procurar", "quero", "autor", "de", "do", "da");
            return !string.IsNullOrWhiteSpace(cleaned) && cleaned.Length >= 2;
        }

        private static bool IsAffirmative(string s) =>
            ContainsAny(s, "sim", "claro", "por favor", "ok", "okay", "pode ser", "isso", "exato", "yes");

        private static bool IsNegative(string s) =>
            s == "não" || s == "nao" || ContainsAny(s, "agora não", "agora nao", "deixa estar");

        private static bool IsThanks(string s) =>
            ContainsAny(s, "obrigado", "obrigada", "brigado", "thanks");

        private static bool IsGoodbye(string s) =>
            ContainsAny(s, "adeus", "tchau", "xau", "até logo", "ate logo", "até breve", "ate breve", "bye");

        private static bool ContainsAny(string text, params string[] terms) =>
            terms.Any(term => text.Contains(term));

        private static string ExtractSearchTerm(string input, params string[] keywords)
        {
            var text = input;
            foreach (var keyword in keywords.OrderByDescending(k => k.Length))
            {
                int idx = text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                    text = text.Remove(idx, keyword.Length);
            }
            text = text.Trim(' ', '?', '!', '.', ',', ':', ';', '"', '\'');
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }

        private static string Pick(params string[] options) =>
            options[new Random().Next(options.Length)];
    }
}
