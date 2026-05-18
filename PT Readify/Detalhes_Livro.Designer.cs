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
            this.components = new Container();
            this.pictureCapa = new PictureBox();
            this.table = new TableLayoutPanel();
            this.bottomPanel = new Panel();
            this.btnFechar = new Button();

            ((ISupportInitialize)(this.pictureCapa)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();

            // pictureCapa
            this.pictureCapa.Name = "pictureCapa";
            this.pictureCapa.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureCapa.Dock = DockStyle.Left;
            this.pictureCapa.Width = 220;
            this.pictureCapa.Margin = new Padding(8);

            // table
            this.table.Name = "table";
            this.table.Dock = DockStyle.Fill;
            this.table.ColumnCount = 2;
            this.table.ColumnStyles.Clear();
            this.table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.table.RowCount = 0;
            this.table.Padding = new Padding(8);
            this.table.AutoScroll = true;

            // bottomPanel
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Dock = DockStyle.Bottom;
            this.bottomPanel.Height = 40;
            this.bottomPanel.Padding = new Padding(6);

            // btnFechar
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Text = "Fechar";
            this.btnFechar.AutoSize = true;
            this.btnFechar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnFechar.Click += new EventHandler(this.BtnFechar_Click);

            // adicionar controls ao bottomPanel
            this.bottomPanel.Controls.Add(this.btnFechar);

            // adicionar controls ao form
            this.Controls.Add(this.table);
            this.Controls.Add(this.pictureCapa);
            this.Controls.Add(this.bottomPanel);

            // Eventos e propriedades do form
            this.Name = "Detalhes_Livro";
            this.ClientSize = new Size(800, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Load += new EventHandler(this.Detalhes_Livro_Load);
            this.bottomPanel.Resize += new EventHandler(this.BottomPanel_Resize);

            ((ISupportInitialize)(this.pictureCapa)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}