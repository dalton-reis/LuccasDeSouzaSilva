namespace POC_TelaDupla
{
    partial class TelaMain
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
            this.btAbreSide = new System.Windows.Forms.Button();
            this.btFechaTela = new System.Windows.Forms.Button();
            this.lbCount = new System.Windows.Forms.Label();
            this.btCount = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btAbreSide
            // 
            this.btAbreSide.Location = new System.Drawing.Point(13, 13);
            this.btAbreSide.Name = "btAbreSide";
            this.btAbreSide.Size = new System.Drawing.Size(100, 23);
            this.btAbreSide.TabIndex = 0;
            this.btAbreSide.Text = "Abre Tela 2";
            this.btAbreSide.UseVisualStyleBackColor = true;
            this.btAbreSide.Click += new System.EventHandler(this.btAbreSide_Click);
            // 
            // btFechaTela
            // 
            this.btFechaTela.Location = new System.Drawing.Point(119, 13);
            this.btFechaTela.Name = "btFechaTela";
            this.btFechaTela.Size = new System.Drawing.Size(100, 23);
            this.btFechaTela.TabIndex = 1;
            this.btFechaTela.Text = "Fecha Tela 2";
            this.btFechaTela.UseVisualStyleBackColor = true;
            this.btFechaTela.Click += new System.EventHandler(this.btFechaTela_Click);
            // 
            // lbCount
            // 
            this.lbCount.AutoSize = true;
            this.lbCount.Location = new System.Drawing.Point(28, 120);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(16, 17);
            this.lbCount.TabIndex = 2;
            this.lbCount.Text = "0";
            // 
            // btCount
            // 
            this.btCount.Location = new System.Drawing.Point(31, 79);
            this.btCount.Name = "btCount";
            this.btCount.Size = new System.Drawing.Size(75, 23);
            this.btCount.TabIndex = 3;
            this.btCount.Text = "Count";
            this.btCount.UseVisualStyleBackColor = true;
            this.btCount.Click += new System.EventHandler(this.btCount_Click);
            // 
            // TelaMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.btCount);
            this.Controls.Add(this.lbCount);
            this.Controls.Add(this.btFechaTela);
            this.Controls.Add(this.btAbreSide);
            this.Name = "TelaMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btAbreSide;
        private System.Windows.Forms.Button btFechaTela;
        private System.Windows.Forms.Label lbCount;
        private System.Windows.Forms.Button btCount;
    }
}

