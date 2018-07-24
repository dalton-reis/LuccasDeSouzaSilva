using System;
using System.Windows.Forms;

namespace ProjetoTCC
{
    public partial class FormGraphs : Form
    {
        public bool isGraphReady = false;

        public FormGraphs()
        {
            InitializeComponent();
            isGraphReady = true;
            this.FormClosing += new FormClosingEventHandler(this.onFormClosing);

            this.cbAlfa1.Checked = true;
            this.cbAlfa2.Checked = true;
            this.cbBeta1.Checked = true;
            this.cbBeta2.Checked = true;
            this.cbGamma1.Checked = true;
            this.cbGamma2.Checked = true;
            this.cbTheta.Checked = true;
            this.cbDelta.Checked = true;

            

//            this.cbAlfa1.CheckedChanged += CbAlfa1_CheckedChanged;
        }

        private void CbAlfa1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbAlfa1.Checked)
            {
                this.chartAlpha.Series["Alpha1"].Points.Clear();
            }
        }

        private void onFormClosing(object sender, FormClosingEventArgs e)
        {
            isGraphReady = false;
        }

        public void updateGraphs(NeuroData nr)
        {
            updateChartAlpha(nr);
            updateChartBeta(nr);
            updateChartGamma(nr);
            updateChartTheta(nr);
            updateChartDelta(nr);
        }

        delegate void NeuroDataDelegate(NeuroData nr);

        public void updateChartAlpha(NeuroData nr)
        {
            if (chartAlpha.InvokeRequired)
            {
                NeuroDataDelegate del = new NeuroDataDelegate(updateChartAlpha);
                this.Invoke(del, new object[] { nr });
            } else
            {
                if (this.cbAlfa1.Checked)
                {
                    if (this.chartAlpha.Series["Alpha1"].Points.Count > 5)
                    {
                        this.chartAlpha.Series["Alpha1"].Points.RemoveAt(0);
                    }
                    this.chartAlpha.Series["Alpha1"].Points.Add(nr.Alpha1);
                }
                if (this.cbAlfa2.Checked)
                {
                    if (this.chartAlpha.Series["Alpha2"].Points.Count > 5)
                    {
                        this.chartAlpha.Series["Alpha2"].Points.RemoveAt(0);
                    }
                    this.chartAlpha.Series["Alpha2"].Points.Add(nr.Alpha2);
                }
            }
        }

        public void updateChartBeta(NeuroData nr)
        {
            if (chartAlpha.InvokeRequired)
            {
                NeuroDataDelegate del = new NeuroDataDelegate(updateChartBeta);
                this.Invoke(del, new object[] { nr });
            }
            else
            {
                if (this.cbBeta1.Checked)
                {
                    if (this.chartBeta.Series["Beta1"].Points.Count > 5)
                    {
                        this.chartBeta.Series["Beta1"].Points.RemoveAt(0);
                    }
                    this.chartBeta.Series["Beta1"].Points.Add(nr.Beta1);
                }
                if (this.cbBeta2.Checked)
                {
                    if (this.chartBeta.Series["Beta2"].Points.Count > 5)
                    {
                        this.chartBeta.Series["Beta2"].Points.RemoveAt(0);
                    }
                    this.chartBeta.Series["Beta2"].Points.Add(nr.Beta2);
                }
            }
        }

        public void updateChartGamma(NeuroData nr)
        {
            if (chartAlpha.InvokeRequired)
            {
                NeuroDataDelegate del = new NeuroDataDelegate(updateChartGamma);
                this.Invoke(del, new object[] { nr });
            }
            else
            {
                if (this.cbGamma1.Checked)
                {
                    if (this.chartGamma.Series["Gamma1"].Points.Count > 5)
                    {
                        this.chartGamma.Series["Gamma1"].Points.RemoveAt(0);
                    }
                    this.chartGamma.Series["Gamma1"].Points.Add(nr.Gamma1);
                }
                if (this.cbGamma2.Checked)
                {
                    if (this.chartGamma.Series["Gamma2"].Points.Count > 5)
                    {
                        this.chartGamma.Series["Gamma2"].Points.RemoveAt(0);
                    }
                    this.chartGamma.Series["Gamma2"].Points.Add(nr.Gamma2);
                }
            }
        }

        public void updateChartTheta(NeuroData nr)
        {
            if (chartAlpha.InvokeRequired)
            {
                NeuroDataDelegate del = new NeuroDataDelegate(updateChartTheta);
                this.Invoke(del, new object[] { nr });
            }
            else
            {
                if (this.cbTheta.Checked)
                {
                    if (this.chartTheta.Series["Theta"].Points.Count > 5)
                    {
                        this.chartTheta.Series["Theta"].Points.RemoveAt(0);
                    }
                    this.chartTheta.Series["Theta"].Points.Add(nr.Theta);
                }
            }
        }

        public void updateChartDelta(NeuroData nr)
        {
            if (chartAlpha.InvokeRequired)
            {
                NeuroDataDelegate del = new NeuroDataDelegate(updateChartDelta);
                this.Invoke(del, new object[] { nr });
            }
            else
            {
                if (this.cbDelta.Checked)
                {
                    if (this.chartDelta.Series["Delta"].Points.Count > 5)
                    {
                        this.chartDelta.Series["Delta"].Points.RemoveAt(0);
                    }
                    this.chartDelta.Series["Delta"].Points.Add(nr.Delta);
                }
            }
        }
    }
}
