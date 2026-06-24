using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BusinessLogicLayer;

namespace PT_Readify
{
    
    internal enum ChatState { Idle, WaitingBookQuery, AfterBookResults }

    internal class ChatConversationEngine
    {
        // ==========================================
        // CAMPOS PRIVADOS
        // ==========================================

        private ChatState _state = ChatState.Idle;
        private readonly List<(string Role, string Text)> _history = new List<(string, string)>();
        private DataTable _lastSearchResults;
        private string _lastTopic;
        private string _userName;
        private static readonly Random _random = new Random();

       
        private static readonly Dictionary<string, string> _staticResponses = new Dictionary<string, string>()
        {
            { "status", "Estou bem, obrigado por perguntar! Em que posso ajudar?" },
            { "name", "Chamo-me Assistente PT Readify. Estou aqui para conversar e ajudar com livros, empréstimos e compras." },
            { "time", null }, // Tratado especialmente para incluir a hora
            { "website", "O nosso site é https://siteptreadify.vercel.app/" },
            { "duration", "O prazo padrão para qualquer empréstimo na PT Readify é de **15 dias**. Pode solicitar uma renovação antes do prazo terminar, desde que o livro não esteja reservado por outro leitor." }
        };

        public bool ShouldEndConversation { get; private set; }

        public ChatConversationEngine() => _userName = ResolveUserName();

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
            _userName = ResolveUserName();
            var name = string.IsNullOrEmpty(_userName) ? "" : string.Format(" {0}", _userName);
            return string.Format("Olá{0}! Sou o assistente da PT Readify. Podemos conversar à vontade — pergunte sobre livros, empréstimos, compras, devoluções, ou diga o que precisa.\n\nComo posso ajudar hoje?", name);
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

            // ==========================================
            // 1. VERIFICAÇÕES GLOBAIS (Prioridade Absoluta)
            // ==========================================

            if (IsGoodbye(s) || Match(s, "terminar conversa", "fechar chat", "sair do chat"))
                return HandleGoodbye();

            if (IsThanks(s))
                return HandleThanks();

            if (Match(s, "olá", "ola", "oi", "hey", "bom dia", "boa tarde", "boa noite"))
            {
                _state = ChatState.Idle;
                return GetGreeting();
            }

            if (Match(s, "ajuda", "help", "o que sabes", "o que podes fazer"))
            {
                _state = ChatState.Idle;
                return GetHelpText();
            }

            if (IsNegative(s))
            {
                _state = ChatState.Idle;
                return "Está bem, cancelei a operação. Em que mais posso ajudar ou que dúvida tem?";
            }

            // ==========================================
            // 2. MÁQUINA DE ESTADOS
            // ==========================================

            if (_state == ChatState.WaitingBookQuery)
            {
                if (WantsBookSearch(s))
                    return "Estou à espera do nome do livro! Diga-me o título ou o autor.";
                return HandleBookQuery(input);
            }

            if (_state == ChatState.AfterBookResults)
            {
                if (IsAffirmative(s)) return DescribeBookAtIndex(0);
                if (Match(s, "primeiro", "1", "um")) return DescribeBookAtIndex(0);
                if (Match(s, "segundo", "2", "dois")) return DescribeBookAtIndex(1);
                if (Match(s, "terceiro", "3", "três", "tres")) return DescribeBookAtIndex(2);
                if (Match(s, "outro", "mais", "nova pesquisa"))
                {
                    _state = ChatState.WaitingBookQuery;
                    return "Claro! Diga-me o título ou autor do livro que procura.";
                }
            }

            if (IsAffirmative(s) && CheckLastBotMessage(new[] { "Quer saber mais", "lista completa" }))
                return DescribeBookAtIndex(0);

            if (IsAffirmative(s) && CheckLastBotMessage(new[] { "Quer pesquisar" }))
            {
                _state = ChatState.WaitingBookQuery;
                return "Perfeito! Qual é o título ou autor?";
            }

