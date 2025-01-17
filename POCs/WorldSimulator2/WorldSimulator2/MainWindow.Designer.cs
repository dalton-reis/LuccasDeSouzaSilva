﻿namespace WorldSimulator
{
    partial class MainWindow
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea21 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend21 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea22 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend22 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea23 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend23 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea24 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend24 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea25 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend25 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.btAbrirMundo = new System.Windows.Forms.Button();
            this.btFecharMundo = new System.Windows.Forms.Button();
            this.chAlpha = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chBeta = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chGamma = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chDelta = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chTheta = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pbSinal = new System.Windows.Forms.PictureBox();
            this.lbCamDevices = new System.Windows.Forms.ListBox();
            this.btCamAction = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.chAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chBeta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chGamma)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chDelta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chTheta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSinal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btAbrirMundo
            // 
            this.btAbrirMundo.Location = new System.Drawing.Point(82, 12);
            this.btAbrirMundo.Name = "btAbrirMundo";
            this.btAbrirMundo.Size = new System.Drawing.Size(114, 33);
            this.btAbrirMundo.TabIndex = 0;
            this.btAbrirMundo.Text = "Abrir mundo";
            this.btAbrirMundo.UseVisualStyleBackColor = true;
            this.btAbrirMundo.Click += new System.EventHandler(this.btAbrirMundo_Click);
            // 
            // btFecharMundo
            // 
            this.btFecharMundo.Location = new System.Drawing.Point(82, 51);
            this.btFecharMundo.Name = "btFecharMundo";
            this.btFecharMundo.Size = new System.Drawing.Size(114, 33);
            this.btFecharMundo.TabIndex = 1;
            this.btFecharMundo.Text = "Fechar mundo";
            this.btFecharMundo.UseVisualStyleBackColor = true;
            this.btFecharMundo.Click += new System.EventHandler(this.btFecharMundo_Click);
            // 
            // chAlpha
            // 
            chartArea21.Name = "ChartArea1";
            this.chAlpha.ChartAreas.Add(chartArea21);
            legend21.Name = "Legend1";
            this.chAlpha.Legends.Add(legend21);
            this.chAlpha.Location = new System.Drawing.Point(889, 4);
            this.chAlpha.Name = "chAlpha";
            this.chAlpha.Size = new System.Drawing.Size(460, 102);
            this.chAlpha.TabIndex = 14;
            this.chAlpha.Text = "chart1";
            // 
            // chBeta
            // 
            chartArea22.Name = "ChartArea1";
            this.chBeta.ChartAreas.Add(chartArea22);
            legend22.Name = "Legend1";
            this.chBeta.Legends.Add(legend22);
            this.chBeta.Location = new System.Drawing.Point(889, 112);
            this.chBeta.Name = "chBeta";
            this.chBeta.Size = new System.Drawing.Size(460, 102);
            this.chBeta.TabIndex = 16;
            this.chBeta.Text = "chart1";
            // 
            // chGamma
            // 
            chartArea23.Name = "ChartArea1";
            this.chGamma.ChartAreas.Add(chartArea23);
            legend23.Name = "Legend1";
            this.chGamma.Legends.Add(legend23);
            this.chGamma.Location = new System.Drawing.Point(889, 220);
            this.chGamma.Name = "chGamma";
            this.chGamma.Size = new System.Drawing.Size(460, 102);
            this.chGamma.TabIndex = 17;
            this.chGamma.Text = "chart1";
            // 
            // chDelta
            // 
            chartArea24.Name = "ChartArea1";
            this.chDelta.ChartAreas.Add(chartArea24);
            legend24.Name = "Legend1";
            this.chDelta.Legends.Add(legend24);
            this.chDelta.Location = new System.Drawing.Point(889, 328);
            this.chDelta.Name = "chDelta";
            this.chDelta.Size = new System.Drawing.Size(460, 102);
            this.chDelta.TabIndex = 18;
            this.chDelta.Text = "chart1";
            // 
            // chTheta
            // 
            chartArea25.Name = "ChartArea1";
            this.chTheta.ChartAreas.Add(chartArea25);
            legend25.Name = "Legend1";
            this.chTheta.Legends.Add(legend25);
            this.chTheta.Location = new System.Drawing.Point(889, 436);
            this.chTheta.Name = "chTheta";
            this.chTheta.Size = new System.Drawing.Size(460, 102);
            this.chTheta.TabIndex = 19;
            this.chTheta.Text = "chart1";
            // 
            // pbSinal
            // 
            this.pbSinal.ImageLocation = "";
            this.pbSinal.Location = new System.Drawing.Point(12, 12);
            this.pbSinal.Name = "pbSinal";
            this.pbSinal.Size = new System.Drawing.Size(64, 64);
            this.pbSinal.TabIndex = 15;
            this.pbSinal.TabStop = false;
            // 
            // lbCamDevices
            // 
            this.lbCamDevices.FormattingEnabled = true;
            this.lbCamDevices.ItemHeight = 16;
            this.lbCamDevices.Location = new System.Drawing.Point(12, 190);
            this.lbCamDevices.Name = "lbCamDevices";
            this.lbCamDevices.Size = new System.Drawing.Size(184, 84);
            this.lbCamDevices.TabIndex = 20;
            // 
            // btCamAction
            // 
            this.btCamAction.Location = new System.Drawing.Point(12, 143);
            this.btCamAction.Name = "btCamAction";
            this.btCamAction.Size = new System.Drawing.Size(119, 41);
            this.btCamAction.TabIndex = 21;
            this.btCamAction.Text = "button1";
            this.btCamAction.UseVisualStyleBackColor = true;
            this.btCamAction.Click += new System.EventHandler(this.btCamAction_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(254, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(597, 509);
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(1361, 638);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btCamAction);
            this.Controls.Add(this.lbCamDevices);
            this.Controls.Add(this.chTheta);
            this.Controls.Add(this.chDelta);
            this.Controls.Add(this.chGamma);
            this.Controls.Add(this.chBeta);
            this.Controls.Add(this.pbSinal);
            this.Controls.Add(this.chAlpha);
            this.Controls.Add(this.btFecharMundo);
            this.Controls.Add(this.btAbrirMundo);
            this.DoubleBuffered = true;
            this.HelpButton = true;
            this.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chBeta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chGamma)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chDelta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chTheta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSinal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btAbrirMundo;
        private System.Windows.Forms.Button btFecharMundo;
        private System.Windows.Forms.DataVisualization.Charting.Chart chAlpha;
        private System.Windows.Forms.PictureBox pbSinal;
        private System.Windows.Forms.DataVisualization.Charting.Chart chBeta;
        private System.Windows.Forms.DataVisualization.Charting.Chart chGamma;
        private System.Windows.Forms.DataVisualization.Charting.Chart chDelta;
        private System.Windows.Forms.DataVisualization.Charting.Chart chTheta;
        private System.Windows.Forms.ListBox lbCamDevices;
        private System.Windows.Forms.Button btCamAction;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

