using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.WindowsAPICodePack.Dialogs;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using Accord.Video.DirectShow;

namespace ProjetoTCC
{
    public partial class FormConfig : Form
    {
        bool configAlt = false;

        public FormConfig()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormClosing += new FormClosingEventHandler(this.onFormClosing);

            lbFileFolder1.Text = "Local dos arquivos:";
            lbFileFolder2.Text = Biblioteca.caminhoArquivos.Trim();
            if(lbFileFolder2.Text.Length > 50)
            {
                lbFileFolder2.Text = lbFileFolder2.Text.Substring(0, 46) + "...";
            }

            listCamera.Items.Clear();

            FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (VideoDevices.Count >= 1)
            {
                this.listCamera.DisplayMember = "Name";
                this.listCamera.ValueMember = "MonikerString";
                foreach (FilterInfo VideoCaptureDevice in VideoDevices)
                {
                    listCamera.Items.Add(VideoCaptureDevice);
                }
            }
            else
            {
                listCamera.Items.Add("-----");
            }
            listCamera.SelectedIndex = 0;

            tbNomeEstab.Text = Biblioteca.nomeEstab;
            tbEnderecoEstab.Text = Biblioteca.enderecoEstab;
            tbFoneEstab.Text = Biblioteca.foneEstab;

            string logo = Biblioteca.caminhoLogo.Trim();

            lbFileLogo1.Text = "Imagem para Logo:";
            lbFileLogo2.Text = logo;
            if (lbFileLogo2.Text.Length > 55)
            {
                lbFileLogo2.Text = lbFileLogo2.Text.Substring(0, 50) + "...";
            }
            if (logo.Length > 0 && File.Exists(logo))
            {
                this.updateConfigLogo(logo);
            }
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            Biblioteca.updateConfig(lbFileFolder2.Text, listCamera.SelectedIndex, tbNomeEstab.Text, tbEnderecoEstab.Text, tbFoneEstab.Text, lbFileLogo2.Text);
            this.configAlt = false;
            this.Close();
        }

        private void btFileChooser_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                lbFileFolder2.Text = dialog.FileName;
                configAlt = true;
            }
        }

        private void btFileLogo_Click(object sender, EventArgs e)
        {
            // Configure open file dialog box 
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
                configAlt = true;
                lbFileLogo2.Text = dlg.FileName;
                this.updateConfigLogo(dlg.FileName);
            }
        }

        private void updateConfigLogo(string logo)
        {
            Image img = ResizeImage(Image.FromFile(logo), pbLogo.Size.Width, pbLogo.Size.Height);

            pbLogo.Image = img;
            pbLogo.ErrorImage = img;
            pbLogo.InitialImage = img;
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap videoOutput = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(videoOutput))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.DrawImage(image, new System.Drawing.Rectangle(0, 0, width, height));
            }
            return videoOutput;
        }

        private void btAtualizaCamera_Click(object sender, EventArgs e)
        {
            listCamera.Items.Clear();
            FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (VideoDevices.Count >= 1)
            {
                this.listCamera.DisplayMember = "Name";
                this.listCamera.ValueMember = "MonikerString";
                foreach (FilterInfo VideoCaptureDevice in VideoDevices)
                {
                    listCamera.Items.Add(VideoCaptureDevice);
                }
            }
            else
            {
                listCamera.Items.Add("-----");
            }
            listCamera.SelectedIndex = 0;
        }

        private void onFormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.configAlt)
            {
                DialogResult result = MessageBox.Show("Deseja salvar as alterações realizadas?", "Aviso", MessageBoxButtons.YesNoCancel);

                if (result.Equals(DialogResult.Yes))
                {
                    Biblioteca.updateConfig(lbFileFolder2.Text, listCamera.SelectedIndex, tbNomeEstab.Text, tbEnderecoEstab.Text, tbFoneEstab.Text, lbFileLogo2.Text);
                } else if (result.Equals(DialogResult.Cancel))
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
