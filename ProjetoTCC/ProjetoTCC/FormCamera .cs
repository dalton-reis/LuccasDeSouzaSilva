using Accord.Video;
using Accord.Video.DirectShow;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ProjetoTCC
{
    public partial class FormCamera : Form
    {
        public Bitmap foto { get; private set; } = null;

        public FormCamera()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.FormClosing += new FormClosingEventHandler(this.onFormClosing);

            pbCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Right)));

            this.pnlFoto.Location = new System.Drawing.Point(12, 12);
            this.pnlFoto.Name = "pnlFoto";
            this.pnlFoto.Size = new System.Drawing.Size(270, 270);

            iniciaVideo();
            this.pnlFoto.Refresh();
        }

        private VideoCaptureDevice videoCamera = null;

        public void iniciaVideo()
        {
            if (videoCamera == null)
            {
                // enumerate video devices
                FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (VideoDevices.Count > 0)
                {
                    int indexCamera = -1;
                    int count = 0;
                    foreach (FilterInfo VideoCaptureDevice in VideoDevices)
                    {
                        Debug.WriteLine(VideoCaptureDevice.Name + " __ " + VideoCaptureDevice.MonikerString);
                        if (VideoCaptureDevice.Name.Equals(Biblioteca.nomeCamera))
                        {
                            indexCamera = count;
                            break;
                        }
                        else
                        {
                            count++;
                        }
                    }
                    //Integrated Webcam __ @device:pnp:\\?\usb#vid_064e&pid_9202&mi_00#7&1cef3b3&0&0000#{65e8773d-8f56-11d0-a3b9-00a0c9223196}\global

                    bool iniciaCamera = false;
                    if (indexCamera >= 0)
                    {
                        videoCamera = new VideoCaptureDevice(VideoDevices[indexCamera].MonikerString);
                        iniciaCamera = true;
                    }
                    else
                    {
                        //seleciona na hora
                        videoCamera = new VideoCaptureDevice(VideoDevices[0].MonikerString);
                        iniciaCamera = true;
                    }
                    if (iniciaCamera)
                    {
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
                        finally
                        {
                        }
                    }
                }
                else
                {

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
    }
}
