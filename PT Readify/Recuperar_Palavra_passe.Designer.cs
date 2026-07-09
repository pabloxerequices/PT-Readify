namespace PT_Readify
{
    partial class Recuperar_Palavra_passe
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
            this.panelEmail = new System.Windows.Forms.Panel();
            this.btnEnviarCodigo = new System.Windows.Forms.Button();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.panelCodigo = new System.Windows.Forms.Panel();
            this.btnVerificarCodigo = new System.Windows.Forms.Button();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.btnReenviarCodigo = new System.Windows.Forms.Button();
            this.panelNovaPassword = new System.Windows.Forms.Panel();
            this.btnAlterarPassword = new System.Windows.Forms.Button();
            this.txtConfirmarPassword = new System.Windows.Forms.TextBox();
            this.txtNovaPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmarPassword = new System.Windows.Forms.Label();
            this.lblNovaPassword = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnVoltarCodigo = new System.Windows.Forms.Button();
            this.btnVoltarEmail = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lblPassoAtual = new System.Windows.Forms.Label();
            this.lblPasso1 = new System.Windows.Forms.Label();
            this.lblPasso2 = new System.Windows.Forms.Label();
            this.lblPasso3 = new System.Windows.Forms.Label();
            this.panelEmail.SuspendLayout();
            this.panelCodigo.SuspendLayout();
            this.panelNovaPassword.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEmail
            // 
            this.panelEmail.BackColor = System.Drawing.Color.White;
            this.panelEmail.Controls.Add(this.btnEnviarCodigo);
            this.panelEmail.Controls.Add(this.txtEmail);
            this.panelEmail.Controls.Add(this.lblEmail);
            this.panelEmail.Location = new System.Drawing.Point(30, 110);
            this.panelEmail.Name = "panelEmail";
            this.panelEmail.Size = new System.Drawing.Size(340, 130);
            this.panelEmail.TabIndex = 0;
            // 
            // btnEnviarCodigo
            // 
            this.btnEnviarCodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnEnviarCodigo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviarCodigo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviarCodigo.ForeColor = System.Drawing.Color.White;
            this.btnEnviarCodigo.Location = new System.Drawing.Point(20, 90);
            this.btnEnviarCodigo.Name = "btnEnviarCodigo";
            this.btnEnviarCodigo.Size = new System.Drawing.Size(300, 40);
            this.btnEnviarCodigo.TabIndex = 2;
            this.btnEnviarCodigo.Text = "Enviar Código";
            this.btnEnviarCodigo.UseVisualStyleBackColor = false;
            this.btnEnviarCodigo.Click += new System.EventHandler(this.BtnEnviarCodigo_Click);
            // 
            // txtEmail
            // 
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(20, 55);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(300, 27);
            this.txtEmail.TabIndex = 1;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblEmail.Location = new System.Drawing.Point(20, 25);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(197, 19);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Introduza o seu email:";
            // 
            // panelCodigo
            // 
            this.panelCodigo.BackColor = System.Drawing.Color.White;
            this.panelCodigo.Controls.Add(this.btnReenviarCodigo);
            this.panelCodigo.Controls.Add(this.btnVerificarCodigo);
            this.panelCodigo.Controls.Add(this.txtCodigo);
            this.panelCodigo.Controls.Add(this.lblCodigo);
            this.panelCodigo.Location = new System.Drawing.Point(30, 110);
            this.panelCodigo.Name = "panelCodigo";
            this.panelCodigo.Size = new System.Drawing.Size(340, 130);
            this.panelCodigo.TabIndex = 1;
            this.panelCodigo.Visible = false;
            // 
            // btnVerificarCodigo
            // 
            this.btnVerificarCodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnVerificarCodigo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerificarCodigo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerificarCodigo.ForeColor = System.Drawing.Color.White;
            this.btnVerificarCodigo.Location = new System.Drawing.Point(20, 80);
            this.btnVerificarCodigo.Name = "btnVerificarCodigo";
            this.btnVerificarCodigo.Size = new System.Drawing.Size(300, 40);
            this.btnVerificarCodigo.TabIndex = 2;
            this.btnVerificarCodigo.Text = "Verificar Código";
            this.btnVerificarCodigo.UseVisualStyleBackColor = false;
            this.btnVerificarCodigo.Click += new System.EventHandler(this.BtnVerificarCodigo_Click);
            // 
            // txtCodigo
            // 
            this.txtCodigo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCodigo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigo.Location = new System.Drawing.Point(20, 45);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(300, 27);
            this.txtCodigo.TabIndex = 1;
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblCodigo.Location = new System.Drawing.Point(20, 15);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(241, 19);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Introduza o código recebido:";
            // 
            // btnReenviarCodigo
            // 
            this.btnReenviarCodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnReenviarCodigo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReenviarCodigo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReenviarCodigo.ForeColor = System.Drawing.Color.White;
            this.btnReenviarCodigo.Location = new System.Drawing.Point(20, 125);
            this.btnReenviarCodigo.Name = "btnReenviarCodigo";
            this.btnReenviarCodigo.Size = new System.Drawing.Size(300, 30);
            this.btnReenviarCodigo.TabIndex = 3;
            this.btnReenviarCodigo.Text = "Reenviar Código";
            this.btnReenviarCodigo.UseVisualStyleBackColor = false;
            this.btnReenviarCodigo.Click += new System.EventHandler(this.btnReenviarCodigo_Click);
            // 
            // panelNovaPassword
            // 
            this.panelNovaPassword.BackColor = System.Drawing.Color.White;
            this.panelNovaPassword.Controls.Add(this.btnAlterarPassword);
            this.panelNovaPassword.Controls.Add(this.txtConfirmarPassword);
            this.panelNovaPassword.Controls.Add(this.txtNovaPassword);
            this.panelNovaPassword.Controls.Add(this.lblConfirmarPassword);
            this.panelNovaPassword.Controls.Add(this.lblNovaPassword);
            this.panelNovaPassword.Location = new System.Drawing.Point(30, 110);
            this.panelNovaPassword.Name = "panelNovaPassword";
            this.panelNovaPassword.Size = new System.Drawing.Size(340, 180);
            this.panelNovaPassword.TabIndex = 2;
            this.panelNovaPassword.Visible = false;
            // 
            // btnAlterarPassword
            // 
            this.btnAlterarPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnAlterarPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlterarPassword.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlterarPassword.ForeColor = System.Drawing.Color.White;
            this.btnAlterarPassword.Location = new System.Drawing.Point(20, 140);
            this.btnAlterarPassword.Name = "btnAlterarPassword";
            this.btnAlterarPassword.Size = new System.Drawing.Size(300, 40);
            this.btnAlterarPassword.TabIndex = 4;
            this.btnAlterarPassword.Text = "Alterar Password";
            this.btnAlterarPassword.UseVisualStyleBackColor = false;
            this.btnAlterarPassword.Click += new System.EventHandler(this.BtnAlterarPassword_Click);
            // 
            // txtConfirmarPassword
            // 
            this.txtConfirmarPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfirmarPassword.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirmarPassword.Location = new System.Drawing.Point(20, 105);
            this.txtConfirmarPassword.Name = "txtConfirmarPassword";
            this.txtConfirmarPassword.PasswordChar = '*';
            this.txtConfirmarPassword.Size = new System.Drawing.Size(300, 27);
            this.txtConfirmarPassword.TabIndex = 3;
            // 
            // txtNovaPassword
            // 
            this.txtNovaPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNovaPassword.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNovaPassword.Location = new System.Drawing.Point(20, 65);
            this.txtNovaPassword.Name = "txtNovaPassword";
            this.txtNovaPassword.PasswordChar = '*';
            this.txtNovaPassword.Size = new System.Drawing.Size(300, 27);
            this.txtNovaPassword.TabIndex = 2;
            // 
            // lblConfirmarPassword
            // 
            this.lblConfirmarPassword.AutoSize = true;
            this.lblConfirmarPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmarPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblConfirmarPassword.Location = new System.Drawing.Point(20, 85);
            this.lblConfirmarPassword.Name = "lblConfirmarPassword";
            this.lblConfirmarPassword.Size = new System.Drawing.Size(197, 19);
            this.lblConfirmarPassword.TabIndex = 1;
            this.lblConfirmarPassword.Text = "Confirmar nova password:";
            // 
            // lblNovaPassword
            // 
            this.lblNovaPassword.AutoSize = true;
            this.lblNovaPassword.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNovaPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblNovaPassword.Location = new System.Drawing.Point(20, 25);
            this.lblNovaPassword.Name = "lblNovaPassword";
            this.lblNovaPassword.Size = new System.Drawing.Size(158, 19);
            this.lblNovaPassword.TabIndex = 0;
            this.lblNovaPassword.Text = "Nova password:";
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(220, 310);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(150, 40);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.BtnCancelar_Click);
            // 
            // btnVoltarCodigo
            // 
            this.btnVoltarCodigo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnVoltarCodigo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoltarCodigo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltarCodigo.ForeColor = System.Drawing.Color.White;
            this.btnVoltarCodigo.Location = new System.Drawing.Point(30, 310);
            this.btnVoltarCodigo.Name = "btnVoltarCodigo";
            this.btnVoltarCodigo.Size = new System.Drawing.Size(100, 40);
            this.btnVoltarCodigo.TabIndex = 4;
            this.btnVoltarCodigo.Text = "Voltar";
            this.btnVoltarCodigo.UseVisualStyleBackColor = false;
            this.btnVoltarCodigo.Click += new System.EventHandler(this.BtnVoltarCodigo_Click);
            // 
            // btnVoltarEmail
            // 
            this.btnVoltarEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnVoltarEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoltarEmail.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltarEmail.ForeColor = System.Drawing.Color.White;
            this.btnVoltarEmail.Location = new System.Drawing.Point(30, 310);
            this.btnVoltarEmail.Name = "btnVoltarEmail";
            this.btnVoltarEmail.Size = new System.Drawing.Size(100, 40);
            this.btnVoltarEmail.TabIndex = 3;
            this.btnVoltarEmail.Text = "Voltar";
            this.btnVoltarEmail.UseVisualStyleBackColor = false;
            this.btnVoltarEmail.Click += new System.EventHandler(this.BtnVoltarEmail_Click);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.White;
            this.panelMain.Controls.Add(this.lblPasso3);
            this.panelMain.Controls.Add(this.lblPasso2);
            this.panelMain.Controls.Add(this.lblPasso1);
            this.panelMain.Controls.Add(this.lblPassoAtual);
            this.panelMain.Controls.Add(this.lblTitulo);
            this.panelMain.Controls.Add(this.pictureBoxLogo);
            this.panelMain.Controls.Add(this.panelNovaPassword);
            this.panelMain.Controls.Add(this.panelCodigo);
            this.panelMain.Controls.Add(this.panelEmail);
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(400, 360);
            this.panelMain.TabIndex = 6;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblTitulo.Location = new System.Drawing.Point(140, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(220, 31);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Text = "Recuperar Password";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBoxLogo.Location = new System.Drawing.Point(30, 10);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(90, 50);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // lblPassoAtual
            // 
            this.lblPassoAtual.AutoSize = true;
            this.lblPassoAtual.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassoAtual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblPassoAtual.Location = new System.Drawing.Point(30, 70);
            this.lblPassoAtual.Name = "lblPassoAtual";
            this.lblPassoAtual.Size = new System.Drawing.Size(89, 19);
            this.lblPassoAtual.TabIndex = 2;
            this.lblPassoAtual.Text = "Passo 1 de 3";
            // 
            // lblPasso1
            // 
            this.lblPasso1.AutoSize = true;
            this.lblPasso1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasso1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblPasso1.Location = new System.Drawing.Point(30, 90);
            this.lblPasso1.Name = "lblPasso1";
            this.lblPasso1.Size = new System.Drawing.Size(80, 15);
            this.lblPasso1.TabIndex = 3;
            this.lblPasso1.Text = "● Email";
            // 
            // lblPasso2
            // 
            this.lblPasso2.AutoSize = true;
            this.lblPasso2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasso2.ForeColor = System.Drawing.Color.Gray;
            this.lblPasso2.Location = new System.Drawing.Point(130, 90);
            this.lblPasso2.Name = "lblPasso2";
            this.lblPasso2.Size = new System.Drawing.Size(95, 15);
            this.lblPasso2.TabIndex = 4;
            this.lblPasso2.Text = "○ Código";
            // 
            // lblPasso3
            // 
            this.lblPasso3.AutoSize = true;
            this.lblPasso3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPasso3.ForeColor = System.Drawing.Color.Gray;
            this.lblPasso3.Location = new System.Drawing.Point(240, 90);
            this.lblPasso3.Name = "lblPasso3";
            this.lblPasso3.Size = new System.Drawing.Size(105, 15);
            this.lblPasso3.TabIndex = 5;
            this.lblPasso3.Text = "○ Password";
            // 
            // Recuperar_Palavra_passe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(400, 360);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnVoltarCodigo);
            this.Controls.Add(this.btnVoltarEmail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Recuperar_Palavra_passe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Recuperar Password - Readify";
            this.Load += new System.EventHandler(this.Recuperar_Palavra_passe_Load);
            this.panelEmail.ResumeLayout(false);
            this.panelEmail.PerformLayout();
            this.panelCodigo.ResumeLayout(false);
            this.panelCodigo.PerformLayout();
            this.panelNovaPassword.ResumeLayout(false);
            this.panelNovaPassword.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnEnviarCodigo;
        private System.Windows.Forms.Panel panelCodigo;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Button btnVerificarCodigo;
        private System.Windows.Forms.Button btnReenviarCodigo;
        private System.Windows.Forms.Panel panelNovaPassword;
        private System.Windows.Forms.Label lblNovaPassword;
        private System.Windows.Forms.TextBox txtNovaPassword;
        private System.Windows.Forms.Label lblConfirmarPassword;
        private System.Windows.Forms.TextBox txtConfirmarPassword;
        private System.Windows.Forms.Button btnAlterarPassword;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnVoltarEmail;
        private System.Windows.Forms.Button btnVoltarCodigo;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.Label lblPassoAtual;
        private System.Windows.Forms.Label lblPasso1;
        private System.Windows.Forms.Label lblPasso2;
        private System.Windows.Forms.Label lblPasso3;
    }
}
