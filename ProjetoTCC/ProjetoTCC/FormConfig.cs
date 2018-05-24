using DirectShowLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetoTCC
{
    public partial class FormConfig : Form
    {
        public FormConfig()
        {
            InitializeComponent();

            lbFileFolder.Text = "Local dos arquivos: " + Biblioteca.caminhoArquivos;

            lbCamera.Items.Clear();

            DsDevice[] capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            lbCamera.ScrollAlwaysVisible = true;
            foreach (DsDevice dd in capDevices)
            {
                lbCamera.Items.Add(dd.Name);
                Debug.WriteLine(dd.Name + " _ " + dd.ClassID + " _ " + dd.Mon);
            }

            tbNomeEstab.Text = Biblioteca.nomeEstab;
            tbEnderecoEstab.Text = Biblioteca.enderecoEstab;
            tbFoneEstab.Text = Biblioteca.foneEstab;

            lbFileLogo.Text = "Imagem para Logo: " + Biblioteca.caminhoLogo;
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            Biblioteca.updateConfig(lbFileFolder.Text, lbCamera.SelectedIndex, tbNomeEstab.Text, tbEnderecoEstab.Text, tbFoneEstab.Text, lbFileLogo.Text);
            this.Close();
        }

        private void btFileChooser_Click(object sender, EventArgs e)
        {

        }

        private void btFIleLogo_Click(object sender, EventArgs e)
        {

        }

        private void btAtualizaCamera_Click(object sender, EventArgs e)
        {
            lbCamera.Items.Clear();
            DsDevice[] capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            foreach (DsDevice dd in capDevices)
            {
                lbCamera.Items.Add(dd.Name);
                Debug.WriteLine(dd.Name + " _ " + dd.ClassID + " _ " + dd.Mon + " _ " + dd.DevicePath);
            }
        }
    }
}
