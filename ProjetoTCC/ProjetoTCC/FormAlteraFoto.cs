using Accord.Video.DirectShow;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ProjetoTCC
{
    public partial class FormAlteraFoto : Form
    {
        public string caminhoArq = "";

        public Bitmap foto = null;

        public FormAlteraFoto()
        {
            InitializeComponent();

            FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (VideoDevices.Count < 1)
            {
                btCamera.Enabled = false;
            }
        }        

        private void btCamera_Click(object sender, EventArgs e)
        {
            FormCamera formCamera = new FormCamera();
            formCamera.StartPosition = FormStartPosition.CenterParent;
            if (DialogResult.OK == formCamera.ShowDialog()) {
                this.foto = formCamera.foto;   
            }
            this.Close();
        }

        private void btEscolheArquivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "";

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                dlg.Filter = String.Format("{0}{1}{2} ({3})|{3}", dlg.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";
            }

            dlg.Filter = String.Format("{0}{1}{2} ({3})|{3}", dlg.Filter, sep, "All Files", "*.*");

            dlg.DefaultExt = ".png"; // Default file extension 

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.caminhoArq = dlg.FileName;
            }
            this.Close();
        }
    }
}
