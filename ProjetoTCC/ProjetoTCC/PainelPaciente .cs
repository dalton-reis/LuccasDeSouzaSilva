using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProjetoTCC
{
    class PainelPaciente : PainelPadrao
    {

        private int modo = 0;

        private DataGridView dtGrid;
        private Panel pnlEdit;


        //N, L, V, S, D
        private char btEsqState = 'N';
        private char btDirState = 'L';

        private List<Paciente> listaPacientes;

        public int selectedIndex = 0;
        public Paciente pacienteSelecionado = null;

        public PainelPaciente(Panel pnlOrigem, List<Paciente> listaPacientes)
        {
            initPainel(pnlOrigem);

            this.listaPacientes = listaPacientes;

            this.btEsq.Click += new System.EventHandler(this.btEsq_Click);
            this.btDir.Click += new System.EventHandler(this.btDir_Click);

            alteraModo(0);
        }

        private void alteraModo(int mod)
        {
            modo = mod;

            if(modo == 0)
            {
                if (pnlEdit != null)
                {
                    this.pnlArea.Controls.Remove(pnlEdit);
                }

                btEsqState = 'N';
                btDirState = 'L';

                btEsq.Text = "Novo";
                btDir.Text = "Localiza";

                this.dtGrid = new DataGridView();
                ((System.ComponentModel.ISupportInitialize)(this.dtGrid)).BeginInit();
                pnlArea.Controls.Add(dtGrid);

                DataGridViewTextBoxColumn ColID = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn Col1 = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn Col2 = new DataGridViewTextBoxColumn();
                DataGridViewTextBoxColumn Col3 = new DataGridViewTextBoxColumn();
                DataGridViewButtonColumn Col4 = new DataGridViewButtonColumn();

                ColID.Visible = false;

                Col1.HeaderText = "Nome";
                Col1.Name = "Nome";
                
                Col2.HeaderText = "Data Nasc";
                Col2.Name = "Data Nasc";

                Col3.HeaderText = "CPF";
                Col3.Name = "CPF";

                Col4.HeaderText = "";
                Col4.Name = "";
                Col4.Width = 20;

                this.dtGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

                this.dtGrid.Columns.Add(ColID);
                this.dtGrid.Columns.Add(Col1);
                this.dtGrid.Columns.Add(Col2);
                this.dtGrid.Columns.Add(Col3);
                this.dtGrid.Columns.Add(Col4);

                foreach (Paciente p in listaPacientes)
                {
                    string[] row = new string[] { p.ID.ToString(), p.nome, p.dataNasc.ToString("dd/MM/yyyy"), p.cpf, "+"};
                    dtGrid.Rows.Add(row);
                }

                this.dtGrid.Location = new System.Drawing.Point(0, 0);
                this.dtGrid.Name = "dataGridView1";
                this.dtGrid.RowTemplate.Height = 20;
                this.dtGrid.Size = pnlArea.Size;
                this.dtGrid.ReadOnly = true;
                this.dtGrid.AllowUserToAddRows = false;
                this.dtGrid.AllowUserToDeleteRows = false;
                this.dtGrid.AllowUserToResizeRows = false;

                this.dtGrid.CellContentClick += new DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            } else //modo == 1
            {
                Debug.WriteLine("1 -----> Bt ID:" + this.dtGrid.CurrentRow.Cells[0].Value.ToString());

                long ID = long.Parse(this.dtGrid.CurrentRow.Cells[0].Value.ToString());

                this.pacienteSelecionado = listaPacientes.Where(p => p.ID == ID).First();

                if (dtGrid != null)
                {
                    this.pnlArea.Controls.Remove(dtGrid);
                    this.disposeGrid();
                }

                btEsqState = 'V';
                btEsq.Text = "Voltar";

                btDirState = 'S';
                btDir.Text = "Salvar";

                pnlEdit = new Panel();

                this.pnlArea.Controls.Add(pnlEdit);

                this.pnlEdit.BackColor = this.pnlArea.BackColor;
                this.pnlEdit.BorderStyle = this.pnlArea.BorderStyle;
                this.pnlEdit.Location = new System.Drawing.Point(-1, 0);
                this.pnlEdit.Name = "pnlArea";
                this.pnlEdit.Size = this.pnlArea.Size;
                this.pnlEdit.TabIndex = 0;

                Label lbID = new System.Windows.Forms.Label();
                Label lbNome = new System.Windows.Forms.Label();
                TextBox tbNome = new System.Windows.Forms.TextBox();

                this.pnlEdit.Controls.Add(lbID);
                this.pnlEdit.Controls.Add(lbNome);
                this.pnlEdit.Controls.Add(tbNome);

                lbID.AutoSize = false;
                lbID.Location = new System.Drawing.Point(3, 2);
                lbID.Name = "lbID";
                lbID.Size = new System.Drawing.Size(40, 20);
                lbID.TabIndex = 0;
                lbID.Text = "ID: " + this.pacienteSelecionado.ID;

                lbNome.AutoSize = false;
                lbNome.Location = new System.Drawing.Point(lbID.Location.X, lbID.Location.Y+lbID.Size.Height);
                lbNome.Name = "lbNome";
                lbNome.Size = new System.Drawing.Size(40, 20);
                lbNome.TabIndex = 0;
                lbNome.Text = "Nome: ";

                tbNome.Location = new System.Drawing.Point(lbNome.Location.X+lbNome.Size.Width, lbNome.Location.Y);
                tbNome.Name = "tbNome";
                tbNome.Text = this.pacienteSelecionado.nome;
                tbNome.Size = new System.Drawing.Size(150, lbNome.Size.Height);
                tbNome.TabIndex = 1;

                PictureBox pictureBox1 = new System.Windows.Forms.PictureBox();
                DateTimePicker dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            }
        }
        
        public void SelecionaLinha(int index)
        {
            this.selectedIndex = index;
        }

        public void setGridRowSelectionChange(DataGridViewCellEventHandler handler)
        {
            this.dtGrid.RowEnter += new DataGridViewCellEventHandler(handler);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                
                alteraModo(1);
            }
        }

        protected override void btEsq_Click(object sender, EventArgs e)
        {
            executeBtClick(btEsqState);            
        }
        
        protected override void btDir_Click(object sender, EventArgs e)
        {
            executeBtClick(btDirState);
        }

        private void executeBtClick(char btState)
        {
            switch (Char.ToUpper(btState))
            {
                case 'N':
                    clickNovo();
                    break;
                case 'L':
                    clickLocaliza();
                    break;
                case 'V':
                    clickVolta();
                    break;
                case 'S':
                    clickSalva();
                    break;
                case 'D':
                    clickDeleta();
                    break;
            }
        }

        private void clickNovo()
        {
            MessageBox.Show("Novo Paciente!");
            alteraModo(1);
        }

        private void clickLocaliza()
        {
            MessageBox.Show("Localiza Paciente!");
        }

        protected void clickVolta()
        {
//            MessageBox.Show("Voltar!");
            alteraModo(0);
        }

        protected void clickSalva()
        {
            MessageBox.Show("Salva Paciente!");
        }

        protected void clickDeleta()
        {
            MessageBox.Show("Deleta Paciente!");
        }

        private void disposeGrid()
        {
            dtGrid = null;
        }
    }
}
