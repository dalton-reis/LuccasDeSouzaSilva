namespace ProjetoTCC
{
    partial class FormCamera
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
            this.pbCamera = new System.Windows.Forms.PictureBox();
            this.btCapturar = new System.Windows.Forms.Button();
            this.pnlFoto = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCamera
            // 
            this.pbCamera.Location = new System.Drawing.Point(12, 12);
            this.pbCamera.Name = "pbCamera";
            this.pbCamera.Size = new System.Drawing.Size(500, 500);
            this.pbCamera.TabIndex = 0;
            this.pbCamera.TabStop = false;
            // 
            // btCapturar
            // 
            this.btCapturar.Location = new System.Drawing.Point(518, 12);
            this.btCapturar.Name = "btCapturar";
            this.btCapturar.Size = new System.Drawing.Size(88, 30);
            this.btCapturar.TabIndex = 1;
            this.btCapturar.Text = "Tirar foto";
            this.btCapturar.UseVisualStyleBackColor = true;
            this.btCapturar.Click += new System.EventHandler(this.btCapturar_Click);
            // 
            // pnlFoto
            // 
            this.pnlFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFoto.Location = new System.Drawing.Point(12, 12);
            this.pnlFoto.Name = "pnlFoto";
            this.pnlFoto.Size = new System.Drawing.Size(270, 270);
            this.pnlFoto.TabIndex = 0;
            // 
            // FormCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 522);
            this.Controls.Add(this.btCapturar);
            this.Controls.Add(this.pbCamera);
            this.Controls.Add(this.pnlFoto);
            this.Name = "FormCamera";
            ((System.ComponentModel.ISupportInitialize)(this.pbCamera)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCamera;
        private System.Windows.Forms.Panel pnlFoto;
        private System.Windows.Forms.Button btCapturar;
    }
}