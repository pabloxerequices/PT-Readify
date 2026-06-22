using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace PT_Readify
{
    public partial class Configuracoes : Form
    {
        private Config _config;

        public Configuracoes()
        {
            InitializeComponent();
            ApplyStyleToControls();
            LoadSettingsToControls();
        }

        private void ApplyStyleToControls()
        {
            // Fonte padrão consistente
            var f = new Font("Segoe UI", 10F);
            this.Font = f;
            foreach (Control c in this.pnlContent.Controls.OfType<Control>())
            {
                c.Font = f;
            }
        }

        private void LoadSettingsToControls()
        {
            _config = ConfigManager.Current ?? Config.Default();

            cfgComboTheme.Items.Clear();
            cfgComboTheme.Items.Add("Claro");
            cfgComboTheme.Items.Add("Escuro");
            cfgComboTheme.SelectedItem = _config.Theme ?? "Claro";

            cfgToggleFullscreen.Checked = _config.FullscreenReading;

            cfgComboFont.Items.Clear();
            cfgComboFont.Items.AddRange(new object[] { "Segoe UI", "Arial", "Times New Roman", "Calibri" });
            cfgComboFont.SelectedItem = string.IsNullOrWhiteSpace(_config.FontName) ? "Segoe UI" : _config.FontName;

            cfgNumFontSize.Value = Math.Max(8, Math.Min(72, _config.FontSize));
            cfgNumAutoLogout.Value = Math.Max(0, _config.AutoLogoutMinutes);

            cfgComboLanguage.Items.Clear();
            cfgComboLanguage.Items.Add("Português");
            cfgComboLanguage.Items.Add("English");
            cfgComboLanguage.SelectedItem = _config.Language == "en" ? "English" : "Português";
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            if (_config == null) _config = Config.Default();

            _config.Theme = cfgComboTheme.SelectedItem?.ToString() ?? "Claro";
            _config.FullscreenReading = cfgToggleFullscreen.Checked;
            _config.FontName = cfgComboFont.SelectedItem?.ToString() ?? "Segoe UI";
            _config.FontSize = (int)cfgNumFontSize.Value;
            _config.AutoLogoutMinutes = (int)cfgNumAutoLogout.Value;
            _config.Language = cfgComboLanguage.SelectedItem?.ToString() == "English" ? "en" : "pt";

            if (string.IsNullOrWhiteSpace(_config.OriginalLanguage))
                _config.OriginalLanguage = _config.Language;

            ConfigManager.Save(_config);

            MessageBox.Show("Configurações guardadas.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Aplicar imediatamente (aplica apenas elementos da janela principal; outros forms podem recarregar)
            TryApplyRuntimeSettings();
        }

        private void TryApplyRuntimeSettings()
        {
            try
            {
                var cfg = ConfigManager.Current;
                if (cfg == null) return;

                // Em vez de acessar membros privados do main_menu, delegue a aplicação ao próprio form
                foreach (Form open in Application.OpenForms)
                {
                    if (open is main_menu mn)
                    {
                        // pede ao main_menu para aplicar as configurações
                        mn.ApplyConfig(cfg);
                    }
                }
            }
            catch { }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonRestaurar_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Restaurar padrões irá repor todas as preferências. Continuar?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            ConfigManager.RestoreDefaults();
            LoadSettingsToControls();
        }

        private void buttonVoltarLingua_Click(object sender, EventArgs e)
        {
            var cfg = ConfigManager.Current;
            if (cfg == null || string.IsNullOrWhiteSpace(cfg.OriginalLanguage))
            {
                MessageBox.Show("Linguagem original não definida.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cfg.Language = cfg.OriginalLanguage;
            ConfigManager.Save(cfg);
            LoadSettingsToControls();
            MessageBox.Show("Linguagem restaurada para o original.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}