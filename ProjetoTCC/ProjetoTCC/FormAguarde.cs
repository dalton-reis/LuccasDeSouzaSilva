using System.ComponentModel;
using System.Windows.Forms;

namespace ProjetoTCC
{
    public partial class FormAguarde : Form
    {
        BackgroundWorker backWorker = null;

        bool isFinished = false;

        public FormAguarde(string mensagem, DoWorkEventHandler workEvent)
        {
            InitializeComponent();
            this.lbMsg.Text = mensagem;

            this.FormClosing += new FormClosingEventHandler(this.onFormClosing);

            this.isFinished = false;

            backWorker = new BackgroundWorker();

            backWorker.DoWork += workEvent;
            backWorker.RunWorkerCompleted += BackWorker_RunWorkerCompleted;

            //            backWorker.WorkerReportsProgress = true;
            //            backWorker.WorkerSupportsCancellation = true;

            backWorker.RunWorkerAsync();
        }

        private void BackWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.isFinished = true;
            this.Close();
        }

        private void onFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isFinished)
            {
                e.Cancel = true;
            }
        }
    }
}
