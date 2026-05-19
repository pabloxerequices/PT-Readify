// Designer gerado manualmente para Detalhes_Livro
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PT_Readify
{
    partial class Detalhes_Livro
    {
        private IContainer components = null;
        private PictureBox pictureCapa;
        private TableLayoutPanel table;
        private Panel bottomPanel;
        private Button btnFechar;

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
            this.pictureCapa = new System.Windows.Forms.PictureBox();
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnFechar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCapa)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureCapa
            // 
            this.pictureCapa.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureCapa.Location = new System.Drawing.Point(0, 0);
            this.pictureCapa.Margin = new System.Windows.Forms.Padding(8);
            this.pictureCapa.Name = "pictureCapa";
            this.pictureCapa.Size = new System.Drawing.Size(220, 410);
            this.pictureCapa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureCapa.TabIndex = 1;
            this.pictureCapa.TabStop = false;
            // 
            // table
            // 
            this.table.AutoScroll = true;
            this.table.ColumnCount = 2;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(220, 0);
            this.table.Name = "table";
            this.table.Padding = new System.Windows.Forms.Padding(8);
            this.table.RowCount = 1;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.table.Size = new System.Drawing.Size(580, 410);
            this.table.TabIndex = 0;
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.btnFechar);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 410);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(6);
            this.bottomPanel.Size = new System.Drawing.Size(800, 40);
            this.bottomPanel.TabIndex = 2;
            this.bottomPanel.Resize += new System.EventHandler(this.BottomPanel_Resize);
            // 
            // btnFechar
            // 
            this.btnFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFechar.AutoSize = true;
            this.btnFechar.Location = new System.Drawing.Point(600, 0);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 0;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.Click += new System.EventHandler(this.BtnFechar_Click);
            // 
            // Detalhes_Livro
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.table);
            this.Controls.Add(this.pictureCapa);
            this.Controls.Add(this.bottomPanel);
            this.Name = "Detalhes_Livro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.Detalhes_Livro_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureCapa)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}