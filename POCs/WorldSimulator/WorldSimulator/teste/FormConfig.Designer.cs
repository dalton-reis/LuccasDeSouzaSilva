namespace WorldSimulator
{
    partial class FormConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbFileFolder = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbFileLogo = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lbCamera = new System.Windows.Forms.ListBox();
            this.tbNomeEstab = new System.Windows.Forms.TextBox();
            this.tbEnderecoEstab = new System.Windows.Forms.TextBox();
            this.tbFoneEstab = new System.Windows.Forms.TextBox();
            this.btFileChooser = new System.Windows.Forms.Button();
            this.btAtualizaCamera = new System.Windows.Forms.Button();
            this.btSalvar = new System.Windows.Forms.Button();
            this.btFIleLogo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(91, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Camera padrão:";
            // 
            // lbFileFolder
            // 
            this.lbFileFolder.AutoSize = true;
            this.lbFileFolder.Location = new System.Drawing.Point(52, 39);
            this.lbFileFolder.Name = "lbFileFolder";
            this.lbFileFolder.Size = new System.Drawing.Size(131, 17);
            this.lbFileFolder.TabIndex = 1;
            this.lbFileFolder.Text = "Local dos arquivos:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nome:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(212, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Informações do estabelecimento";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Endereço:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 235);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Telefone:";
            // 
            // lbFileLogo
            // 
            this.lbFileLogo.AutoSize = true;
            this.lbFileLogo.Location = new System.Drawing.Point(47, 274);
            this.lbFileLogo.Name = "lbFileLogo";
            this.lbFileLogo.Size = new System.Drawing.Size(130, 17);
            this.lbFileLogo.TabIndex = 6;
            this.lbFileLogo.Text = "Imagem para Logo:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "Informações técnicas";
            // 
            // pbLogo
            // 
            this.pbLogo.Location = new System.Drawing.Point(10, 300);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(347, 129);
            this.pbLogo.TabIndex = 8;
            this.pbLogo.TabStop = false;
            // 
            // lbCamera
            // 
            this.lbCamera.FormattingEnabled = true;
            this.lbCamera.ItemHeight = 16;
            this.lbCamera.Location = new System.Drawing.Point(207, 84);
            this.lbCamera.Name = "lbCamera";
            this.lbCamera.Size = new System.Drawing.Size(262, 20);
            this.lbCamera.TabIndex = 9;
            // 
            // tbNomeEstab
            // 
            this.tbNomeEstab.Location = new System.Drawing.Point(67, 174);
            this.tbNomeEstab.Name = "tbNomeEstab";
            this.tbNomeEstab.Size = new System.Drawing.Size(295, 22);
            this.tbNomeEstab.TabIndex = 10;
            // 
            // tbEnderecoEstab
            // 
            this.tbEnderecoEstab.Location = new System.Drawing.Point(91, 204);
            this.tbEnderecoEstab.Name = "tbEnderecoEstab";
            this.tbEnderecoEstab.Size = new System.Drawing.Size(271, 22);
            this.tbEnderecoEstab.TabIndex = 11;
            // 
            // tbFoneEstab
            // 
            this.tbFoneEstab.Location = new System.Drawing.Point(86, 235);
            this.tbFoneEstab.Name = "tbFoneEstab";
            this.tbFoneEstab.Size = new System.Drawing.Size(226, 22);
            this.tbFoneEstab.TabIndex = 12;
            // 
            // btFileChooser
            // 
            this.btFileChooser.Location = new System.Drawing.Point(15, 36);
            this.btFileChooser.Name = "btFileChooser";
            this.btFileChooser.Size = new System.Drawing.Size(31, 23);
            this.btFileChooser.TabIndex = 13;
            this.btFileChooser.Text = "...";
            this.btFileChooser.UseVisualStyleBackColor = true;
            this.btFileChooser.Click += new System.EventHandler(this.btFileChooser_Click);
            // 
            // btAtualizaCamera
            // 
            this.btAtualizaCamera.Location = new System.Drawing.Point(10, 81);
            this.btAtualizaCamera.Name = "btAtualizaCamera";
            this.btAtualizaCamera.Size = new System.Drawing.Size(75, 23);
            this.btAtualizaCamera.TabIndex = 14;
            this.btAtualizaCamera.Text = "Atualizar";
            this.btAtualizaCamera.UseVisualStyleBackColor = true;
            this.btAtualizaCamera.Click += new System.EventHandler(this.btAtualizaCamera_Click);
            // 
            // btSalvar
            // 
            this.btSalvar.Location = new System.Drawing.Point(506, 410);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(75, 23);
            this.btSalvar.TabIndex = 15;
            this.btSalvar.Text = "Salvar";
            this.btSalvar.UseVisualStyleBackColor = true;
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // btFIleLogo
            // 
            this.btFIleLogo.Location = new System.Drawing.Point(10, 271);
            this.btFIleLogo.Name = "btFIleLogo";
            this.btFIleLogo.Size = new System.Drawing.Size(31, 23);
            this.btFIleLogo.TabIndex = 16;
            this.btFIleLogo.Text = "...";
            this.btFIleLogo.UseVisualStyleBackColor = true;
            this.btFIleLogo.Click += new System.EventHandler(this.btFIleLogo_Click);
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 445);
            this.Controls.Add(this.btFIleLogo);
            this.Controls.Add(this.btSalvar);
            this.Controls.Add(this.btAtualizaCamera);
            this.Controls.Add(this.btFileChooser);
            this.Controls.Add(this.tbFoneEstab);
            this.Controls.Add(this.tbEnderecoEstab);
            this.Controls.Add(this.tbNomeEstab);
            this.Controls.Add(this.lbCamera);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lbFileLogo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbFileFolder);
            this.Controls.Add(this.label1);
            this.Name = "FormConfig";
            this.Text = "FormConfig";
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbFileFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbFileLogo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ListBox lbCamera;
        private System.Windows.Forms.TextBox tbNomeEstab;
        private System.Windows.Forms.TextBox tbEnderecoEstab;
        private System.Windows.Forms.TextBox tbFoneEstab;
        private System.Windows.Forms.Button btFileChooser;
        private System.Windows.Forms.Button btAtualizaCamera;
        private System.Windows.Forms.Button btSalvar;
        private System.Windows.Forms.Button btFIleLogo;
    }
}