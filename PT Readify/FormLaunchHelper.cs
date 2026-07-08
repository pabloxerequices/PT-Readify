using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PT_Readify
{
    public static class FormLaunchHelper
    {
        private static readonly Dictionary<Type, Form> OpenForms = new Dictionary<Type, Form>();

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
            Type formType = form.GetType();
            
            // Check if form of same type is already open
            if (OpenForms.TryGetValue(formType, out Form existingForm))
            {
                if (!existingForm.IsDisposed)
                {
                    existingForm.Activate();
                    existingForm.BringToFront();
                    form.Dispose();
                    return;
                }
                else
                {
                    OpenForms.Remove(formType);
                }
            }

            PrepareForm(form);
            
            if (owner != null)
                form.Show(owner);
            else
                form.Show();

            // Track the form
            OpenForms[formType] = form;
            form.FormClosed += (s, e) => OpenForms.Remove(formType);
        }

        public static DialogResult ShowDialog(Form form, IWin32Window owner = null)
        {
            // For dialogs, we don't prevent duplicates as they are modal
            PrepareForm(form);
            return owner != null ? form.ShowDialog(owner) : form.ShowDialog();
        }
    }
}
