using System.Windows.Forms;

namespace PT_Readify
{
    public static class FormLaunchHelper
    {
        public static bool ShouldApplyFullscreen(Form form)
        {
            if (form == null) return false;
            if (form is Detalhes_Livro) return false;
            if (form is Chat_Bot) return false;
            return true;
        }

        public static void PrepareForm(Form form)
        {
            var cfg = ConfigManager.Current;
            if (cfg == null || !cfg.FullscreenReading) return;
            if (!ShouldApplyFullscreen(form)) return;

            form.StartPosition = FormStartPosition.Manual;
            form.FormBorderStyle = FormBorderStyle.None;
            form.WindowState = FormWindowState.Maximized;
            form.Bounds = Screen.PrimaryScreen.WorkingArea;
        }

        public static void Show(Form form, Form owner = null)
        {
            PrepareForm(form);
            if (owner != null)
                form.Show(owner);
            else
                form.Show();
        }

        public static DialogResult ShowDialog(Form form, IWin32Window owner = null)
        {
            PrepareForm(form);
            return owner != null ? form.ShowDialog(owner) : form.ShowDialog();
        }
    }
}
