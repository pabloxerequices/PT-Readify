namespace PT_Readify
{
    partial class Carrinho
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.dataGridViewCarrinho = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.labelTotal = new System.Windows.Forms.Label();
            this.btnComprar = new Guna.UI2.WinForms.Guna2Button();
            this.btnReservar = new Guna.UI2.WinForms.Guna2Button();
            this.btnEmprestar = new Guna.UI2.WinForms.Guna2Button();
            this.btnFinalizarCompra = new Guna.UI2.WinForms.Guna2Button();
            this.btnLimparCarrinho = new Guna.UI2.WinForms.Guna2Button();
            this.panelTop.SuspendLayout();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCarrinho)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(41)))), ((int)(((byte)(52)))));
            this.panelTop.Controls.Add(this.labelTitulo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1000, 60);
            this.panelTop.TabIndex = 0;
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.ForeColor = System.Drawing.Color.White;
            this.labelTitulo.Location = new System.Drawing.Point(20, 12);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(173, 37);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "🛒 Carrinho";
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.panelContent.Controls.Add(this.dataGridViewCarrinho);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 60);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(15);
            this.panelContent.Size = new System.Drawing.Size(1000, 400);
            this.panelContent.TabIndex = 1;
            // 
            // dataGridViewCarrinho
            // 
            this.dataGridViewCarrinho.AllowUserToAddRows = false;
            this.dataGridViewCarrinho.AllowUserToDeleteRows = false;
            this.dataGridViewCarrinho.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewCarrinho.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewCarrinho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCarrinho.Location = new System.Drawing.Point(15, 15);
            this.dataGridViewCarrinho.Name = "dataGridViewCarrinho";
            this.dataGridViewCarrinho.RowHeadersVisible = false;
            this.dataGridViewCarrinho.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCarrinho.Size = new System.Drawing.Size(970, 370);
            this.dataGridViewCarrinho.TabIndex = 0;
            this.dataGridViewCarrinho.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCarrinho_CellContentClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(41)))), ((int)(((byte)(52)))));
            this.panelBottom.Controls.Add(this.labelTotal);
            this.panelBottom.Controls.Add(this.btnComprar);
            this.panelBottom.Controls.Add(this.btnReservar);
            this.panelBottom.Controls.Add(this.btnEmprestar);
            this.panelBottom.Controls.Add(this.btnFinalizarCompra);
            this.panelBottom.Controls.Add(this.btnLimparCarrinho);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 460);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1000, 80);
            this.panelBottom.TabIndex = 2;
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTotal.ForeColor = System.Drawing.Color.White;
            this.labelTotal.Location = new System.Drawing.Point(20, 25);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(114, 25);
            this.labelTotal.TabIndex = 0;
            this.labelTotal.Text = "Total: €0,00";
            // 
            // btnComprar
            // 
            this.btnComprar.BorderRadius = 6;
            this.btnComprar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnComprar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnComprar.ForeColor = System.Drawing.Color.White;
            this.btnComprar.Location = new System.Drawing.Point(380, 20);
            this.btnComprar.Name = "btnComprar";
            this.btnComprar.Size = new System.Drawing.Size(110, 40);
            this.btnComprar.TabIndex = 1;
            this.btnComprar.Text = "Comprar";
            this.btnComprar.Click += new System.EventHandler(this.btnComprar_Click);
            // 
            // btnReservar
            // 
            this.btnReservar.BorderRadius = 6;
            this.btnReservar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnReservar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReservar.ForeColor = System.Drawing.Color.White;
            this.btnReservar.Location = new System.Drawing.Point(500, 20);
            this.btnReservar.Name = "btnReservar";
            this.btnReservar.Size = new System.Drawing.Size(110, 40);
            this.btnReservar.TabIndex = 2;
            this.btnReservar.Text = "Reservar";
            this.btnReservar.Click += new System.EventHandler(this.btnReservar_Click);
            // 
            // btnEmprestar
            // 
            this.btnEmprestar.BorderRadius = 6;
            this.btnEmprestar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnEmprestar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEmprestar.ForeColor = System.Drawing.Color.White;
            this.btnEmprestar.Location = new System.Drawing.Point(620, 20);
            this.btnEmprestar.Name = "btnEmprestar";
            this.btnEmprestar.Size = new System.Drawing.Size(110, 40);
            this.btnEmprestar.TabIndex = 3;
            this.btnEmprestar.Text = "Emprestar";
            this.btnEmprestar.Click += new System.EventHandler(this.btnEmprestar_Click);
            // 
            // btnFinalizarCompra
            // 
            this.btnFinalizarCompra.BorderRadius = 6;
            this.btnFinalizarCompra.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.btnFinalizarCompra.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnFinalizarCompra.ForeColor = System.Drawing.Color.White;
            this.btnFinalizarCompra.Location = new System.Drawing.Point(740, 20);
            this.btnFinalizarCompra.Name = "btnFinalizarCompra";
            this.btnFinalizarCompra.Size = new System.Drawing.Size(110, 40);
            this.btnFinalizarCompra.TabIndex = 4;
            this.btnFinalizarCompra.Text = "Confirmar";
            this.btnFinalizarCompra.Click += new System.EventHandler(this.btnFinalizarCompra_Click);
            // 
            // btnLimparCarrinho
            // 
            this.btnLimparCarrinho.BorderRadius = 6;
            this.btnLimparCarrinho.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLimparCarrinho.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLimparCarrinho.ForeColor = System.Drawing.Color.White;
            this.btnLimparCarrinho.Location = new System.Drawing.Point(860, 20);
            this.btnLimparCarrinho.Name = "btnLimparCarrinho";
            this.btnLimparCarrinho.Size = new System.Drawing.Size(120, 40);
            this.btnLimparCarrinho.TabIndex = 5;
            this.btnLimparCarrinho.Text = "Limpar";
            this.btnLimparCarrinho.Click += new System.EventHandler(this.btnLimparCarrinho_Click);
            // 
            // Carrinho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1000, 540);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.MinimumSize = new System.Drawing.Size(1000, 540);
            this.Name = "Carrinho";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Carrinho de Compras - PT Readify";
            this.Load += new System.EventHandler(this.Carrinho_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCarrinho)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.DataGridView dataGridViewCarrinho;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label labelTotal;
        private Guna.UI2.WinForms.Guna2Button btnComprar;
        private Guna.UI2.WinForms.Guna2Button btnReservar;
        private Guna.UI2.WinForms.Guna2Button btnEmprestar;
        private Guna.UI2.WinForms.Guna2Button btnFinalizarCompra;
        private Guna.UI2.WinForms.Guna2Button btnLimparCarrinho;
    }
}