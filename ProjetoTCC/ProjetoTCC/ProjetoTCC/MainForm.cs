using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

using System.Drawing;
using System.Text;

namespace ProjetoTCC
{
    public partial class MainForm : Form
    {
        Thread thBrainwaveReader = null;
        Thread thUpdateGraphs = null;
        Thread thVideoFrameReader = null;

        bool startBrainReader = false;
        bool startUpdateGraphs = false;

        bool isClosing = false;

        NeuroController BrainController = null;

        Queue<NeuroData> NeuroDataQueue = null;
        Queue<NeuroData> GraphDataQueue = null;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public MainForm()
        { 
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(this.onFormClosing);

            iniConfig();

            initPainelPacientes();
            initListaEspecialistas();

            initPainelSessao();

            BrainController = new NeuroController();

            vpBarMed.setBarColor(Brushes.LightBlue);
            vpBarMed.Value = 50;
            vpBarAtt.setBarColor(Brushes.LightGreen);
            vpBarAtt.Value = 50;
            vpBarQlty.setBarColor(Brushes.LightSeaGreen);
            vpBarQlty.Value = 0;

            lbForte.SendToBack();

            lbFraco.SendToBack();

            lbAlto.SendToBack();
            lbElevado.SendToBack();
            lbNormal.SendToBack();
            lbReduzido.SendToBack();

            enableBtDetalheGraph(false);
            displayPnlCharts(false);
        }

        public void displayPnlCharts(bool value)
        {
            if (pnlCharts != null)
            {
                pnlCharts.Enabled = value;
                pnlCharts.Visible = value;
                if (value)
                {
                    updateProgBarQlty(0);
                    updateProgBarAtt(0);
                    updateProgBarMed(0);
                    this.pnlPaciente.Size = new Size(this.pnlPaciente.Size.Width, this.pnlCharts.Location.Y - 10);                    
                } else
                {
                    this.pnlPaciente.Size = new Size(this.pnlPaciente.Size.Width, this.btConfig.Location.Y + this.btConfig.Size.Height - 6);
                }
                this.pnlPaciente.Refresh();
            } 
        }

        public void initPainelSessao()
        {
            pnlSessao.iniPainelSessao();

            pnlSessao.setBtEsqAction(executaBtEsqSessao);
            pnlSessao.setBtDirAction(executaBtDirSessao);

            pnlSessao.setConnectBrainEvent(connectBrainEvent);            

            pnlSessao.listaPacienteSessao(pnlPaciente.getPacienteID());

            pnlSessao.setGridCelContentClick(pnlSessaoGridCelContentClick);

            pnlSessao.setVideoPlayerDisplayHandler(videoPlayerDisplay);

            pnlSessao.setExcluirSessaoEvent(excluirSessaoEvent);
        }

        private void onFormClosing(object sender, FormClosingEventArgs e)
        {
            isClosing = true;

            startBrainReader = false;
            startUpdateGraphs = false;

            if (this.thBrainwaveReader != null)
            {
                this.thBrainwaveReader.Join();
            }

            if (this.thVideoFrameReader != null)
            {
                this.thVideoFrameReader.Join();
            }

            pnlSessao.dispose();

            pnlPaciente.dispose();

            pnlSessao.Dispose();
            pnlPaciente.Dispose();
        }

        private void videoPlayerDisplay(object sender, VideoPlayer.FrameProcessEventArgs e)
        {
            Bitmap frame = e.videoFrame;

            NeuroData nr = FrameToNeuro(frame);
            
            if(nr != null)
            {
                lock (GraphDataQueue)
                {
                    GraphDataQueue.Enqueue(nr);
                }
            }
        }        

        private NeuroData FrameToNeuro(Bitmap frame) {            
            byte[] aux = null;

            int[] nrData = new int[11];
            //PoorSignal, Meditation, Attention, Alpha1,
            //Alpha2, Beta1, Beta2, Gamma1, Gamma2, Delta, 
            //Theta;

            Color c = frame.GetPixel(0, 0);
            nrData[0] = c.R;
            nrData[1] = c.G;
            nrData[2] = c.B;

            for (int i = 1; i <= 7; i++)
            {
                c = frame.GetPixel(i, 0);
                aux = new byte[] { c.A, c.R, c.G, c.B };
                nrData[i+2] = BitConverter.ToInt32(aux, 0);
            }

            return new NeuroData(nrData[0], nrData[1], nrData[2], nrData[3],
                nrData[4], nrData[5], nrData[6], nrData[7], nrData[8], nrData[9], nrData[10]);            
        }

        public void executaBtEsqSessao(object sender, EventArgs e)
        {
            bool continua = true;

            if (pnlSessao.btEsqState.Equals('N'))
            {
                if (BaseDados.getEspecialistas().Count == 0)
                {
                    continua = false;
                    MessageBox.Show("Não há especialistas cadastrados no sistema. " +
                    "\nÉ necessário ao menos 1 especialista cadastrado para iniciar uma sessão.");
                }
            }
            if (continua)
            {
                pnlSessao.btEsq_Click();
                if (pnlSessao.btEsqState.Equals('N'))
                {
                    pnlSessao.setBrainReader(false);
                    startBrainReader = false;
                    startUpdateGraphs = false;
                    displayPnlCharts(false);
                } else
                {
                    displayPnlCharts(true);
                }
            }
        }

