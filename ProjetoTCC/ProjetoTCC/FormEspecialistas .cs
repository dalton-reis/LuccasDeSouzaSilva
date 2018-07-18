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

            BaseDados.updateEspecialistas();
            pnlEspecialista.iniPainelEspecialista(BaseDados.getEspecialistas());

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }
    }
}
