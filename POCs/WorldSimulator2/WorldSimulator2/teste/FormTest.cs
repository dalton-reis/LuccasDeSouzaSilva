using DirectShowLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldSimulator
{
    public partial class FormTest : Form
    {
        Capture cam = null;

        public FormTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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

                button1.Text = "Parar video";

                cam = new Capture(VIDEOCONFIG, pictureBox1, "testFile.wmv");

                cam.Start();
            }
            else
            {
                button1.Text = "Iniciar Camera";
                // Pause the recording
                cam.Pause();
                // Close it down
                cam.Dispose();
                cam = null;
            }
        }
    }
}
