using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace ProjetoTCC
{
    class PainelPaciente : PainelPadrao
    {
        private int modo = 0;

        private DataGridView dtGrid;
        private Panel pnlEdit;

        //N, L, V, S, D
        public char btEsqState { get; private set; } = 'N';
        public char btDirState { get; private set; } = 'L';

        private List<Paciente> listaPacientes;
        private List<Paciente> gridListaPacientes;

        public int selectedIndex = 0;
        public Paciente pacienteSelecionado = null;

        private DataGridViewCellEventHandler dtGridRowSelectHandler = null;

        private bool novoRegistro = false;
        private string caminhoFoto = "";

        public PainelPaciente()
        {
        }

        public void iniPainelPaciente(List<Paciente> listaPacientes)
        {
            initPainel(this);

            this.btEsqState = 'N';
            this.btDirState = 'L';

            this.listaPacientes = listaPacientes;
            this.gridListaPacientes = this.listaPacientes.OrderBy(p => p.nome).ToList();

            //this.btEsq.Click += new System.EventHandler(this.btEsq_Click);
            //this.btDir.Click += new System.EventHandler(this.btDir_Click);
            //this.btDel.Click += new System.EventHandler(this.btDel_Click);

            this.btDel.Enabled = false;
            this.btDel.Visible = false;
            btDel.Text = "Excluir";

            alteraModo(0);

            if(this.gridListaPacientes.Count > 0)
            {
                this.selectedIndex = 0;
            } else
            {
                this.selectedIndex = -1;
            }
        }

        public void btEsq_Click()
        {
            executeBtClick(btEsqState);
        }

        public void btDir_Click()
        {
            executeBtClick(btDirState);
        }

        public long getPacienteID()
        {
            if (this.selectedIndex >= 0 && this.gridListaPacientes.Count > this.selectedIndex)
            {
                return long.Parse(this.dtGrid.Rows[this.selectedIndex].Cells[1].Value.ToString());
            }
            return -1;
        }

        private void createGrid()
        {
            btEsqState = 'N';
            btDirState = 'L';

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
            if(listaPacientes.Count > 0)
            {
                ColID.Width = 15 + (listaPacientes.Max(p => p.ID).ToString().Length * 10);
            } else
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

            if (!novoRegistro)
            {
                btDel.Visible = true;
                btDel.Enabled = true;
            } else
            {
                btDel.Visible = false;
                btDel.Enabled = false;
            }

            this.pnlEdit = new Panel();
            this.pnlEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEdit.AutoScroll = true;
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
                lbID.Text = "ID: " + Paciente.ProxID();
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
                lbID.Text = "ID: " + this.pacienteSelecionado.ID;
                tbNome.Text = this.pacienteSelecionado.nome;
                dtpDataNasc.Value = this.pacienteSelecionado.dataNasc;
                tbCpf.Text = this.pacienteSelecionado.cpf;
                tbRg.Text = this.pacienteSelecionado.rg;
                rbSexoF.Checked = (this.pacienteSelecionado.sexo.Equals("F"));
                rbSexoM.Checked = (this.pacienteSelecionado.sexo.Equals("M"));
                rbSexoO.Checked = (this.pacienteSelecionado.sexo.Equals("O"));
                tbDescricao.Text = this.pacienteSelecionado.descricao;
            }

            pbFoto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Right)));
            pbFoto.Size = new System.Drawing.Size(270, 270);
            pbFoto.Location = new System.Drawing.Point(this.pnlEdit.Size.Width - pbFoto.Size.Width-1, 0);
            pbFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pbFoto.Name = "pbFoto";
            pbFoto.TabIndex = 0;
            pbFoto.TabStop = false;

            string FotoFile = @"resources/img/foto.png";

            if (!novoRegistro && this.pacienteSelecionado != null)
            {
                Directory.CreateDirectory(Biblioteca.getPacientesFolder() + "\\P_" + this.pacienteSelecionado.ID);
                FotoFile = Biblioteca.getPacientesFolder() + "\\P_" + this.pacienteSelecionado.ID + "\\foto.png";
                if (!File.Exists(FotoFile))
                {
                    FotoFile = @"resources/img/foto.png";
                }                
            }

            Image imgPaciente = ResizeImage(Image.FromFile(FotoFile), pbFoto.Size.Width, pbFoto.Size.Height);
            Image imgBackup = ResizeImage(Image.FromFile(@"resources/img/foto.png"), pbFoto.Size.Width, pbFoto.Size.Height);

            pbFoto.Image = imgPaciente;
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
            lbID.Size = new System.Drawing.Size(pnlEdit.ClientSize.Width - pbFoto.Location.X-10, 20);
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

            dtpDataNasc.Location = new System.Drawing.Point(lbDataNasc.Location.X + lbDataNasc.Size.Width, lbDataNasc.Location.Y -3);
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

        private void filtraPacientes()
        {
            if(dtGrid != null && gridListaPacientes != null)
            {
                //filtra

                foreach (Paciente p in gridListaPacientes)
                {
                    string[] row = new string[] { "+", p.ID.ToString(), p.nome, p.dataNasc.ToString("dd/MM/yyyy"), p.cpf };
                    dtGrid.Rows.Add(row);
                }
            }
        }

        private void alteraModo(int mod)
        {
            modo = mod;

            if(modo == 0)
            {
                if (pnlEdit != null)
                {
                    this.pnlArea.Controls.Clear();
                    this.disposeEdit();
                }
                createGrid();
                filtraPacientes();
            } else //modo == 1
            {
                if (novoRegistro)
                {
                    this.pacienteSelecionado = null;
                }
                else
                {
                    long ID = long.Parse(this.dtGrid.CurrentRow.Cells[1].Value.ToString());

                    this.pacienteSelecionado = gridListaPacientes.Where(p => p.ID == ID).First();
                }
                this.pnlArea.Controls.Clear();
                if(this.dtGrid != null)
                {
                    this.disposeGrid();
                }

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
            if(this.dtGrid != null)
            {
                this.dtGrid.RowEnter += this.dtGridRowSelectHandler;
            }
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

        protected void btEsq_Click(object sender, EventArgs e)
        {
            executeBtClick(btEsqState);            
        }
        
        protected void btDir_Click(object sender, EventArgs e)
        {
            executeBtClick(btDirState);
        }

        public void setExcluirPacienteEvent(EventHandler handler)
        {
            this.btDel.Click += new System.EventHandler(handler);
        }

        public void excluirPacienteSelecionado()
        {            
            List<Sessao> sessoes = Biblioteca.getPacienteSessoes();

            if(sessoes.Count > 0)
            {
                MessageBox.Show("Não é possível excluir um paciente que possui sessões.");
            } else
            {
                long ID = this.pacienteSelecionado.ID;
                listaPacientes.Remove(listaPacientes.Where(p => p.ID == ID).First());
                gridListaPacientes.Remove(gridListaPacientes.Where(p => p.ID == ID).First());
                Biblioteca.excluiPacienteSelecionado(ID);

//                MessageBox.Show("Paciente Excluído com sucesso!");

                if (this.gridListaPacientes.Count > 0)
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
            MessageBox.Show("Localiza Paciente!");
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
                MessageBox.Show("O nome do paciente não pode ser vazio!");
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

            if(cpf.Trim().Length < 1 && false)
            {
                MessageBox.Show("O CPF do paciente não pode ser vazio!");
                salva = false;
            }

            string rg = pnlEdit.Controls.Find("tbRg", true)[0].Text.Trim();

            if (rg.Trim().Length < 1 && false)
            {
                MessageBox.Show("O RG do paciente não pode ser vazio!");
                salva = false;
            }            

            if (salva)
            {
                if(this.pacienteSelecionado != null) {
                    this.pacienteSelecionado.updateValues(nome, dataNasc, this.caminhoFoto, cpf, rg, descricao, sexo);

                    long ID = this.pacienteSelecionado.ID;
                    gridListaPacientes.Where(p => p.ID == ID).First().updateValues(nome, dataNasc, this.caminhoFoto, cpf, rg, descricao, sexo);

                    Biblioteca.updatePacienteSelecionado(ID, nome, dataNasc, this.caminhoFoto, cpf, rg, descricao, sexo);
                } else
                {
                    Paciente pac = new Paciente(nome, dataNasc, "", cpf, rg, descricao, sexo);
                    this.addPaciente(pac);
                }

                novoRegistro = false;
                alteraModo(0);
            }
        }

        public void dispose()
        {
            if (this.dtGrid != null)
            {
                disposeGrid();
            }
            if (this.pnlEdit != null)
            {
                disposeEdit();
            }
            this.pnlArea.Controls.Clear();
        }

        private void disposeEdit()
        {
            this.pnlArea.Controls.Remove(pnlEdit);
            pnlEdit = null;
        }

        private void disposeGrid()
        {
            this.pnlArea.Controls.Remove(dtGrid);
            dtGrid = null;
        }

        public void setBtEsqAction(EventHandler handler)
        {
            this.btEsq.Click += new System.EventHandler(handler);
        }

        public void setBtDirAction(EventHandler handler)
        {
            this.btDir.Click += new System.EventHandler(handler);
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

                Bitmap foto = (Bitmap)Image.FromFile(this.caminhoFoto);
                long ID = (this.pacienteSelecionado != null) ? this.pacienteSelecionado.ID : Paciente.ProxID();
                string fileName = Biblioteca.getPacientesFolder() + "\\E_" + ID;

                Directory.CreateDirectory(fileName);

                this.caminhoFoto = fileName + "\\foto.png";

                foto.Save(this.caminhoFoto, System.Drawing.Imaging.ImageFormat.Png);

                Image imgPaciente = ResizeImage(Image.FromFile(this.caminhoFoto), pbFoto.Size.Width, pbFoto.Size.Height);
                Image imgBackup = ResizeImage(Image.FromFile(@"resources/img/foto.png"), pbFoto.Size.Width, pbFoto.Size.Height);

                pbFoto.Image = imgPaciente;
                pbFoto.ErrorImage = imgBackup;
                pbFoto.InitialImage = imgBackup;
            } else if (formAlteraFoto.foto != null)
            {
                PictureBox pbFoto = (PictureBox)(pnlEdit.Controls.Find("pbFoto", true)[0]);

                Bitmap foto = formAlteraFoto.foto;
                long ID = (this.pacienteSelecionado != null) ? this.pacienteSelecionado.ID : Paciente.ProxID();
                string fileName = Biblioteca.getPacientesFolder() + "\\P_" + ID;

                Directory.CreateDirectory(fileName);

                this.caminhoFoto = fileName + "\\foto.png";

                foto.Save(this.caminhoFoto, System.Drawing.Imaging.ImageFormat.Png);

                Image imgPaciente = ResizeImage(Image.FromFile(this.caminhoFoto), pbFoto.Size.Width, pbFoto.Size.Height);
                Image imgBackup = ResizeImage(Image.FromFile(@"resources/img/foto.png"), pbFoto.Size.Width, pbFoto.Size.Height);

                pbFoto.Image = imgPaciente;
                pbFoto.ErrorImage = imgBackup;
                pbFoto.InitialImage = imgBackup;
            }
            formAlteraFoto = null;
        }

        private void addPaciente(Paciente pac)
        {
            Biblioteca.addPaciente(pac);

            this.listaPacientes.Add(pac);
            this.gridListaPacientes = this.listaPacientes.OrderBy(p => p.nome).ToList();

            if (this.gridListaPacientes.Count > 0)
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