        public void executaBtEsqPaciente(object sender, EventArgs e)
        {
            pnlPaciente.btEsq_Click();
            if (pnlPaciente.btEsqState.Equals('N'))
            {
                startBrainReader = false;
                startUpdateGraphs = false;
            }
        }

        public void executaBtDirPaciente(object sender, EventArgs e)
        {
            bool salva = false;
            if (pnlPaciente.btDirState.Equals('S'))
            {
                salva = true;
            }
            pnlPaciente.btDir_Click();
            if (salva)
            {
                pnlPaciente.SelecionaLinha(0);
                pnlSessao.listaPacienteSessao(pnlPaciente.getPacienteID());
            }
            if (pnlPaciente.btDirState.Equals('L'))
            {
                pnlPaciente.enableBtDir(false);
            } else
            {
                pnlPaciente.enableBtDir(true);
            }
        }

        public void executaBtDirSessao(object sender, EventArgs e)
        {
            pnlSessao.btDir_Click();
            if (pnlSessao.btEsqState.Equals('N'))
            {
                pnlSessao.setBrainReader(false);
                startBrainReader = false;
                startUpdateGraphs = false;
                displayPnlCharts(false);
            }
            else
            {
                displayPnlCharts(true);
            }
        }

        public void iniciaThreadBrainwave()
        {
            startBrainReader = true;
            thBrainwaveReader = new Thread(BrainwaveReader);
            thBrainwaveReader.Start();
            iniciaThreadGraphs();
        }

        public void BrainwaveReader()
        {
            startBrainReader = true;
            NeuroDataQueue = new Queue<NeuroData>();
            GraphDataQueue = new Queue<NeuroData>();
            bool[] toReadData = new bool[11] { true, true, true,
                true, true, true, true, true, true, true, true };
            bool sleep = false;

            while (startBrainReader) {
                NeuroData nr = BrainController.readData(toReadData);
                if (nr != null && nr.getTotalPower() > 0) {
                    sleep = false;
                    lock (NeuroDataQueue) {
                        NeuroDataQueue.Enqueue(nr);
                    }
                    lock (GraphDataQueue) {
                        GraphDataQueue.Enqueue(nr);
                    }
                    if (pnlSessao.isRecording) {
                        pnlSessao.addNeuroDataVideo(nr);
                    }
                } else {
                    sleep = true;
                }
                if (sleep) {
                    Thread.Sleep(1);
                }
            }
            BrainController.closeReader();
            NeuroDataQueue = null;
            GraphDataQueue = null;
        }

