using DirectShowLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldSimulator
{
    public partial class Form1 : Form
    {
        Capture cam = null;

        public Form1()
        {
            InitializeComponent();

            iniConfig();

            initListaPacientes();

            pnlSessao.iniPainelSessao();
            pnlSessao.listaPacienteSessao(pnlPaciente.getPacienteID());
        }

        private void iniConfig()
        {
            Biblioteca.iniConfig();
            //this.FormClosing += new FormClosingEventHandler(this.onFormClosing);
        }
        
        private void initListaPacientes()
        {
            Biblioteca.updatePacientes();
            pnlPaciente.iniPainelPaciente(Biblioteca.getPacientes());
            pnlPaciente.setGridRowSelectionChange(rowSelectionChange);
        }

        protected void rowSelectionChange(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView) sender;

            if (e.RowIndex > -1 && e.RowIndex != pnlPaciente.selectedIndex)
            {
                pnlPaciente.SelecionaLinha(e.RowIndex);
                pnlSessao.listaPacienteSessao(pnlPaciente.getPacienteID());
                pnlSessao.Enabled = true;
            }
        }

        private void btConfig_Click(object sender, EventArgs e)
        {
            FormConfig formConfig = new FormConfig();
            formConfig.ShowDialog();
            formConfig = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cam == null)
            {
                DsDevice[] capDevices;

                // Get the collection of video devices
                capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

                int VIDEODEVICE = 0; // zero based index of video capture device to use
                const int VIDEOWIDTH = 640; // Depends on video device caps
                const int VIDEOHEIGHT = 480; // Depends on video device caps
                const int VIDEOBITSPERPIXEL = 24; // BitsPerPixel values determined by device

                int[] VIDEOCONFIG = new int[] { VIDEODEVICE, pictureBox1.Size.Width, pictureBox1.Size.Height, VIDEOBITSPERPIXEL };

                button2.Text = "Parar video";

                cam = new Capture(VIDEOCONFIG, pictureBox1, "testFile.wmv");

                cam.Start();
            }
            else
            {
                button2.Text = "Iniciar Camera";
                // Pause the recording
                cam.Pause();
                // Close it down
                cam.Dispose();
                cam = null;
            }            
        }

        //private void onFormClosing(object sender, FormClosingEventArgs e)
        //{            
        //}

    }
}
