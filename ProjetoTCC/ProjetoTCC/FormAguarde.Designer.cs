namespace ProjetoTCC
{
    partial class FormAguarde
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
            this.lbAguarde = new System.Windows.Forms.Label();
            this.lbMsg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbAguarde
            // 
            this.lbAguarde.AutoSize = true;
            this.lbAguarde.Location = new System.Drawing.Point(80, 9);
            this.lbAguarde.Name = "lbAguarde";
            this.lbAguarde.Size = new System.Drawing.Size(186, 17);
            this.lbAguarde.TabIndex = 0;
            this.lbAguarde.Text = "Aguarde alguns segundos...";
            // 
            // lbMsg
            // 
            this.lbMsg.AutoSize = true;
            this.lbMsg.Location = new System.Drawing.Point(12, 56);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(310, 17);
            this.lbMsg.TabIndex = 1;
            this.lbMsg.Text = "Verificando conexão com dispositivo MindWave.";
            // 
            // FormAguarde
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 130);
            this.Controls.Add(this.lbMsg);
            this.Controls.Add(this.lbAguarde);
            this.Name = "FormAguarde";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbAguarde;
        private System.Windows.Forms.Label lbMsg;
    }
}