using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WorldSimulator
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
        private List<Paciente> gridListaPacientes;

        public int selectedIndex = 0;
        public Paciente pacienteSelecionado = null;

        private DataGridViewCellEventHandler dtGridRowSelectHandler = null;

        public PainelPaciente()
        {
        }

        public void iniPainelPaciente(List<Paciente> listaPacientes)
        {
            initPainel(this);

            this.listaPacientes = listaPacientes;
            this.gridListaPacientes = this.listaPacientes.OrderBy(p => p.nome).ToList();

            this.btEsq.Click += new System.EventHandler(this.btEsq_Click);
            this.btDir.Click += new System.EventHandler(this.btDir_Click);

            alteraModo(0);

            if(this.gridListaPacientes.Count > 0)
            {
                this.selectedIndex = 0;
            } else
            {
                this.selectedIndex = -1;
            }
        }

        public long getPacienteID()
        {
            if (this.selectedIndex >= 0 && this.gridListaPacientes.Count > this.selectedIndex)
            {
                return this.gridListaPacientes.ElementAt(this.selectedIndex).ID;
            }
            return -1;
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

            if (this.dtGridRowSelectHandler != null)
            {
                this.dtGrid.RowEnter += this.dtGridRowSelectHandler;
            }

            DataGridViewTextBoxColumn ColID = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColName = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColDataNasc = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColCpf = new DataGridViewTextBoxColumn();
            DataGridViewButtonColumn ColBt = new DataGridViewButtonColumn();

            ColID.Visible = false;

            ColName.HeaderText = "Nome";
            ColName.Name = "Nome";

            ColDataNasc.HeaderText = "Data Nasc";
            ColDataNasc.Name = "Data Nasc";

            ColCpf.HeaderText = "CPF";
            ColCpf.Name = "CPF";

            ColBt.HeaderText = "";
            ColBt.Name = "";
            ColBt.Width = 20;

            this.dtGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.dtGrid.Columns.Add(ColID);
            this.dtGrid.Columns.Add(ColName);
            this.dtGrid.Columns.Add(ColDataNasc);
            this.dtGrid.Columns.Add(ColCpf);
            this.dtGrid.Columns.Add(ColBt);

            foreach (Paciente p in gridListaPacientes)
            {
                string[] row = new string[] { p.ID.ToString(), p.nome, p.dataNasc.ToString("dd/MM/yyyy"), p.cpf, "+" };
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

            Label lbID = new System.Windows.Forms.Label();
            Label lbNome = new System.Windows.Forms.Label();
            TextBox tbNome = new System.Windows.Forms.TextBox();
            PictureBox pbFoto = new System.Windows.Forms.PictureBox();
            Label lbDataNasc = new System.Windows.Forms.Label();
            DateTimePicker dtpDataNasc = new System.Windows.Forms.DateTimePicker();
            Label lbDescricao = new System.Windows.Forms.Label();
            TextBox tbDescricao = new System.Windows.Forms.TextBox();

            this.pnlEdit.Controls.Add(pbFoto);
            this.pnlEdit.Controls.Add(lbID);
            this.pnlEdit.Controls.Add(lbNome);
            this.pnlEdit.Controls.Add(tbNome);
            this.pnlEdit.Controls.Add(lbDataNasc);
            this.pnlEdit.Controls.Add(dtpDataNasc);
            this.pnlEdit.Controls.Add(lbDescricao);
            this.pnlEdit.Controls.Add(tbDescricao);

            pbFoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Right)));
            pbFoto.Size = new System.Drawing.Size(200, 200);
            pbFoto.Location = new System.Drawing.Point(this.pnlEdit.Size.Width - pbFoto.Size.Width, 0);
            pbFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pbFoto.Name = "pbFoto";
            pbFoto.TabIndex = 0;
            pbFoto.TabStop = false;

            Bitmap bmpImg = ResizeImage(Image.FromFile(@"resources/img/foto.png"), pbFoto.Size.Width, pbFoto.Size.Height);

            Image img = Image.FromHbitmap(bmpImg.GetHbitmap());

            pbFoto.ErrorImage = img;
            pbFoto.InitialImage = img;

            if (this.pacienteSelecionado.caminhoFoto.Length > 0)
            {
                pbFoto.ImageLocation = this.pacienteSelecionado.caminhoFoto;
            }
            else
            {
                pbFoto.ImageLocation = @"resources/img/foto.png";
            }

            lbID.AutoSize = false;
            lbID.Location = new System.Drawing.Point(3, 2);
            lbID.Name = "lbID";
            lbID.Size = new System.Drawing.Size(210, 20);
            lbID.Text = "ID: " + this.pacienteSelecionado.ID;
            lbID.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom))
            );

            lbNome.AutoSize = false;
            lbNome.Location = new System.Drawing.Point(lbID.Location.X, lbID.Location.Y + lbID.Size.Height + 2);
            lbNome.Name = "lbNome";
            lbNome.Size = new System.Drawing.Size(40, 20);
            lbNome.Text = "Nome: ";
            lbNome.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom))
            );

            tbNome.Location = new System.Drawing.Point(lbNome.Location.X + lbNome.Size.Width, lbNome.Location.Y);
            tbNome.Name = "tbNome";
            tbNome.Text = this.pacienteSelecionado.nome;
            tbNome.Size = new System.Drawing.Size(175, lbNome.Size.Height);
            tbNome.TabIndex = 1;
            tbNome.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom))
            );

            lbDataNasc.AutoSize = false;
            lbDataNasc.Location = new System.Drawing.Point(lbNome.Location.X, lbNome.Location.Y + lbNome.Size.Height + 2);
            lbDataNasc.Name = "lbDataNasc";
            lbDataNasc.Size = new System.Drawing.Size(40, 20);
            lbDataNasc.TabIndex = 0;
            lbDataNasc.Text = "Data Nasc: ";
            lbDataNasc.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom))
            );

            dtpDataNasc.Location = new System.Drawing.Point(lbDataNasc.Location.X + lbDataNasc.Size.Width, lbDataNasc.Location.Y);
            dtpDataNasc.Name = "dtpDataNasc";
            dtpDataNasc.Size = new System.Drawing.Size(120, 22);
            dtpDataNasc.TabIndex = 2;
            dtpDataNasc.MaxDate = DateTime.Now;
            dtpDataNasc.ShowUpDown = false;
            dtpDataNasc.ShowCheckBox = false;
            dtpDataNasc.Format = DateTimePickerFormat.Custom;
            dtpDataNasc.CustomFormat = "dd/MM/yyyy";
            dtpDataNasc.Value = this.pacienteSelecionado.dataNasc;
            dtpDataNasc.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom))
            );

            lbDescricao.AutoSize = false;
            lbDescricao.Location = new System.Drawing.Point(lbDataNasc.Location.X, lbDataNasc.Location.Y + lbDataNasc.Size.Height + 2);
            lbDescricao.Name = "lbDescricao";
            lbDescricao.Size = new System.Drawing.Size(40, 20);
            lbDescricao.TabIndex = 0;
            lbDescricao.Text = "Descrição:";
            lbDescricao.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom))
            );

            tbDescricao.Location = new System.Drawing.Point(lbDescricao.Location.X, lbDescricao.Location.Y + lbDescricao.Size.Height);
            tbDescricao.Name = "tbDescricao";
            tbDescricao.Text = this.pacienteSelecionado.descricao;
            tbDescricao.Size = new System.Drawing.Size(200, lbDescricao.Size.Height * 5);
            tbDescricao.Multiline = true;
            tbDescricao.TabIndex = 1;
            tbDescricao.Anchor = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom))
            );
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
                createGrid();
            } else //modo == 1
            {
                long ID = long.Parse(this.dtGrid.CurrentRow.Cells[0].Value.ToString());

                this.pacienteSelecionado = gridListaPacientes.Where(p => p.ID == ID).First();

                this.pnlArea.Controls.Remove(dtGrid);
                this.disposeGrid();
                
                createEdit();
            }
        }
        
        public void SelecionaLinha(int index)
        {
            this.selectedIndex = index;
        }

        public void setGridRowSelectionChange(DataGridViewCellEventHandler handler)
        {
            this.dtGridRowSelectHandler = new DataGridViewCellEventHandler(handler);
            this.dtGrid.RowEnter += this.dtGridRowSelectHandler;
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
            this.pacienteSelecionado = new Paciente("", DateTime.Now, "", "", "", "");
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

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

    }
}
