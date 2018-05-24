using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectShowLib;
//
//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text.pdf;

namespace ProjetoTCC
{
    public partial class Form1 : Form
    {
        Capture cam = null;

        public Form1()
        {
            InitializeComponent();

            //            iniConfig();

            //            initListaPacientes();

            //            pnlSessao.iniPainelSessao();
            //            pnlSessao.listaPacienteSessao(pnlPaciente.getPacienteID());
        }

        private void iniConfig()
        {
            //            Biblioteca.iniConfig();
            //this.FormClosing += new FormClosingEventHandler(this.onFormClosing);
        }

        private void initListaPacientes()
        {
            //Biblioteca.updatePacientes();
            //pnlPaciente.iniPainelPaciente(Biblioteca.getPacientes());
            //pnlPaciente.setGridRowSelectionChange(rowSelectionChange);
        }

        protected void rowSelectionChange(object sender, DataGridViewCellEventArgs e)
        {
            //var senderGrid = (DataGridView)sender;

            //if (e.RowIndex > -1 && e.RowIndex != pnlPaciente.selectedIndex)
            //{
            //    //pnlPaciente.SelecionaLinha(e.RowIndex);
            //    //pnlSessao.listaPacienteSessao(pnlPaciente.getPacienteID());
            //    //pnlSessao.Enabled = true;
            //}
        }

        private void btConfig_Click(object sender, EventArgs e)
        {
            //FormConfig formConfig = new FormConfig();
            //formConfig.ShowDialog();
            //formConfig = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cam == null)
            {
                DsDevice[] capDevices;

                // Get the collection of video devices
                capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

                int VIDEODEVICE = 0; // zero based index of video capture device to use
                const int VIDEOWIDTH = 640; // Depends on video device caps
                const int VIDEOHEIGHT = 480; // Depends on video device caps
                const int VIDEOBITSPERPIXEL = 24; // BitsPerPixel values determined by device

                //                int[] VIDEOCONFIG = new int[] { VIDEODEVICE, panel1.Size.Width, panel1.Size.Height, VIDEOBITSPERPIXEL };
                int[] VIDEOCONFIG = new int[] { VIDEODEVICE, VIDEOWIDTH, VIDEOHEIGHT, VIDEOBITSPERPIXEL };

                button2.Text = "Parar video";
                cam = new Capture(VIDEOCONFIG, pictureBox1, "testFile.wmv");

                //                cam = new Capture(VIDEODEVICE, VIDEOWIDTH, VIDEOHEIGHT, VIDEOBITSPERPIXEL, pictureBox1);

                cam.Start();
            }
            else
            {
                button2.Text = "Iniciar Camera";
                // Pause the recording
                cam.Pause();
                // Close it down
                cam.Dispose();
                cam = null;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ////Teste gera PDF
            //Document doc = new Document(PageSize.A4);//criando e estipulando o tipo da folha usada
            //doc.SetMargins(40, 40, 40, 80);//estibulando o espaçamento das margens que queremos
            //doc.AddCreationDate();//adicionando as configuracoes

            ////caminho onde sera criado o pdf + nome desejado
            ////OBS: o nome sempre deve ser terminado com .pdf
            //string caminho = @"teste_contrato.pdf";
            ////            string caminho = @"C:\Users\anton\Desktop\Projetos\projeto Borsari&Borsari\x\Relatorios\" + "CONTRATO.pdf";

            ////criando o arquivo pdf embranco, passando como parametro a variavel                
            ////doc criada acima e a variavel caminho 
            ////tambem criada acima.
            //PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));

            //doc.Open();

            //PdfPTable tbRow1 = new PdfPTable(new float[] { 40, 85, 15, 25 });
            //tbRow1.WidthPercentage = 100;
            //Paragraph[] row1 = { new Paragraph("Nome:"),
            //    new Paragraph("Luccas de Souza Silva"),
            //    new Paragraph("Sexo:"),
            //    new Paragraph("Masculino") };
            //foreach (Paragraph row in row1)
            //{
            //    tbRow1.AddCell(row);
            //}
            //doc.Add(tbRow1);

            //PdfPTable tbRow2 = new PdfPTable(new float[] { 40, 25, 35, 25, 15, 25 });
            //tbRow2.WidthPercentage = 100;
            //Paragraph[] row2 = { new Paragraph("Data de Nascimento:"),
            //    new Paragraph("03/04/1996"),
            //    new Paragraph("Data da Sessão:"),
            //    new Paragraph("12/05/2018"),
            //    new Paragraph("Idade:"),
            //    new Paragraph("1200 meses")
            //};

            //foreach (Paragraph row in row2)
            //{
            //    tbRow2.AddCell(row);
            //}
            //doc.Add(tbRow2);

            ////            doc.Add(table);
            ////fechando documento para que seja salva as alteraçoes.
            ////criando a variavel para paragrafo
            //Paragraph paragrafo = new Paragraph("", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14));
            ////etipulando o alinhamneto
            //paragrafo.Alignment = Element.ALIGN_JUSTIFIED; //Alinhamento Justificado
            //                                               //adicioando texto

            ////AQUI ONDE VAMOS ADICIONAR A VARIAVEL DO TIPO "Font"
            //paragrafo.Font = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14, (int)System.Drawing.FontStyle.Regular);

            ////adicionando outro paragrafo com o texto, para que seja feita a quebra de pagina.
            //paragrafo = new Paragraph("", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14));
            //paragrafo.Alignment = Element.ALIGN_JUSTIFIED; //Alinhamento Justificado
            //paragrafo.Font = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14, (int)System.Drawing.FontStyle.Regular);
            ////            paragrafo.Add(texto);
            //doc.Add(paragrafo);

            //doc.Close();
            ////Abrindo o arquivo após cria-lo.
            //System.Diagnostics.Process.Start(caminho);
        }

        //private void onFormClosing(object sender, FormClosingEventArgs e)
        //{            
        //}

    }
}