        private void pnlSessaoGridCelContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                pnlSessao.GridCelContentClick(sender, e);
                displayPnlCharts(true);
                startUpdateGraphs = true;
                GraphDataQueue = new Queue<NeuroData>();
//                iniciaThreadVideoReader();
                iniciaThreadGraphs();
            }
        }      

        public void iniciaThreadGraphs()
        {
            enableBtDetalheGraph(true);
            thUpdateGraphs = new Thread(GraphsUpdater);
            thUpdateGraphs.Start();
        }

        FormGraphs formGraph = null;

        public void GraphsUpdater() {
            while (startUpdateGraphs) {
                NeuroData nr = null;
                lock (this.GraphDataQueue) {
                    if (this.GraphDataQueue != null 
                        && this.GraphDataQueue.Count > 0) {
                        try {
                            nr = this.GraphDataQueue.Dequeue();
                        } catch (Exception ex) {
                            nr = null;
                        }
                    }
                }                
                if (nr != null) {
                    if(nr.Meditation <= 100) {
                        this.updateProgBarMed(nr.Meditation);
                    }
                    if (nr.Attention <= 100) {
                        this.updateProgBarAtt(nr.Attention);
                    }

                    int p = (int) (((nr.PoorSignal*100)/255)*2);

                    if (p > 100 && (nr.Meditation > 15 || nr.Attention > 15)) {
                        p -= 100;
                    }

                    if (p > 200) {
                        p = 200;
                    } else if(p < 0) {
                        p = 0;
                    }

                    this.updateProgBarQlty(200 - p);
                    if (this.formGraph != null && this.formGraph.isGraphReady) {
                        this.formGraph.updateGraphs(nr);
                    }
                }
            }
            if (!isClosing) {
                this.enableBtDetalheGraph(false);
            }
        }

        delegate void BoolArgReturnVoidDelegate(bool value);

        private void enableBtDetalheGraph(bool value)
        {            
            if (this.btDetalheGraph.InvokeRequired)
            {
                BoolArgReturnVoidDelegate del = new BoolArgReturnVoidDelegate(enableBtDetalheGraph);
                if (this != null && !this.IsDisposed)
                {
                    this.Invoke(del, new object[] { value });
                }
            }
            else
            {
                this.btDetalheGraph.Enabled = value;
            }
        }

        delegate void IntArgReturnVoidDelegate(int value);

        private void updateProgBarMed(int value)
        {
            if (this.vpBarMed.InvokeRequired)
            {
                IntArgReturnVoidDelegate del = new IntArgReturnVoidDelegate(updateProgBarMed);
                if (this != null && !this.IsDisposed)
                {
                    this.Invoke(del, new object[] { value });
                }
            }
            else
            {
                this.vpBarMed.Value = value;
            }
        }

        private void updateProgBarAtt(int value)
        {
            if (this.vpBarAtt.InvokeRequired)
            {
                IntArgReturnVoidDelegate del = new IntArgReturnVoidDelegate(updateProgBarAtt);
                if (this != null && !this.IsDisposed)
                {
                    this.Invoke(del, new object[] { value });
                }
            }
            else
            {
                this.vpBarAtt.Value = value;
            }
        }

        private void updateProgBarQlty(int value)
        {
            if (this.vpBarQlty.InvokeRequired)
            {
                IntArgReturnVoidDelegate del = new IntArgReturnVoidDelegate(updateProgBarQlty);
                if (this != null && !this.IsDisposed)
                {
                    this.Invoke(del, new object[] { value });
                }
            }
            else
            {
                this.vpBarQlty.Value = value;
            }
        }

        private void iniConfig()
        {
            BaseDados.iniConfig();
        }
        
        private void initPainelPacientes()
        {
            BaseDados.updatePacientes();
            pnlPaciente.iniPainelPaciente(BaseDados.getPacientes());

            pnlPaciente.setBtEsqAction(executaBtEsqPaciente);
            pnlPaciente.setBtDirAction(executaBtDirPaciente);

            pnlPaciente.setGridRowSelectionChange(rowSelectionChange);
            pnlPaciente.setExcluirPacienteEvent(excluirPacienteEvent);
        }

        private void connectBrainEvent(object sender, EventArgs e)
        {
            startBrainReader = true;
            startUpdateGraphs = true;
            FormAguarde formAguarde = new FormAguarde("Verificando conexão com dispositivo MindWave.", BackWorker_MindWaveConnect);
            formAguarde.ShowDialog(this);
        }

        private void BackWorker_MindWaveConnect(object sender, DoWorkEventArgs e)
        {
            bool connect = BrainController.testConnection();
            if (connect)
            {
                bool IsReadMindWaveData = BrainController.preReadData();
                if (IsReadMindWaveData)
                {
                    MessageBox.Show("Conexão com MindWave efetuada com sucesso!");
                    iniciaThreadBrainwave();
                    pnlSessao.setBrainReader(true);
                    pnlSessao.enableBtConnectBrain(false);
                }
                else
                {
                    MessageBox.Show("Não foi possível estabelecer uma conexão com o dispositivo MindWave.\n" +
                        "Verifique o pareamento e bateria e tente novamente.");
                }
            }
            else
            {
                MessageBox.Show("Não foi possível conectar com o dispositivo MindWave.\n" +
                    "Verifique o pareamento e bateria e tente novamente.");
            }
        }

        private void initListaEspecialistas()
        {
            BaseDados.updateEspecialistas();
        }

        protected void excluirPacienteEvent(object sender, EventArgs e)
        {
            pnlPaciente.excluirPacienteSelecionado();
            pnlPaciente.SelecionaLinha(0);
            pnlSessao.listaPacienteSessao(pnlPaciente.getPacienteID());
        }

        protected void excluirSessaoEvent(object sender, EventArgs e)
        {
            pnlSessao.setBrainReader(false);
            startBrainReader = false;
            startUpdateGraphs = false;
            displayPnlCharts(false);
        }

        protected void rowSelectionChange(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView) sender;

            if (e.RowIndex > -1 && e.RowIndex != pnlPaciente.selectedIndex)
            {
                pnlPaciente.SelecionaLinha(e.RowIndex);
                pnlSessao.listaPacienteSessao(pnlPaciente.getPacienteID());
            }
        }

        private void btConfig_Click(object sender, EventArgs e)
        {
            FormConfig formConfig = new FormConfig
            {
                StartPosition = FormStartPosition.CenterParent
            };
            formConfig.ShowDialog();
            formConfig = null;
        }

        private void btEspecialistas_Click(object sender, EventArgs e)
        {
            FormEspecialistas formEspecialistas = new FormEspecialistas
            {
                StartPosition = FormStartPosition.CenterParent
            };
            formEspecialistas.ShowDialog();
            formEspecialistas = null;
        }

        private void btDetalheGraph_Click(object sender, EventArgs e)
        {
            if (formGraph == null)
            {
                formGraph = new FormGraphs();
                formGraph.Show();
            } else if(!formGraph.isGraphReady)
            {
                formGraph = new FormGraphs();
                formGraph.Show();
            }
        }
    }
}
