namespace PT_Readify
{
    partial class Carteira
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
            this.panelHeader = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.btnFechar = new Guna.UI2.WinForms.Guna2Button();
            this.lblTitulo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.panelAutenticacao = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.lblTituloAuth = new System.Windows.Forms.Label();
            this.lblPasswordLabel = new System.Windows.Forms.Label();
            this.txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnEntrar = new Guna.UI2.WinForms.Guna2Button();
            this.panelCarteira = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.lblTituloCarteira = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.btnAlterarPagamento = new Guna.UI2.WinForms.Guna2Button();
            this.panelEdicao = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.lblTituloEdicao = new System.Windows.Forms.Label();
            this.lblSelecione = new System.Windows.Forms.Label();
            this.cbMetodosPagamento = new Guna.UI2.WinForms.Guna2ComboBox();
            this.panelPayPal = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.lblEmailPayPal = new System.Windows.Forms.Label();
            this.txtEmailPayPal = new Guna.UI2.WinForms.Guna2TextBox();
            this.panelMBWay = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.lblTelemovelMBWay = new System.Windows.Forms.Label();
            this.txtTelemovelMBWay = new Guna.UI2.WinForms.Guna2TextBox();
            this.panelTransferencia = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.lblIbanTransferencia = new System.Windows.Forms.Label();
            this.txtIbanTransferencia = new Guna.UI2.WinForms.Guna2TextBox();
            this.panelApplePay = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.lblAppleID = new System.Windows.Forms.Label();
            this.txtAppleID = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.btnVoltar = new Guna.UI2.WinForms.Guna2Button();
            this.panelHeader.SuspendLayout();
            this.panelAutenticacao.SuspendLayout();
            this.panelCarteira.SuspendLayout();
            this.panelEdicao.SuspendLayout();
            this.panelPayPal.SuspendLayout();
            this.panelMBWay.SuspendLayout();
            this.panelTransferencia.SuspendLayout();
            this.panelApplePay.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(58)))), ((int)(((byte)(92)))));
            this.panelHeader.Controls.Add(this.btnFechar);
            this.panelHeader.Controls.Add(this.lblTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(58)))), ((int)(((byte)(92)))));
            this.panelHeader.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(58)))), ((int)(((byte)(92)))));
            this.panelHeader.FillColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(58)))), ((int)(((byte)(92)))));
            this.panelHeader.FillColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(58)))), ((int)(((byte)(92)))));
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(500, 64);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHeader_Paint);
            // 
            // btnFechar
            // 
            this.btnFechar.Animated = true;
            this.btnFechar.BorderRadius = 6;
            this.btnFechar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnFechar.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnFechar.ForeColor = System.Drawing.Color.White;
            this.btnFechar.Location = new System.Drawing.Point(408, 16);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(80, 32);
            this.btnFechar.TabIndex = 0;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(12, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(210, 42);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Carteira Digital";
            this.lblTitulo.Click += new System.EventHandler(this.lblTitulo_Click);
            // 
            // panelAutenticacao
            // 
            this.panelAutenticacao.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAutenticacao.BackColor = System.Drawing.Color.White;
            this.panelAutenticacao.Controls.Add(this.lblTituloAuth);
            this.panelAutenticacao.Controls.Add(this.lblPasswordLabel);
            this.panelAutenticacao.Controls.Add(this.txtPassword);
            this.panelAutenticacao.Controls.Add(this.btnEntrar);
            this.panelAutenticacao.Location = new System.Drawing.Point(0, 64);
            this.panelAutenticacao.Name = "panelAutenticacao";
            this.panelAutenticacao.Size = new System.Drawing.Size(500, 409);
            this.panelAutenticacao.TabIndex = 1;
            // 
            // lblTituloAuth
            // 
            this.lblTituloAuth.AutoSize = true;
            this.lblTituloAuth.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTituloAuth.Location = new System.Drawing.Point(60, 80);
            this.lblTituloAuth.Name = "lblTituloAuth";
            this.lblTituloAuth.Size = new System.Drawing.Size(267, 30);
            this.lblTituloAuth.TabIndex = 0;
            this.lblTituloAuth.Text = "Acesso à Carteira Digital";
            // 
            // lblPasswordLabel
            // 
            this.lblPasswordLabel.AutoSize = true;
            this.lblPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblPasswordLabel.Location = new System.Drawing.Point(60, 150);
            this.lblPasswordLabel.Name = "lblPasswordLabel";
            this.lblPasswordLabel.Size = new System.Drawing.Size(178, 20);
            this.lblPasswordLabel.TabIndex = 1;
            this.lblPasswordLabel.Text = "Introduza a sua Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtPassword.BorderRadius = 8;
            this.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPassword.DefaultText = "";
            this.txtPassword.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtPassword.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtPassword.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPassword.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtPassword.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtPassword.ForeColor = System.Drawing.Color.Black;
            this.txtPassword.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPassword.Location = new System.Drawing.Point(60, 175);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.PlaceholderText = "Digite aqui...";
            this.txtPassword.SelectedText = "";
            this.txtPassword.Size = new System.Drawing.Size(380, 45);
            this.txtPassword.TabIndex = 2;
            // 
            // btnEntrar
            // 
            this.btnEntrar.BorderRadius = 8;
            this.btnEntrar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnEntrar.ForeColor = System.Drawing.Color.White;
            this.btnEntrar.Location = new System.Drawing.Point(60, 250);
            this.btnEntrar.Name = "btnEntrar";
            this.btnEntrar.Size = new System.Drawing.Size(380, 45);
            this.btnEntrar.TabIndex = 3;
            this.btnEntrar.Text = "Entrar";
            this.btnEntrar.Click += new System.EventHandler(this.BtnEntrar_Click);
            // 
            // panelCarteira
            // 
            this.panelCarteira.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCarteira.BackColor = System.Drawing.Color.White;
            this.panelCarteira.Controls.Add(this.lblTituloCarteira);
            this.panelCarteira.Controls.Add(this.lblSaldo);
            this.panelCarteira.Controls.Add(this.btnAlterarPagamento);
            this.panelCarteira.Location = new System.Drawing.Point(0, 64);
            this.panelCarteira.Name = "panelCarteira";
            this.panelCarteira.Size = new System.Drawing.Size(500, 409);
            this.panelCarteira.TabIndex = 2;
            this.panelCarteira.Visible = false;
            // 
            // lblTituloCarteira
            // 
            this.lblTituloCarteira.AutoSize = true;
            this.lblTituloCarteira.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTituloCarteira.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(58)))), ((int)(((byte)(92)))));
            this.lblTituloCarteira.Location = new System.Drawing.Point(60, 60);
            this.lblTituloCarteira.Name = "lblTituloCarteira";
            this.lblTituloCarteira.Size = new System.Drawing.Size(208, 32);
            this.lblTituloCarteira.TabIndex = 0;
            this.lblTituloCarteira.Text = "A Minha Carteira";
            // 
            // lblSaldo
            // 
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblSaldo.Location = new System.Drawing.Point(60, 130);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(380, 50);
            this.lblSaldo.TabIndex = 1;
            this.lblSaldo.Text = "Saldo Atual: 150.00€";
            // 
            // btnAlterarPagamento
            // 
            this.btnAlterarPagamento.BorderRadius = 8;
            this.btnAlterarPagamento.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnAlterarPagamento.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnAlterarPagamento.ForeColor = System.Drawing.Color.White;
            this.btnAlterarPagamento.Location = new System.Drawing.Point(60, 240);
            this.btnAlterarPagamento.Name = "btnAlterarPagamento";
            this.btnAlterarPagamento.Size = new System.Drawing.Size(380, 45);
            this.btnAlterarPagamento.TabIndex = 2;
            this.btnAlterarPagamento.Text = "Alterar Forma de Pagamento";
            this.btnAlterarPagamento.Click += new System.EventHandler(this.BtnAlterarPagamento_Click);
            // 
            // panelEdicao
            // 
            this.panelEdicao.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEdicao.BackColor = System.Drawing.Color.White;
            this.panelEdicao.Controls.Add(this.lblTituloEdicao);
            this.panelEdicao.Controls.Add(this.lblSelecione);
            this.panelEdicao.Controls.Add(this.cbMetodosPagamento);
            this.panelEdicao.Controls.Add(this.panelPayPal);
            this.panelEdicao.Controls.Add(this.panelMBWay);
            this.panelEdicao.Controls.Add(this.panelTransferencia);
            this.panelEdicao.Controls.Add(this.panelApplePay);
            this.panelEdicao.Controls.Add(this.btnSalvar);
            this.panelEdicao.Controls.Add(this.btnVoltar);
            this.panelEdicao.Location = new System.Drawing.Point(0, 64);
            this.panelEdicao.Name = "panelEdicao";
            this.panelEdicao.Size = new System.Drawing.Size(500, 409);
            this.panelEdicao.TabIndex = 3;
            this.panelEdicao.Visible = false;
            // 
            // lblTituloEdicao
            // 
            this.lblTituloEdicao.AutoSize = true;
            this.lblTituloEdicao.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTituloEdicao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(58)))), ((int)(((byte)(92)))));
            this.lblTituloEdicao.Location = new System.Drawing.Point(60, 25);
            this.lblTituloEdicao.Name = "lblTituloEdicao";
            this.lblTituloEdicao.Size = new System.Drawing.Size(248, 30);
            this.lblTituloEdicao.TabIndex = 0;
            this.lblTituloEdicao.Text = "Configurar Pagamento";
            // 
            // lblSelecione
            // 
            this.lblSelecione.AutoSize = true;
            this.lblSelecione.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSelecione.Location = new System.Drawing.Point(60, 70);
            this.lblSelecione.Name = "lblSelecione";
            this.lblSelecione.Size = new System.Drawing.Size(203, 19);
            this.lblSelecione.TabIndex = 1;
            this.lblSelecione.Text = "Selecione o método pretendido:";
            // 
            // cbMetodosPagamento
            // 
            this.cbMetodosPagamento.BackColor = System.Drawing.Color.Transparent;
            this.cbMetodosPagamento.BorderRadius = 6;
            this.cbMetodosPagamento.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbMetodosPagamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMetodosPagamento.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbMetodosPagamento.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbMetodosPagamento.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbMetodosPagamento.ForeColor = System.Drawing.Color.Black;
            this.cbMetodosPagamento.ItemHeight = 30;
            this.cbMetodosPagamento.Items.AddRange(new object[] {
            "PayPal",
            "MBWay",
            "Transferência Bancária",
            "Apple Pay"});
            this.cbMetodosPagamento.Location = new System.Drawing.Point(60, 95);
            this.cbMetodosPagamento.Name = "cbMetodosPagamento";
            this.cbMetodosPagamento.Size = new System.Drawing.Size(380, 36);
            this.cbMetodosPagamento.TabIndex = 2;
            this.cbMetodosPagamento.SelectedIndexChanged += new System.EventHandler(this.CbMetodosPagamento_SelectedIndexChanged);
            // 
            // panelPayPal
            // 
            this.panelPayPal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelPayPal.BorderRadius = 8;
            this.panelPayPal.Controls.Add(this.lblEmailPayPal);
            this.panelPayPal.Controls.Add(this.txtEmailPayPal);
            this.panelPayPal.Location = new System.Drawing.Point(60, 150);
            this.panelPayPal.Name = "panelPayPal";
            this.panelPayPal.Size = new System.Drawing.Size(380, 100);
            this.panelPayPal.TabIndex = 3;
            this.panelPayPal.Visible = false;
            // 
            // lblEmailPayPal
            // 
            this.lblEmailPayPal.AutoSize = true;
            this.lblEmailPayPal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmailPayPal.Location = new System.Drawing.Point(10, 10);
            this.lblEmailPayPal.Name = "lblEmailPayPal";
            this.lblEmailPayPal.Size = new System.Drawing.Size(108, 19);
            this.lblEmailPayPal.TabIndex = 0;
            this.lblEmailPayPal.Text = "Email do PayPal:";
            // 
            // txtEmailPayPal
            // 
            this.txtEmailPayPal.BorderRadius = 6;
            this.txtEmailPayPal.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmailPayPal.DefaultText = "";
            this.txtEmailPayPal.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtEmailPayPal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtEmailPayPal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmailPayPal.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmailPayPal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmailPayPal.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmailPayPal.ForeColor = System.Drawing.Color.Black;
            this.txtEmailPayPal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmailPayPal.Location = new System.Drawing.Point(10, 35);
            this.txtEmailPayPal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmailPayPal.Name = "txtEmailPayPal";
            this.txtEmailPayPal.PlaceholderText = "seu.email@paypal.com";
            this.txtEmailPayPal.SelectedText = "";
            this.txtEmailPayPal.Size = new System.Drawing.Size(360, 35);
            this.txtEmailPayPal.TabIndex = 1;
            // 
            // panelMBWay
            // 
            this.panelMBWay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelMBWay.BorderRadius = 8;
            this.panelMBWay.Controls.Add(this.lblTelemovelMBWay);
            this.panelMBWay.Controls.Add(this.txtTelemovelMBWay);
            this.panelMBWay.Location = new System.Drawing.Point(60, 150);
            this.panelMBWay.Name = "panelMBWay";
            this.panelMBWay.Size = new System.Drawing.Size(380, 100);
            this.panelMBWay.TabIndex = 4;
            this.panelMBWay.Visible = false;
            // 
            // lblTelemovelMBWay
            // 
            this.lblTelemovelMBWay.AutoSize = true;
            this.lblTelemovelMBWay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTelemovelMBWay.Location = new System.Drawing.Point(10, 10);
            this.lblTelemovelMBWay.Name = "lblTelemovelMBWay";
            this.lblTelemovelMBWay.Size = new System.Drawing.Size(145, 19);
            this.lblTelemovelMBWay.TabIndex = 0;
            this.lblTelemovelMBWay.Text = "Número de Telemóvel:";
            // 
            // txtTelemovelMBWay
            // 
            this.txtTelemovelMBWay.BorderRadius = 6;
            this.txtTelemovelMBWay.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTelemovelMBWay.DefaultText = "";
            this.txtTelemovelMBWay.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTelemovelMBWay.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTelemovelMBWay.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTelemovelMBWay.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTelemovelMBWay.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTelemovelMBWay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTelemovelMBWay.ForeColor = System.Drawing.Color.Black;
            this.txtTelemovelMBWay.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTelemovelMBWay.Location = new System.Drawing.Point(10, 35);
            this.txtTelemovelMBWay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTelemovelMBWay.Name = "txtTelemovelMBWay";
            this.txtTelemovelMBWay.PlaceholderText = "+351 912345678";
            this.txtTelemovelMBWay.SelectedText = "";
            this.txtTelemovelMBWay.Size = new System.Drawing.Size(360, 35);
            this.txtTelemovelMBWay.TabIndex = 1;
            // 
            // panelTransferencia
            // 
            this.panelTransferencia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelTransferencia.BorderRadius = 8;
            this.panelTransferencia.Controls.Add(this.lblIbanTransferencia);
            this.panelTransferencia.Controls.Add(this.txtIbanTransferencia);
            this.panelTransferencia.Location = new System.Drawing.Point(60, 150);
            this.panelTransferencia.Name = "panelTransferencia";
            this.panelTransferencia.Size = new System.Drawing.Size(380, 100);
            this.panelTransferencia.TabIndex = 5;
            this.panelTransferencia.Visible = false;
            // 
            // lblIbanTransferencia
            // 
            this.lblIbanTransferencia.AutoSize = true;
            this.lblIbanTransferencia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblIbanTransferencia.Location = new System.Drawing.Point(10, 10);
            this.lblIbanTransferencia.Name = "lblIbanTransferencia";
            this.lblIbanTransferencia.Size = new System.Drawing.Size(103, 19);
            this.lblIbanTransferencia.TabIndex = 0;
            this.lblIbanTransferencia.Text = "IBAN da Conta:";
            // 
            // txtIbanTransferencia
            // 
            this.txtIbanTransferencia.BorderRadius = 6;
            this.txtIbanTransferencia.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIbanTransferencia.DefaultText = "";
            this.txtIbanTransferencia.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtIbanTransferencia.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtIbanTransferencia.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtIbanTransferencia.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtIbanTransferencia.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtIbanTransferencia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtIbanTransferencia.ForeColor = System.Drawing.Color.Black;
            this.txtIbanTransferencia.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtIbanTransferencia.Location = new System.Drawing.Point(10, 35);
            this.txtIbanTransferencia.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIbanTransferencia.Name = "txtIbanTransferencia";
            this.txtIbanTransferencia.PlaceholderText = "PT50 0035 0000 0000 0000 0000 0";
            this.txtIbanTransferencia.SelectedText = "";
            this.txtIbanTransferencia.Size = new System.Drawing.Size(360, 35);
            this.txtIbanTransferencia.TabIndex = 1;
            // 
            // panelApplePay
            // 
            this.panelApplePay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panelApplePay.BorderRadius = 8;
            this.panelApplePay.Controls.Add(this.lblAppleID);
            this.panelApplePay.Controls.Add(this.txtAppleID);
            this.panelApplePay.Location = new System.Drawing.Point(60, 150);
            this.panelApplePay.Name = "panelApplePay";
            this.panelApplePay.Size = new System.Drawing.Size(380, 100);
            this.panelApplePay.TabIndex = 6;
            this.panelApplePay.Visible = false;
            // 
            // lblAppleID
            // 
            this.lblAppleID.AutoSize = true;
            this.lblAppleID.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblAppleID.Location = new System.Drawing.Point(10, 10);
            this.lblAppleID.Name = "lblAppleID";
            this.lblAppleID.Size = new System.Drawing.Size(65, 19);
            this.lblAppleID.TabIndex = 0;
            this.lblAppleID.Text = "Apple ID:";
            // 
            // txtAppleID
            // 
            this.txtAppleID.BorderRadius = 6;
            this.txtAppleID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAppleID.DefaultText = "";
            this.txtAppleID.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAppleID.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAppleID.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAppleID.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAppleID.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAppleID.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAppleID.ForeColor = System.Drawing.Color.Black;
            this.txtAppleID.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAppleID.Location = new System.Drawing.Point(10, 35);
            this.txtAppleID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAppleID.Name = "txtAppleID";
            this.txtAppleID.PlaceholderText = "seu.email@icloud.com";
            this.txtAppleID.SelectedText = "";
            this.txtAppleID.Size = new System.Drawing.Size(360, 35);
            this.txtAppleID.TabIndex = 1;
            // 
            // btnSalvar
            // 
            this.btnSalvar.BorderRadius = 8;
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(60, 290);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(175, 40);
            this.btnSalvar.TabIndex = 7;
            this.btnSalvar.Text = "Salvar Alterações";
            this.btnSalvar.Click += new System.EventHandler(this.BtnSalvar_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.BorderRadius = 8;
            this.btnVoltar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(165)))), ((int)(((byte)(166)))));
            this.btnVoltar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnVoltar.ForeColor = System.Drawing.Color.White;
            this.btnVoltar.Location = new System.Drawing.Point(265, 290);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(175, 40);
            this.btnVoltar.TabIndex = 8;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.Click += new System.EventHandler(this.BtnVoltar_Click);
            // 
            // Carteira
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(500, 473);
            this.Controls.Add(this.panelEdicao);
            this.Controls.Add(this.panelCarteira);
            this.Controls.Add(this.panelAutenticacao);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Carteira";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Carteira Digital";
            this.Load += new System.EventHandler(this.Carteira_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelAutenticacao.ResumeLayout(false);
            this.panelAutenticacao.PerformLayout();
            this.panelCarteira.ResumeLayout(false);
            this.panelCarteira.PerformLayout();
            this.panelEdicao.ResumeLayout(false);
            this.panelEdicao.PerformLayout();
            this.panelPayPal.ResumeLayout(false);
            this.panelPayPal.PerformLayout();
            this.panelMBWay.ResumeLayout(false);
            this.panelMBWay.PerformLayout();
            this.panelTransferencia.ResumeLayout(false);
            this.panelTransferencia.PerformLayout();
            this.panelApplePay.ResumeLayout(false);
            this.panelApplePay.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // --- Header ---
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelHeader;
        private Guna.UI2.WinForms.Guna2Button btnFechar;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitulo;

        // --- Painéis Principais ---
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelAutenticacao;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelCarteira;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelEdicao;

        // --- Autenticação ---
        private System.Windows.Forms.Label lblTituloAuth;
        private System.Windows.Forms.Label lblPasswordLabel;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2Button btnEntrar;

        // --- Carteira ---
        private System.Windows.Forms.Label lblTituloCarteira;
        private System.Windows.Forms.Label lblSaldo;
        private Guna.UI2.WinForms.Guna2Button btnAlterarPagamento;

        // --- Edição ---
        private System.Windows.Forms.Label lblTituloEdicao;
        private System.Windows.Forms.Label lblSelecione;
        private Guna.UI2.WinForms.Guna2ComboBox cbMetodosPagamento;
        private Guna.UI2.WinForms.Guna2Button btnSalvar;
        private Guna.UI2.WinForms.Guna2Button btnVoltar;

        // --- PayPal ---
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelPayPal;
        private System.Windows.Forms.Label lblEmailPayPal;
        private Guna.UI2.WinForms.Guna2TextBox txtEmailPayPal;

        // --- MBWay ---
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelMBWay;
        private System.Windows.Forms.Label lblTelemovelMBWay;
        private Guna.UI2.WinForms.Guna2TextBox txtTelemovelMBWay;

        // --- Transferência ---
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelTransferencia;
        private System.Windows.Forms.Label lblIbanTransferencia;
        private Guna.UI2.WinForms.Guna2TextBox txtIbanTransferencia;

        // --- Apple Pay ---
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelApplePay;
        private System.Windows.Forms.Label lblAppleID;
        private Guna.UI2.WinForms.Guna2TextBox txtAppleID;
    }
}