            // ==========================================
            // 3. RESPOSTAS ESTÁTICAS
            // ==========================================

            if (Match(s, "como estás", "como estas", "tudo bem", "como vai"))
                return _staticResponses["status"];

            if (Match(s, "como te chamas", "qual é o teu nome", "quem és", "quem es"))
                return _staticResponses["name"];

            if (Match(s, "horas", "que horas"))
                return string.Format("Agora são {0:HH:mm}. Mais alguma coisa?", DateTime.Now);

            if (Match(s, "site", "website", "página", "pagina"))
                return _staticResponses["website"];

            if (Match(s, "género", "genero", "géneros", "generos", "categorias"))
                return GetGenerosResponse();

            if (Match(s, "devolver", "devolvo", "devolução", "devolucao", "entregar livro", "entregar o livro"))
            {
                _lastTopic = "devolucao";
                return GetTopicHelp("devolucao");
            }

            if (Match(s, "quanto tempo", "de quanto tempo", "prazo", "dias posso ficar", "duracao", "duração"))
                return _staticResponses["duration"];

            if (Match(s, "como funcionam os emprestimos", "como funciona o emprestimo", "como funcionam os empréstimos", "regras do emprestimo", "regras dos emprestimos"))
            {
                _lastTopic = "emprestimo";
                return GetEmprestimoRules();
            }

            if (Match(s, "como posso efetuará uma compra", "como compro", "como fazer compra", "efetuar compra", "passos para comprar"))
            {
                _lastTopic = "compras";
                return GetComprasRules();
            }

            if (Match(s, "emprest", "requis", "reserva", "levantar livro"))
            {
                _lastTopic = "emprestimo";
                return GetTopicHelp("emprestimo") + "\n\nQuer que eu explique algum passo com mais detalhe?";
            }

            if (Match(s, "compra", "carrinho", "comprar", "pagar"))
            {
                _lastTopic = "compras";
                return GetTopicHelp("compras") + "\n\nTem alguma dúvida sobre o processo de compra?";
            }

            if (Match(s, "histórico", "historico", "compras anteriores", "emprestimos anteriores"))
                return GetHistoricoHelp();

            if (Match(s, "perfil", "conta", "password", "palavra-passe", "senha"))
                return "Pode gerir o perfil no botão \"Perfil\" no menu principal.";

            if (Match(s, "logout", "sair", "terminar sessão", "terminar sessao"))
                return "Use o botão \"Logout\" no menu para terminar sessão.";

            // ==========================================
            // 4. PESQUISA DE LIVROS
            // ==========================================

            if ((WantsBookSearch(s) || (Match(s, "livro", "livros", "autor", "título", "titulo", "pesquisar", "procurar") && HasSearchableContent(input))))
            {
                if (HasSearchableContent(input))
                    return HandleBookQuery(input);

                _state = ChatState.WaitingBookQuery;
                return "Com prazer! Diga-me o título ou autor do livro.";
            }

            
            if (_state == ChatState.Idle && input.Length >= 2 && !IsQuestion(s))
            {
                var guess = TrySearchAsBook(input);
                if (guess != null)
                    return guess;
            }

            return GetContextualFallback();
        }

        // ==========================================
        // MÉTODOS DE MANIPULAÇÃO DE ESTADO
        // ==========================================

        private string HandleGoodbye()
        {
            _state = ChatState.Idle;
            ShouldEndConversation = true;
            return Pick(
                "Até breve! Foi um prazer conversar consigo. Até à próxima!",
                "Adeus! Volte quando quiser — estarei por aqui.",
                "Até logo! Boa leitura.");
        }

        private string HandleThanks()
        {
            _state = ChatState.Idle;
            return Pick(
                "De nada! Quer continuar a conversar ou precisa de mais alguma coisa?",
                "Por nada! Diga-me se precisar de mais informação.");
        }

