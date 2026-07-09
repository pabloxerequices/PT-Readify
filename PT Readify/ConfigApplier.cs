using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace PT_Readify
{
    public static class ConfigApplier
    {
        public static bool IsDarkTheme(Config cfg)
        {
            if (cfg == null) return false;
            var t = cfg.Theme ?? "";
            return t.Equals("Escuro", StringComparison.OrdinalIgnoreCase)
                || t.Equals("Dark", StringComparison.OrdinalIgnoreCase);
        }

        public static void ApplyTheme(Form form, Panel sidebarPanel, Config cfg)
        {
            if (form == null || cfg == null) return;

            if (IsDarkTheme(cfg))
            {
                form.BackColor = Color.FromArgb(45, 45, 48);
                if (sidebarPanel != null)
                    sidebarPanel.BackColor = Color.FromArgb(37, 37, 38);
            }
            else
            {
                form.BackColor = Color.LightBlue;
                if (sidebarPanel != null)
                    sidebarPanel.BackColor = Color.WhiteSmoke;
            }
        }

        public static void ApplyThemeToPanel(Panel panel, Config cfg)
        {
            if (panel == null || cfg == null) return;
            panel.BackColor = IsDarkTheme(cfg)
                ? Color.FromArgb(45, 45, 48)
                : Color.FromArgb(240, 242, 245);
        }

        public static void ApplyThemeToHeader(Panel headerPanel, Config cfg)
        {
            if (headerPanel == null) return;
            headerPanel.BackColor = Color.FromArgb(33, 41, 52);
        }

        public static void ApplyThemeToAllForms(Config cfg)
        {
            if (cfg == null) return;
            
            var isDark = IsDarkTheme(cfg);
            
            foreach (Form form in Application.OpenForms)
            {
                ApplyThemeToForm(form, isDark);
            }
        }
        
        private static void ApplyThemeToForm(Form form, bool isDark)
        {
            // Skip forms that handle their own theme or should not have theme applied
            if (form is Configuracoes || form is Chat_Bot || form is Form1) return;
            
            if (isDark)
            {
                form.BackColor = Color.FromArgb(45, 45, 48);
                
                // Apply to all controls recursively
                ApplyThemeToControls(form.Controls, isDark);
            }
            else
            {
                // Revert to light theme - use default colors
                form.BackColor = SystemColors.Control;
                ApplyThemeToControls(form.Controls, isDark);
            }
        }
        
        private static void ApplyThemeToControls(Control.ControlCollection controls, bool isDark)
        {
            foreach (Control control in controls)
            {
                if (control is Panel panel)
                {
                    panel.BackColor = isDark ? Color.FromArgb(45, 45, 48) : SystemColors.Control;
                    ApplyThemeToControls(panel.Controls, isDark);
                }
                else if (control is Label label)
                {
                    label.ForeColor = isDark ? Color.WhiteSmoke : SystemColors.ControlText;
                }
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = isDark ? Color.FromArgb(60, 60, 65) : Color.White;
                    textBox.ForeColor = isDark ? Color.White : SystemColors.WindowText;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.BackColor = isDark ? Color.FromArgb(60, 60, 65) : Color.White;
                    comboBox.ForeColor = isDark ? Color.White : SystemColors.WindowText;
                }
                else if (control is Button button)
                {
                    button.BackColor = isDark ? Color.FromArgb(70, 70, 75) : SystemColors.Control;
                    button.ForeColor = isDark ? Color.White : SystemColors.ControlText;
                }
                else if (control is DataGridView dataGridView)
                {
                    dataGridView.BackgroundColor = isDark ? Color.FromArgb(45, 45, 48) : SystemColors.Window;
                    dataGridView.BackColor = isDark ? Color.FromArgb(45, 45, 48) : SystemColors.Window;
                }
                
                // Recursively apply to child controls
                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls, isDark);
                }
            }
        }

        public static void ApplyFont(Control root, Config cfg)
        {
            if (root == null || cfg == null) return;
            try
            {
                var font = new Font(cfg.FontName ?? "Segoe UI", Math.Max(10, Math.Min(24, cfg.FontSize)));
                root.Font = font;
            }
            catch
            {
                // ignore invalid font names
            }
        }

        public static void ApplyReadingMode(Form form, Config cfg)
        {
            // Mantido por compatibilidade — fullscreen não se aplica a Detalhes_Livro.
            FormLaunchHelper.PrepareForm(form);
        }

        public static bool ShouldFullscreenForm(Form form)
        {
            return FormLaunchHelper.ShouldApplyFullscreen(form);
        }

        public static Font GetReadingFont(Config cfg)
        {
            try
            {
                return new Font(cfg?.FontName ?? "Segoe UI", Math.Max(10, Math.Min(24, cfg?.FontSize ?? 15)));
            }
            catch
            {
                return new Font("Segoe UI", 15);
            }
        }
    }
}
