using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjetoTCC
{
    public class VerticalProgressBar : ProgressBar
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x04;
                return cp;
            }
        }       

        //Property to set to decide whether to print a % or Text
        public int DisplayStyle { get; set; }
        //1 -> %
        //2 -> text

        //Property to hold the custom text
        public string CustomText { get; set; }

        public VerticalProgressBar()
        {
            // Modify the ControlStyles flags
            //http://msdn.microsoft.com/en-us/library/system.windows.forms.controlstyles.aspx
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        Brush barColor = Brushes.Green;

        public void setBarColor(Brush color)
        {
            barColor = color;
        }

        public VerticalProgressBar(Brush color)
        {
            // Modify the ControlStyles flags
            //http://msdn.microsoft.com/en-us/library/system.windows.forms.controlstyles.aspx
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            barColor = color;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            Graphics g = e.Graphics;

            ProgressBarRenderer.DrawVerticalBar(g, rect);
            rect.Inflate(-3, -3);
            if (Value > 0)
            {
                // As we doing this ourselves we need to draw the chunks on the progress bar
//                Rectangle clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);
                Rectangle clip = new Rectangle(rect.X, rect.Height - (int)Math.Round(((float)Value / Maximum) * rect.Height), rect.Width, rect.Height);
                ProgressBarRenderer.DrawVerticalChunks(g, clip);

                e.Graphics.FillRectangle(barColor, clip);
            }

            // Set the Display text (Either a % amount or our custom text

            string text = "";

            if (DisplayStyle == 1)
            {
                text = Value.ToString() + "%";
            } else if (DisplayStyle == 2)
            {
                text = CustomText;
            }

            using (Font f = new Font(FontFamily.GenericSerif, 10))
            {
                SizeF len = g.MeasureString(text, f);
                // Calculate the location of the text (the middle of progress bar)
                // Point location = new Point(Convert.ToInt32((rect.Width / 2) - (len.Width / 2)), Convert.ToInt32((rect.Height / 2) - (len.Height / 2)));
                Point location = new Point(Convert.ToInt32((Width / 2) - len.Width / 2), Convert.ToInt32((Height / 2) - len.Height / 2));
                // The commented-out code will centre the text into the highlighted area only. This will centre the text regardless of the highlighted area.
                // Draw the custom text
                g.DrawString(text, f, Brushes.Red, location);
            }

        }
    }

    partial class MainForm
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
            this.btConfig = new System.Windows.Forms.Button();
            this.btEspecialistas = new System.Windows.Forms.Button();
            this.pnlCharts = new System.Windows.Forms.Panel();
            this.lbAlto = new System.Windows.Forms.Label();
            this.lbElevado = new System.Windows.Forms.Label();
            this.lbReduzido = new System.Windows.Forms.Label();
            this.lbNormal = new System.Windows.Forms.Label();
            this.lbFraco = new System.Windows.Forms.Label();
            this.lbForte = new System.Windows.Forms.Label();
            this.lbCalma = new System.Windows.Forms.Label();
            this.lbAtencao = new System.Windows.Forms.Label();
            this.lbQldSinal = new System.Windows.Forms.Label();
            this.btDetalheGraph = new System.Windows.Forms.Button();
            this.vpBarQlty = new ProjetoTCC.VerticalProgressBar();
            this.vpBarAtt = new ProjetoTCC.VerticalProgressBar();
            this.vpBarMed = new ProjetoTCC.VerticalProgressBar();
            this.pnlPaciente = new ProjetoTCC.PainelPaciente();
            this.pnlSessao = new ProjetoTCC.PainelSessao();
            this.pnlCharts.SuspendLayout();
            this.SuspendLayout();
            // 
            // btConfig
            // 
            this.btConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btConfig.Location = new System.Drawing.Point(1150, 669);
            this.btConfig.Name = "btConfig";
            this.btConfig.Size = new System.Drawing.Size(100, 40);
            this.btConfig.TabIndex = 3;
            this.btConfig.Text = "Config";
            this.btConfig.UseVisualStyleBackColor = true;
            this.btConfig.Click += new System.EventHandler(this.btConfig_Click);
            // 
            // btEspecialistas
            // 
            this.btEspecialistas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEspecialistas.Location = new System.Drawing.Point(1044, 669);
            this.btEspecialistas.Name = "btEspecialistas";
            this.btEspecialistas.Size = new System.Drawing.Size(100, 40);
            this.btEspecialistas.TabIndex = 4;
            this.btEspecialistas.Text = "Especialistas";
            this.btEspecialistas.UseVisualStyleBackColor = true;
            this.btEspecialistas.Click += new System.EventHandler(this.btEspecialistas_Click);
            // 
            // pnlCharts
            // 
            this.pnlCharts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlCharts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCharts.Controls.Add(this.lbAlto);
            this.pnlCharts.Controls.Add(this.lbElevado);
            this.pnlCharts.Controls.Add(this.lbReduzido);
            this.pnlCharts.Controls.Add(this.lbNormal);
            this.pnlCharts.Controls.Add(this.lbFraco);
            this.pnlCharts.Controls.Add(this.lbForte);
            this.pnlCharts.Controls.Add(this.lbCalma);
            this.pnlCharts.Controls.Add(this.lbAtencao);
            this.pnlCharts.Controls.Add(this.lbQldSinal);
            this.pnlCharts.Controls.Add(this.btDetalheGraph);
            this.pnlCharts.Controls.Add(this.vpBarQlty);
            this.pnlCharts.Controls.Add(this.vpBarAtt);
            this.pnlCharts.Controls.Add(this.vpBarMed);
            this.pnlCharts.Location = new System.Drawing.Point(12, 380);
            this.pnlCharts.Name = "pnlCharts";
            this.pnlCharts.Size = new System.Drawing.Size(592, 329);
            this.pnlCharts.MaximumSize = new System.Drawing.Size(592, 329);
            this.pnlCharts.TabIndex = 18;
            // 
            // lbAlto
            // 
            this.lbAlto.AutoSize = true;
            this.lbAlto.BackColor = System.Drawing.Color.Transparent;
            this.lbAlto.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAlto.Location = new System.Drawing.Point(250, 80);
            this.lbAlto.Name = "lbAlto";
            this.lbAlto.Size = new System.Drawing.Size(261, 17);
            this.lbAlto.TabIndex = 29;
            this.lbAlto.Text = "Alto_________________________";
            this.lbAlto.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbElevado
            // 
            this.lbElevado.AutoSize = true;
            this.lbElevado.BackColor = System.Drawing.Color.Transparent;
            this.lbElevado.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbElevado.Location = new System.Drawing.Point(223, 122);
            this.lbElevado.Name = "lbElevado";
            this.lbElevado.Size = new System.Drawing.Size(255, 17);
            this.lbElevado.TabIndex = 28;
            this.lbElevado.Text = "Elevado_____________________";
            this.lbElevado.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbReduzido
            // 
            this.lbReduzido.AutoSize = true;
            this.lbReduzido.BackColor = System.Drawing.Color.Transparent;
            this.lbReduzido.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbReduzido.Location = new System.Drawing.Point(213, 205);
            this.lbReduzido.Name = "lbReduzido";
            this.lbReduzido.Size = new System.Drawing.Size(283, 17);
            this.lbReduzido.TabIndex = 27;
            this.lbReduzido.Text = "Reduzido_______________________";
            this.lbReduzido.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbNormal
            // 
            this.lbNormal.AutoSize = true;
            this.lbNormal.BackColor = System.Drawing.Color.Transparent;
            this.lbNormal.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNormal.Location = new System.Drawing.Point(227, 164);
            this.lbNormal.Name = "lbNormal";
            this.lbNormal.Size = new System.Drawing.Size(248, 17);
            this.lbNormal.TabIndex = 26;
            this.lbNormal.Text = "Normal_____________________";
            this.lbNormal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbFraco
            // 
            this.lbFraco.AutoSize = true;
            this.lbFraco.BackColor = System.Drawing.Color.Transparent;
            this.lbFraco.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFraco.Location = new System.Drawing.Point(3, 246);
            this.lbFraco.Name = "lbFraco";
            this.lbFraco.Size = new System.Drawing.Size(112, 17);
            this.lbFraco.TabIndex = 25;
            this.lbFraco.Text = "Fraco_______";
            this.lbFraco.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbForte
            // 
            this.lbForte.AutoSize = true;
            this.lbForte.BackColor = System.Drawing.Color.Transparent;
            this.lbForte.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbForte.Location = new System.Drawing.Point(3, 37);
            this.lbForte.Name = "lbForte";
            this.lbForte.Size = new System.Drawing.Size(109, 17);
            this.lbForte.TabIndex = 24;
            this.lbForte.Text = "Forte_______";
            this.lbForte.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbCalma
            // 
            this.lbCalma.AutoSize = true;
            this.lbCalma.Location = new System.Drawing.Point(435, 16);
            this.lbCalma.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCalma.Name = "lbCalma";
            this.lbCalma.Size = new System.Drawing.Size(133, 17);
            this.lbCalma.TabIndex = 22;
            this.lbCalma.Text = "Calma/Relaxamento";
            this.lbCalma.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbAtencao
            // 
            this.lbAtencao.AutoSize = true;
            this.lbAtencao.Location = new System.Drawing.Point(334, 16);
            this.lbAtencao.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAtencao.Name = "lbAtencao";
            this.lbAtencao.Size = new System.Drawing.Size(60, 17);
            this.lbAtencao.TabIndex = 21;
            this.lbAtencao.Text = "Atenção";
            this.lbAtencao.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbQldSinal
            // 
            this.lbQldSinal.AutoSize = true;
            this.lbQldSinal.Location = new System.Drawing.Point(40, 16);
            this.lbQldSinal.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbQldSinal.Name = "lbQldSinal";
            this.lbQldSinal.Size = new System.Drawing.Size(128, 17);
            this.lbQldSinal.TabIndex = 20;
            this.lbQldSinal.Text = "Qualidade do Sinal";
            this.lbQldSinal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.btDetalheGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDetalheGraph.Location = new System.Drawing.Point(479, 299);
            this.btDetalheGraph.Name = "btDetalheGraph";
            this.btDetalheGraph.Size = new System.Drawing.Size(108, 25);
            this.btDetalheGraph.TabIndex = 19;
            this.btDetalheGraph.Text = "Detalhes";
            this.btDetalheGraph.UseVisualStyleBackColor = true;
            this.btDetalheGraph.Click += new System.EventHandler(this.btDetalheGraph_Click);
            // 
            // vpBarQlty
            // 
            this.vpBarQlty.CustomText = null;
            this.vpBarQlty.DisplayStyle = 0;
            this.vpBarQlty.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.vpBarQlty.Location = new System.Drawing.Point(63, 48);
            this.vpBarQlty.Maximum = 205;
            this.vpBarQlty.Name = "vpBarQlty";
            this.vpBarQlty.Size = new System.Drawing.Size(100, 221);
            this.vpBarQlty.TabIndex = 3;
            this.vpBarQlty.Value = 200;
            // 
            // vpBarAtt
            // 
            this.vpBarAtt.CustomText = null;
            this.vpBarAtt.DisplayStyle = 0;
            this.vpBarAtt.ForeColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.vpBarAtt.Location = new System.Drawing.Point(317, 48);
            this.vpBarAtt.Maximum = 103;
            this.vpBarAtt.Name = "vpBarAtt";
            this.vpBarAtt.Size = new System.Drawing.Size(100, 221);
            this.vpBarAtt.TabIndex = 2;
            this.vpBarAtt.Value = 80;
            // 
            // vpBarMed
            // 
            this.vpBarMed.CustomText = null;
            this.vpBarMed.DisplayStyle = 0;
            this.vpBarMed.Location = new System.Drawing.Point(456, 48);
            this.vpBarMed.Maximum = 103;
            this.vpBarMed.Name = "vpBarMed";
            this.vpBarMed.Size = new System.Drawing.Size(100, 221);
            this.vpBarMed.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.vpBarMed.TabIndex = 1;
            this.vpBarMed.Value = 25;
            // 
            // pnlPaciente
            // 
            this.pnlPaciente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlPaciente.BackColor = System.Drawing.SystemColors.Control;
            this.pnlPaciente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPaciente.Location = new System.Drawing.Point(12, 6);
            this.pnlPaciente.MinimumSize = new System.Drawing.Size(500, 2);
            this.pnlPaciente.Name = "pnlPaciente";
            this.pnlPaciente.Size = new System.Drawing.Size(592, 368);
            this.pnlPaciente.TabIndex = 0;
            // 
            // pnlSessao
            // 
            this.pnlSessao.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSessao.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSessao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSessao.Location = new System.Drawing.Point(611, 6);
            this.pnlSessao.Name = "pnlSessao";
            this.pnlSessao.Size = new System.Drawing.Size(639, 656);
            this.pnlSessao.TabIndex = 19;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 721);
            this.Controls.Add(this.btEspecialistas);
            this.Controls.Add(this.pnlCharts);
            this.Controls.Add(this.btConfig);
            this.Controls.Add(this.pnlPaciente);
            this.Controls.Add(this.pnlSessao);
            this.MinimumSize = new System.Drawing.Size(1280, 768);
            this.Name = "MainForm";
            this.Text = "MindsEye";
            this.pnlCharts.ResumeLayout(false);
            this.pnlCharts.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private PainelPaciente pnlPaciente;
        private System.Windows.Forms.Button btConfig;
        private System.Windows.Forms.Button btEspecialistas;
        private System.Windows.Forms.Panel pnlCharts;
        private Button btDetalheGraph;
        private VerticalProgressBar vpBarQlty;
        private VerticalProgressBar vpBarAtt;
        private VerticalProgressBar vpBarMed;
        private Label lbQldSinal;
        private Label lbCalma;
        private Label lbAtencao;
        private Label lbFraco;
        private Label lbForte;
        private Label lbReduzido;
        private Label lbNormal;
        private Label lbAlto;
        private Label lbElevado;
        private PainelSessao pnlSessao;
    }

}

