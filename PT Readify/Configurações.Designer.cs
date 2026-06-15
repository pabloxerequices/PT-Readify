namespace PT_Readify
{
    partial class Configurações
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.headerPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.headerLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.containerPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.groupTema = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTema = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.toggleTemaEscuro = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.lblFullscreen = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.chkFullscreen = new Guna.UI2.WinForms.Guna2ToggleSwitch();
            this.groupVisual = new Guna.UI2.WinForms.Guna2Panel();
            this.lblFonte = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.comboFont = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblTextoTam = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.trackTextSize = new Guna.UI2.WinForms.Guna2TrackBar();
            this.groupAcess = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTTSRate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.trackTTSRate = new Guna.UI2.WinForms.Guna2TrackBar();
            this.lbTDD = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.comboTTSVoices = new Guna.UI2.WinForms.Guna2ComboBox();
            this.groupAvancadas = new Guna.UI2.WinForms.Guna2Panel();
            this.btnLimparHistorico = new Guna.UI2.WinForms.Guna2Button();
            this.btnRestaurarPadrao = new Guna.UI2.WinForms.Guna2Button();
            this.actionsPanel = new Guna.UI2.WinForms.Guna2Panel();
            this.btnSalvarConfig = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancelar = new Guna.UI2.WinForms.Guna2Button();
            this.headerPanel.SuspendLayout();
            this.containerPanel.SuspendLayout();
            this.groupTema.SuspendLayout();
            this.groupVisual.SuspendLayout();
            this.groupAcess.SuspendLayout();
            this.groupAvancadas.SuspendLayout();
            this.actionsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(135)))), ((int)(((byte)(140)))));
            this.headerPanel.Controls.Add(this.headerLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(720, 72);
            this.headerPanel.TabIndex = 0;
            // 
            // headerLabel
            // 
            this.headerLabel.BackColor = System.Drawing.Color.Transparent;
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.headerLabel.ForeColor = System.Drawing.Color.White;
            this.headerLabel.Location = new System.Drawing.Point(18, 18);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(140, 32);
            this.headerLabel.TabIndex = 0;
            this.headerLabel.Text = "Configurações";
            // 
            // containerPanel
            // 
            this.containerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.containerPanel.Controls.Add(this.groupTema);
            this.containerPanel.Controls.Add(this.groupVisual);
            this.containerPanel.Controls.Add(this.groupAcess);
            this.containerPanel.Controls.Add(this.groupAvancadas);
            this.containerPanel.Controls.Add(this.actionsPanel);
            this.containerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.containerPanel.Location = new System.Drawing.Point(0, 72);
            this.containerPanel.Name = "containerPanel";
            this.containerPanel.Padding = new System.Windows.Forms.Padding(18);
            this.containerPanel.Size = new System.Drawing.Size(720, 649);
            this.containerPanel.TabIndex = 1;
            // 
            // groupTema
            // 
            this.groupTema.BackColor = System.Drawing.Color.White;
            this.groupTema.BorderRadius = 8;
            this.groupTema.Controls.Add(this.lblTema);
            this.groupTema.Controls.Add(this.toggleTemaEscuro);
            this.groupTema.Controls.Add(this.lblFullscreen);
            this.groupTema.Controls.Add(this.chkFullscreen);
            this.groupTema.Location = new System.Drawing.Point(18, 18);
            this.groupTema.Name = "groupTema";
            this.groupTema.Size = new System.Drawing.Size(684, 80);
            this.groupTema.TabIndex = 0;
            // 
            // lblTema
            // 
            this.lblTema.BackColor = System.Drawing.Color.Transparent;
            this.lblTema.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTema.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(135)))), ((int)(((byte)(140)))));
            this.lblTema.Location = new System.Drawing.Point(16, 20);
            this.lblTema.Name = "lblTema";
            this.lblTema.Size = new System.Drawing.Size(90, 22);
            this.lblTema.TabIndex = 0;
            this.lblTema.Text = "Tema Escuro:";
            // 
            // toggleTemaEscuro
            // 
            this.toggleTemaEscuro.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.toggleTemaEscuro.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(139)))), ((int)(((byte)(147)))));
            this.toggleTemaEscuro.CheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.toggleTemaEscuro.CheckedState.InnerColor = System.Drawing.Color.White;
            this.toggleTemaEscuro.Location = new System.Drawing.Point(580, 20);
            this.toggleTemaEscuro.Name = "toggleTemaEscuro";
            this.toggleTemaEscuro.Size = new System.Drawing.Size(42, 20);
            this.toggleTemaEscuro.TabIndex = 1;
            this.toggleTemaEscuro.CheckedChanged += new System.EventHandler(this.toggleTemaEscuro_CheckedChanged);
            // 
            // lblFullscreen
            // 
            this.lblFullscreen.BackColor = System.Drawing.Color.Transparent;
            this.lblFullscreen.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFullscreen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(135)))), ((int)(((byte)(140)))));
            this.lblFullscreen.Location = new System.Drawing.Point(16, 44);
            this.lblFullscreen.Name = "lblFullscreen";
            this.lblFullscreen.Size = new System.Drawing.Size(164, 22);
            this.lblFullscreen.TabIndex = 2;
            this.lblFullscreen.Text = "Modo Leitura Fullscreen:";
            // 
            // chkFullscreen
            // 
            this.chkFullscreen.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkFullscreen.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(139)))), ((int)(((byte)(147)))));
            this.chkFullscreen.CheckedState.InnerBorderColor = System.Drawing.Color.White;
            this.chkFullscreen.CheckedState.InnerColor = System.Drawing.Color.White;
            this.chkFullscreen.Location = new System.Drawing.Point(580, 46);
            this.chkFullscreen.Name = "chkFullscreen";
            this.chkFullscreen.Size = new System.Drawing.Size(42, 20);
            this.chkFullscreen.TabIndex = 3;
            // 
            // groupVisual
            // 
            this.groupVisual.BackColor = System.Drawing.Color.White;
            this.groupVisual.BorderRadius = 8;
            this.groupVisual.Controls.Add(this.lblFonte);
            this.groupVisual.Controls.Add(this.comboFont);
            this.groupVisual.Controls.Add(this.lblTextoTam);
            this.groupVisual.Controls.Add(this.trackTextSize);
            this.groupVisual.Location = new System.Drawing.Point(18, 110);
            this.groupVisual.Name = "groupVisual";
            this.groupVisual.Size = new System.Drawing.Size(684, 120);
            this.groupVisual.TabIndex = 2;
            // 
            // lblFonte
            // 
            this.lblFonte.BackColor = System.Drawing.Color.Transparent;
            this.lblFonte.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblFonte.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(135)))), ((int)(((byte)(140)))));
            this.lblFonte.Location = new System.Drawing.Point(16, 16);
            this.lblFonte.Name = "lblFonte";
            this.lblFonte.Size = new System.Drawing.Size(43, 22);
            this.lblFonte.TabIndex = 0;
            this.lblFonte.Text = "Fonte:";
            // 
            // comboFont
            // 
            this.comboFont.BackColor = System.Drawing.Color.Transparent;
            this.comboFont.BorderRadius = 6;
            this.comboFont.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFont.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.comboFont.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.comboFont.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboFont.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.comboFont.ItemHeight = 30;
            this.comboFont.Location = new System.Drawing.Point(240, 12);
            this.comboFont.Name = "comboFont";
            this.comboFont.Size = new System.Drawing.Size(300, 36);
            this.comboFont.TabIndex = 1;
            // 
            // lblTextoTam
            // 
            this.lblTextoTam.BackColor = System.Drawing.Color.Transparent;
            this.lblTextoTam.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTextoTam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(135)))), ((int)(((byte)(140)))));
            this.lblTextoTam.Location = new System.Drawing.Point(16, 64);
            this.lblTextoTam.Name = "lblTextoTam";
            this.lblTextoTam.Size = new System.Drawing.Size(131, 22);
            this.lblTextoTam.TabIndex = 2;
            this.lblTextoTam.Text = "Tamanho do Texto:";
            // 
            // trackTextSize
            // 
            this.trackTextSize.BackColor = System.Drawing.Color.Transparent;
            this.trackTextSize.Location = new System.Drawing.Point(240, 64);
            this.trackTextSize.Name = "trackTextSize";
            this.trackTextSize.Size = new System.Drawing.Size(300, 23);
            this.trackTextSize.TabIndex = 3;
            this.trackTextSize.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(139)))), ((int)(((byte)(147)))));
            this.trackTextSize.Value = 14;
            // 
            // groupAcess
            // 
            this.groupAcess.BackColor = System.Drawing.Color.White;
            this.groupAcess.BorderRadius = 8;
            this.groupAcess.Controls.Add(this.lblTTSRate);
            this.groupAcess.Controls.Add(this.trackTTSRate);
            this.groupAcess.Controls.Add(this.lbTDD);
            this.groupAcess.Controls.Add(this.comboTTSVoices);
            this.groupAcess.Location = new System.Drawing.Point(18, 242);
            this.groupAcess.Name = "groupAcess";
            this.groupAcess.Size = new System.Drawing.Size(684, 120);
            this.groupAcess.TabIndex = 3;
            // 
            // lblTTSRate
            // 
            this.lblTTSRate.BackColor = System.Drawing.Color.Transparent;
            this.lblTTSRate.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTTSRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(135)))), ((int)(((byte)(140)))));
            this.lblTTSRate.Location = new System.Drawing.Point(16, 64);
            this.lblTTSRate.Name = "lblTTSRate";
            this.lblTTSRate.Size = new System.Drawing.Size(151, 22);
            this.lblTTSRate.TabIndex = 2;
            this.lblTTSRate.Text = "Velocidade da Leitura:";
            // 
            // trackTTSRate
            // 
            this.trackTTSRate.BackColor = System.Drawing.Color.Transparent;
            this.trackTTSRate.Location = new System.Drawing.Point(240, 64);
            this.trackTTSRate.Name = "trackTTSRate";
            this.trackTTSRate.Size = new System.Drawing.Size(300, 23);
            this.trackTTSRate.TabIndex = 3;
            this.trackTTSRate.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(139)))), ((int)(((byte)(147)))));
            this.trackTTSRate.Value = 0;
            // 
            // lbTDD
            // 
            this.lbTDD.BackColor = System.Drawing.Color.Transparent;
            this.lbTDD.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lbTDD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(135)))), ((int)(((byte)(140)))));
            this.lbTDD.Location = new System.Drawing.Point(16, 16);
            this.lbTDD.Name = "lbTDD";
            this.lbTDD.Size = new System.Drawing.Size(206, 22);
            this.lbTDD.TabIndex = 0;
            this.lbTDD.Text = "Temporizador de Desconexão:";
            // 
            // comboTTSVoices
            // 
            this.comboTTSVoices.BackColor = System.Drawing.Color.Transparent;
            this.comboTTSVoices.BorderRadius = 6;
            this.comboTTSVoices.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboTTSVoices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTTSVoices.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.comboTTSVoices.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.comboTTSVoices.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboTTSVoices.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.comboTTSVoices.ItemHeight = 30;
            this.comboTTSVoices.Location = new System.Drawing.Point(240, 12);
            this.comboTTSVoices.Name = "comboTTSVoices";
            this.comboTTSVoices.Size = new System.Drawing.Size(300, 36);
            this.comboTTSVoices.TabIndex = 1;
            // 
            // groupAvancadas
            // 
            this.groupAvancadas.BackColor = System.Drawing.Color.White;
            this.groupAvancadas.BorderRadius = 8;
            this.groupAvancadas.Controls.Add(this.btnLimparHistorico);
            this.groupAvancadas.Controls.Add(this.btnRestaurarPadrao);
            this.groupAvancadas.Location = new System.Drawing.Point(18, 374);
            this.groupAvancadas.Name = "groupAvancadas";
            this.groupAvancadas.Size = new System.Drawing.Size(684, 120);
            this.groupAvancadas.TabIndex = 4;
            // 
            // btnLimparHistorico
            // 
            this.btnLimparHistorico.BorderRadius = 8;
            this.btnLimparHistorico.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(195)))), ((int)(((byte)(85)))));
            this.btnLimparHistorico.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLimparHistorico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(70)))), ((int)(((byte)(50)))));
            this.btnLimparHistorico.Location = new System.Drawing.Point(18, 20);
            this.btnLimparHistorico.Name = "btnLimparHistorico";
            this.btnLimparHistorico.Size = new System.Drawing.Size(300, 40);
            this.btnLimparHistorico.TabIndex = 0;
            this.btnLimparHistorico.Text = "Limpar Histórico de Leitura";
            // 
            // btnRestaurarPadrao
            // 
            this.btnRestaurarPadrao.BorderRadius = 8;
            this.btnRestaurarPadrao.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnRestaurarPadrao.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRestaurarPadrao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnRestaurarPadrao.Location = new System.Drawing.Point(18, 70);
            this.btnRestaurarPadrao.Name = "btnRestaurarPadrao";
            this.btnRestaurarPadrao.Size = new System.Drawing.Size(300, 36);
            this.btnRestaurarPadrao.TabIndex = 1;
            this.btnRestaurarPadrao.Text = "Restaurar Padrões";
            // 
            // actionsPanel
            // 
            this.actionsPanel.BackColor = System.Drawing.Color.Transparent;
            this.actionsPanel.Controls.Add(this.btnSalvarConfig);
            this.actionsPanel.Controls.Add(this.btnCancelar);
            this.actionsPanel.Location = new System.Drawing.Point(18, 510);
            this.actionsPanel.Name = "actionsPanel";
            this.actionsPanel.Size = new System.Drawing.Size(684, 64);
            this.actionsPanel.TabIndex = 5;
            // 
            // btnSalvarConfig
            // 
            this.btnSalvarConfig.BorderRadius = 8;
            this.btnSalvarConfig.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(139)))), ((int)(((byte)(147)))));
            this.btnSalvarConfig.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSalvarConfig.ForeColor = System.Drawing.Color.White;
            this.btnSalvarConfig.Location = new System.Drawing.Point(366, 12);
            this.btnSalvarConfig.Name = "btnSalvarConfig";
            this.btnSalvarConfig.Size = new System.Drawing.Size(168, 40);
            this.btnSalvarConfig.TabIndex = 0;
            this.btnSalvarConfig.Text = "Guardar";
            // 
            // btnCancelar
            // 
            this.btnCancelar.BorderRadius = 8;
            this.btnCancelar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnCancelar.Location = new System.Drawing.Point(540, 12);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(162, 40);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            // 
            // Configurações
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 721);
            this.Controls.Add(this.containerPanel);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Configurações";
            this.Text = "Configurações";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.containerPanel.ResumeLayout(false);
            this.groupTema.ResumeLayout(false);
            this.groupTema.PerformLayout();
            this.groupVisual.ResumeLayout(false);
            this.groupVisual.PerformLayout();
            this.groupAcess.ResumeLayout(false);
            this.groupAcess.PerformLayout();
            this.groupAvancadas.ResumeLayout(false);
            this.actionsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel headerPanel;
        private Guna.UI2.WinForms.Guna2HtmlLabel headerLabel;
        private Guna.UI2.WinForms.Guna2Panel containerPanel;
        private Guna.UI2.WinForms.Guna2Panel groupTema;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTema;
        private Guna.UI2.WinForms.Guna2ToggleSwitch toggleTemaEscuro;
        private Guna.UI2.WinForms.Guna2Panel groupVisual;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblFonte;
        private Guna.UI2.WinForms.Guna2ComboBox comboFont;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTextoTam;
        private Guna.UI2.WinForms.Guna2TrackBar trackTextSize;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblFullscreen;
        private Guna.UI2.WinForms.Guna2ToggleSwitch chkFullscreen;
        private Guna.UI2.WinForms.Guna2Panel groupAcess;
        private Guna.UI2.WinForms.Guna2HtmlLabel lbTDD;
        private Guna.UI2.WinForms.Guna2ComboBox comboTTSVoices;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTTSRate;
        private Guna.UI2.WinForms.Guna2TrackBar trackTTSRate;
        private Guna.UI2.WinForms.Guna2Panel groupAvancadas;
        private Guna.UI2.WinForms.Guna2Button btnLimparHistorico;
        private Guna.UI2.WinForms.Guna2Button btnRestaurarPadrao;
        private Guna.UI2.WinForms.Guna2Panel actionsPanel;
        private Guna.UI2.WinForms.Guna2Button btnSalvarConfig;
        private Guna.UI2.WinForms.Guna2Button btnCancelar;
    }
}