        // ==========================================
        // MÉTODOS DE REGRAS DE NEGÓCIO
        // ==========================================
        private string GetEmprestimoRules()
        {
            return "Os empréstimos funcionam assim:\n" +
                   "1. Cada leitor pode requisitar livros que estejam disponíveis.\n" +
                   "2. O prazo de entrega é de **15 dias**.\n" +
                   "3. Pode levantar o livro fisicamente ou consultar o estado no seu menu de perfil.\n" +
                   "4. Se atrasar a entrega, a sua conta poderá ficar suspensa temporariamente para novas requisições.\n\n" +
                   "Quer que eu explique como requisitar passo a passo?";
        }

       
        private string GetComprasRules()
        {
            return "Para efetuar uma compra na PT Readify, siga estes passos simples:\n" +
                   "1. Vá ao menu **\"Livros\"** no ecrã principal.\n" +
                   "2. Clique no livro que deseja e selecione **\"Adicionar ao Carrinho\"**.\n" +
                   "3. Aceda ao seu **Carrinho de Compras**, valide os itens e clique em **\"Concluir Compra\"**.\n" +
                   "4. Escolha o método de pagamento e confirme.\n\n" +
                   "Ficou com alguma dúvida sobre o processo?";
        }

        
        private string GetTopicHelp(string topic)
        {
            switch (topic)
            {
                case "devolucao":
                    return "Para devolver um livro emprestado:\n" +
                           "1. Dirija-se ao menu **\"Histórico de Empréstimos\"** ou **\"Perfil\"**.\n" +
                           "2. Identifique o livro que está atualmente consigo.\n" +
                           "3. Se a entrega for feita via balcão físico, o funcionário dará baixa no sistema.\n" +
                           "4. Certifique-se de que o estado do livro passa para **'Devolvido'** na sua aplicação.";
                case "emprestimo":
                    return "Para requisitar um livro:\n" +
                           "1. Vá ao Menu **\"Requisições/Empréstimos\"**\n" +
                           "2. Escolha o livro que pretende (Prazo: 15 dias)\n" +
                           "3. Confirme a requisição.";
                case "compras":
                    return "Para comprar:\n" +
                           "1. Menu **\"Livros\"** → escolha o livro\n" +
                           "2. Adicione ao carrinho\n" +
                           "3. Conclua no carrinho de compras.";
                default:
                    return "";
            }
        }

