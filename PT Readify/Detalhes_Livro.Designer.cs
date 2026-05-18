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
            this.components = new Container();
            this.pictureCapa = new PictureBox();
            this.contentPanel = new Panel();
            this.table = new TableLayoutPanel();
            this.bottomPanel = new Panel();
            this.btnFechar = new Button();

            // pictureCapa
            this.pictureCapa.Name = "pictureCapa";
            this.pictureCapa.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureCapa.Dock = DockStyle.Left;
            this.pictureCapa.Width = 220;
            this.pictureCapa.Padding = new Padding(8);
            this.pictureCapa.BorderStyle = BorderStyle.None;
            this.pictureCapa.BackColor = SystemColors.ControlLight;

            // contentPanel
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.AutoScroll = true;
            this.contentPanel.Padding = new Padding(12);

            // table
            this.table.Name = "table";
            this.table.AutoSize = true;
            this.table.Dock = DockStyle.Top;
            this.table.ColumnCount = 2;
            this.table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            // Designer requires RowCount > 0 — usar 1 como valor inicial
            this.table.RowCount = 1;
            this.table.RowStyles.Clear();
            this.table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.table.GrowStyle = TableLayoutPanelGrowStyle.AddRows;

            // btnFechar
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Text = "Fechar";
            this.btnFechar.Anchor = AnchorStyles.Right;
            this.btnFechar.AutoSize = true;
            this.btnFechar.Padding = new Padding(6);
            this.btnFechar.Margin = new Padding(6);
            this.btnFechar.Click += new System.EventHandler(this.BtnFechar_Click);

            // bottomPanel
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Dock = DockStyle.Bottom;
            this.bottomPanel.Height = 48;
            this.bottomPanel.Padding = new Padding(8);
            this.bottomPanel.BackColor = SystemColors.Control;
            this.bottomPanel.Controls.Add(this.btnFechar);

            // Evitar lambdas no Designer — usar handler nomeado
            this.bottomPanel.Resize += new System.EventHandler(this.BottomPanel_Resize);

            // montar hierarchy
            this.contentPanel.Controls.Add(this.table);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.pictureCapa);
            this.Controls.Add(this.bottomPanel);

            // Form
            this.ClientSize = new Size(800, 500);
            this.Name = "Detalhes_Livro";
        }

        #endregion
    }
}