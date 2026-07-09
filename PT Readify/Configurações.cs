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
            this.FormClosed += Configuracoes_FormClosed;
        }

        private void Configuracoes_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Form open in Application.OpenForms)
            {
                if (open is main_menu mn)
                {
                    mn.ShowBooksPanel();
                    break;
                }
            }
        }

        private void ApplyStyleToControls()
        {
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

                ApplyFormLanguage();
                ApplyFormTheme();

                cfgComboTheme.Items.Clear();
                cfgComboTheme.Items.Add(LanguageHelper.T("ThemeLight", _config));
                cfgComboTheme.Items.Add(LanguageHelper.T("ThemeDark", _config));
                cfgComboTheme.SelectedItem = ConfigApplier.IsDarkTheme(_config)
                    ? LanguageHelper.T("ThemeDark", _config)
                    : LanguageHelper.T("ThemeLight", _config);

                cfgToggleFullscreen.Checked = _config.FullscreenReading;

                cfgComboFont.Items.Clear();
                cfgComboFont.Items.AddRange(new object[] { "Segoe UI", "Arial", "Times New Roman", "Calibri" });
                cfgComboFont.SelectedItem = string.IsNullOrWhiteSpace(_config.FontName) ? "Segoe UI" : _config.FontName;

                cfgNumFontSize.Value = Math.Max(10, Math.Min(24, _config.FontSize));

                cfgComboAutoLogout.Items.Clear();
                foreach (var minutes in AutoLogoutManager.AllowedMinutes)
                {
                    cfgComboAutoLogout.Items.Add(new LogoutOptionItem(minutes, LanguageHelper.LogoutOptionLabel(minutes, _config)));
                }
                SelectLogoutOption(_config.AutoLogoutMinutes);

                cfgComboLanguage.Items.Clear();
                cfgComboLanguage.Items.Add("Português");
                cfgComboLanguage.Items.Add("English");
                cfgComboLanguage.SelectedItem = _config.Language == "en" ? "English" : "Português";
        }

        private void SelectLogoutOption(int minutes)
        {
            minutes = AutoLogoutManager.NormalizeMinutes(minutes);
            for (int i = 0; i < cfgComboAutoLogout.Items.Count; i++)
            {
                if (cfgComboAutoLogout.Items[i] is LogoutOptionItem item && item.Minutes == minutes)
                {
                    cfgComboAutoLogout.SelectedIndex = i;
                    return;
                }
            }
            cfgComboAutoLogout.SelectedIndex = 0;
        }

        private int GetSelectedLogoutMinutes()
        {
            if (cfgComboAutoLogout.SelectedItem is LogoutOptionItem item)
                return item.Minutes;
            return 1;
        }

        private string GetSelectedTheme()
        {
            var selected = cfgComboTheme.SelectedItem?.ToString() ?? "";
            if (selected.Equals(LanguageHelper.T("ThemeDark", _config), StringComparison.OrdinalIgnoreCase)
                || selected.Equals("Dark", StringComparison.OrdinalIgnoreCase)
                || selected.Equals("Escuro", StringComparison.OrdinalIgnoreCase))
                return "Escuro";
            return "Claro";
        }

        private void ApplyFormLanguage()
        {
            this.Text = LanguageHelper.T("SettingsTitle", _config);
            lblTitle.Text = "⚙️ " + LanguageHelper.T("SettingsTitle", _config);
            lblTheme.Text = LanguageHelper.T("Theme", _config);
            lblFullscreen.Text = LanguageHelper.T("Fullscreen", _config);
            lblFont.Text = LanguageHelper.T("Font", _config);
            lblFontSize.Text = LanguageHelper.T("FontSize", _config);
            lblAutoLogout.Text = LanguageHelper.T("AutoLogout", _config);
            lblLanguage.Text = LanguageHelper.T("Language", _config);
            btnSalvar.Text = LanguageHelper.T("Save", _config);
            btnCancelar.Text = LanguageHelper.T("Cancel", _config);
            btnRestaurar.Text = LanguageHelper.T("Reset", _config);
            btnVoltarLingua.Text = LanguageHelper.T("RestoreLanguage", _config);
        }

        private void ApplyFormTheme()
        {
            ConfigApplier.ApplyThemeToHeader(pnlTop, _config);
            ConfigApplier.ApplyThemeToPanel(pnlContent, _config);

            var isDark = ConfigApplier.IsDarkTheme(_config);
            var labelColor = isDark ? Color.WhiteSmoke : Color.FromArgb(50, 50, 50);
            foreach (var lbl in pnlContent.Controls.OfType<Label>())
            {
                lbl.ForeColor = labelColor;
            }
            lblTitle.ForeColor = Color.White;
        }

        private void buttonSalvar_Click(object sender, EventArgs e)
        {
            if (_config == null) _config = Config.Default();

            var previousLanguage = _config.Language;

            _config.Theme = GetSelectedTheme();
            _config.FullscreenReading = cfgToggleFullscreen.Checked;
            _config.FontName = cfgComboFont.SelectedItem?.ToString() ?? "Segoe UI";
            _config.FontSize = (int)cfgNumFontSize.Value;
            _config.AutoLogoutMinutes = GetSelectedLogoutMinutes();
            _config.Language = cfgComboLanguage.SelectedItem?.ToString() == "English" ? "en" : "pt";

            if (string.IsNullOrWhiteSpace(_config.OriginalLanguage))
                _config.OriginalLanguage = previousLanguage ?? "pt";

            ConfigManager.Save(_config);

            MessageBox.Show(
                LanguageHelper.T("Saved", _config),
                LanguageHelper.T("Success", _config),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            TryApplyRuntimeSettings();
            LoadSettingsToControls();
            Close();
        }

        private void TryApplyRuntimeSettings()
        {
            try
            {
                var cfg = ConfigManager.Current;
                if (cfg == null) return;

                // Apply theme to all open forms
                ConfigApplier.ApplyThemeToAllForms(cfg);

                foreach (Form open in Application.OpenForms)
                {
                    if (open is main_menu mn)
                        mn.ApplyConfig(cfg);
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
            var confirm = MessageBox.Show(
                LanguageHelper.T("ResetConfirm", _config),
                LanguageHelper.T("Confirm", _config),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            ConfigManager.RestoreDefaults();
            _config = ConfigManager.Current;
            LoadSettingsToControls();
            TryApplyRuntimeSettings();
        }

        private void buttonVoltarLingua_Click(object sender, EventArgs e)
        {
            var cfg = ConfigManager.Current;
            if (cfg == null || string.IsNullOrWhiteSpace(cfg.OriginalLanguage))
            {
                MessageBox.Show(
                    LanguageHelper.T("NoOriginalLanguage", _config),
                    LanguageHelper.T("Info", _config),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            cfg.Language = cfg.OriginalLanguage;
            ConfigManager.Save(cfg);
            _config = cfg;
            LoadSettingsToControls();
            TryApplyRuntimeSettings();

            MessageBox.Show(
                LanguageHelper.T("LanguageRestored", _config),
                LanguageHelper.T("Success", _config),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private sealed class LogoutOptionItem
        {
            public int Minutes { get; }
            public string Label { get; }

            public LogoutOptionItem(int minutes, string label)
            {
                Minutes = minutes;
                Label = label;
            }

            public override string ToString() => Label;
        }
    }
}
