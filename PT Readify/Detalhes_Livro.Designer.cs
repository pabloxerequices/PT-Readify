using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PT_Readify
{
    partial class Detalhes_Livro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private PictureBox pictureCapa;
        private Panel contentPanel;
        private TableLayoutPanel table;
        private Panel bottomPanel;
        private Button btnFechar;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (pictureCapa != null && pictureCapa.Image != null)
                {
                    pictureCapa.Image.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pictureCapa = new System.Windows.Forms.PictureBox();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.table = new System.Windows.Forms.TableLayoutPanel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnFechar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCapa)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureCapa
            // 
            this.pictureCapa.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pictureCapa.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureCapa.Location = new System.Drawing.Point(0, 0);
            this.pictureCapa.Name = "pictureCapa";
            this.pictureCapa.Padding = new System.Windows.Forms.Padding(8);
            this.pictureCapa.Size = new System.Drawing.Size(220, 452);
            this.pictureCapa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureCapa.TabIndex = 1;
            this.pictureCapa.TabStop = false;
            // 
            // contentPanel
            // 
            this.contentPanel.AutoScroll = true;
            this.contentPanel.Controls.Add(this.table);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(220, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(12);
            this.contentPanel.Size = new System.Drawing.Size(580, 452);
            this.contentPanel.TabIndex = 0;
            // 
            // table
            // 
            this.table.AutoSize = true;
            this.table.ColumnCount = 2;
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table.Dock = System.Windows.Forms.DockStyle.Top;
            this.table.Location = new System.Drawing.Point(12, 12);
            this.table.Name = "table";
            this.table.RowCount = 1;
            this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table.Size = new System.Drawing.Size(556, 0);
            this.table.TabIndex = 0;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.SystemColors.Control;
            this.bottomPanel.Controls.Add(this.btnFechar);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 452);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(8);
            this.bottomPanel.Size = new System.Drawing.Size(800, 48);
            this.bottomPanel.TabIndex = 2;
            this.bottomPanel.Resize += new System.EventHandler(this.BottomPanel_Resize);
            // 
            // btnFechar
            // 
            this.btnFechar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnFechar.AutoSize = true;
            this.btnFechar.Location = new System.Drawing.Point(600, -1);
            this.btnFechar.Margin = new System.Windows.Forms.Padding(6);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Padding = new System.Windows.Forms.Padding(6);
            this.btnFechar.Size = new System.Drawing.Size(75, 35);
            this.btnFechar.TabIndex = 0;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.Click += new System.EventHandler(this.BtnFechar_Click);
            // 
            // Detalhes_Livro
            // 
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.pictureCapa);
            this.Controls.Add(this.bottomPanel);
            this.Name = "Detalhes_Livro";
            ((System.ComponentModel.ISupportInitialize)(this.pictureCapa)).EndInit();
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}