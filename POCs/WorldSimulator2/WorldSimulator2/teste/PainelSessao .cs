using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WorldSimulator
{
    class PainelSessao : PainelPadrao
    {
        private int modo = 0;

        private DataGridView dtGrid;
        private Panel pnlEdit;

        //N, L, V, S, D
        private char btEsqState = 'N';
        private char btDirState = 'L';

        private List<Sessao> listaSessoes;
        private List<Sessao> gridListaSessoes;

        public int selectedIndex = 0;
        public Sessao sessaoSelecionada = null;

        private Button btStartVideo = null;

        private Capture cam = null;

        private string nomeCamera = "";

        public PainelSessao()
        {
        }

        public void iniPainelSessao()
        {
            initPainel(this);

            this.btEsq.Click += new System.EventHandler(this.btEsq_Click);
            this.btDir.Click += new System.EventHandler(this.btDir_Click);
        }

        public void listaPacienteSessao(long pacienteID)
        {
            if (pacienteID >= 1)
            {
                this.listaSessoes = Biblioteca.getPacienteSessoes(pacienteID);
                this.gridListaSessoes = this.listaSessoes.OrderBy(s => s.dataSessao).ToList();
                alteraModo(0);
            }
        }

        private void createGrid()
        {
            btEsqState = 'N';
            btDirState = 'L';

            btEsq.Text = "Novo";
            btDir.Text = "Localiza";

            this.dtGrid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dtGrid)).BeginInit();
            pnlArea.Controls.Add(dtGrid);

            this.dtGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));

            DataGridViewTextBoxColumn ColID = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColTitulo = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColDataSessao = new DataGridViewTextBoxColumn();
            DataGridViewButtonColumn ColBt = new DataGridViewButtonColumn();

            ColID.Visible = false;

            ColTitulo.HeaderText = "Titulo";
            ColTitulo.Name = "Titulo";

            ColDataSessao.HeaderText = "Data/Hora";
            ColDataSessao.Name = "Data/Hora";

            ColBt.HeaderText = "";
            ColBt.Name = "";
            ColBt.Width = 20;

            this.dtGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.dtGrid.Columns.Add(ColID);
            this.dtGrid.Columns.Add(ColTitulo);
            this.dtGrid.Columns.Add(ColDataSessao);
            this.dtGrid.Columns.Add(ColBt);

            foreach (Sessao s in gridListaSessoes)
            {
                string[] row = new string[] { s.ID.ToString(), s.titulo, s.dataSessao.ToString("dd/MM/yyyy HH:mm"), "+" };
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
        }

        private void createEdit()
        {
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

            PictureBox pbVideo = new System.Windows.Forms.PictureBox();
            
            pbVideo.Size = new System.Drawing.Size(pnlEdit.Size.Width, 250);
            pbVideo.Location = new System.Drawing.Point(0, 0);
            pbVideo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pbVideo.Name = "pbVideo";
            pbVideo.TabIndex = 0;
            pbVideo.TabStop = false;
            //                pbVideo.Resize += new System.EventHandler(this.videoResize);

            this.pnlEdit.Controls.Add(pbVideo);

            btStartVideo = new Button();
            this.pnlEdit.Controls.Add(btStartVideo);

            

            btStartVideo.Size = new System.Drawing.Size(80, 30);
            btStartVideo.Location = new System.Drawing.Point(pbVideo.Location.X + pbVideo.Size.Width - btStartVideo.Size.Width - 5, pbVideo.Size.Height + 5);
            btStartVideo.Name = "btStartVideo";
            btStartVideo.TabIndex = 8;
            btStartVideo.Text = "Inicia Video";
            btStartVideo.UseVisualStyleBackColor = true;

            btStartVideo.Click += new System.EventHandler(this.btIniciaVideoClick);

            Label lbID = new System.Windows.Forms.Label();
            Label lbTitulo = new System.Windows.Forms.Label();
            TextBox tbTitulo = new System.Windows.Forms.TextBox();
            Label lbDataSessao = new System.Windows.Forms.Label();
            DateTimePicker dtpDataSessao = new System.Windows.Forms.DateTimePicker();
            Label lbDescricao = new System.Windows.Forms.Label();
            TextBox tbDescricao = new System.Windows.Forms.TextBox();

            this.pnlEdit.Controls.Add(lbID);
            this.pnlEdit.Controls.Add(lbTitulo);
            this.pnlEdit.Controls.Add(tbTitulo);
            this.pnlEdit.Controls.Add(lbDataSessao);
            this.pnlEdit.Controls.Add(dtpDataSessao);
            this.pnlEdit.Controls.Add(lbDescricao);
            this.pnlEdit.Controls.Add(tbDescricao);

            lbID.AutoSize = false;
            lbID.Location = new System.Drawing.Point(pbVideo.Location.X, pbVideo.Location.Y + pbVideo.Size.Height + 2);
            lbID.Name = "lbID";
            lbID.Size = new System.Drawing.Size(40, 20);
            lbID.TabIndex = 0;
            lbID.Text = "ID: " + this.sessaoSelecionada.ID;
            

            lbTitulo.AutoSize = false;
            lbTitulo.Location = new System.Drawing.Point(lbID.Location.X, lbID.Location.Y + lbID.Size.Height + 2);
            lbTitulo.Name = "lbNome";
            lbTitulo.Size = new System.Drawing.Size(40, 20);
            lbTitulo.TabIndex = 0;
            lbTitulo.Text = "Titulo:";
            

            tbTitulo.Location = new System.Drawing.Point(lbTitulo.Location.X + lbTitulo.Size.Width, lbTitulo.Location.Y);
            tbTitulo.Name = "tbTitulo";
            tbTitulo.Text = this.sessaoSelecionada.titulo;
            tbTitulo.Size = new System.Drawing.Size(200, lbTitulo.Size.Height);
            tbTitulo.TabIndex = 1;
            

            lbDataSessao.AutoSize = false;
            lbDataSessao.Location = new System.Drawing.Point(lbTitulo.Location.X, lbTitulo.Location.Y + lbTitulo.Size.Height + 2);
            lbDataSessao.Name = "lbDataNasc";
            lbDataSessao.Size = new System.Drawing.Size(40, 20);
            lbDataSessao.TabIndex = 0;
            lbDataSessao.Text = "Data Nasc:";
            

            dtpDataSessao.Location = new System.Drawing.Point(lbDataSessao.Location.X + lbDataSessao.Size.Width, lbDataSessao.Location.Y);
            dtpDataSessao.Name = "dtpDataNasc";
            dtpDataSessao.Size = new System.Drawing.Size(150, 22);
            dtpDataSessao.TabIndex = 2;
            dtpDataSessao.MaxDate = DateTime.Now;
            dtpDataSessao.Format = DateTimePickerFormat.Custom;
            dtpDataSessao.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpDataSessao.Value = this.sessaoSelecionada.dataSessao;
            

            lbDescricao.AutoSize = false;
            lbDescricao.Location = new System.Drawing.Point(lbDataSessao.Location.X, lbDataSessao.Location.Y + lbDataSessao.Size.Height + 2);
            lbDescricao.Name = "lbDescricao";
            lbDescricao.Size = new System.Drawing.Size(40, 20);
            lbDescricao.TabIndex = 0;
            lbDescricao.Text = "Descrição:";
            
            tbDescricao.Location = new System.Drawing.Point(lbDescricao.Location.X + lbDescricao.Size.Width, lbDescricao.Location.Y);
            tbDescricao.Name = "tbDescricao";
            tbDescricao.Text = this.sessaoSelecionada.descricao;
            tbDescricao.Size = new System.Drawing.Size(200, lbDescricao.Size.Height * 5);
            tbDescricao.Multiline = true;
            tbDescricao.TabIndex = 1;
            
        }

        private void alteraModo(int mod)
        {
            modo = mod;

            if(modo == 0)
            {
                if (this.dtGrid != null)
                {
                    this.disposeGrid();
                }
                if (this.pnlEdit != null)
                {
                    this.disposeEdit();
                }
                createGrid();
            } else //modo == 1
            {          
                long ID = long.Parse(this.dtGrid.CurrentRow.Cells[0].Value.ToString());

                this.sessaoSelecionada = gridListaSessoes.Where(s => s.ID == ID).First();

                this.pnlArea.Controls.Remove(dtGrid);
                this.disposeGrid();
                
                createEdit();
            }
        }

        public void videoResize(object sender, EventArgs e)
        {
            if(cam != null && false)
            {
                PictureBox pb = (PictureBox) pnlEdit.Controls.Find("pbVideo", true)[0];
                //cam.resize(0, pb.Size.Width, pb.Size.Height);
            }
        }

        public void btIniciaVideoClick(object sender, EventArgs e)
        {
            if(cam == null)
            {
                Control pnlEditControl = pnlEdit.Controls.Find("pbVideo", true)[0];
                Debug.WriteLine("Width: " + pnlEditControl.Size.Width + " Height: " + pnlEditControl.Size.Height);

                int VIDEODEVICE = 0; // zero based index of video capture device to use
                const int VIDEOWIDTH =  640; // Depends on video device caps
                const int VIDEOHEIGHT = 480; // Depends on video device caps
                const int VIDEOBITSPERPIXEL = 24; // BitsPerPixel values determined by device

                int[] VIDEOCONFIG = new int[] { VIDEODEVICE, pnlEditControl.Size.Width*2, pnlEditControl.Size.Height*2, VIDEOBITSPERPIXEL };

                btStartVideo.Text = "Parar video";

                cam = new Capture(VIDEOCONFIG, pnlEdit.Controls.Find("pbVideo", true)[0], "testFile.wmv");

                cam.Start();
            } else
            {
                btStartVideo.Text = "Iniciar Camera";
                // Pause the recording
                cam.Pause();
                // Close it down
                cam.Dispose();
                cam = null;
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
            Debug.WriteLine("--> !!! <---");
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
            MessageBox.Show("Nova Sessao!");
            alteraModo(1);
        }

        private void clickLocaliza()
        {
            MessageBox.Show("Localiza Sessao!");
        }

        protected void clickVolta()
        {
//            MessageBox.Show("Voltar!");
            alteraModo(0);
        }

        protected void clickSalva()
        {
            MessageBox.Show("Salva Sessao!");
        }

        protected void clickDeleta()
        {
            MessageBox.Show("Deleta Sessao!");
        }

        private void disposeGrid()
        {
            this.pnlArea.Controls.Remove(dtGrid);
            this.dtGrid = null;
        }

        private void disposeEdit()
        {
            this.pnlArea.Controls.Remove(pnlEdit);
            this.pnlEdit = null;
            this.cam = null;
            this.nomeCamera = null;
            this.btStartVideo = null;
        }
    }
}
