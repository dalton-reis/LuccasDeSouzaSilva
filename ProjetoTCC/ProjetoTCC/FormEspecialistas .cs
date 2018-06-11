using System.Drawing;
using System.Windows.Forms;

namespace ProjetoTCC
{
    public partial class FormEspecialistas : Form
    {
        public FormEspecialistas()
        {
            InitializeComponent();

            iniConfig();
        }

        public void iniConfig()
        {
            this.pnlEspecialista.Location = new Point(0, 0);
            this.pnlEspecialista.Size = new Size((int)(this.ClientSize.Width), this.ClientSize.Height);

            Biblioteca.updateEspecialistas();
            pnlEspecialista.iniPainelEspecialista(Biblioteca.getEspecialistas());
        }
    }
}
