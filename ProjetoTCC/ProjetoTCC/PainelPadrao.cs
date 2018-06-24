using System.Windows.Forms;

namespace ProjetoTCC
{
    class PainelPadrao : System.Windows.Forms.Panel
    {
        protected Button btEsq = null;
        protected Button btDir = null;
        protected Button btDel = null;
        protected Panel pnlArea = null;
        
        protected void initPainel(Panel pnlOrigem)
        {
            this.btEsq = new Button();
            this.btDir = new Button();
            this.btDel = new Button();
            this.pnlArea = new Panel();

            this.SuspendLayout();
            this.BackColor = pnlOrigem.BackColor;
            this.BorderStyle = pnlOrigem.BorderStyle;
            this.Location = pnlOrigem.Location;
            this.Name = "pnlEsq";
            this.Size = pnlOrigem.Size;
            this.TabIndex = 0;

            this.Controls.Add(btEsq);

            this.btEsq.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left))
            );

            this.btEsq.Size = new System.Drawing.Size(80, 30);
            this.btEsq.Location = new System.Drawing.Point(0, 0);
            this.btEsq.Name = "btEsq";
            this.btEsq.TabIndex = 8;
            this.btEsq.Text = "btEsq";
            this.btEsq.UseVisualStyleBackColor = true;

            this.Controls.Add(btDir);

            this.btDir.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right))
            );

            this.btDir.Size = new System.Drawing.Size(80, 30);
            this.btDir.Location = new System.Drawing.Point(this.Size.Width-btDir.Size.Width-2, 0);
            this.btDir.Name = "btDir";
            this.btDir.TabIndex = 9;
            this.btDir.Text = "btDir";
            this.btDir.UseVisualStyleBackColor = true;

            this.Controls.Add(btDel);

            this.btDel.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right))
            );

            this.btDel.Size = new System.Drawing.Size(80, 30);
            this.btDel.Location = new System.Drawing.Point(this.btDir.Location.X - this.btDel.Size.Width - 10, 0);
            this.btDel.Name = "btDel";
            this.btDel.TabIndex = 9;
            this.btDel.Text = "btDel";
            this.btDel.UseVisualStyleBackColor = true;

            this.Controls.Add(pnlArea);

            this.pnlArea.AutoScroll = true;

            this.pnlArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));

            this.pnlArea.BackColor = pnlOrigem.BackColor;
            this.pnlArea.BorderStyle = pnlOrigem.BorderStyle;
            this.pnlArea.Location = new System.Drawing.Point(-1, btEsq.Size.Height);
            this.pnlArea.Name = "pnlArea";
            this.pnlArea.Size = new System.Drawing.Size(pnlOrigem.Size.Width, pnlOrigem.Size.Height-btEsq.Size.Height);
            this.pnlArea.TabIndex = 0;

            this.ResumeLayout(false);
        }

        public void enableBtDir(bool value)
        {
            btDir.Enabled = value;
            btDir.Visible = value;
        }

    }
}
