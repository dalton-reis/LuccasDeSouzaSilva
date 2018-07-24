namespace ProjetoTCC
{
    partial class FormEspecialistas
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
            this.pnlEspecialista = new ProjetoTCC.PainelEspecialista();
            this.SuspendLayout();
            // 
            // pnlEspecialista
            // 
            this.pnlEspecialista.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEspecialista.BackColor = System.Drawing.SystemColors.Control;
            this.pnlEspecialista.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEspecialista.Location = new System.Drawing.Point(12, 12);
            this.pnlEspecialista.Name = "pnlEspecialista";
            this.pnlEspecialista.Size = new System.Drawing.Size(911, 563);
            this.pnlEspecialista.TabIndex = 0;
            // 
            // FormEspecialistas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 480);
            this.Controls.Add(this.pnlEspecialista);
            this.Name = "FormEspecialistas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Especialistas";
            this.ResumeLayout(false);

        }

        #endregion

        private PainelEspecialista pnlEspecialista;
    }
}