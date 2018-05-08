using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DirectShowLib;
using System.Runtime.InteropServices;

namespace WorldSimulator
{
    public partial class MainWindow : Form
    {
        static Mundo mundo;
        static Thread th;
        public NeuroController neuroControl;
        public static Queue<NeuroData> NeuroDataQueue;
        public static bool IsReadMindWaveData;

        public static List<string> listDataAlpha1;
        public static List<string> listDataAlpha2;

        public static List<string> listDataBeta1;
        public static List<string> listDataBeta2;

        public static List<string> listDataGamma1;
        public static List<string> listDataGamma2;

        public static List<string> listDataDelta;
        public static List<string> listDataTheta;

        public static Stopwatch startTime;
        public static int status = 0;

        bool newSimulation = false;

        public MainWindow()
        {
            mundo = new Mundo(this);

            
            
            InitializeComponent();

            neuroControl = new NeuroController();
            NeuroDataQueue = new Queue<NeuroData>();
            IsReadMindWaveData = false;
            pbSinal.ImageLocation = @"resources/icons/nosignal.png";

            chAlpha.Series.Add("Alpha1");
            chAlpha.Series.Add("Alpha2");

            chBeta.Series.Add("Beta1");
            chBeta.Series.Add("Beta2");

            chGamma.Series.Add("Gamma1");
            chGamma.Series.Add("Gamma2");

            chDelta.Series.Add("Delta");
            chTheta.Series.Add("Theta");

            btCamAction.Text = "Iniciar Camera";

            lbCamDevices.Items.Clear();

            DsDevice[] capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            foreach (DsDevice dd in capDevices)
            {
                lbCamDevices.Items.Add(dd.Name);
            }
        }

        public void createVars()
        {
            listDataAlpha1 = new List<string>();
            listDataAlpha2 = new List<string>();
            listDataBeta1 = new List<string>();
            listDataBeta2 = new List<string>();
            listDataGamma1 = new List<string>();
            listDataGamma2 = new List<string>();
            listDataDelta = new List<string>();
            listDataTheta = new List<string>();

            chAlpha.Series["Alpha1"].BorderWidth = 4;
            chAlpha.Series["Alpha1"].ChartType = SeriesChartType.FastLine;
            chAlpha.Series["Alpha1"].BorderColor = Color.AliceBlue;

            chAlpha.Series["Alpha2"].BorderWidth = 4;
            chAlpha.Series["Alpha2"].ChartType = SeriesChartType.FastLine;
            chAlpha.Series["Alpha2"].BorderColor = Color.Red;

            chAlpha.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;

            chBeta.Series["Beta1"].BorderWidth = 4;
            chBeta.Series["Beta1"].ChartType = SeriesChartType.FastLine;
            chBeta.Series["Beta1"].BorderColor = Color.AliceBlue;

            chBeta.Series["Beta2"].BorderWidth = 4;
            chBeta.Series["Beta2"].ChartType = SeriesChartType.FastLine;
            chBeta.Series["Beta2"].BorderColor = Color.Red;

            chBeta.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;

            chGamma.Series["Gamma1"].BorderWidth = 4;
            chGamma.Series["Gamma1"].ChartType = SeriesChartType.FastLine;
            chGamma.Series["Gamma1"].BorderColor = Color.AliceBlue;

            chGamma.Series["Gamma2"].BorderWidth = 4;
            chGamma.Series["Gamma2"].ChartType = SeriesChartType.FastLine;
            chGamma.Series["Gamma2"].BorderColor = Color.Red;

            chGamma.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;

            chDelta.Series["Delta"].BorderWidth = 4;
            chDelta.Series["Delta"].ChartType = SeriesChartType.FastLine;
            chDelta.Series["Delta"].BorderColor = Color.AliceBlue;

            chDelta.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;

            chTheta.Series["Theta"].BorderWidth = 4;
            chTheta.Series["Theta"].ChartType = SeriesChartType.FastLine;
            chTheta.Series["Theta"].BorderColor = Color.Red;

            chTheta.ChartAreas["ChartArea1"].AxisX.LabelStyle.Enabled = false;
        }

