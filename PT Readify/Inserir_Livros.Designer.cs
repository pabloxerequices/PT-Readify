namespace PT_Readify
{
    partial class Inserir_Livros
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox3;

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;

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

        /// <summary>
        /// InitializeComponent - layout manual (sem TableLayoutPanel)
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();

            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();

            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();

            // 
            // Inserir_Livros (Form)
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1033, 577);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(900, 520);
            this.Name = "Inserir_Livros";
            this.Text = "Inserir Livro";

            // 
            // label1 - Nome
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nome:";

            // 
            // textBox1 - Nome
            // 
            this.textBox1.Location = new System.Drawing.Point(180, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(520, 23);
            this.textBox1.TabIndex = 1;

            // 
            // label4 - Biografia do livro
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Biografia do livro:";
            this.label4.Click += new System.EventHandler(this.label4_Click);

            // 
            // textBox2 - Biografia (multiline)
            // 
            this.textBox2.Location = new System.Drawing.Point(180, 66);
            this.textBox2.Multiline = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(520, 140);
            this.textBox2.TabIndex = 3;

            // 
            // label5 - Data de Lançamento
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Data de Lançamento:";

            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(180, 214);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 23);
            this.dateTimePicker1.TabIndex = 5;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);

            // 
            // label6 - Autor
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 260);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 15);
            this.label6.TabIndex = 6;
            this.label6.Text = "Autor:";

            // 
            // textBox4 - Autor
            // 
            this.textBox4.Location = new System.Drawing.Point(180, 256);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(520, 23);
            this.textBox4.TabIndex = 7;

            // 
            // label7 - Editora
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 300);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 15);
            this.label7.TabIndex = 8;
            this.label7.Text = "Editora:";

            // 
            // textBox5 - Editora
            // 
            this.textBox5.Location = new System.Drawing.Point(180, 296);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(520, 23);
            this.textBox5.TabIndex = 9;

            // 
            // label3 - Preço
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 340);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Preço:";

            // 
            // textBox3 - Preço
            // 
            this.textBox3.Location = new System.Drawing.Point(180, 336);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(200, 23);
            this.textBox3.TabIndex = 11;

            // 
            // pictureBox1 - capa do livro (direita)
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(732, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(252, 340);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;

            // 
            // label2 - Capa (direita)
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(732, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Capa:";

            // 
            // button1 - Inserir Livro (direita, em baixo da capa)
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(45, 125, 255);
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(732, 390);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(252, 60);
            this.button1.TabIndex = 14;
            this.button1.Text = "Inserir Livro";
            this.button1.UseVisualStyleBackColor = false;

            // 
            // Add controls to form
            // 
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}