using Accord.Video;
using Accord.Video.DirectShow;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace ProjetoTCC
{
    public partial class FormCamera : Form
    {
        public Bitmap foto { get; private set; } = null;
        public VideoCaptureDevice videoCamera { get; private set; } = null;

        public FormCamera()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.onFormClosing);

            pbCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Right)));

            cbListCameras.DropDownStyle = ComboBoxStyle.DropDownList;
            cbListCameras.SelectedValueChanged += ListCamera_SelectedValueChanged;

            atualizaCameras();

            iniciaVideo(cbListCameras.SelectedIndex);

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void ListCamera_SelectedValueChanged(object sender, EventArgs e)
        {
            btCapturar.Enabled = false;
            if (videoCamera != null)
            {
                videoCamera.SignalToStop();
                while (videoCamera.IsRunning)
                {
                    Thread.Sleep(0500);
                }
                videoCamera = null;
            }
            iniciaVideo(cbListCameras.SelectedIndex);
            Thread.Sleep(1000);
            btCapturar.Enabled = true;
        }

        public void iniciaVideo(int indexCamera)
        {
            if (videoCamera == null)
            {        
                FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                videoCamera = new VideoCaptureDevice(VideoDevices[indexCamera].MonikerString);
                try
                {
                    videoCamera.NewFrame += new NewFrameEventHandler(VideoCamera_OnNewFrame);
                    videoCamera.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocorreu um problema ao iniciar a gravação." +
                        "\nVerifique as configurações da camera e tente novamente." +
                        "\nSituação técnica:" + ex.Message +
                        "\nOrigem:" + ex.StackTrace.Substring(0, 255), "Aviso");
                }                
            }
        }

        private void onFormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCamera != null)
            {            
                try
                {
                    videoCamera.SignalToStop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocorreu um problema ao parar a gravação." +
                       "\nVerifique as configurações da camera e tente novamente." +
                       "\nSituação técnica:" + ex.Message +
                       "\nOrigem:" + ex.StackTrace.Substring(0, 255), "Aviso");
                    e.Cancel = true;
                }
            }
        }

        void VideoCamera_OnNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            System.Windows.Forms.PictureBox videoRecorder = pbCamera;

            Bitmap videoInput = (Bitmap)eventArgs.Frame.Clone();

            int width = videoRecorder.Size.Width;
            int height = videoRecorder.Size.Height;

            Bitmap videoOutput = new Bitmap(width, height);

            using (Graphics gr = Graphics.FromImage(videoOutput))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.DrawImage(videoInput, new System.Drawing.Rectangle(0, 0, width, height));

                Rectangle ee = new Rectangle(10, 10, 270, 270);
                using (Pen pen = new Pen(Color.White, 2))
                {
                    gr.DrawRectangle(pen, ee);
                }
            }

            videoRecorder.Image = videoOutput;
        }

        private void btCapturar_Click(object sender, EventArgs e)
        {
            Rectangle cropSize = new Rectangle(10, 10, 270, 270);
            Bitmap src = (Bitmap)pbCamera.Image;
            Bitmap result = new Bitmap(cropSize.Width, cropSize.Height);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(src, new Rectangle(0, 0, result.Width, result.Height),
                                 cropSize,
                                 GraphicsUnit.Pixel);
            }

            foto = result;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btAtualizaCameras_Click(object sender, EventArgs e)
        {
            atualizaCameras();
        }

        private void atualizaCameras()
        {
            btCapturar.Enabled = false;
            cbListCameras.Items.Clear();
            FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (VideoDevices.Count >= 1)
            {
                int indexCamera = -1;
                int count = -1;
                foreach (FilterInfo VideoCaptureDevice in VideoDevices)
                {
                    cbListCameras.Items.Add(VideoCaptureDevice.Name);
                    if (VideoCaptureDevice.Name.Equals(BaseDados.nomeCamera) && indexCamera < 0)
                    {
                        indexCamera = count;
                    }
                    else
                    {
                        count++;
                    }
                }
                if(indexCamera >= 0)
                {
                    cbListCameras.SelectedIndex = indexCamera;
                }
                else
                {
                    cbListCameras.SelectedIndex = 0;
                }

                if (videoCamera != null)
                {
                    videoCamera.SignalToStop();
                    while (videoCamera.IsRunning)
                    {
                        Thread.Sleep(0500);
                    }
                    videoCamera = null;
                }
                iniciaVideo(cbListCameras.SelectedIndex);
                btCapturar.Enabled = true;
            } else
            {
                MessageBox.Show("Nenhuma câmera foi encontrada." +
                    "\nVerifique se há uma câmera conectada e pressione o botão \"Atualizar\"");
            }
        }
    }
}