        public void cleanVars()
        {
            listDataAlpha1 = null;
            listDataAlpha2 = null;
            listDataBeta1 = null;
            listDataBeta2 = null;
            listDataGamma1 = null;
            listDataGamma2 = null;
            listDataDelta = null;
            listDataTheta = null;
        }

        public void UpdateStats(NeuroData nr)
        {
 //           Debug.WriteLine((double)startTime.ElapsedMilliseconds / 1000);
            listDataAlpha1.Add((double)startTime.ElapsedMilliseconds / 1000 + ";" + nr.Alpha1);
            listDataAlpha2.Add((double)startTime.ElapsedMilliseconds / 1000 + ";" + nr.Alpha2);
            listDataBeta1.Add((double)startTime.ElapsedMilliseconds / 1000 + ";" + nr.Beta1);
            listDataBeta2.Add((double)startTime.ElapsedMilliseconds / 1000 + ";" + nr.Beta2);
            listDataGamma1.Add((double)startTime.ElapsedMilliseconds / 1000 + ";" + nr.Gamma1);
            listDataGamma2.Add((double)startTime.ElapsedMilliseconds / 1000 + ";" + nr.Gamma2);
            listDataDelta.Add((double)startTime.ElapsedMilliseconds / 1000 + ";" + nr.Delta);
            listDataTheta.Add((double)startTime.ElapsedMilliseconds / 1000 + ";" + nr.Theta);

            if (listDataAlpha1.Count > 15)
            {
                listDataAlpha1.RemoveAt(0);
                listDataAlpha2.RemoveAt(0);
                listDataBeta1.RemoveAt(0);
                listDataBeta2.RemoveAt(0);
                listDataGamma1.RemoveAt(0);
                listDataGamma2.RemoveAt(0);
                listDataDelta.RemoveAt(0);
                listDataTheta.RemoveAt(0);
            }

            chAlpha.Series["Alpha1"].Points.Clear();
            chAlpha.Series["Alpha2"].Points.Clear();
            chBeta.Series["Beta1"].Points.Clear();
            chBeta.Series["Beta2"].Points.Clear();
            chGamma.Series["Gamma1"].Points.Clear();
            chGamma.Series["Gamma2"].Points.Clear();
            chDelta.Series["Delta"].Points.Clear();
            chTheta.Series["Theta"].Points.Clear();

            for (int i = 0; i < listDataAlpha1.Count; i++)
            {
                string[] str = listDataAlpha1.ElementAt(i).Split(';');
                chAlpha.Series["Alpha1"].Points.AddXY(Double.Parse(str[0]), long.Parse(str[1]));

                str = listDataAlpha2.ElementAt(i).Split(';');
                chAlpha.Series["Alpha2"].Points.AddXY(Double.Parse(str[0]), long.Parse(str[1]));

                str = listDataBeta1.ElementAt(i).Split(';');
                chBeta.Series["Beta1"].Points.AddXY(Double.Parse(str[0]), long.Parse(str[1]));

                str = listDataBeta2.ElementAt(i).Split(';');
                chBeta.Series["Beta2"].Points.AddXY(Double.Parse(str[0]), long.Parse(str[1]));

                str = listDataGamma1.ElementAt(i).Split(';');
                chGamma.Series["Gamma1"].Points.AddXY(Double.Parse(str[0]), long.Parse(str[1]));

                str = listDataGamma2.ElementAt(i).Split(';');
                chGamma.Series["Gamma2"].Points.AddXY(Double.Parse(str[0]), long.Parse(str[1]));

                str = listDataDelta.ElementAt(i).Split(';');
                chDelta.Series["Delta"].Points.AddXY(Double.Parse(str[0]), long.Parse(str[1]));

                str = listDataTheta.ElementAt(i).Split(';');
                chTheta.Series["Theta"].Points.AddXY(Double.Parse(str[0]), long.Parse(str[1]));
            }

            chAlpha.ChartAreas["ChartArea1"].AxisX.Title = "Tempo execução: " + ((double)startTime.ElapsedMilliseconds / 1000);
        }

