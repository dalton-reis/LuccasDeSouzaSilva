using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POC_TelaDupla
{
    public partial class TelaMain : Form
    {
        int count = 0;
        static TelaSide telaSide = null;
        static bool threadAlive = false;
        static Thread t = null;

        public TelaMain()
        {
            InitializeComponent();
            Debug.Print("Inicia TelaMain");
        }

        private void btAbreSide_Click(object sender, EventArgs e)
        {
            Debug.Print("Abre tela");
            threadAlive = true;
            t = new Thread(testeThread);
            t.Start();
        }

        private void btFechaTela_Click(object sender, EventArgs e)
        {
            Debug.Print("Fecha tela");
            threadAlive = false;
        }

        static void testeThread()
        {
            Debug.Print("Inicia Thread");
            telaSide = new TelaSide();
            telaSide.Show();

            int i = 0;
            while (threadAlive)
            {
                Debug.Print("ThreadAlive");
                if (telaSide != null)
                {
                    Debug.Print("telaSide");
                    if (telaSide.isAlive)
                    {
                        Debug.Print("doMath");
                        i++;
                        telaSide.updateText1(i.ToString());
                    }
                }
                Thread.Sleep(1000);
            }
            Debug.Print("Fim Thread");

            if (telaSide != null)
            {
                telaSide.Close();
                telaSide = null;
            }

            t.Abort();
        }

        private void btCount_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                count++;
                lbCount.Text = count.ToString();
                lbCount.Refresh();
                Thread.Sleep(1000);
            }
        }
    }
}
