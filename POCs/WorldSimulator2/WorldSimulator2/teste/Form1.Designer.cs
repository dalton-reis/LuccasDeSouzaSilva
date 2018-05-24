namespace WorldSimulator
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlGraf = new System.Windows.Forms.Panel();
            this.btConfig = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.pnlSessao = new PainelSessao();
            this.pnlPaciente = new PainelPaciente();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlGraf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlGraf
            // 
            this.pnlGraf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlGraf.BackColor = System.Drawing.SystemColors.Control;
            this.pnlGraf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGraf.Controls.Add(this.pictureBox1);
            this.pnlGraf.Location = new System.Drawing.Point(12, 453);
            this.pnlGraf.Name = "pnlGraf";
            this.pnlGraf.Size = new System.Drawing.Size(592, 256);
            this.pnlGraf.TabIndex = 2;
            // 
            // btConfig
            // 
            this.btConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btConfig.Location = new System.Drawing.Point(726, 669);
            this.btConfig.Name = "btConfig";
            this.btConfig.Size = new System.Drawing.Size(100, 40);
            this.btConfig.TabIndex = 3;
            this.btConfig.Text = "Config";
            this.btConfig.UseVisualStyleBackColor = true;
            this.btConfig.Click += new System.EventHandler(this.btConfig_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(832, 669);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 40);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(938, 669);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 40);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(1044, 669);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 40);
            this.button4.TabIndex = 6;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.Location = new System.Drawing.Point(1150, 669);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 40);
            this.button5.TabIndex = 7;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // pnlSessao
            // 
            this.pnlSessao.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlSessao.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSessao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSessao.Location = new System.Drawing.Point(610, 12);
            this.pnlSessao.MinimumSize = new System.Drawing.Size(500, 2);
            this.pnlSessao.Name = "pnlSessao";
            this.pnlSessao.Size = new System.Drawing.Size(640, 651);
            this.pnlSessao.TabIndex = 1;
            // 
            // pnlPaciente
            // 
            this.pnlPaciente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlPaciente.BackColor = System.Drawing.SystemColors.Control;
            this.pnlPaciente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPaciente.Location = new System.Drawing.Point(12, 12);
            this.pnlPaciente.MinimumSize = new System.Drawing.Size(500, 2);
            this.pnlPaciente.Name = "pnlPaciente";
            this.pnlPaciente.Size = new System.Drawing.Size(592, 435);
            this.pnlPaciente.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(584, 248);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 721);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btConfig);
            this.Controls.Add(this.pnlGraf);
            this.Controls.Add(this.pnlSessao);
            this.Controls.Add(this.pnlPaciente);
            this.MinimumSize = new System.Drawing.Size(1280, 768);
            this.Name = "Form1";
            this.Text = "Form1";
            this.pnlGraf.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PainelPaciente pnlPaciente;
        private PainelSessao pnlSessao;
        private System.Windows.Forms.Panel pnlGraf;
        private System.Windows.Forms.Button btConfig;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

