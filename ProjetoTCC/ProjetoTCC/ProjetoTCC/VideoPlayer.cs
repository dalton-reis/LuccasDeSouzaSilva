using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ProjetoTCC
{
    class VideoPlayer : Panel
    {
        public delegate void FrameProcessEventHandler(object sender, FrameProcessEventArgs e);

        private bool StartVideo = false;
        private bool PauseVideo = false;
        private PictureBox display = null;
        private Panel pnlButtons = null;
        private Button btStartPause = null;
        private Button btStop = null;
        private TrackBar videoTrackBar = null;
        private Label videoTimer = null;
        public string videoURL { get; private set;} = null;

        private Image PlayImage = ResizeImage(Image.FromFile(@"resources/img/play.png"), 25, 25);
        private Image PauseImage = ResizeImage(Image.FromFile(@"resources/img/pause.png"), 25, 25);
        private Image StopImage = ResizeImage(Image.FromFile(@"resources/img/stop.png"), 20, 20);

        private Thread thVideoProcess = null;

        public void setVideoURL(string url)
        {
            this.videoURL = url;
            initVideoTimer();
        }

        public bool isRunning { get; private set; }  = false;

        public FrameProcessEventHandler DisplayImagePipeline = null;        

        public class FrameProcessEventArgs : EventArgs
        {
            public FrameProcessEventArgs(Bitmap bitmap)
            {
                this.videoFrame = bitmap;
            }

            public Bitmap videoFrame { get; set; }
        }

        public void setSize(Size s)
        {
            this.Size = s;
            base.Size = s;
            ajustaPnlButtons();
            ajustaDisplay();
        }

        public VideoPlayer()
        {
            pnlButtons = new Panel();
            display = new PictureBox();
            this.Controls.Add(this.display);
            this.Controls.Add(this.pnlButtons);
            this.BackColor = Color.Transparent;
            ajustaPnlButtons();
            ajustaDisplay();

            this.Resize += executaBtEsqSessao;
        }

        public static Image ResizeImage(Image image, int width, int height)
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

        public void executaBtEsqSessao(object sender, EventArgs e)
        {
            ajustaPnlButtons();
            ajustaDisplay();
        }

        public void ajustaDisplay()
        {
            if (display == null)
            {
                display = new PictureBox();
                this.Controls.Add(this.display);
            }
            display.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));

            display.MinimumSize = new Size(300, 150);
            display.Size = new Size(this.Size.Width, (int)(this.Size.Height - 60));
            display.Location = new Point(0, 0);
            display.BorderStyle = BorderStyle.FixedSingle;
            display.Name = "display";
            display.BackColor = Color.Black;
        }

        public void ajustaPnlButtons()
        {
            if(this.pnlButtons == null)
            {
                this.pnlButtons = new Panel();
                this.Controls.Add(this.pnlButtons);
            }

            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom)));

            this.pnlButtons.BackColor = Color.White;
            this.pnlButtons.BorderStyle = BorderStyle.FixedSingle;
            this.pnlButtons.Location = new System.Drawing.Point(0, (int)(this.Size.Height * 0.8));
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new Size(this.Size.Width, 60);

            if (this.videoTrackBar == null)
            {
                this.videoTrackBar = new TrackBar();
                this.videoTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                this.pnlButtons.Controls.Add(this.videoTrackBar);
                this.videoTrackBar.Scroll += this.TrackBarScroll;
            }
            if (this.btStartPause == null)
            {
                this.btStartPause = new Button();
                this.btStartPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                this.pnlButtons.Controls.Add(this.btStartPause);
                setBtStartPauseImage(PlayImage);
                this.btStartPause.Click += new System.EventHandler(this.btStartPauseClick);
            }
            if (this.btStop == null)
            {
                this.btStop = new Button();
                this.btStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                this.pnlButtons.Controls.Add(this.btStop);
                setBtStopImage(StopImage);
                this.btStop.Click += new System.EventHandler(this.btStopClick);
            }
            if (this.videoTimer == null)
            {
                this.videoTimer = new Label();
                this.videoTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
                this.pnlButtons.Controls.Add(this.videoTimer);
            }

            this.btStartPause.Location = new Point(1,1);
            this.btStartPause.Text = "";
            this.btStartPause.MinimumSize = new Size(50, 50);
            this.btStartPause.Size = new Size(50, 50);

            this.btStop.Location = new Point(this.btStartPause.Location.X + this.btStartPause.Size.Width + 5, this.btStartPause.Location.Y);
            this.btStop.Text = "";
            this.btStop.MinimumSize = new Size(25, 25);
            this.btStop.Size = new Size(25, 25);
            
            this.videoTrackBar.TickStyle = TickStyle.None;
            this.videoTrackBar.Location = new Point(this.btStop.Location.X + this.btStop.Size.Width + 5, this.btStop.Location.Y);
            this.videoTrackBar.Name = "videoTrackBar";
            this.videoTrackBar.MinimumSize = new Size(10, 50);
            this.videoTrackBar.Size = new Size(this.pnlButtons.Size.Width - this.videoTrackBar.Location.X - 5, 100);
            this.videoTrackBar.SendToBack();

            this.videoTimer.Location = new Point(this.btStop.Location.X, this.btStop.Size.Height + 5 + this.btStop.Location.Y);
            this.videoTimer.AutoSize = false;
            this.videoTimer.Name = "videoTimer";
            this.videoTimer.Text = "0:00:00 / 0:00:00";
            this.videoTimer.Size = new System.Drawing.Size(200, 20);

            initVideoTimer();
        }

        int TrackBarScrollValue = -1;

        public void TrackBarScroll(object sender, EventArgs e)
        {
            TrackBar tBar = (TrackBar)sender;
            TrackBarScrollValue = tBar.Value;
        }

        public void btStopClick(object sender, EventArgs e)
        {
            this.StartVideo = false;
            this.PauseVideo = false;
            TrackBarScrollValue = 0;
            setTrackBarPosition(0);
        }

        public void btStartPauseClick(object sender, EventArgs e)
        {
            if (!this.StartVideo)
            {
                this.PauseVideo = false;
                startVideoThread();
            } else
            {
                this.PauseVideo = !this.PauseVideo;
                this.isRunning = !this.PauseVideo;
                if (this.PauseVideo)
                {
                    setBtStartPauseImage(PlayImage);
                } else
                {
                    setBtStartPauseImage(PauseImage);
                }
            }
        }

        private void startVideoThread()
        {
            if(this.videoURL != null && this.videoURL.Trim().Length > 0 && this.videoURL.EndsWith(".avi"))
            {
                if(File.Exists(this.videoURL))
                {
                    thVideoProcess = new Thread(VideoProcess);
                    this.StartVideo = true;
                    this.PauseVideo = false;
                    setBtStartPauseImage(PauseImage);
                    thVideoProcess.Start();
                    this.isRunning = true;
                } else
                {
                    MessageBox.Show("Arquivo de vídeo não encontrado!");
                }
            }
        }

        delegate void IntArgReturnVoidDelegate(int value);

        private void setTrackBarMax(int value)
        {
            if (this.videoTrackBar.InvokeRequired)
            {
                IntArgReturnVoidDelegate del = new IntArgReturnVoidDelegate(setTrackBarMax);
                this.BeginInvoke(del, new object[] { value });
            }
            else
            {
                this.videoTrackBar.Maximum = value;
            }
        }

        delegate void BitmapArgReturnVoidDelegate(Bitmap value);

        private void setDisplayImage(Bitmap value)
        {
            if (this.display.InvokeRequired)
            {
                BitmapArgReturnVoidDelegate del = new BitmapArgReturnVoidDelegate(setDisplayImage);
                this.BeginInvoke(del, new object[] { value });
            }
            else
            {
                var handler = DisplayImagePipeline;
                Bitmap frame = value;

                if (handler != null)
                {
                    handler(null, new FrameProcessEventArgs(value));
                }
                this.display.Image = ResizeImage(value, this.display.Width, this.display.Height);
            }
        }

        private void setTrackBarPosition(int value)
        {
            if (this.videoTrackBar.InvokeRequired)
            {
                IntArgReturnVoidDelegate del = new IntArgReturnVoidDelegate(setTrackBarPosition);
                this.BeginInvoke(del, new object[] { value });
            }
            else
            {
                this.videoTrackBar.Value = value;
            }
        }

        delegate int ReturnIntDelegate();

        private int getTrackBarPosition()
        {
            int ret = 0;
            if (this.videoTrackBar.InvokeRequired)
            {
                ReturnIntDelegate del = new ReturnIntDelegate(getTrackBarPosition);
                ret = (int) this.Invoke(del);
            }
            else
            {
                ret = this.videoTrackBar.Value;
            }
            return ret;
        }

        private void startVideoTimeSeconds(int value)
        {
            if (this.videoTimer.InvokeRequired)
            {
                IntArgReturnVoidDelegate del = new IntArgReturnVoidDelegate(startVideoTimeSeconds);
                this.BeginInvoke(del, new object[] { value });
            }
            else
            {
                string[] relogio = videoTimer.Text.Split('/');
                videoTimer.Text = "0:00:00 / " + segundosParaHora(value);
            }
        }

        private void setVideoTime(int value)
        {
            if (this.videoTimer.InvokeRequired)
            {
                IntArgReturnVoidDelegate del = new IntArgReturnVoidDelegate(setVideoTime);
                this.BeginInvoke(del, new object[] { value });
            } else
            {
                string[] relogio = videoTimer.Text.Split('/');
                videoTimer.Text = segundosParaHora(value) +  " / " + relogio[1].Trim();
            }
        }

        private string segundosParaHora(int valor)
        {
            string hora = "0";
            string min = "00";
            string seg = "00";

            if (valor <= 59)
            {
                seg = valor > 9 ? valor.ToString() : "0" + valor.ToString();
            }
            else
            {
                seg = (valor % 60) > 9 ? (valor % 60).ToString() : "0" + (valor % 60).ToString();
                valor = valor / 60;

                if(valor <= 59)
                {
                    min = valor > 9 ? valor.ToString() : "0" + valor.ToString();
                } else
                {
                    min = (valor % 60) > 9 ? (valor % 60).ToString() : "0" + (valor % 60).ToString();
                    hora = (valor / 60).ToString();
                }
            }
            return hora + ":" + min + ":" + seg;
        }

        private void initVideoTimer()
        {
            if (this.videoURL != null && this.videoURL.Trim().Length > 0 && this.videoURL.EndsWith(".avi"))
            {
                if (File.Exists(this.videoURL))
                {
                    Accord.Video.FFMPEG.VideoFileReader reader = new Accord.Video.FFMPEG.VideoFileReader();
                    reader.Open(this.videoURL);
                    int fps = (int)reader.FrameRate.Value;
                    double videoTimeSeconds = reader.FrameCount / reader.FrameRate.Value;
                    startVideoTimeSeconds((int)Math.Floor(videoTimeSeconds));
                    reader.Close();
                }
                else
                {
                    MessageBox.Show("Arquivo de vídeo não encontrado!");
                }
            }
        }

        private void VideoProcess() {
            Accord.Video.FFMPEG.VideoFileReader reader = new Accord.Video.FFMPEG.VideoFileReader();
            reader.Open(this.videoURL);
            int fps = (int) reader.FrameRate.Value;
            double videoTimeSeconds = reader.FrameCount / reader.FrameRate.Value;
            startVideoTimeSeconds((int) Math.Floor(videoTimeSeconds));
            setTrackBarMax((int) reader.FrameCount+1);
            setTrackBarPosition(0);
            int i = 0;
            Bitmap frame = reader.ReadVideoFrame(i);
            while ((this.StartVideo && frame != null) || (TrackBarScrollValue >= 0)) {
                if (!this.PauseVideo) {
                    if(TrackBarScrollValue >= 0) {
                        i = TrackBarScrollValue;
                        TrackBarScrollValue = -1;
                    } else {
                        i = getTrackBarPosition();
                    }
                    frame = reader.ReadVideoFrame(i);
                    if(frame != null) {
                        setDisplayImage(frame);
                        Thread.Sleep(0020);
                        setTrackBarPosition(i + 1);
                    }
                    double segundos = i / fps;
                    setVideoTime((int) Math.Floor(segundos));
                }
            }
            this.isRunning = false;
            this.StartVideo = false;
            this.PauseVideo = false;
            TrackBarScrollValue = 0;
            setTrackBarPosition(0);
            setVideoTime(0);
            setDisplayImage(reader.ReadVideoFrame(0));
            setBtStartPauseImage(PlayImage);
            reader.Close();
        }

        delegate void ImageArgVoidDelegate(Image value);

        private void setBtStartPauseImage(Image value)
        {
            if (this.btStartPause.InvokeRequired)
            {
                ImageArgVoidDelegate del = new ImageArgVoidDelegate(setBtStartPauseImage);
                this.BeginInvoke(del, new object[] { value });
            }
            else
            {
                this.btStartPause.Image = value;
            }
        }

        private void setBtStopImage(Image value)
        {
            if (this.btStop.InvokeRequired)
            {
                ImageArgVoidDelegate del = new ImageArgVoidDelegate(setBtStopImage);
                this.BeginInvoke(del, new object[] { value });
            }
            else
            {
                this.btStop.Image = value;
            }
        }

        public void SignalToStop()
        {
            this.PauseVideo = true;
            this.StartVideo = false;
            TrackBarScrollValue = -1;
            if (thVideoProcess != null)
            {
                thVideoProcess.Join();
            }
        }

    }
}
