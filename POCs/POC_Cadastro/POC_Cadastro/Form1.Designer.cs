namespace WindowsFormsAlura
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
            this.lbNome = new System.Windows.Forms.Label();
            this.lbCpf = new System.Windows.Forms.Label();
            this.lbRg = new System.Windows.Forms.Label();
            this.lbTelefone1 = new System.Windows.Forms.Label();
            this.lbTelefone2 = new System.Windows.Forms.Label();
            this.lbDataNasc = new System.Windows.Forms.Label();
            this.tbNome = new System.Windows.Forms.TextBox();
            this.tbCpf = new System.Windows.Forms.TextBox();
            this.tbRg = new System.Windows.Forms.TextBox();
            this.tbTel2 = new System.Windows.Forms.TextBox();
            this.tbTel1 = new System.Windows.Forms.TextBox();
            this.btShow = new System.Windows.Forms.Button();
            this.btHide = new System.Windows.Forms.Button();
            this.lbUsuario = new System.Windows.Forms.Label();
            this.tbUsuario = new System.Windows.Forms.TextBox();
            this.btSalvar = new System.Windows.Forms.Button();
            this.dtDataNasc = new System.Windows.Forms.DateTimePicker();
            this.lbEmail = new System.Windows.Forms.Label();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.btAbrir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbNome
            // 
            this.lbNome.AutoSize = true;
            this.lbNome.Location = new System.Drawing.Point(10, 15);
            this.lbNome.Name = "lbNome";
            this.lbNome.Size = new System.Drawing.Size(49, 17);
            this.lbNome.Text = "Nome:";
            // 
            // lbCpf
            // 
            this.lbCpf.AutoSize = true;
            this.lbCpf.Location = new System.Drawing.Point(10, 43);
            this.lbCpf.Name = "lbCpf";
            this.lbCpf.Size = new System.Drawing.Size(33, 17);
            this.lbCpf.Text = "Cpf:";
            // 
            // lbRg
            // 
            this.lbRg.AutoSize = true;
            this.lbRg.Location = new System.Drawing.Point(10, 71);
            this.lbRg.Name = "lbRg";
            this.lbRg.Size = new System.Drawing.Size(30, 17);
            this.lbRg.Text = "Rg:";
            // 
            // lbTelefone1
            // 
            this.lbTelefone1.AutoSize = true;
            this.lbTelefone1.Location = new System.Drawing.Point(10, 99);
            this.lbTelefone1.Name = "lbTelefone1";
            this.lbTelefone1.Size = new System.Drawing.Size(80, 17);
            this.lbTelefone1.Text = "Telefone 1:";
            // 
            // lbTelefone2
            // 
            this.lbTelefone2.AutoSize = true;
            this.lbTelefone2.Location = new System.Drawing.Point(10, 127);
            this.lbTelefone2.Name = "lbTelefone2";
            this.lbTelefone2.Size = new System.Drawing.Size(80, 17);
            this.lbTelefone2.Text = "Telefone 2:";
            // 
            // lbDataNasc
            // 
            this.lbDataNasc.AutoSize = true;
            this.lbDataNasc.Location = new System.Drawing.Point(10, 157);
            this.lbDataNasc.Name = "lbDataNasc";
            this.lbDataNasc.Size = new System.Drawing.Size(78, 17);
            this.lbDataNasc.Text = "Data Nasc:";
            // 
            // tbNome
            // 
            this.tbNome.Location = new System.Drawing.Point(94, 12);
            this.tbNome.Name = "tbNome";
            this.tbNome.Size = new System.Drawing.Size(200, 22);
            this.tbNome.TabIndex = 6;
            // 
            // tbCpf
            // 
            this.tbCpf.Location = new System.Drawing.Point(94, 40);
            this.tbCpf.Name = "tbCpf";
            this.tbCpf.Size = new System.Drawing.Size(200, 22);
            this.tbCpf.TabIndex = 7;
            // 
            // tbRg
            // 
            this.tbRg.Location = new System.Drawing.Point(94, 68);
            this.tbRg.Name = "tbRg";
            this.tbRg.Size = new System.Drawing.Size(200, 22);
            this.tbRg.TabIndex = 8;
            // 
            // tbTel2
            // 
            this.tbTel2.Location = new System.Drawing.Point(94, 124);
            this.tbTel2.Name = "tbTel2";
            this.tbTel2.Size = new System.Drawing.Size(200, 22);
            this.tbTel2.TabIndex = 10;
            // 
            // tbTel1
            // 
            this.tbTel1.Location = new System.Drawing.Point(94, 96);
            this.tbTel1.Name = "tbTel1";
            this.tbTel1.Size = new System.Drawing.Size(200, 22);
            this.tbTel1.TabIndex = 9;
            // 
            // btShow
            // 
            this.btShow.Location = new System.Drawing.Point(331, 9);
            this.btShow.Name = "btShow";
            this.btShow.Size = new System.Drawing.Size(75, 23);
            this.btShow.Text = "Mostra";
            this.btShow.UseVisualStyleBackColor = true;
            this.btShow.Click += new System.EventHandler(this.btShow_Click);
            // 
            // btHide
            // 
            this.btHide.Location = new System.Drawing.Point(412, 9);
            this.btHide.Name = "btHide";
            this.btHide.Size = new System.Drawing.Size(75, 23);
            this.btHide.Text = "Esconde";
            this.btHide.UseVisualStyleBackColor = true;
            this.btHide.Click += new System.EventHandler(this.btHide_Click);
            // 
            // lbUsuario
            // 
            this.lbUsuario.AutoSize = true;
            this.lbUsuario.Location = new System.Drawing.Point(10, 211);
            this.lbUsuario.Name = "lbUsuario";
            this.lbUsuario.Size = new System.Drawing.Size(61, 17);
            this.lbUsuario.Text = "Usuario:";
            // 
            // tbUsuario
            // 
            this.tbUsuario.Location = new System.Drawing.Point(94, 208);
            this.tbUsuario.Name = "tbUsuario";
            this.tbUsuario.Size = new System.Drawing.Size(200, 22);
            this.tbUsuario.TabIndex = 15;
            // 
            // btSalvar
            // 
            this.btSalvar.Location = new System.Drawing.Point(331, 234);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(75, 23);
            this.btSalvar.Text = "Salvar";
            this.btSalvar.UseVisualStyleBackColor = true;
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // dtDataNasc
            // 
            this.dtDataNasc.Location = new System.Drawing.Point(94, 152);
            this.dtDataNasc.Name = "dtDataNasc";
            this.dtDataNasc.Size = new System.Drawing.Size(200, 22);
            this.dtDataNasc.TabIndex = 17;
            // 
            // label1
            // 
            this.lbEmail.AutoSize = true;
            this.lbEmail.Location = new System.Drawing.Point(10, 183);
            this.lbEmail.Name = "label1";
            this.lbEmail.Size = new System.Drawing.Size(46, 17);
            this.lbEmail.Text = "Email:";
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(94, 180);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(200, 22);
            this.tbEmail.TabIndex = 19;
            // 
            // btAbrir
            // 
            this.btAbrir.Location = new System.Drawing.Point(412, 234);
            this.btAbrir.Name = "btAbrir";
            this.btAbrir.Size = new System.Drawing.Size(75, 23);
            this.btAbrir.Text = "Abrir";
            this.btAbrir.UseVisualStyleBackColor = true;
            this.btAbrir.Click += new System.EventHandler(this.btAbrir_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.btAbrir);
            this.Controls.Add(this.lbEmail);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.dtDataNasc);
            this.Controls.Add(this.btSalvar);
            this.Controls.Add(this.lbUsuario);
            this.Controls.Add(this.tbUsuario);
            this.Controls.Add(this.btHide);
            this.Controls.Add(this.btShow);
            this.Controls.Add(this.lbNome);
            this.Controls.Add(this.tbTel1);
            this.Controls.Add(this.lbCpf);
            this.Controls.Add(this.tbNome);
            this.Controls.Add(this.tbTel2);
            this.Controls.Add(this.lbDataNasc);
            this.Controls.Add(this.lbRg);
            this.Controls.Add(this.tbCpf);
            this.Controls.Add(this.lbTelefone2);
            this.Controls.Add(this.lbTelefone1);
            this.Controls.Add(this.tbRg);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbNome;
        private System.Windows.Forms.Label lbCpf;
        private System.Windows.Forms.Label lbRg;
        private System.Windows.Forms.Label lbTelefone1;
        private System.Windows.Forms.Label lbTelefone2;
        private System.Windows.Forms.Label lbDataNasc;
        private System.Windows.Forms.TextBox tbNome;
        private System.Windows.Forms.TextBox tbCpf;
        private System.Windows.Forms.TextBox tbRg;
        private System.Windows.Forms.TextBox tbTel2;
        private System.Windows.Forms.TextBox tbTel1;
        private System.Windows.Forms.Button btShow;
        private System.Windows.Forms.Button btHide;
        private System.Windows.Forms.Label lbUsuario;
        private System.Windows.Forms.TextBox tbUsuario;
        private System.Windows.Forms.Button btSalvar;
        private System.Windows.Forms.DateTimePicker dtDataNasc;
        private System.Windows.Forms.Label lbEmail;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Button btAbrir;
    }
}