        // ==========================================
        // MÉTODOS DE PESQUISA E RESULTADOS DE LIVROS
        // ==========================================

       
        private string HandleBookQuery(string input)
        {
            _state = ChatState.AfterBookResults;
            try
            {
                string searchTerm = CleanCommandWords(input);

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    _state = ChatState.Idle;
                    return "Não percebi o nome do livro. Ficou com mais alguma dúvida ou quer tentar outro assunto?";
                }

                var results = BLL.Livros.pesquisarLivro(searchTerm, searchTerm, null, null);
                _lastSearchResults = results;

                if (results == null || results.Rows.Count == 0)
                {
                    _state = ChatState.Idle;
                    return string.Format("Não encontrei nenhum livro relacionado com \"{0}\". Ficou com mais alguma dúvida ou quer tentar outra pesquisa?", searchTerm);
                }

                var sb = new StringBuilder(string.Format("Encontrei {0} livro(s):\n", results.Rows.Count));
                for (int i = 0; i < Math.Min(5, results.Rows.Count); i++)
                {
                    var row = results.Rows[i];
                    sb.AppendLine(string.Format("{0}. **{1}** — {2} ({3}€)", i + 1, row["Titulo"], row["Autor"], row["Preço"]));
                }

                if (results.Rows.Count > 5)
                    sb.AppendLine(string.Format("... e mais {0}. Diga \"primeiro\", \"segundo\", etc.", results.Rows.Count - 5));

                sb.Append("\nQuer saber mais sobre algum? Diga \"sim\", \"primeiro\"… ou peça outra pesquisa.");
                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                _state = ChatState.Idle;
                return string.Format("Tive um problema ao pesquisar. Tente outra vez ou use o menu \"Livros\".\n({0})", ex.Message);
            }
        }

        
        private string DescribeBookAtIndex(int index)
        {
            if (_lastSearchResults == null || _lastSearchResults.Rows.Count == 0)
            {
                _state = ChatState.Idle;
                return "Já não tenho essa pesquisa em memória. Ficou com alguma dúvida ou quer procurar outro livro?";
            }

            if (index >= _lastSearchResults.Rows.Count)
                return string.Format("Só tenho {0} resultado(s). Escolha entre 1 e {0}.", _lastSearchResults.Rows.Count);

            var row = _lastSearchResults.Rows[index];
            _state = ChatState.Idle;
            return string.Format(
                "Sobre **\"{0}\"**:\n" +
                "• **Autor:** {1}\n" +
                "• **Preço:** {2}€\n" +
                "• **Estado:** {3}\n\n" +
                "Para requisitar, vá a \"Requisições/Empréstimos\". Para comprar, use \"Livros\" e o carrinho.",
                row["Titulo"], row["Autor"], row["Preço"], row["Estado_Livro"]);
        }

       
        private string TrySearchAsBook(string input)
        {
            if (input.Split(' ').Length > 6) return null;
            try
            {
                var cleaned = CleanCommandWords(input);
                if (string.IsNullOrWhiteSpace(cleaned) || cleaned.Length < 2) return null;

                var results = BLL.Livros.pesquisarLivro(cleaned, cleaned, null, null);
                if (results == null || results.Rows.Count == 0)
                {
                    _state = ChatState.Idle;
                    return null;
                }

                _lastSearchResults = results;
                _state = ChatState.AfterBookResults;

                if (results.Rows.Count == 1)
                    return string.Format("Acho que se refere a **\"{0}\"** de {1}. Quer saber mais detalhes?", results.Rows[0]["Titulo"], results.Rows[0]["Autor"]);
                else
                    return string.Format("Encontrei {0} livros relacionados com \"{1}\". Quer que mostre a lista completa?", results.Rows.Count, cleaned);
            }
            catch
            {
                _state = ChatState.Idle;
                return null;
            }
        }

        // ==========================================
        // MÉTODOS AUXILIARES DE RESPOSTA
        // ==========================================

        
        private string GetContextualFallback()
        {
            _state = ChatState.Idle;

            if (_lastTopic == "emprestimo")
                return "Ainda sobre empréstimos: lembre-se que o prazo máximo é de 15 dias. Tem mais alguma dúvida sobre isto?";
            if (_lastTopic == "compras")
                return "Ficou com alguma dúvida sobre o processo de compras ou precisa de ajuda com outro assunto?";
            if (_lastTopic == "devolucao")
                return "Ficou claro como efetuar a devolução? Diga-me se tem mais alguma questão.";

            return Pick(
                "Não percebi bem o que quis dizer. Tem mais alguma dúvida sobre livros, empréstimos ou compras?",
                "Não consegui identificar o que precisa. Em que mais posso ajudar?",
                "Ficou com alguma dúvida? Experimente perguntar: \"Como funcionam os empréstimos?\" ou diga o livro que procura.");
        }

        
        private string GetGreeting()
        {
            _userName = ResolveUserName();
            var name = string.IsNullOrEmpty(_userName) ? "" : string.Format(", {0}", _userName);
            if (globais.id_utilizador > 0)
                return string.Format("Olá{0}! Sobre o que quer conversar hoje — livros, prazos de empréstimos, devoluções ou compras?", name);
            else
                return string.Format("Olá{0}! Bem-vindo(a) à PT Readify. Como lhe posso ser útil?", name);
        }

        
        private string GetHelpText()
        {
            return "Podemos conversar naturalmente. Exemplos do que me pode perguntar:\n" +
                   "• \"Como posso efetuar uma compra?\"\n" +
                   "• \"De quanto tempo são os empréstimos?\"\n" +
                   "• \"Como funcionam os empréstimos?\"\n" +
                   "• \"Como devolvo um livro?\"\n" +
                   "• \"Quero o livro [Nome do Livro]\"\n\nEstou pronto, pergunte o que quiser!";
        }

       
        private string GetHistoricoHelp()
        {
            if (globais.id_utilizador <= 0)
                return "Para ver o seu histórico precisa de iniciar sessão primeiro. Depois use as opções de histórico no menu.";
            return "No menu principal tem acesso direto ao **\"Histórico de Compras\"** e **\"Histórico de Empréstimos\"**.";
        }

        
        private string GetGenerosResponse()
        {
            try
            {
                var generos = BLL.Livros.ObterGeneros();
                if (generos == null || generos.Count == 0)
                    return "Ainda não há géneros registados.";
                var lista = string.Join("\n• ", generos.Take(12));
                if (generos.Count > 12)
                    return string.Format("Géneros:\n• {0}\n... e mais {1}.", lista, generos.Count - 12);
                else
                    return string.Format("Géneros:\n• {0}", lista);
            }
            catch (Exception ex)
            {
                return string.Format("Não consegui listar géneros. ({0})", ex.Message);
            }
        }

        // ==========================================
        // MÉTODOS AUXILIARES DE VALIDAÇÃO
        // ==========================================

        
        private static bool Match(string text, params string[] terms)
        {
            if (string.IsNullOrEmpty(text)) return false;
            return terms.Any(term => text.Contains(term));
        }

        private static bool WantsBookSearch(string s) =>
            Match(s, "quero um livro", "quero livro", "procurar livro", "pesquisar livro", "recomenda");

        private static bool HasSearchableContent(string input)
        {
            var cleaned = CleanCommandWords(input);
            return !string.IsNullOrWhiteSpace(cleaned) && cleaned.Length >= 2;
        }

        private static bool IsQuestion(string s) =>
            Match(s, "como", "quem", "onde", "quando", "porquê", "porque", "qual", "quais");

        private static bool IsAffirmative(string s) =>
            Match(s, "sim", "claro", "por favor", "ok", "okay", "pode ser", "isso", "exato", "yes");

        private static bool IsNegative(string s)
        {
            if (s == "não" || s == "nao")
                return true;
            return Match(s, "agora não", "agora nao", "deixa estar");
        }

        private static bool IsThanks(string s) =>
            Match(s, "obrigado", "obrigada", "brigado", "thanks", "agradecido");

        private static bool IsGoodbye(string s) =>
            Match(s, "adeus", "tchau", "xau", "até logo", "ate logo", "até breve", "ate breve", "bye");

       
        private bool CheckLastBotMessage(string[] keywords)
        {
            if (_history.Count < 2) return false;
            var lastBot = _history.Last(h => h.Role == "bot").Text ?? "";
            return keywords.Any(k => lastBot.Contains(k));
        }

        
        private static string CleanCommandWords(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            string[] stopWords = { "pesquisar", "procurar", "livro", "livros", "autor", "título", "titulo", "quero", "preciso", "encontra" };
            var words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var filteredWords = words.Where(w => !stopWords.Contains(w.ToLowerInvariant().Trim('?', '!', '.', ',')));

            string result = string.Join(" ", filteredWords).Trim(' ', '?', '!', '.', ',', ':', ';', '"', '\'');
            return string.IsNullOrWhiteSpace(result) ? input : result;
        }

       
        private static string Pick(params string[] options) =>
            options[_random.Next(options.Length)];

       
        private static string ResolveUserName()
        {
            if (globais.id_utilizador <= 0) return null;
            try
            {
                var dt = BLL.utilizador.LoadById(globais.id_utilizador);
                if (dt != null && dt.Rows.Count > 0)
                    return dt.Rows[0]["Nome"]?.ToString()?.Split(' ').FirstOrDefault();
            }
            catch { }
            return null;
        }
    }

   
}