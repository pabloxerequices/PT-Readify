namespace PT_Readify
{
    partial class Configuracoes
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlContent;

        private Guna.UI2.WinForms.Guna2ComboBox cfgComboTheme;
        private Guna.UI2.WinForms.Guna2ToggleSwitch cfgToggleFullscreen;
        private System.Windows.Forms.Label lblFullscreen;
        private System.Windows.Forms.Label lblTheme;

        private System.Windows.Forms.Label lblFont;
        private Guna.UI2.WinForms.Guna2ComboBox cfgComboFont;
        private System.Windows.Forms.Label lblFontSize;
        private Guna.UI2.WinForms.Guna2NumericUpDown cfgNumFontSize;

        private System.Windows.Forms.Label lblAutoLogout;
        private Guna.UI2.WinForms.Guna2ComboBox cfgComboAutoLogout;

        private System.Windows.Forms.Label lblLanguage;
        private Guna.UI2.WinForms.Guna2ComboBox cfgComboLanguage;

        private Guna.UI2.WinForms.Guna2Button btnSalvar;
        private Guna.UI2.WinForms.Guna2Button btnCancelar;
        private Guna.UI2.WinForms.Guna2Button btnRestaurar;
        private Guna.UI2.WinForms.Guna2Button btnVoltarLingua;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // instantiate
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();

            this.lblTheme = new System.Windows.Forms.Label();
            this.cfgComboTheme = new Guna.UI2.WinForms.Guna2ComboBox();

            this.lblFullscreen = new System.Windows.Forms.Label();
            this.cfgToggleFullscreen = new Guna.UI2.WinForms.Guna2ToggleSwitch();

            this.lblFont = new System.Windows.Forms.Label();
            this.cfgComboFont = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.cfgNumFontSize = new Guna.UI2.WinForms.Guna2NumericUpDown();

            this.lblAutoLogout = new System.Windows.Forms.Label();
            this.cfgComboAutoLogout = new Guna.UI2.WinForms.Guna2ComboBox();

            this.lblLanguage = new System.Windows.Forms.Label();
            this.cfgComboLanguage = new Guna.UI2.WinForms.Guna2ComboBox();

            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancelar = new Guna.UI2.WinForms.Guna2Button();
            this.btnRestaurar = new Guna.UI2.WinForms.Guna2Button();
            this.btnVoltarLingua = new Guna.UI2.WinForms.Guna2Button();

            // pnlTop
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(33, 41, 52);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 64;
            this.pnlTop.Controls.Add(this.lblTitle);

            // lblTitle
            this.lblTitle.Text = "⚙️ Configurações";
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(16, 14);
            this.lblTitle.AutoSize = true;

            // pnlContent
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.pnlContent.Padding = new System.Windows.Forms.Padding(24);

            // NOTE: Use valores literais; estas posições são estáticas e compatíveis com o Designer.
            // Theme
            this.lblTheme.Location = new System.Drawing.Point(24, 20);
            this.lblTheme.AutoSize = true;
            this.lblTheme.Text = "Tema:";
            this.lblTheme.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.cfgComboTheme.Location = new System.Drawing.Point(220, 16);
            this.cfgComboTheme.Size = new System.Drawing.Size(240, 36);
            this.cfgComboTheme.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cfgComboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cfgComboTheme.Font = new System.Drawing.Font("Segoe UI", 9F);

            // Fullscreen
            this.lblFullscreen.Location = new System.Drawing.Point(24, 68);
            this.lblFullscreen.AutoSize = true;
            this.lblFullscreen.Text = "Modo de leitura fullscreen:";
            this.lblFullscreen.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.cfgToggleFullscreen.Location = new System.Drawing.Point(220, 64);
            this.cfgToggleFullscreen.Size = new System.Drawing.Size(60, 22);
            this.cfgToggleFullscreen.CheckedState.BorderColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.cfgToggleFullscreen.UncheckedState.BorderColor = System.Drawing.Color.Gray;

            // Font
            this.lblFont.Location = new System.Drawing.Point(24, 116);
            this.lblFont.AutoSize = true;
            this.lblFont.Text = "Fonte:";
            this.lblFont.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.cfgComboFont.Location = new System.Drawing.Point(220, 112);
            this.cfgComboFont.Size = new System.Drawing.Size(240, 36);
            this.cfgComboFont.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cfgComboFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Font size
            this.lblFontSize.Location = new System.Drawing.Point(24, 164);
            this.lblFontSize.AutoSize = true;
            this.lblFontSize.Text = "Tamanho (pt):";
            this.lblFontSize.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.cfgNumFontSize.Location = new System.Drawing.Point(220, 160);
            this.cfgNumFontSize.Size = new System.Drawing.Size(120, 36);
            this.cfgNumFontSize.Minimum = 15;
            this.cfgNumFontSize.Maximum = 100;
            this.cfgNumFontSize.Value = 15;

            // AutoLogout
            this.lblAutoLogout.Location = new System.Drawing.Point(24, 212);
            this.lblAutoLogout.AutoSize = true;
            this.lblAutoLogout.Text = "Temporizador de desconexão:";
            this.lblAutoLogout.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.cfgComboAutoLogout.Location = new System.Drawing.Point(220, 208);
            this.cfgComboAutoLogout.Size = new System.Drawing.Size(240, 36);
            this.cfgComboAutoLogout.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cfgComboAutoLogout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Language
            this.lblLanguage.Location = new System.Drawing.Point(24, 260);
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Text = "Linguagem:";
            this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.cfgComboLanguage.Location = new System.Drawing.Point(220, 256);
            this.cfgComboLanguage.Size = new System.Drawing.Size(240, 36);
            this.cfgComboLanguage.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cfgComboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Buttons
            this.btnSalvar.Location = new System.Drawing.Point(220, 320);
            this.btnSalvar.Size = new System.Drawing.Size(120, 38);
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Click += new System.EventHandler(this.buttonSalvar_Click);

            this.btnCancelar.Location = new System.Drawing.Point(350, 320);
            this.btnCancelar.Size = new System.Drawing.Size(120, 38);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);

            this.btnRestaurar.Location = new System.Drawing.Point(480, 320);
            this.btnRestaurar.Size = new System.Drawing.Size(150, 38);
            this.btnRestaurar.Text = "Restaurar Padrões";
            this.btnRestaurar.FillColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.btnRestaurar.ForeColor = System.Drawing.Color.White;
            this.btnRestaurar.Click += new System.EventHandler(this.buttonRestaurar_Click);

            this.btnVoltarLingua.Location = new System.Drawing.Point(620, 256);
            this.btnVoltarLingua.Size = new System.Drawing.Size(180, 28);
            this.btnVoltarLingua.Text = "Voltar à linguagem original";
            this.btnVoltarLingua.FillColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnVoltarLingua.ForeColor = System.Drawing.Color.White;
            this.btnVoltarLingua.Click += new System.EventHandler(this.buttonVoltarLingua_Click);

            // Assemble controls
            this.pnlContent.Controls.Add(this.lblTheme);
            this.pnlContent.Controls.Add(this.cfgComboTheme);
            this.pnlContent.Controls.Add(this.lblFullscreen);
            this.pnlContent.Controls.Add(this.cfgToggleFullscreen);
            this.pnlContent.Controls.Add(this.lblFont);
            this.pnlContent.Controls.Add(this.cfgComboFont);
            this.pnlContent.Controls.Add(this.lblFontSize);
            this.pnlContent.Controls.Add(this.cfgNumFontSize);
            this.pnlContent.Controls.Add(this.lblAutoLogout);
            this.pnlContent.Controls.Add(this.cfgComboAutoLogout);
            this.pnlContent.Controls.Add(this.lblLanguage);
            this.pnlContent.Controls.Add(this.cfgComboLanguage);
            this.pnlContent.Controls.Add(this.btnSalvar);
            this.pnlContent.Controls.Add(this.btnCancelar);
            this.pnlContent.Controls.Add(this.btnRestaurar);
            this.pnlContent.Controls.Add(this.btnVoltarLingua);

            // Form
            this.ClientSize = new System.Drawing.Size(980, 520);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlTop);
            this.ShowIcon = false;
            this.Text = "Configurações";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        }
    }
}