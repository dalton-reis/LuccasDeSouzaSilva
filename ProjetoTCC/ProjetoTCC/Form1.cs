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
    public partial class Form1 : Form
    {

        private List<Paciente> listaPacientes = null;

        public Form1()
        {
            InitializeComponent();

            initListaPacientes();

            this.Controls.Remove(pnlEsq);

            pnlEsq = new PainelPaciente(pnlEsq, listaPacientes);
            ((PainelPaciente) pnlEsq).setGridRowSelectionChange(rowSelectionChange);

            this.Controls.Add(pnlEsq);


//            this.Controls.Remove(pnlDir);
//            pnlDir = new PainelPadraoA(pnlDir);
//            this.Controls.Add(pnlDir);            
        }

        private void initListaPacientes()
        {
            listaPacientes = new List<Paciente>();

            Paciente p1 = new Paciente("João de Souza Borges", DateTime.Parse("01/07/2012"), "", "", "", "");
            Paciente p2 = new Paciente("Carolina D'alencar", DateTime.Parse("16/01/2013"), "", "", "", "");
            Paciente p3 = new Paciente("Guilherme Antunes", DateTime.Parse("25/11/2011"), "", "", "", "");

            listaPacientes.Add(p1);
            listaPacientes.Add(p2);
            listaPacientes.Add(p3);
        }
        
        protected void rowSelectionChange(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView) sender;

            if (e.RowIndex > -1 && e.RowIndex != ((PainelPaciente)pnlEsq).selectedIndex)
            {
                ((PainelPaciente)pnlEsq).SelecionaLinha(e.RowIndex);
//                ((PainelSessao)pnlDir).atualizaLista(((PainelPaciente)pnlEsq).getPacienteSelecionado());
                Debug.WriteLine("2------ > Linha selecionada -> " + (0));
            }
        }
    }
}
