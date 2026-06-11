namespace PT_Readify
{
    partial class Hstórico_de_compras
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
            this.panelContent = new System.Windows.Forms.Panel();
            this.dataGridViewHistorico_Compras = new System.Windows.Forms.DataGridView();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.labelTotal = new System.Windows.Forms.Label();
            this.btnAdicionarLivro = new Guna.UI2.WinForms.Guna2Button();
            this.btnFinalizarCompra = new Guna.UI2.WinForms.Guna2Button();
            this.btnLimparCarrinho = new Guna.UI2.WinForms.Guna2Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTitulo = new System.Windows.Forms.Label();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistorico_Compras)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.panelContent.Controls.Add(this.dataGridViewHistorico_Compras);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 60);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(15);
            this.panelContent.Size = new System.Drawing.Size(1073, 439);
            this.panelContent.TabIndex = 4;
            // 
            // dataGridViewHistorico_Compras
            // 
            this.dataGridViewHistorico_Compras.AllowUserToAddRows = false;
            this.dataGridViewHistorico_Compras.AllowUserToDeleteRows = false;
            this.dataGridViewHistorico_Compras.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewHistorico_Compras.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewHistorico_Compras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewHistorico_Compras.Location = new System.Drawing.Point(15, 15);
            this.dataGridViewHistorico_Compras.Name = "dataGridViewHistorico_Compras";
            this.dataGridViewHistorico_Compras.RowHeadersVisible = false;
            this.dataGridViewHistorico_Compras.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewHistorico_Compras.Size = new System.Drawing.Size(1043, 409);
            this.dataGridViewHistorico_Compras.TabIndex = 0;
            this.dataGridViewHistorico_Compras.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCarrinho_CellContentClick);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(41)))), ((int)(((byte)(52)))));
            this.panelBottom.Controls.Add(this.labelTotal);
            this.panelBottom.Controls.Add(this.btnAdicionarLivro);
            this.panelBottom.Controls.Add(this.btnFinalizarCompra);
            this.panelBottom.Controls.Add(this.btnLimparCarrinho);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 499);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1073, 80);
            this.panelBottom.TabIndex = 5;
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTotal.ForeColor = System.Drawing.Color.White;
            this.labelTotal.Location = new System.Drawing.Point(20, 25);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(0, 25);
            this.labelTotal.TabIndex = 0;
            // 
            // btnAdicionarLivro
            // 
            this.btnAdicionarLivro.BorderRadius = 6;
            this.btnAdicionarLivro.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnAdicionarLivro.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdicionarLivro.ForeColor = System.Drawing.Color.White;
            this.btnAdicionarLivro.Location = new System.Drawing.Point(72, 20);
            this.btnAdicionarLivro.Name = "btnAdicionarLivro";
            this.btnAdicionarLivro.Size = new System.Drawing.Size(193, 40);
            this.btnAdicionarLivro.TabIndex = 1;
            this.btnAdicionarLivro.Text = "+ Adicionar";
            // 
            // btnFinalizarCompra
            // 
            this.btnFinalizarCompra.BorderRadius = 6;
            this.btnFinalizarCompra.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnFinalizarCompra.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnFinalizarCompra.ForeColor = System.Drawing.Color.White;
            this.btnFinalizarCompra.Location = new System.Drawing.Point(466, 20);
            this.btnFinalizarCompra.Name = "btnFinalizarCompra";
            this.btnFinalizarCompra.Size = new System.Drawing.Size(213, 40);
            this.btnFinalizarCompra.TabIndex = 2;
            this.btnFinalizarCompra.Text = "✓ Listar por  Datas";
            // 
            // btnLimparCarrinho
            // 
            this.btnLimparCarrinho.BorderRadius = 6;
            this.btnLimparCarrinho.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnLimparCarrinho.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLimparCarrinho.ForeColor = System.Drawing.Color.White;
            this.btnLimparCarrinho.Location = new System.Drawing.Point(789, 20);
            this.btnLimparCarrinho.Name = "btnLimparCarrinho";
            this.btnLimparCarrinho.Size = new System.Drawing.Size(220, 40);
            this.btnLimparCarrinho.TabIndex = 3;
            this.btnLimparCarrinho.Text = "🗑 Devolução da Compra";
            this.btnLimparCarrinho.Click += new System.EventHandler(this.btnLimparCarrinho_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(41)))), ((int)(((byte)(52)))));
            this.panelTop.Controls.Add(this.labelTitulo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1073, 60);
            this.panelTop.TabIndex = 3;
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitulo.ForeColor = System.Drawing.Color.White;
            this.labelTitulo.Location = new System.Drawing.Point(7, 9);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(382, 45);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "⟳ Histórico de Compras";
            // 
            // Hstórico_de_compras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 579);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "Hstórico_de_compras";
            this.Text = "Hstórico_de_compras";
            this.Load += new System.EventHandler(this.Hstórico_de_compras_Load);
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistorico_Compras)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.DataGridView dataGridViewHistorico_Compras;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label labelTotal;
        private Guna.UI2.WinForms.Guna2Button btnAdicionarLivro;
        private Guna.UI2.WinForms.Guna2Button btnFinalizarCompra;
        private Guna.UI2.WinForms.Guna2Button btnLimparCarrinho;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTitulo;
    }
}