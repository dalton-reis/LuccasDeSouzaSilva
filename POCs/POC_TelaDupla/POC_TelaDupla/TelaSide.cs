using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POC_TelaDupla
{
    public partial class TelaSide : Form
    {
//        private TelaMain mainForm = null;
        public Boolean isAlive { get; private set; } = false;

        public TelaSide()
        {
            InitializeComponent();
            isAlive = true;
        }

        public TelaSide(Form form)
        {
 //           mainForm = form as TelaMain;
            InitializeComponent();
            isAlive = true;
        }

        public void updateText1(string text)
        {
            this.lbText1.Text = "A: " + text;
            this.lbText1.Refresh();
        }

        public void updateText2(string text)
        {
            this.lbText2.Text = "B: " + text;
            this.lbText2.Refresh();
        }
 
    }
}
