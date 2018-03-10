using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppTeste1
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Console.WriteLine("Teste");
            this.button2.Text = "Meh";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.button2.Text = "Nope";
            DialogResult res = MessageBox.Show("Teste Caramba!");
            this.button2.Text = "Ops";
        }
    }
}
