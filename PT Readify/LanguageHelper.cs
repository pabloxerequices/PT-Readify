using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PT_Readify
{
    public static class LanguageHelper
    {
        private static readonly Dictionary<string, string> Pt = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "SettingsTitle", "Configurações" },
            { "Theme", "Tema:" },
            { "ThemeLight", "Claro" },
            { "ThemeDark", "Escuro" },
            { "Fullscreen", "Modo de leitura fullscreen:" },
            { "Font", "Fonte:" },
            { "FontSize", "Tamanho (pt):" },
            { "AutoLogout", "Temporizador de desconexão:" },
            { "Language", "Linguagem:" },
            { "Save", "Salvar" },
            { "Cancel", "Cancelar" },
            { "Reset", "Restaurar Padrões" },
            { "RestoreLanguage", "Voltar à linguagem original" },
            { "Never", "Nunca" },
            { "Saved", "Configurações guardadas." },
            { "Success", "Sucesso" },
            { "ResetConfirm", "Restaurar padrões irá repor todas as preferências. Continuar?" },
            { "Confirm", "Confirmar" },
            { "NoOriginalLanguage", "Linguagem original não definida." },
            { "Info", "Informação" },
            { "LanguageRestored", "Linguagem restaurada para o original." },
            { "Logout", "Logout" },
            { "Books", "Livros" },
            { "Profile", "Perfil" },
            { "Help", "Ajuda" },
            { "Loans", "Requisições/Empréstimos" },
            { "PurchaseHistory", "Histórico de Compras" },
            { "LoanHistory", "Histórico de Empréstimos" },
            { "Reservations", "Reservas" },
            { "Assistant", "Assistente" },
            { "AutoLogoutTitle", "Sessão expirada" },
            { "AutoLogoutMessage", "A sua sessão foi terminada por inatividade." },
            { "Close", "Fechar" },
            { "BooksInCart", "Livros ({0} no carrinho)" },
            { "Email:", "Email:" },
            { "Password:", "Palavra-passe:" },
            { "Login", "Login" },
            { "Welcome", "Bem-vindo" },
            { "Enter", "Entrar" },
            { "Register", "Registre-se" },
            { "ForgotPassword", "Esqueceu a password?" },
            { "NoAccount", "Ainda não tem uma conta?" }
        };

        private static readonly Dictionary<string, string> En = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "SettingsTitle", "Settings" },
            { "Theme", "Theme:" },
            { "ThemeLight", "Light" },
            { "ThemeDark", "Dark" },
            { "Fullscreen", "Fullscreen reading mode:" },
            { "Font", "Font:" },
            { "FontSize", "Size (pt):" },
            { "AutoLogout", "Disconnect timer:" },
            { "Language", "Language:" },
            { "Save", "Save" },
            { "Cancel", "Cancel" },
            { "Reset", "Restore Defaults" },
            { "RestoreLanguage", "Return to original language" },
            { "Never", "Never" },
            { "Saved", "Settings saved." },
            { "Success", "Success" },
            { "ResetConfirm", "Restore defaults will reset all preferences. Continue?" },
            { "Confirm", "Confirm" },
            { "NoOriginalLanguage", "Original language is not defined." },
            { "Info", "Information" },
            { "LanguageRestored", "Language restored to original." },
            { "Logout", "Logout" },
            { "Books", "Books" },
            { "Profile", "Profile" },
            { "Help", "Help" },
            { "Loans", "Requests/Loans" },
            { "PurchaseHistory", "Purchase History" },
            { "LoanHistory", "Loan History" },
            { "Reservations", "Reservations" },
            { "Assistant", "Assistant" },
            { "AutoLogoutTitle", "Session expired" },
            { "AutoLogoutMessage", "Your session was ended due to inactivity." },
            { "Close", "Close" },
            { "BooksInCart", "Books ({0} in cart)" },
            { "Email:", "Email:" },
            { "Password:", "Password:" },
            { "Login", "Login" },
            { "Welcome", "Welcome" },
            { "Enter", "Enter" },
            { "Register", "Register" },
            { "ForgotPassword", "Forgot password?" },
            { "NoAccount", "Don't have an account?" }
        };

        public static bool IsEnglish(Config cfg)
        {
            return cfg != null && cfg.Language != null
                && cfg.Language.Equals("en", StringComparison.OrdinalIgnoreCase);
        }

        public static string T(string key, Config cfg = null)
        {
            cfg = cfg ?? ConfigManager.Current;
            var dict = IsEnglish(cfg) ? En : Pt;
            return dict.TryGetValue(key, out var value) ? value : key;
        }

        public static string LogoutOptionLabel(int minutes, Config cfg = null)
        {
            if (minutes <= 0) return T("Never", cfg);
            return $"{minutes} min";
        }
    }
}
