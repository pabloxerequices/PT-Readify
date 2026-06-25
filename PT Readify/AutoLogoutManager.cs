using System;
using System.Windows.Forms;

namespace PT_Readify
{
    public sealed class AutoLogoutManager : IMessageFilter, IDisposable
    {
        private static AutoLogoutManager _instance;
        private readonly Timer _checkTimer;
        private DateTime _lastActivity = DateTime.Now;
        private Form _hostForm;
        private bool _loggingOut;

        public static readonly int[] AllowedMinutes = { 0, 1, 5, 10, 15 };

        private AutoLogoutManager()
        {
            _checkTimer = new Timer { Interval = 15000 };
            _checkTimer.Tick += OnCheckIdle;
        }

        public static void Attach(Form host)
        {
            if (host == null) return;

            if (_instance == null)
            {
                _instance = new AutoLogoutManager();
                Application.AddMessageFilter(_instance);
            }

            _instance._hostForm = host;
            AutoLogoutManager.ResetActivity();
            _instance._checkTimer.Start();
        }

        public static void Detach()
        {
            if (_instance == null) return;
            _instance._checkTimer.Stop();
            _instance._hostForm = null;
        }

        public static void ResetActivity()
        {
            if (_instance != null)
                _instance._lastActivity = DateTime.Now;
        }

        public static int NormalizeMinutes(int minutes)
        {
            if (minutes <= 0) return 0;
            foreach (var option in AllowedMinutes)
            {
                if (minutes <= option) return option;
            }
            return 15;
        }

        public bool PreFilterMessage(ref Message m)
        {
            const int WM_MOUSEMOVE = 0x0200;
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_RBUTTONDOWN = 0x0204;
            const int WM_MBUTTONDOWN = 0x0207;
            const int WM_KEYDOWN = 0x0100;
            const int WM_SYSKEYDOWN = 0x0104;

            switch (m.Msg)
            {
                case WM_MOUSEMOVE:
                case WM_LBUTTONDOWN:
                case WM_RBUTTONDOWN:
                case WM_MBUTTONDOWN:
                case WM_KEYDOWN:
                case WM_SYSKEYDOWN:
                    _lastActivity = DateTime.Now;
                    break;
            }

            return false;
        }

        private void OnCheckIdle(object sender, EventArgs e)
        {
            if (_loggingOut || _hostForm == null || _hostForm.IsDisposed)
                return;

            if (globais.id_utilizador <= 0)
                return;

            var cfg = ConfigManager.Current;
            if (cfg == null || cfg.AutoLogoutMinutes <= 0)
                return;

            var idleMinutes = (DateTime.Now - _lastActivity).TotalMinutes;
            if (idleMinutes < cfg.AutoLogoutMinutes)
                return;

            _loggingOut = true;
            try
            {
                var cfgCopy = ConfigManager.Current;
                MessageBox.Show(
                    LanguageHelper.T("AutoLogoutMessage", cfgCopy),
                    LanguageHelper.T("AutoLogoutTitle", cfgCopy),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                globais.id_utilizador = 0;
                globais.iisAdmin = false;

                var login = new Form1();
                login.Show();
                _hostForm.Close();
            }
            finally
            {
                _loggingOut = false;
                ResetActivity();
            }
        }

        public void Dispose()
        {
            _checkTimer?.Stop();
            _checkTimer?.Dispose();
            if (_instance == this)
            {
                Application.RemoveMessageFilter(this);
                _instance = null;
            }
        }
    }
}
