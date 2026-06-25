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
        private Panel pnlHeader;
        private Label lblHeaderTitle;
        private Panel pnlBody;
        private Panel pnlCoverWrapper;
        private Panel pnlCoverFrame;
        private PictureBox pictureCapa;
        private Panel pnlDetailsScroll;
        private FlowLayoutPanel flowDetails;
        private Panel pnlFooter;
        private Button btnFechar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            if (disposing && pictureCapa?.Image != null)
            {
                pictureCapa.Image.Dispose();
                pictureCapa.Image = null;
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new Panel();
            this.lblHeaderTitle = new Label();
            this.pnlBody = new Panel();
            this.pnlDetailsScroll = new Panel();
            this.flowDetails = new FlowLayoutPanel();
            this.pnlCoverWrapper = new Panel();
            this.pnlCoverFrame = new Panel();
            this.pictureCapa = new PictureBox();
            this.pnlFooter = new Panel();
            this.btnFechar = new Button();

            this.pnlHeader.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlDetailsScroll.SuspendLayout();
            this.pnlCoverWrapper.SuspendLayout();
            this.pnlCoverFrame.SuspendLayout();
            ((ISupportInitialize)(this.pictureCapa)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = Color.FromArgb(45, 139, 150);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 58;
            this.pnlHeader.Padding = new Padding(20, 0, 20, 0);
            this.pnlHeader.Controls.Add(this.lblHeaderTitle);

            // lblHeaderTitle
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblHeaderTitle.ForeColor = Color.White;
            this.lblHeaderTitle.Location = new Point(20, 14);
            this.lblHeaderTitle.Text = "Detalhes do Livro";

            // pnlBody
            this.pnlBody.BackColor = Color.FromArgb(240, 240, 240);
            this.pnlBody.Dock = DockStyle.Fill;
            this.pnlBody.Padding = new Padding(16);
            this.pnlBody.Controls.Add(this.pnlDetailsScroll);
            this.pnlBody.Controls.Add(this.pnlCoverWrapper);

            // pnlCoverWrapper
            this.pnlCoverWrapper.BackColor = Color.FromArgb(240, 240, 240);
            this.pnlCoverWrapper.Dock = DockStyle.Left;
            this.pnlCoverWrapper.Width = 280;
            this.pnlCoverWrapper.Padding = new Padding(8, 8, 16, 8);
            this.pnlCoverWrapper.Controls.Add(this.pnlCoverFrame);

            // pnlCoverFrame
            this.pnlCoverFrame.BackColor = Color.FromArgb(146, 201, 217);
            this.pnlCoverFrame.Dock = DockStyle.Fill;
            this.pnlCoverFrame.Padding = new Padding(6);
            this.pnlCoverFrame.Controls.Add(this.pictureCapa);

            // pictureCapa
            this.pictureCapa.BackColor = Color.White;
            this.pictureCapa.Dock = DockStyle.Fill;
            this.pictureCapa.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureCapa.TabStop = false;

            // pnlDetailsScroll
            this.pnlDetailsScroll.AutoScroll = true;
            this.pnlDetailsScroll.BackColor = Color.White;
            this.pnlDetailsScroll.Dock = DockStyle.Fill;
            this.pnlDetailsScroll.Padding = new Padding(12);
            this.pnlDetailsScroll.Controls.Add(this.flowDetails);

            // flowDetails
            this.flowDetails.AutoSize = true;
            this.flowDetails.Dock = DockStyle.Top;
            this.flowDetails.FlowDirection = FlowDirection.TopDown;
            this.flowDetails.WrapContents = false;
            this.flowDetails.Padding = new Padding(4);
            this.flowDetails.Width = 480;

            // pnlFooter
            this.pnlFooter.BackColor = Color.FromArgb(166, 131, 100);
            this.pnlFooter.Dock = DockStyle.Bottom;
            this.pnlFooter.Height = 54;
            this.pnlFooter.Padding = new Padding(12, 10, 12, 10);
            this.pnlFooter.Controls.Add(this.btnFechar);

            // btnFechar
            this.btnFechar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnFechar.BackColor = Color.FromArgb(242, 192, 105);
            this.btnFechar.FlatAppearance.BorderSize = 0;
            this.btnFechar.FlatStyle = FlatStyle.Flat;
            this.btnFechar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnFechar.ForeColor = Color.FromArgb(60, 45, 30);
            this.btnFechar.Size = new Size(110, 34);
            this.btnFechar.Text = "Fechar";
            this.btnFechar.Cursor = Cursors.Hand;
            this.btnFechar.Click += new EventHandler(this.BtnFechar_Click);
            this.pnlFooter.Resize += new EventHandler(this.FooterPanel_Resize);

            // Detalhes_Livro
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.ClientSize = new Size(860, 560);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new Size(720, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Detalhes do Livro";
            this.Load += new EventHandler(this.Detalhes_Livro_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlBody.ResumeLayout(false);
            this.pnlDetailsScroll.ResumeLayout(false);
            this.pnlDetailsScroll.PerformLayout();
            this.pnlCoverWrapper.ResumeLayout(false);
            this.pnlCoverFrame.ResumeLayout(false);
            ((ISupportInitialize)(this.pictureCapa)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
