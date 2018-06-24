using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace ProjetoTCC
{
    class PainelEspecialista : PainelPadrao
    {
        private int modo = 0;

        private DataGridView dtGrid;
        private Panel pnlEdit;

        //N, L, V, S, D
        public char btEsqState { get; private set; } = 'N';
        public char btDirState { get; private set; } = 'L';

        private List<Especialista> listaEspecialistas;
        private List<Especialista> gridListaEspecialistas;

        public int selectedIndex = 0;
        public Especialista especialistaSelecionado = null;

        private DataGridViewCellEventHandler dtGridRowSelectHandler = null;

        private bool novoRegistro = false;
        private string caminhoFoto = "";

        public PainelEspecialista()
        {
        }

        private void updateBtEsqState(char value)
        {
            btEsqState = value;
        }

        private void updateBtDirState(char value)
        {
            btDirState = value;
            if (value == 'L')
            {
                enableBtDir(false);
            }
            else
            {
                enableBtDir(true);
            }
        }

        public void iniPainelEspecialista(List<Especialista> listaEspecialistas)
        {
            initPainel(this);

            updateBtEsqState('N');
            updateBtDirState('L');

            this.listaEspecialistas = listaEspecialistas;
            this.gridListaEspecialistas = this.listaEspecialistas.OrderBy(p => p.nome).ToList();

            this.btEsq.Click += new System.EventHandler(this.btEsq_Click);
            this.btDir.Click += new System.EventHandler(this.btDir_Click);

            this.btDel.Click += new System.EventHandler(this.btDel_Click);
            this.btDel.Enabled = false;
            this.btDel.Visible = false;
            btDel.Text = "Excluir";

            alteraModo(0);

            if (this.gridListaEspecialistas.Count > 0)
            {
                this.selectedIndex = 0;
            }
            else
            {
                this.selectedIndex = -1;
            }
        }

        private void inactivateButtons()
        {
            this.btEsq.Enabled = false;
            this.btEsq.Visible = false;

            this.btDir.Enabled = false;
            this.btDir.Visible = false;

            this.pnlArea.Visible = false;
            this.pnlArea.Enabled = false;
        }

        public long getEspecialistaID()
        {
            if (this.selectedIndex >= 0 && this.gridListaEspecialistas.Count > this.selectedIndex)
            {
                return long.Parse(this.dtGrid.Rows[this.selectedIndex].Cells[1].Value.ToString());
            }
            return -1;
        }

        private void createGrid()
        {
            updateBtEsqState('N');
            updateBtDirState('L');

            btEsq.Text = "Novo";
            btDir.Text = "Localiza";

            btDel.Enabled = false;
            btDel.Visible = false;

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

            ColID.HeaderText = "ID";
            ColID.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ColID.Name = "ID";
            if (listaEspecialistas.Count > 0)
            {
                ColID.Width = 15 + (listaEspecialistas.Max(p => p.ID).ToString().Length * 10);
            }
            else
            {
                ColID.Width = 25;
            }
            ColID.Visible = true;

            ColName.HeaderText = "Nome";
            ColName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            ColName.Name = "Nome";
            ColName.Width = 200;

            ColDataNasc.HeaderText = "Data Nasc";
            ColDataNasc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ColDataNasc.Name = "Data Nasc";
            ColDataNasc.Width = 98;

            ColCpf.HeaderText = "CPF";
            ColCpf.Name = "CPF";
            ColCpf.Width = 100;
            ColCpf.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            ColBt.HeaderText = "";
            ColBt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ColBt.Name = "";
            ColBt.Width = 25;
            ColBt.MinimumWidth = 20;
            ColBt.Resizable = DataGridViewTriState.False;

            this.dtGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.dtGrid.GridColor = Color.Black;
            this.dtGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            this.dtGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dtGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dtGrid.EnableHeadersVisualStyles = false;
            this.dtGrid.RowHeadersVisible = false;

            this.dtGrid.Columns.Add(ColBt);
            this.dtGrid.Columns.Add(ColID);
            this.dtGrid.Columns.Add(ColName);
            this.dtGrid.Columns.Add(ColDataNasc);
            this.dtGrid.Columns.Add(ColCpf);

            foreach (Especialista p in gridListaEspecialistas)
            {
                string[] row = new string[] { "+", p.ID.ToString(), p.nome, p.dataNasc.ToString("dd/MM/yyyy"), p.cpf };
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
            updateBtEsqState('V');
            updateBtDirState('S');

            btEsq.Text = "Voltar";
            btDir.Text = "Salvar";

            if (!novoRegistro)
            {
                btDel.Visible = true;
                btDel.Enabled = true;
            }
            else
            {
                btDel.Visible = false;
                btDel.Enabled = false;
            }

            this.pnlEdit = new Panel
            {
                Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right))),
                AutoScroll = true,
                BackColor = this.pnlArea.BackColor,
                BorderStyle = this.pnlArea.BorderStyle,
                Location = new System.Drawing.Point(-1, 0),
                Name = "pnlArea",
                Size = this.pnlArea.Size,
                TabIndex = 0
            };

            this.pnlArea.Controls.Add(pnlEdit);

            Label lbID = new System.Windows.Forms.Label();

            Label lbNome = new System.Windows.Forms.Label();
            TextBox tbNome = new System.Windows.Forms.TextBox();

            PictureBox pbFoto = new System.Windows.Forms.PictureBox();

            Label lbDataNasc = new System.Windows.Forms.Label();
            DateTimePicker dtpDataNasc = new System.Windows.Forms.DateTimePicker();

            Label lbCpf = new System.Windows.Forms.Label();
            TextBox tbCpf = new System.Windows.Forms.TextBox();

            Label lbRg = new System.Windows.Forms.Label();
            TextBox tbRg = new System.Windows.Forms.TextBox();

            Label lbSexo = new System.Windows.Forms.Label();
            RadioButton rbSexoM = new System.Windows.Forms.RadioButton();
            RadioButton rbSexoF = new System.Windows.Forms.RadioButton();
            RadioButton rbSexoO = new System.Windows.Forms.RadioButton();

            Label lbDescricao = new System.Windows.Forms.Label();
            TextBox tbDescricao = new System.Windows.Forms.TextBox();

            this.pnlEdit.Controls.Add(pbFoto);
            this.pnlEdit.Controls.Add(lbID);
            this.pnlEdit.Controls.Add(lbNome);
            this.pnlEdit.Controls.Add(tbNome);
            this.pnlEdit.Controls.Add(lbDataNasc);
            this.pnlEdit.Controls.Add(dtpDataNasc);

            this.pnlEdit.Controls.Add(lbCpf);
            this.pnlEdit.Controls.Add(tbCpf);
            this.pnlEdit.Controls.Add(lbRg);
            this.pnlEdit.Controls.Add(tbRg);

            this.pnlEdit.Controls.Add(lbSexo);
            this.pnlEdit.Controls.Add(rbSexoF);
            this.pnlEdit.Controls.Add(rbSexoM);
            this.pnlEdit.Controls.Add(rbSexoO);

            this.pnlEdit.Controls.Add(lbDescricao);
            this.pnlEdit.Controls.Add(tbDescricao);

            if (novoRegistro)
            {
                lbID.Text = "ID: " + Especialista.ProxID();
                tbNome.Text = "";
                dtpDataNasc.Value = DateTime.Now;
                tbCpf.Text = "";
                tbRg.Text = "";
                rbSexoF.Checked = true;
                rbSexoM.Checked = false;
                rbSexoO.Checked = false;
                tbDescricao.Text = "";
            }
            else
            {
                lbID.Text = "ID: " + this.especialistaSelecionado.ID;
                tbNome.Text = this.especialistaSelecionado.nome;
                dtpDataNasc.Value = this.especialistaSelecionado.dataNasc;
                tbCpf.Text = this.especialistaSelecionado.cpf;
                tbRg.Text = this.especialistaSelecionado.rg;
                rbSexoF.Checked = (this.especialistaSelecionado.sexo.Equals("F"));
                rbSexoM.Checked = (this.especialistaSelecionado.sexo.Equals("M"));
                rbSexoO.Checked = (this.especialistaSelecionado.sexo.Equals("O"));
                tbDescricao.Text = this.especialistaSelecionado.descricao;
            }

            pbFoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Right)));
            pbFoto.Size = new System.Drawing.Size(270, 270);
            pbFoto.Location = new System.Drawing.Point(this.pnlEdit.Size.Width - pbFoto.Size.Width - 1, 0);
            pbFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pbFoto.Name = "pbFoto";
            pbFoto.TabIndex = 0;
            pbFoto.TabStop = false;

            string FotoFile = @"resources/img/foto.png";

            if (!novoRegistro && this.especialistaSelecionado != null)
            {
                Directory.CreateDirectory(Biblioteca.getEspecialistasFolder() + "\\E_" + this.especialistaSelecionado.ID);
                FotoFile = Biblioteca.getEspecialistasFolder() + "\\E_" + this.especialistaSelecionado.ID + "\\foto.png";
                if (!File.Exists(FotoFile))
                {
                    FotoFile = @"resources/img/foto.png";
                }
            }

            Image imgEspecialista = null;
            Image imgBackup = null;

            using (var bmpTemp = new Bitmap(FotoFile))
            {
                imgEspecialista = ResizeImage(new Bitmap(bmpTemp), pbFoto.Size.Width, pbFoto.Size.Height);
            }

            using (var bmpTemp = new Bitmap(@"resources/img/foto.png"))
            {
                imgBackup = ResizeImage(new Bitmap(bmpTemp), pbFoto.Size.Width, pbFoto.Size.Height);
            }

            pbFoto.Image = imgEspecialista;
            pbFoto.ErrorImage = imgBackup;
            pbFoto.InitialImage = imgBackup;

            Button btAlteraFoto = new Button();
            btAlteraFoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right
            | System.Windows.Forms.AnchorStyles.Top)));
            this.pnlEdit.Controls.Add(btAlteraFoto);

            btAlteraFoto.Size = new System.Drawing.Size(130, 30);
            btAlteraFoto.Location = new System.Drawing.Point(pnlEdit.Size.Width - btAlteraFoto.Size.Width - 5, pbFoto.Location.Y + pbFoto.Size.Height + 5);
            btAlteraFoto.Name = "btAlteraFoto";
            btAlteraFoto.TabIndex = 8;
            btAlteraFoto.Text = "Alterar Foto";
            btAlteraFoto.UseVisualStyleBackColor = true;
            btAlteraFoto.Click += new System.EventHandler(this.btAlteraFoto);

            AnchorStyles anchorEdit = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top))
            );

            lbID.AutoSize = false;
            lbID.Location = new System.Drawing.Point(4, 3);
            lbID.Name = "lbID";
            lbID.Size = new System.Drawing.Size(pnlEdit.ClientSize.Width - pbFoto.Location.X - 10, 20);
            lbID.Anchor = anchorEdit;

            lbNome.AutoSize = false;
            lbNome.Location = new System.Drawing.Point(lbID.Location.X, lbID.Location.Y + lbID.Size.Height + 5);
            lbNome.Name = "lbNome";
            lbNome.Size = new System.Drawing.Size(50, 20);
            lbNome.Text = "Nome: ";
            lbNome.Anchor = anchorEdit;

            tbNome.Location = new System.Drawing.Point(lbNome.Location.X + lbNome.Size.Width, lbNome.Location.Y - 3);
            tbNome.Name = "tbNome";
            tbNome.Size = new System.Drawing.Size(220, lbNome.Size.Height);
            tbNome.TabIndex = 1;
            tbNome.Anchor = anchorEdit;

            lbDataNasc.AutoSize = false;
            lbDataNasc.Location = new System.Drawing.Point(lbNome.Location.X, lbNome.Location.Y + lbNome.Size.Height + 5);
            lbDataNasc.Name = "lbDataNasc";
            lbDataNasc.Size = new System.Drawing.Size(80, 20);
            lbDataNasc.TabIndex = 0;
            lbDataNasc.Text = "Data Nasc: ";
            lbDataNasc.Anchor = anchorEdit;

            dtpDataNasc.Location = new System.Drawing.Point(lbDataNasc.Location.X + lbDataNasc.Size.Width, lbDataNasc.Location.Y - 3);
            dtpDataNasc.Name = "dtpDataNasc";
            dtpDataNasc.Size = new System.Drawing.Size(120, 22);
            dtpDataNasc.TabIndex = 2;
            dtpDataNasc.MaxDate = DateTime.Now;
            dtpDataNasc.ShowUpDown = false;
            dtpDataNasc.ShowCheckBox = false;
            dtpDataNasc.Format = DateTimePickerFormat.Custom;
            dtpDataNasc.CustomFormat = "dd/MM/yyyy";
            dtpDataNasc.Anchor = anchorEdit;

            lbCpf.AutoSize = false;
            lbCpf.Location = new System.Drawing.Point(lbDataNasc.Location.X, lbDataNasc.Location.Y + lbDataNasc.Size.Height + 5);
            lbCpf.Name = "lbCpf";
            lbCpf.Size = new System.Drawing.Size(50, 20);
            lbCpf.Text = "CPF: ";
            lbCpf.Anchor = anchorEdit;

            tbCpf.Location = new System.Drawing.Point(lbCpf.Location.X + lbCpf.Size.Width, lbCpf.Location.Y - 3);
            tbCpf.Name = "tbCpf";
            tbCpf.Size = new System.Drawing.Size(220, lbCpf.Size.Height);
            tbCpf.TabIndex = 1;
            tbCpf.Anchor = anchorEdit;

            lbRg.AutoSize = false;
            lbRg.Location = new System.Drawing.Point(lbCpf.Location.X, lbCpf.Location.Y + lbCpf.Size.Height + 5);
            lbRg.Name = "lbRg";
            lbRg.Size = new System.Drawing.Size(50, 20);
            lbRg.Text = "RG: ";
            lbRg.Anchor = anchorEdit;

            tbRg.Location = new System.Drawing.Point(lbRg.Location.X + lbRg.Size.Width, lbRg.Location.Y - 3);
            tbRg.Name = "tbRg";
            tbRg.Size = new System.Drawing.Size(220, lbRg.Size.Height);
            tbRg.TabIndex = 1;
            tbRg.Anchor = anchorEdit;

            //----------------------------------------------------------------------------------------------------------------------------

            lbSexo.AutoSize = false;
            lbSexo.Location = new System.Drawing.Point(lbRg.Location.X, lbRg.Location.Y + lbRg.Size.Height + 5);
            lbSexo.Name = "lbSexo";
            lbSexo.Size = new System.Drawing.Size(50, 20);
            lbSexo.TabIndex = 0;
            lbSexo.Text = "Sexo: ";
            lbSexo.Anchor = anchorEdit;

            rbSexoF.Location = new System.Drawing.Point(lbSexo.Location.X + lbSexo.Size.Width, lbSexo.Location.Y);
            rbSexoF.Name = "rbSexoF";
            rbSexoF.Text = "Feminino";
            rbSexoF.Size = new System.Drawing.Size(90, 20);
            rbSexoF.TabStop = true;
            rbSexoF.UseVisualStyleBackColor = true;
            rbSexoF.Anchor = anchorEdit;

            rbSexoM.Location = new System.Drawing.Point(rbSexoF.Location.X + rbSexoF.Size.Width, rbSexoF.Location.Y);
            rbSexoM.Name = "rbSexoM";
            rbSexoM.Text = "Masculino";
            rbSexoM.Size = new System.Drawing.Size(95, 20);
            rbSexoM.TabStop = true;
            rbSexoM.UseVisualStyleBackColor = true;
            rbSexoM.Anchor = anchorEdit;

            rbSexoO.Location = new System.Drawing.Point(rbSexoM.Location.X + rbSexoM.Size.Width, rbSexoF.Location.Y);
            rbSexoO.Name = "rbSexoO";
            rbSexoO.Text = "Outro";
            rbSexoO.Size = new System.Drawing.Size(65, 20);
            rbSexoO.TabStop = true;
            rbSexoO.UseVisualStyleBackColor = true;
            rbSexoO.Anchor = anchorEdit;

            lbDescricao.AutoSize = false;
            lbDescricao.Location = new System.Drawing.Point(lbSexo.Location.X, lbSexo.Location.Y + lbSexo.Size.Height + 5);
            lbDescricao.Name = "lbDescricao";
            lbDescricao.Size = new System.Drawing.Size(100, 20);
            lbDescricao.TabIndex = 0;
            lbDescricao.Text = "Observações:";
            lbDescricao.Anchor = anchorEdit;

            tbDescricao.Location = new System.Drawing.Point(lbDescricao.Location.X, lbDescricao.Location.Y + lbDescricao.Size.Height);
            tbDescricao.Name = "tbDescricao";
            tbDescricao.Size = new System.Drawing.Size(300, 150);
            tbDescricao.Multiline = true;
            tbDescricao.ScrollBars = ScrollBars.Both;
            tbDescricao.TabIndex = 1;
            tbDescricao.Anchor = anchorEdit;
        }

        private void alteraModo(int mod)
        {
            modo = mod;

            if (modo == 0)
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
            }
            else //modo == 1
            {
                if (novoRegistro)
                {
                    this.especialistaSelecionado = null;
                }
                else
                {
                    long ID = long.Parse(this.dtGrid.CurrentRow.Cells[1].Value.ToString());

                    this.especialistaSelecionado = gridListaEspecialistas.Where(p => p.ID == ID).First();
                }
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
                this.novoRegistro = false;
                alteraModo(1);
            }
        }

        protected void btEsq_Click(object sender, EventArgs e)
        {
            executeBtClick(btEsqState);
        }

        protected void btDir_Click(object sender, EventArgs e)
        {
            executeBtClick(btDirState);
        }

        protected void btDel_Click(object sender, EventArgs e)
        {
            List<Sessao> sessoes = Biblioteca.getSessoesEspecialista(this.especialistaSelecionado.ID);

            if (sessoes.Count > 0)
            {
                MessageBox.Show("Não é possível excluir um especialista que está vinculado a uma sessão.");
            }
            else
            {
                long ID = this.especialistaSelecionado.ID;
                listaEspecialistas.Remove(listaEspecialistas.Where(p => p.ID == ID).First());
                gridListaEspecialistas.Remove(gridListaEspecialistas.Where(p => p.ID == ID).First());
                Biblioteca.excluiEspecialistaSelecionado(ID);

                if (this.gridListaEspecialistas.Count > 0)
                {
                    this.selectedIndex = 0;
                }
                else
                {
                    this.selectedIndex = -1;
                }

                novoRegistro = false;
                alteraModo(0);
            }
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
            }
        }

        private void clickNovo()
        {
            novoRegistro = true;
            alteraModo(1);
        }

        private void clickLocaliza()
        {
            MessageBox.Show("Localiza Especialista!");
        }

        protected void clickVolta()
        {
            novoRegistro = false;
            alteraModo(0);
        }

        protected void clickSalva()
        {
            bool salva = true;

            string nome = pnlEdit.Controls.Find("tbNome", true)[0].Text.Trim();

            if (nome.Trim().Length < 1)
            {
                MessageBox.Show("O nome do especialista não pode ser vazio!");
                salva = false;
            }
            
            DateTime dataNasc = ((DateTimePicker)(pnlEdit.Controls.Find("dtpDataNasc", true)[0])).Value;

            string sexo = "";
            if (((RadioButton)(pnlEdit.Controls.Find("rbSexoF", true)[0])).Checked)
            {
                sexo = "F";
            }
            else if (((RadioButton)(pnlEdit.Controls.Find("rbSexoM", true)[0])).Checked)
            {
                sexo = "M";
            }
            else
            {
                sexo = "O";
            }
            string descricao = pnlEdit.Controls.Find("tbDescricao", true)[0].Text.Trim();

            string cpf = pnlEdit.Controls.Find("tbCpf", true)[0].Text.Trim();

            //if (cpf.Trim().Length < 1 && false)
            //{ //valida cpf e verifica se nao ja existe
            //    MessageBox.Show("O CPF do especialista não pode ser vazio!");
            //    salva = false;
            //}

            string rg = pnlEdit.Controls.Find("tbRg", true)[0].Text.Trim();

            //if (rg.Trim().Length < 1 && false)
            //{
            //    MessageBox.Show("O RG do especialista não pode ser vazio!");
            //    salva = false;
            //}

            if (salva)
            {
                if (this.especialistaSelecionado != null)
                {
                    this.especialistaSelecionado.updateValues(nome, dataNasc, cpf, rg, descricao, sexo);

                    long ID = this.especialistaSelecionado.ID;
                    gridListaEspecialistas.Where(p => p.ID == ID).First().updateValues(nome, dataNasc, cpf, rg, descricao, sexo);

                    Biblioteca.updateEspecialistaSelecionado(ID, nome, dataNasc, cpf, rg, descricao, sexo);

                    MessageBox.Show("Especialista Atualizado com sucesso!");
                }
                else
                {
                    Especialista esp = new Especialista(nome, dataNasc, cpf, rg, descricao, sexo);
                    this.addEspecialista(esp);

                    MessageBox.Show("Especialista Salvo com sucesso!");
                }

                novoRegistro = false;
                alteraModo(0);
            }
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
        }

        public static Image ResizeImage(Image image, int width, int height)
        {

            Bitmap videoOutput = new Bitmap(width, height);

            using (Graphics gr = Graphics.FromImage(videoOutput))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.DrawImage(image, new System.Drawing.Rectangle(0, 0, width, height));
            }

            return videoOutput;
        }

        public void btAlteraFoto(object sender, EventArgs e)
        {
            FormAlteraFoto formAlteraFoto = new FormAlteraFoto();
            formAlteraFoto.StartPosition = FormStartPosition.CenterParent;
            formAlteraFoto.ShowDialog();

            if (formAlteraFoto.caminhoArq != null && formAlteraFoto.caminhoArq.Trim().Length > 0)
            {
                this.caminhoFoto = formAlteraFoto.caminhoArq;

                PictureBox pbFoto = (PictureBox)(pnlEdit.Controls.Find("pbFoto", true)[0]);

                Bitmap foto = null;

                using (var bmpTemp = new Bitmap(this.caminhoFoto))
                {
                    foto = new Bitmap(bmpTemp);
                }

                long ID = (this.especialistaSelecionado != null) ? this.especialistaSelecionado.ID : Especialista.ProxID();
                string fileName = Biblioteca.getEspecialistasFolder() + "\\E_" + ID;

                Directory.CreateDirectory(fileName);

                this.caminhoFoto = fileName + "\\foto.png";

                foto.Save(this.caminhoFoto, System.Drawing.Imaging.ImageFormat.Png);

                Image imgEspecialista = null;
                Image imgBackup = null;

                using (var bmpTemp = new Bitmap(this.caminhoFoto))
                {
                    imgEspecialista = ResizeImage(new Bitmap(bmpTemp), pbFoto.Size.Width, pbFoto.Size.Height);
                }

                using (var bmpTemp = new Bitmap(@"resources/img/foto.png"))
                {
                    imgBackup = ResizeImage(new Bitmap(bmpTemp), pbFoto.Size.Width, pbFoto.Size.Height);
                }

                pbFoto.Image = imgEspecialista;
                pbFoto.ErrorImage = imgBackup;
                pbFoto.InitialImage = imgBackup;
            }
            else if (formAlteraFoto.foto != null)
            {
                PictureBox pbFoto = (PictureBox)(pnlEdit.Controls.Find("pbFoto", true)[0]);

                Bitmap foto = formAlteraFoto.foto;

                long ID = (this.especialistaSelecionado != null) ? this.especialistaSelecionado.ID : Especialista.ProxID();
                string fileName = Biblioteca.getEspecialistasFolder() + "\\E_" + ID;

                Directory.CreateDirectory(fileName);

                this.caminhoFoto = fileName + "\\foto.png";

                foto.Save(this.caminhoFoto, System.Drawing.Imaging.ImageFormat.Png);

                Image imgEspecialista = null;
                Image imgBackup = null;

                using (var bmpTemp = new Bitmap(this.caminhoFoto))
                {
                    imgEspecialista = ResizeImage(new Bitmap(bmpTemp), pbFoto.Size.Width, pbFoto.Size.Height);
                }

                using (var bmpTemp = new Bitmap(@"resources/img/foto.png"))
                {
                    imgBackup = ResizeImage(new Bitmap(bmpTemp), pbFoto.Size.Width, pbFoto.Size.Height);
                }

                pbFoto.Image = imgEspecialista;
                pbFoto.ErrorImage = imgBackup;
                pbFoto.InitialImage = imgBackup;
            }
            formAlteraFoto = null;
        }

        private void addEspecialista(Especialista pac)
        {
            Biblioteca.addEspecialista(pac);

            this.listaEspecialistas.Add(pac);
            this.gridListaEspecialistas = this.listaEspecialistas.OrderBy(p => p.nome).ToList();

            if (this.gridListaEspecialistas.Count > 0)
            {
                this.selectedIndex = 0;
            }
            else
            {
                this.selectedIndex = -1;
            }

            alteraModo(0);
        }

    }
}