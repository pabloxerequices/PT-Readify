using System;
using System.Windows.Forms;

namespace PT_Readify
{
    public partial class InputMultaForm : Form
    {
        private Label lblPrompt;
        private TextBox txtValor;
        private Button btnOK;
        private Button btnCancel;

        public int ValorMulta { get; private set; }

        public InputMultaForm(string prompt, string defaultValue = "1000")
        {
            this.Text = "Multa por Livro Estragado";
            this.Size = new System.Drawing.Size(400, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblPrompt = new Label
            {
                Text = prompt,
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(340, 30),
                AutoSize = true
            };

            txtValor = new TextBox
            {
                Text = defaultValue,
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(340, 25)
            };

            btnOK = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(200, 100),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += (s, e) => {
                if (int.TryParse(txtValor.Text, out int valor) && valor >= 0)
                {
                    ValorMulta = valor;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Valor inválido. Insira um número positivo em cêntimos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new System.Drawing.Point(280, 100),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += (s, e) => {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.Add(lblPrompt);
            this.Controls.Add(txtValor);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        public static int ShowDialog(string prompt, string defaultValue = "1000")
        {
            using (var form = new InputMultaForm(prompt, defaultValue))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    return form.ValorMulta;
                }
                return -1;
            }
        }
    }
}