        private void btAbrirMundo_Click(object sender, EventArgs e)
        {
            pbSinal.ImageLocation = @"resources/icons/connecting_1.png";
            MessageBox.Show("Verificando conexão com dispositivo...");
            bool connect = neuroControl.testConnection();
            if (connect)
            {
                pbSinal.ImageLocation = @"resources/icons/connecting_2.png";
                bool IsReadMindWaveData = neuroControl.preReadData();
                pbSinal.ImageLocation = @"resources/icons/connecting_3.png";
                if (IsReadMindWaveData)
                {
                    pbSinal.ImageLocation = @"resources/icons/connected.png";
                    MessageBox.Show("Conexão efetuada com sucesso! Iniciando simulação...");
                    IsReadMindWaveData = true;
                    th = new Thread(readMindWaveData);

                    createVars();

                    th.Start();

                    if (mundo == null || newSimulation)
                    {
                        mundo = new Mundo(this);
                    }
                    newSimulation = true;
                    mundo.Start();
                } else
                {
                    MessageBox.Show("Não foi possível estabelecer uma conexão com o dispositivo.\n" +
                        "Verifique o pareamento e bateria e tente novamente.");
                }
            } else
            {
                pbSinal.ImageLocation = @"resources/icons/nosignal.png";
                MessageBox.Show("Não foi possível conectar com o dispositivo.\n" +
                    "Verifique o pareamento e bateria e tente novamente.");
            }
        }

        private void readMindWaveData()
        {
            IsReadMindWaveData = true;
            NeuroDataQueue.Clear();
            bool[] toReadData = new bool[11] { true, true, true, true, true, true, true, true, true, true, true };
            startTime = new Stopwatch();
            startTime.Start();

            bool sleep = false;

            while (IsReadMindWaveData)
            {
                //Debug.WriteLine("InsideWhile");
                NeuroData nr = neuroControl.readData(toReadData);
                if (nr != null)
                {
                    if(nr.getTotalPower() > 0) {
                        sleep = false;
                    } else
                    {
                        sleep = true;
                    }
                    lock (NeuroDataQueue)
                    {
                        NeuroDataQueue.Enqueue(nr);
                    }
                } else
                {
                    sleep = true;                    
                }
                if (sleep)
                {
                    Thread.Sleep(1);
                }
            }
        }

        public NeuroData getMindWaveData()
        {
            if (NeuroDataQueue.Count > 0)
            {
                NeuroData nr = null;
                lock (NeuroDataQueue) {
                    nr = NeuroDataQueue.Dequeue();
                }
                UpdateStats(nr);
                return nr;
            } else
            {
                return null;
            }
        }

        private void btFecharMundo_Click(object sender, EventArgs e)
        {
            if (IsReadMindWaveData)
            {
                finishSimul();
                if (mundo != null)
                {
                    mundo.Stop();
                }
                mundo = null;
                newSimulation = false;
            }
        }

        private void finishSimul()
        {
            startTime.Stop();
            IsReadMindWaveData = false;
            neuroControl.closeReader();
            pbSinal.ImageLocation = @"resources/icons/nosignal.png";
            cleanVars();
            Debug.WriteLine("Finalizando simulação.");
        }

        public void finishSimulation()
        {
            finishSimul();
        }

        Capture cam = null;

        private void btCamAction_Click(object sender, EventArgs e)
        {
            if (cam == null)
            {
                if (lbCamDevices.SelectedIndex > -1)
                {

                    int VIDEODEVICE = lbCamDevices.SelectedIndex; // zero based index of video capture device to use
                    const int VIDEOWIDTH = 640; // Depends on video device caps
                    const int VIDEOHEIGHT = 480; // Depends on video device caps
                    const int VIDEOBITSPERPIXEL = 24; // BitsPerPixel values determined by device

                    int[] VIDEOCONFIG = new int[] { VIDEODEVICE, VIDEOWIDTH, VIDEOHEIGHT, VIDEOBITSPERPIXEL };

                    cam = new Capture(VIDEOCONFIG, pictureBox2, "testFile.wmv");

                    cam.Start();
                    btCamAction.Text = "Parar Camera";
                } else
                {
                    MessageBox.Show("Nenhuma câmera foi selecionada para a gravação!");
                }
            }
            else
            {
                btCamAction.Text = "Iniciar Camera";

                // Pause the recording
                cam.Pause();

                // Close it down
                cam.Dispose();
                cam = null;
            }
        }
        
    }
}
