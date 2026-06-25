using System;
using System.Drawing;
using System.Windows.Forms;

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

        public static void ApplyFont(Control root, Config cfg)
        {
            if (root == null || cfg == null) return;
            try
            {
                var font = new Font(cfg.FontName ?? "Segoe UI", Math.Max(15, Math.Min(100, cfg.FontSize)));
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
                return new Font(cfg?.FontName ?? "Segoe UI", Math.Max(15, Math.Min(100, cfg?.FontSize ?? 15)));
            }
            catch
            {
                return new Font("Segoe UI", 15);
            }
        }
    }
}
