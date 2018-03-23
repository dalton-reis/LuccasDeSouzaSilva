namespace POC_TelaDupla
{
    partial class TelaSide
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
            this.lbText1 = new System.Windows.Forms.Label();
            this.lbText2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbText1
            // 
            this.lbText1.AutoSize = true;
            this.lbText1.Location = new System.Drawing.Point(13, 13);
            this.lbText1.Name = "lbText1";
            this.lbText1.Size = new System.Drawing.Size(21, 17);
            this.lbText1.TabIndex = 0;
            this.lbText1.Text = "A:";
            // 
            // lbText2
            // 
            this.lbText2.AutoSize = true;
            this.lbText2.Location = new System.Drawing.Point(12, 46);
            this.lbText2.Name = "lbText2";
            this.lbText2.Size = new System.Drawing.Size(21, 17);
            this.lbText2.TabIndex = 1;
            this.lbText2.Text = "B:";
            // 
            // TelaSide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.lbText2);
            this.Controls.Add(this.lbText1);
            this.Name = "TelaSide";
            this.Text = "TelaSide";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbText1;
        private System.Windows.Forms.Label lbText2;
    }
}