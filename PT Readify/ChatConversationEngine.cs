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

            if (ShouldSearchBooks(s, input))
            {
                if (HasSearchableContent(input))
                    return HandleBookQuery(input);

                _state = ChatState.WaitingBookQuery;
                return "Com prazer! Diga-me o título ou autor do livro.";
            }

            if (IsGreeting(s))
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
                _lastSearchResults = null;
                return "Está bem, cancelei a operação. Tem mais alguma dúvida em que possa ajudar?";
            }

            var basicAnswer = TryAnswerBasicQuestion(s);
            if (basicAnswer != null)
            {
                _state = ChatState.Idle;
                return basicAnswer;
            }

            var appAnswer = TryAnswerApplicationQuestion(s);
            if (appAnswer != null)
            {
                _state = ChatState.Idle;
                return appAnswer;
            }

            // ==========================================
            // 2. MÁQUINA DE ESTADOS
            // ==========================================

            if (_state == ChatState.WaitingBookQuery)
            {
                if (WantsBookSearch(s))
                    return "Estou à espera do nome do livro! Diga-me o título ou o autor.";
                if (!LooksLikeBookTitleInput(s, input))
                {
                    _state = ChatState.Idle;
                    _lastSearchResults = null;
                }
                else
                {
                    return HandleBookQuery(input);
                }
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

            if (IsAboutLoans(s))
            {
                _lastTopic = "emprestimo";
                return IsLoanRulesQuestion(s) ? GetEmprestimoRules() : GetTopicHelp("emprestimo") + "\n\nQuer que eu explique algum passo com mais detalhe?";
            }

            if (IsAboutSales(s))
            {
                _lastTopic = "compras";
                return IsSalesRulesQuestion(s) ? GetComprasRules() : GetTopicHelp("compras") + "\n\nTem alguma dúvida sobre o processo de compra?";
            }

            if (Match(s, "perfil", "conta", "password", "palavra-passe", "senha"))
                return GetPerfilHelp();

            if (Match(s, "logout", "sair", "terminar sessão", "terminar sessao"))
                return "Use o botão \"Logout\" no menu para terminar sessão.";

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
                   "Tem mais alguma dúvida sobre empréstimos?";
        }

       
        private string GetComprasRules()
        {
            return "As vendas (compras) na PT Readify funcionam assim:\n" +
                   "1. Vá ao menu **\"Livros\"** no ecrã principal.\n" +
                   "2. Clique no livro que deseja e selecione **\"Adicionar ao Carrinho\"**.\n" +
                   "3. Aceda ao seu **Carrinho de Compras**, valide os itens e clique em **\"Concluir Compra\"**.\n" +
                   "4. Escolha o método de pagamento e confirme.\n" +
                   "5. Depois da compra, pode consultar o histórico no menu principal.\n\n" +
                   "Tem mais alguma dúvida sobre vendas ou compras?";
        }

        private string HandleBookNotFound(string searchTerm, string originalInput)
        {
            _state = ChatState.Idle;
            _lastSearchResults = null;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return "Não percebi o nome do livro. Cancelei a pesquisa.\n\n" +
                       GetBookSearchHelpText(null) +
                       "\n\nTem mais alguma dúvida em que possa ajudar?";
            }

            var sb = new StringBuilder();
            sb.AppendFormat("Não encontrei nenhum livro relacionado com \"{0}\".", searchTerm);

            if (WasSearchTermShortened(originalInput, searchTerm))
            {
                sb.AppendFormat(
                    "\n\nDa sua mensagem usei apenas **\"{0}\"** (removi palavras como \"quero\", \"procurar\" ou \"livro\").",
                    searchTerm);
            }

            sb.Append("\n\n");
            sb.Append(GetBookSearchHelpText(searchTerm));
            sb.Append("\n\nCancelei a pesquisa. Tem mais alguma dúvida em que possa ajudar?");
            return sb.ToString();
        }

        private string GetBookSearchHelpText(string attemptedTerm)
        {
            var sb = new StringBuilder();
            sb.Append("**Como pesquisar na base de dados:**\n");
            sb.Append("• Escreva só o **título** ou o **nome do autor**\n");
            sb.Append("• Use palavras-chave curtas (não precisa da frase completa)\n");
            sb.Append("• Exemplos com livros disponíveis:");
            sb.Append(GetBookSearchExamples());

            if (!string.IsNullOrWhiteSpace(attemptedTerm))
                sb.AppendFormat("\n• Em vez de \"{0}\", tente uma palavra mais curta do título ou autor", attemptedTerm);

            return sb.ToString();
        }

        private string GetBookSearchExamples()
        {
            try
            {
                var books = BLL.Livros.pesquisarLivro(null, null, null, null);
                if (books == null || books.Rows.Count == 0)
                    return "\n  - \"1984\"\n  - \"Harry Potter\"\n  - \"Camões\"";

                var sb = new StringBuilder();
                int count = Math.Min(3, books.Rows.Count);
                for (int i = 0; i < count; i++)
                {
                    var row = books.Rows[i];
                    sb.AppendFormat("\n  - \"{0}\" ou \"{1}\"", row["Titulo"], row["Autor"]);
                }
                return sb.ToString();
            }
            catch
            {
                return "\n  - \"1984\"\n  - \"Harry Potter\"\n  - \"Camões\"";
            }
        }

        private string TryAnswerBasicQuestion(string s)
        {
            if (IsDateTimeQuestion(s))
                return GetDateTimeResponse(s);

            if (IsLoanRulesQuestion(s))
            {
                _lastTopic = "emprestimo";
                return GetEmprestimoRules();
            }

            if (IsSalesRulesQuestion(s))
            {
                _lastTopic = "compras";
                return GetComprasRules();
            }

            return null;
        }

        private string TryAnswerApplicationQuestion(string s)
        {
            if (IsMenuQuestion(s))
            {
                _lastTopic = "menu";
                return GetMenuHelp();
            }

            if (IsHistoricoQuestion(s))
            {
                _lastTopic = "historico";
                return GetHistoricoHelp(s);
            }

            if (IsReservaQuestion(s))
            {
                _lastTopic = "reservas";
                return GetReservasHelp();
            }

            if (IsSupportQuestion(s))
            {
                _lastTopic = "apoio";
                return GetApoioHelp();
            }

            if (IsNotificacaoQuestion(s))
                return GetNotificacoesHelp();

            if (IsConfiguracaoQuestion(s))
                return GetConfiguracoesHelp();

            if (IsCarrinhoQuestion(s))
            {
                _lastTopic = "compras";
                return GetCarrinhoHelp();
            }

            if (Match(s, "iniciar sessão", "iniciar sessao", "login", "registar", "criar conta", "conta nova"))
                return GetContaHelp(s);

            return null;
        }

        private string GetMenuHelp()
        {
            return "O **menu principal** fica na barra lateral esquerda. Pode usar estas opções:\n" +
                   "• **Perfil** — ver e editar os seus dados\n" +
                   "• **Livros** — pesquisar livros, comprar e adicionar ao carrinho\n" +
                   "• **Requisições/Empréstimos** — requisitar livros disponíveis\n" +
                   "• **Histórico de Compras** — consultar compras anteriores\n" +
                   "• **Histórico de Empréstimos** — consultar empréstimos e devolver livros\n" +
                   "• **Reservas** — ver livros reservados (quando estão esgotados)\n" +
                   "• **Assistente** — abrir este chat de ajuda\n" +
                   "• **Configurações** — alterar tema e tamanho de letra\n" +
                   "• **Ajuda** — abrir informação no site oficial\n" +
                   "• **Logout** — terminar sessão\n\n" +
                   "Basta clicar num botão para abrir a secção pretendida.";
        }

        private string GetHistoricoHelp(string s)
        {
            if (globais.id_utilizador <= 0)
                return "Para ver históricos precisa de **iniciar sessão** primeiro.";

            if (Match(s, "compra", "compras", "comprei", "paguei"))
            {
                return "**Histórico de Compras:**\n" +
                       "1. No menu lateral, clique em **\"Histórico de Compras\"**\n" +
                       "2. Veja a lista de livros que comprou, com datas e valores\n" +
                       "3. Serve para consultar compras anteriores e confirmar pagamentos\n\n" +
                       "Tem mais alguma dúvida sobre históricos?";
            }

            if (Match(s, "emprest", "requis", "devol", "multa"))
            {
                return "**Histórico de Empréstimos:**\n" +
                       "1. No menu lateral, clique em **\"Histórico de Empréstimos\"**\n" +
                       "2. Veja empréstimos ativos, devolvidos e eventuais multas\n" +
                       "3. Pode usar o botão **\"Devolver livro\"** para registar devoluções\n" +
                       "4. O prazo padrão de empréstimo é de **15 dias**\n\n" +
                       "Tem mais alguma dúvida sobre históricos?";
            }

            return "**Históricos na PT Readify:**\n" +
                   "• **Histórico de Compras** — registo de livros que comprou\n" +
                   "• **Histórico de Empréstimos** — registo de livros requisitados, devolvidos e multas\n\n" +
                   "Ambos estão no menu lateral esquerdo. Precisa de ter sessão iniciada.\n\n" +
                   "Quer saber mais sobre compras ou empréstimos?";
        }

        private string GetReservasHelp()
        {
            return "As **reservas** servem para livros **esgotados** (sem stock):\n" +
                   "1. Vá a **Requisições/Empréstimos** no menu\n" +
                   "2. Escolha um livro esgotado e clique em **Reservar**\n" +
                   "3. Quando o livro voltar a ter stock, recebe uma **notificação**\n" +
                   "4. Consulte as reservas ativas no menu **Reservas**\n" +
                   "5. Tem **7 dias** para levantar o livro depois de ficar disponível\n\n" +
                   "Se o livro tiver stock, use **Requisitar** em vez de Reservar.\n\n" +
                   "Tem mais alguma dúvida sobre reservas?";
        }

        private string GetApoioHelp()
        {
            return "Para **apoio ao cliente**, pode:\n" +
                   "• Continuar a conversar comigo neste **Assistente** para dúvidas rápidas\n" +
                   "• Clicar em **Ajuda** no menu principal (abre o site oficial)\n" +
                   "• Visitar o site: **https://siteptreadify.vercel.app/**\n\n" +
                   "No site encontra informação sobre a PT Readify e formas de contacto da equipa.\n\n" +
                   "Em que mais posso ajudar?";
        }

        private string GetNotificacoesHelp()
        {
            return "A aplicação envia **notificações** quando, por exemplo:\n" +
                   "• Um livro que reservou fica novamente disponível\n\n" +
                   "Ao iniciar sessão, pode ser avisado se tiver notificações novas.\n" +
                   "Consulte-as quando a app lhe mostrar o aviso no ecrã.\n\n" +
                   "Tem mais alguma dúvida?";
        }

        private string GetConfiguracoesHelp()
        {
            return "Nas **Configurações** (menu lateral) pode personalizar a aplicação:\n" +
                   "• Alterar o **tema** (claro ou escuro)\n" +
                   "• Ajustar o **tamanho da letra**\n\n" +
                   "As alterações aplicam-se ao menu e às secções principais.\n\n" +
                   "Precisa de ajuda com mais alguma opção do menu?";
        }

        private string GetCarrinhoHelp()
        {
            return "O **carrinho** é usado apenas para **compras**:\n" +
                   "1. Vá ao menu **Livros** e escolha os livros que quer comprar\n" +
                   "2. Adicione-os ao carrinho (o botão **Livros** mostra quantos itens tem)\n" +
                   "3. Abra o carrinho, confirme os itens e conclua a compra\n\n" +
                   "Para **emprestar** ou **reservar**, use **Requisições/Empréstimos** — o carrinho não serve para isso.\n\n" +
                   "Tem mais alguma dúvida?";
        }

        private string GetPerfilHelp()
        {
            return "**Perfil** (canto superior esquerdo do menu):\n" +
                   "• Ver e editar **nome**, **email**, **telefone** e **palavra-passe**\n" +
                   "• Gerir os seus dados de conta\n\n" +
                   "Precisa de ter sessão iniciada para aceder ao perfil.\n\n" +
                   "Tem mais alguma dúvida sobre a conta?";
        }

        private string GetContaHelp(string s)
        {
            if (Match(s, "registar", "criar conta", "conta nova"))
            {
                return "Para **criar conta**:\n" +
                       "1. No ecrã de login, escolha a opção de registo\n" +
                       "2. Preencha nome, email e palavra-passe\n" +
                       "3. Depois pode iniciar sessão e usar livros, empréstimos e compras\n\n" +
                       "Tem mais alguma dúvida?";
            }

            return "Para **iniciar sessão**:\n" +
                   "1. Introduza o seu **email** e **palavra-passe** no ecrã inicial\n" +
                   "2. Depois de entrar, o menu principal fica disponível\n" +
                   "3. Sem sessão, não consegue ver históricos, reservas nem efetuar compras\n\n" +
                   "Tem mais alguma dúvida?";
        }

        private static string GetDateTimeResponse(string s)
        {
            var now = DateTime.Now;
            var wantsDate = Match(s, "data", "dia", "hoje");
            var wantsTime = Match(s, "hora", "horas");

            if (wantsDate && wantsTime)
                return string.Format("Hoje é {0:dddd, dd 'de' MMMM 'de' yyyy} e são {1:HH:mm}. Tem mais alguma dúvida?", now, now);

            if (wantsDate)
                return string.Format("Hoje é {0:dddd, dd 'de' MMMM 'de' yyyy}. Tem mais alguma dúvida?", now);

            return string.Format("Agora são {0:HH:mm}. Tem mais alguma dúvida?", now);
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
                string searchTerm = ExtractBookSearchTerm(input);

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return HandleBookNotFound(null, input);
                }

                var results = SearchBooks(searchTerm);
                _lastSearchResults = results;

                if (results == null || results.Rows.Count == 0)
                {
                    return HandleBookNotFound(searchTerm, input);
                }

                var sb = new StringBuilder(string.Format("Encontrei {0} livro(s) para \"{1}\":\n", results.Rows.Count, searchTerm));
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
            if (input.Split(' ').Length > 12) return null;
            try
            {
                var searchTerm = ExtractBookSearchTerm(input);
                if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2) return null;

                var results = SearchBooks(searchTerm);
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
                    return string.Format("Encontrei {0} livros relacionados com \"{1}\". Quer que mostre a lista completa?", results.Rows.Count, searchTerm);
            }
            catch
            {
                _state = ChatState.Idle;
                return null;
            }
        }

        private static DataTable SearchBooks(string searchTerm)
        {
            var byTitle = BLL.Livros.pesquisarLivro(searchTerm, null, null, null);
            var byAuthor = BLL.Livros.pesquisarLivro(null, searchTerm, null, null);
            return MergeBookResults(byTitle, byAuthor);
        }

        private static DataTable MergeBookResults(DataTable byTitle, DataTable byAuthor)
        {
            if (byTitle == null || byTitle.Rows.Count == 0)
                return byAuthor;
            if (byAuthor == null || byAuthor.Rows.Count == 0)
                return byTitle;

            var merged = byTitle.Clone();
            var seen = new HashSet<int>();
            foreach (DataRow row in byTitle.Rows)
            {
                int id = Convert.ToInt32(row["Id_Livro"]);
                if (seen.Add(id))
                    merged.ImportRow(row);
            }
            foreach (DataRow row in byAuthor.Rows)
            {
                int id = Convert.ToInt32(row["Id_Livro"]);
                if (seen.Add(id))
                    merged.ImportRow(row);
            }
            return merged;
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
            if (_lastTopic == "menu")
                return "Precisa de ajuda com alguma opção específica do menu?";
            if (_lastTopic == "historico")
                return "Quer saber mais sobre o histórico de compras ou de empréstimos?";
            if (_lastTopic == "reservas")
                return "Ficou claro como funcionam as reservas? Tem mais alguma dúvida?";
            if (_lastTopic == "apoio")
                return "Posso ajudar com mais alguma coisa ou prefere consultar o site oficial?";

            return Pick(
                "Não percebi bem o que quis dizer. Tem mais alguma dúvida sobre livros, empréstimos, reservas ou o menu?",
                "Não consegui identificar o que precisa. Experimente perguntar: \"Como uso o menu?\" ou \"Como funcionam as reservas?\"",
                "Ficou com alguma dúvida? Pergunte sobre o menu, históricos, reservas ou apoio ao cliente.");
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
                   "• \"Como uso o menu?\"\n" +
                   "• \"Como funcionam os empréstimos?\" ou \"Como funcionam as vendas?\"\n" +
                   "• \"Como funcionam os históricos?\" ou \"Como funcionam as reservas?\"\n" +
                   "• \"Como falo com o apoio ao cliente?\"\n" +
                   "• \"Que horas são?\" ou \"Que dia é hoje?\"\n" +
                   "• \"Quero o livro [Nome do Livro]\"\n\nEstou pronto, pergunte o que quiser!";
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

        private static bool MatchWholeWord(string text, params string[] terms)
        {
            if (string.IsNullOrEmpty(text)) return false;

            foreach (var term in terms)
            {
                if (string.IsNullOrEmpty(term))
                    continue;

                if (term.Contains(" "))
                {
                    if (text.Contains(term))
                        return true;
                    continue;
                }

                int index = 0;
                while ((index = text.IndexOf(term, index, StringComparison.Ordinal)) >= 0)
                {
                    bool startsAtWord = index == 0 || !char.IsLetterOrDigit(text[index - 1]);
                    int end = index + term.Length;
                    bool endsAtWord = end >= text.Length || !char.IsLetterOrDigit(text[end]);
                    if (startsAtWord && endsAtWord)
                        return true;
                    index++;
                }
            }

            return false;
        }

        private static bool IsGreeting(string s)
        {
            if (Match(s, "bom dia", "boa tarde", "boa noite"))
                return true;

            return MatchWholeWord(s, "olá", "ola", "oi", "hey");
        }

        private static bool ShouldSearchBooks(string s, string input)
        {
            if (WantsBookSearch(s))
                return true;

            return Match(s, "livro", "livros", "autor", "título", "titulo", "pesquisar", "procurar")
                && HasSearchableContent(input);
        }

        private static bool WantsBookSearch(string s) =>
            Match(s, "quero um livro", "quero o livro", "quero livro", "procurar livro", "pesquisar livro", "recomenda");

        private static bool HasSearchableContent(string input)
        {
            var searchTerm = ExtractBookSearchTerm(input);
            return !string.IsNullOrWhiteSpace(searchTerm) && searchTerm.Length >= 2;
        }

        private static bool IsQuestion(string s) =>
            Match(s, "como", "quem", "onde", "quando", "porquê", "porque", "qual", "quais");

        private static bool LooksLikeGeneralQuestion(string s) =>
            IsQuestion(s) || IsDateTimeQuestion(s) || IsLoanRulesQuestion(s) || IsSalesRulesQuestion(s);

        private static bool LooksLikeBookTitleInput(string s, string rawInput)
        {
            if (LooksLikeGeneralQuestion(s))
                return false;
            if (IsAboutLoans(s) || IsAboutSales(s))
                return false;
            if (Match(s, "devolver", "devolvo", "devolução", "devolucao", "histórico", "historico", "perfil", "site", "género", "genero", "menu", "reserva", "reservas", "apoio", "suporte", "contacto", "notifica", "configura", "carrinho"))
                return false;
            return HasSearchableContent(rawInput);
        }

        private static bool IsMenuQuestion(string s) =>
            Match(s, "como usar o menu", "como usar a aplicação", "como usar a aplicacao", "opções do menu", "opcoes do menu", "barra lateral") ||
            (Match(s, "menu") && Match(s, "como", "onde", "usar", "funcion", "opções", "opcoes", "navegar"));

        private static bool IsHistoricoQuestion(string s) =>
            Match(s, "históric", "histor", "compras anteriores", "emprestimos anteriores");

        private static bool IsReservaQuestion(string s) =>
            Match(s, "reserva", "reservas") ||
            (Match(s, "reservar") && !Match(s, "emprest", "requisitar", "requis"));

        private static bool IsSupportQuestion(string s) =>
            Match(s, "apoio ao cliente", "suporte ao cliente", "apoio", "suporte", "contacto", "contactar", "atendimento", "falar com");

        private static bool IsNotificacaoQuestion(string s) =>
            Match(s, "notifica", "notificação", "notificacao", "alerta", "avisos");

        private static bool IsConfiguracaoQuestion(string s) =>
            Match(s, "configura", "definições", "definicoes", "tema escuro", "tamanho da letra", "tamanho de letra");

        private static bool IsCarrinhoQuestion(string s) =>
            Match(s, "carrinho") && Match(s, "como", "onde", "funcion", "usar", "adicionar", "carrinho");

        private static bool IsDateTimeQuestion(string s) =>
            Match(s, "que horas", "horas são", "hora são", "que dia", "que data", "data de hoje", "data hoje", "dia é hoje", "dia de hoje", "data e hora", "dia e hora") ||
            (Match(s, "hora", "horas", "data", "hoje") && Match(s, "que", "qual", "hoje", "agora", "são", "e"));

        private static bool IsAboutLoans(string s) =>
            Match(s, "emprest", "requis", "levantar livro");

        private static bool IsAboutSales(string s) =>
            Match(s, "compra", "comprar", "compro", "venda", "vendas", "carrinho", "pagar");

        private static bool IsLoanRulesQuestion(string s) =>
            IsAboutLoans(s) && Match(s, "como", "funcion", "regras", "passos", "explic", "funciona");

        private static bool IsSalesRulesQuestion(string s) =>
            IsAboutSales(s) && Match(s, "como", "funcion", "regras", "passos", "explic", "funciona", "efetuar", "fazer");

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

        
        private static string ExtractBookSearchTerm(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            var result = input.Trim();

            string[] prefixes =
            {
                "quero um livro ",
                "quero o livro ",
                "quero livro ",
                "preciso do livro ",
                "preciso de um livro ",
                "procurar livro ",
                "pesquisar livro ",
                "procurar o livro ",
                "pesquisar o livro ",
                "encontrar livro ",
                "encontra livro ",
                "autor ",
                "título ",
                "titulo ",
                "quero ",
                "preciso ",
                "procurar ",
                "pesquisar ",
                "encontrar ",
                "encontra "
            };

            var lower = result.ToLowerInvariant();
            bool changed;
            do
            {
                changed = false;
                foreach (var prefix in prefixes)
                {
                    if (lower.StartsWith(prefix))
                    {
                        result = result.Substring(prefix.Length).Trim();
                        lower = result.ToLowerInvariant();
                        changed = true;
                        break;
                    }
                }
            } while (changed);

            if (lower.EndsWith(" livro"))
                result = result.Substring(0, result.Length - 6).Trim();
            else if (lower.EndsWith(" livros"))
                result = result.Substring(0, result.Length - 7).Trim();

            result = result.Trim('?', '!', '.', ',', ':', ';', '"', '\'', '[', ']', '(', ')');
            return string.IsNullOrWhiteSpace(result) ? null : result;
        }

        private static bool WasSearchTermShortened(string originalInput, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(originalInput) || string.IsNullOrWhiteSpace(searchTerm))
                return false;

            return NormalizeSearchText(originalInput) != NormalizeSearchText(searchTerm);
        }

        private static string NormalizeSearchText(string text)
        {
            return string.Join(" ",
                text.ToLowerInvariant()
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
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