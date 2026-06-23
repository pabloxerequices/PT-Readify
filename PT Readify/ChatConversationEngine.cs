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
            return $"Olá{name}! Sou o assistente da PT Readify. Podemos conversar à vontade — pergunte sobre livros, empréstimos, compras, devoluções, ou diga o que precisa.\n\nComo posso ajudar hoje?";
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

            // 1. Despedidas e Encerramento
            if (IsGoodbye(s) || ContainsAny(s, "terminar conversa", "fechar chat", "sair do chat"))
            {
                _state = ChatState.Idle;
                ShouldEndConversation = true;
                return Pick(
                    "Até breve! Foi um prazer conversar consigo. Até à próxima!",
                    "Adeus! Volte quando quiser — estarei por aqui.",
                    "Até logo! Boa leitura.");
            }

            // 2. Agradecimentos
            if (IsThanks(s))
                return Pick(
                    "De nada! Quer continuar a conversar ou precisa de mais alguma coisa?",
                    "Por nada! Diga-me se precisar de mais informação.");

            // 3. Gestão de Estados Existentes (Máquina de Estados)
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
                if (ContainsAny(s, "primeiro", "1", "um")) return DescribeBookAtIndex(0);
                if (ContainsAny(s, "segundo", "2", "dois")) return DescribeBookAtIndex(1);
                if (ContainsAny(s, "terceiro", "3", "três", "tres")) return DescribeBookAtIndex(2);
                if (ContainsAny(s, "outro", "mais", "nova pesquisa"))
                {
                    _state = ChatState.WaitingBookQuery;
                    return "Claro! Diga-me o título ou autor do livro que procura.";
                }
            }

            // Contexto com base no histórico
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

            // 4. Pequenas Conversas / Chit-Chat
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

            // ==========================================
            // NOVAS REGRAS INTELIGENTES (A TUAS PERGUNTAS)
            // ==========================================

            // Pergunta: Como devolvo um livro?
            if (ContainsAny(s, "devolver", "devolvo", "devolução", "devolucao", "entregar livro"))
            {
                _lastTopic = "devolucao";
                return GetDevolucaoHelp();
            }

            // Pergunta: Quanto tempo/prazo são os empréstimos?
            if (ContainsAny(s, "quanto tempo", "de quanto tempo", "prazo", "dias posso ficar", "duracao", "duração"))
            {
                return "O prazo padrão para qualquer empréstimo na PT Readify é de **15 dias**. " +
                       "Pode solicitar uma renovação antes do prazo terminar, desde que o livro não esteja reservado por outro leitor.";
            }

            // Pergunta: Como funcionam os empréstimos? (Geral)
            if (ContainsAny(s, "como funcionam os emprestimos", "como funciona o emprestimo", "como funcionam os emppstimos", "regras do emprestimo"))
            {
                _lastTopic = "emprestimo";
                return "Os empréstimos funcionam assim:\n" +
                       "1. Cada leitor pode requisitar livros que estejam disponíveis.\n" +
                       "2. O prazo de entrega é de **15 dias**.\n" +
                       "3. Pode levantar o livro fisicamente ou consultar o estado no seu menu de perfil.\n" +
                       "4. Se atrasar a entrega, a sua conta poderá ficar suspensa temporariamente para novas requisições.\n\n" +
                       "Quer que eu explique como requisitar passo a passo?";
            }

            // Pergunta: Como posso efetuar uma compra?
            if (ContainsAny(s, "como posso efetuar uma compra", "como compro", "como fazer compra", "efetuar compra", "passos para comprar"))
            {
                _lastTopic = "compras";
                return "Para efetuar uma compra na PT Readify, siga estes passos simples:\n" +
                       "1. Vá ao menu **\"Livros\"** no ecrã principal.\n" +
                       "2. Clique no livro que deseja e selecione **\"Adicionar ao Carrinho\"**.\n" +
                       "3. Aceda ao seu **Carrinho de Compras**, valide os itens e clique em **\"Concluir Compra\"**.\n" +
                       "4. Escolha o método de pagamento e confirme.\n\n" +
                       "Ficou com alguma dúvida sobre o processo?";
            }

            // Suporte aos termos antigos de Empréstimo/Compra
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

            // 5. Pesquisa de Livros por Contexto Natural
            if (WantsBookSearch(s) || (ContainsAny(s, "livro", "livros", "autor", "título", "titulo", "pesquisar", "procurar") && HasSearchableContent(input)))
            {
                if (HasSearchableContent(input))
                    return HandleBookQuery(input);
                _state = ChatState.WaitingBookQuery;
                return "Com prazer! Diga-me o título ou autor do livro.";
            }

            // Se o utilizador apenas digitou algo solto, tenta adivinhar se é um livro
            if (!IsAffirmative(s) && _state == ChatState.Idle && input.Length >= 2 &&
                !ContainsAny(s, "como", "quem", "onde", "quando", "porquê", "porque"))
            {
                var guess = TrySearchAsBook(input);
                if (guess != null)
                    return guess;
            }

            return GetContextualFallback();
        }

        // ==========================================
        // MÉTODOS AUXILIARES E RESPOSTAS FORMATADAS
        // ==========================================

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
                    if (cleaned.Length >= 2) titulo = cleaned;
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
            if (input.Split(' ').Length > 6) return null;
            try
            {
                var results = BLL.Livros.pesquisarLivro(input, null, null, null);
                if (results == null || results.Rows.Count == 0) return null;

                _lastSearchResults = results;
                _state = ChatState.AfterBookResults;

                if (results.Rows.Count == 1)
                    return $"Acho que se refere a \"{results.Rows[0]["Titulo"]}\" de {results.Rows[0]["Autor"]}. Quer saber mais detalhes?";

                return $"Encontrei {results.Rows.Count} livros relacionados com \"{input}\". Quer que mostre a lista completa?";
            }
            catch { return null; }
        }

        private string GetDevolucaoHelp()
        {
            return "Para devolver um livro emprestado:\n" +
                   "1. Dirija-se ao menu **\"Histórico de Empréstimos\"** ou **\"Perfil\"**.\n" +
                   "2. Identifique o livro que está atualmente consigo.\n" +
                   "3. Se a entrega for feita via balcão físico, o funcionário fará a leitura do código do livro e dará a baixa no sistema.\n" +
                   "4. Certifique-se de que o estado do livro passa para 'Devolvido' na sua aplicação.";
        }

        private string GetContextualFallback()
        {
            if (_lastTopic == "emprestimo") return "Ainda sobre empréstimos: lembre-se que o prazo máximo é de 15 dias. Quer saber como pedir um?";
            if (_lastTopic == "compras") return "Deseja que o guie até ao ecrã de 'Livros' para adicionar um produto ao carrinho?";
            if (_lastTopic == "devolucao") return "Ficou claro como efetuar a devolução? Lembre-se de não ultrapassar os 15 dias regulamentares.";

            return Pick(
                "Interessante. Pode explicar um pouco mais? Ou diga \"Ajuda\".",
                "Não percebi bem. Quer falar de livros, empréstimos, devoluções ou compras?",
                "Experimente perguntar: \"Como funcionam os empréstimos?\" ou \"Como posso comprar?\"");
        }

        private string GetGreeting()
        {
            var name = string.IsNullOrEmpty(_userName) ? "" : $", {_userName}";
            if (globais.id_utilizador > 0)
                return $"Olá{name}! Sobre o que quer conversar hoje — livros, prazos de empréstimos, devoluções ou compras?";
            return $"Olá{name}! Bem-vindo(a) à PT Readify. Como lhe posso ser útil?";
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

        private string GetEmprestimoHelp()
        {
            return "Para requisitar um livro:\n" +
                   "1. Vá ao Menu \"Requisições/Empréstimos\"\n" +
                   "2. Escolha o livro que pretende (Prazo: 15 dias)\n" +
                   "3. Confirme a requisição.";
        }

        private string GetComprasHelp()
        {
            return "Para comprar:\n" +
                   "1. Menu \"Livros\" → escolha o livro\n" +
                   "2. Adicione ao carrinho\n" +
                   "3. Conclua no carrinho de compras.";
        }

        private string GetHistoricoHelp()
        {
            if (globais.id_utilizador <= 0)
                return "Para ver o seu histórico precisa de iniciar sessão primeiro. Depois use as opções de histórico no menu.";
            return "No menu principal tem acesso direto ao \"Histórico de Compras\" e \"Histórico de Empréstimos\".";
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