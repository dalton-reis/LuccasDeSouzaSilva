namespace ProjetoTCC
{
    partial class FormAlteraFoto
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
            this.btEscolheArquivo = new System.Windows.Forms.Button();
            this.btCamera = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btEscolheArquivo
            // 
            this.btEscolheArquivo.Location = new System.Drawing.Point(12, 12);
            this.btEscolheArquivo.Name = "btEscolheArquivo";
            this.btEscolheArquivo.Size = new System.Drawing.Size(125, 25);
            this.btEscolheArquivo.TabIndex = 0;
            this.btEscolheArquivo.Text = "Escolher arquivo";
            this.btEscolheArquivo.UseVisualStyleBackColor = true;
            this.btEscolheArquivo.Click += new System.EventHandler(this.btEscolheArquivo_Click);
            // 
            // btCamera
            // 
            this.btCamera.Location = new System.Drawing.Point(144, 12);
            this.btCamera.Name = "btCamera";
            this.btCamera.Size = new System.Drawing.Size(75, 25);
            this.btCamera.TabIndex = 1;
            this.btCamera.Text = "Câmera";
            this.btCamera.UseVisualStyleBackColor = true;
            this.btCamera.Click += new System.EventHandler(this.btCamera_Click);
            // 
            // FormAlteraFoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 49);
            this.Controls.Add(this.btCamera);
            this.Controls.Add(this.btEscolheArquivo);
            this.Name = "FormAlteraFoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btEscolheArquivo;
        private System.Windows.Forms.Button btCamera;
    }
}