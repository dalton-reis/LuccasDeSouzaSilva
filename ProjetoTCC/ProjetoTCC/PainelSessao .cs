using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Drawing;
using Accord.Video.DirectShow;
using System.Drawing.Drawing2D;
using Accord.Video;
using Accord.Video.FFMPEG;
using static ProjetoTCC.VideoPlayer;

namespace ProjetoTCC
{
    class PainelSessao : PainelPadrao
    {
        private int modo = 0;

        private DataGridView dtGrid;
        private Panel pnlEdit;

        private DataGridViewCellEventHandler dtGridCelContentClick = null;

        //N, L, V, S, D
        public char btEsqState { get; private set; } = 'N';

        public char btDirState { get; private set; } = 'L';        

        private List<Sessao> listaSessoes;
        private List<Sessao> gridListaSessoes;

        public int selectedIndex = 0;
        public Sessao sessaoSelecionada = null;

        private Button btStartVideo = null;
        private Button btConnectBrain = null;
        private EventHandler ConnectBrainEvent = null;

        private VideoCaptureDevice videoCamera = null;
        private string nomeCamera = "";

        public bool isRecording { get; private set; } = false;
        public bool isBrainReader { get; private set; } = false;
        private bool isVideoRecorded = false;

        private bool novoRegistro = false;

        private long IDPaciente;

        private string caminhoSessao;

        VideoPlayer videoPlayer = null;

        Label lbID = null;
        Label lbMaterial = null;
        TextBox tbMaterial = null;
        Label lbDataSessao = null;
        DateTimePicker dtpDataSessao = null;

        Button btAtualizaCamera = null;
        Label lbCamera = null;
        ComboBox listCamera = null;

        Label lbEspecialista = null;
        ComboBox listEspecialista = null;

        Thread thWriteVideoFile = null;

        public PainelSessao()
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

        public void setBrainReader(bool enable)
        {
            this.isBrainReader = enable;
        }

        delegate void BoolArgReturnVoidDelegate(bool value);

        public void enableBtConnectBrain(bool enable)
        {
            if (this.btConnectBrain != null)
            {
                if (this.btConnectBrain.InvokeRequired)
                {
                    BoolArgReturnVoidDelegate del = new BoolArgReturnVoidDelegate(enableBtConnectBrain);
                    if (this != null && !this.IsDisposed)
                    {
                        this.Invoke(del, new object[] { enable });
                    }
                } else
                {
                    this.btConnectBrain.Enabled = enable;
                }
            }
        }

        public void iniPainelSessao()
        {
            initPainel(this);

            updateBtEsqState('N');
            updateBtDirState('L');

            this.btDel.Click += new System.EventHandler(this.btDel_Click);

            this.btDel.Enabled = false;
            this.btDel.Visible = false;
            btDel.Text = "Excluir";
        }

        public void listaPacienteSessao(long pacienteID)
        {
            if (pacienteID > -1)
            {
                ativaPainel();
                this.Enabled = true;
                this.IDPaciente = pacienteID;
                Biblioteca.updateSessoesPaciente(this.IDPaciente);
                this.listaSessoes = Biblioteca.getPacienteSessoes();
                this.gridListaSessoes = this.listaSessoes.OrderBy(s => s.dataSessao).ToList();
                alteraModo(0);
                if (this.gridListaSessoes.Count > 0)
                {
                    this.selectedIndex = 0;
                }
                else
                {
                    this.selectedIndex = -1;
                }
            } else
            {
                inativaPainel();
                this.Enabled = false;
            }
        }

        private void inativaPainel()
        {
            inativaBotoes();

            this.pnlArea.Visible = false;
            this.pnlArea.Enabled = false;
        }

        private void inativaBotoes()
        {
            this.btEsq.Enabled = false;
            this.btEsq.Visible = false;

            this.btDir.Enabled = false;
            this.btDir.Visible = false;
        }

        private void ativaPainel()
        {
            ativaBotoes();

            this.pnlArea.Visible = true;
            this.pnlArea.Enabled = true;
        }

        private void ativaBotoes()
        {
            this.btEsq.Enabled = true;
            this.btEsq.Visible = true;

            this.btDir.Enabled = true;
            this.btDir.Visible = true;
        }

        private void createGrid()
        {
            updateBtEsqState('N');
            updateBtDirState('L');

            this.btEsq.Text = "Novo";
            this.btDir.Text = "Localiza";

            this.btDel.Enabled = false;
            this.btDel.Visible = false;

            this.dtGrid = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dtGrid)).BeginInit();
            this.pnlArea.Controls.Add(dtGrid);

            this.dtGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));

            DataGridViewTextBoxColumn ColID = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColMaterial = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColDataSessao = new DataGridViewTextBoxColumn();
            DataGridViewButtonColumn ColBt = new DataGridViewButtonColumn();

            DataGridViewTextBoxColumn ColIdadeSessao = new DataGridViewTextBoxColumn();

            DataGridViewTextBoxColumn ColIdadeLing = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColIdadeLog = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColIdadeMat = new DataGridViewTextBoxColumn();

            ColID.Visible = false;

            ColMaterial.HeaderText = "Material";
            ColMaterial.Name = "Material";
            ColMaterial.Width = 150;

            ColDataSessao.HeaderText = "Data/Hora";
            ColDataSessao.Name = "Data/Hora";
            ColDataSessao.Width = 125;

            ColBt.HeaderText = "";
            ColBt.Name = "";
            ColBt.Width = 20;

            ColIdadeSessao.HeaderText = "Idade";
            ColIdadeSessao.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            ColIdadeSessao.Name = "Idade";
            ColIdadeSessao.Width = 100;

            ColIdadeLing.HeaderText = "Linguagem";
            ColIdadeLing.Name = "Linguagem";
            ColIdadeLing.Width = 100;
            ColIdadeLing.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            ColIdadeLog.HeaderText = "Logica";
            ColIdadeLog.Name = "Logica";
            ColIdadeLog.Width = 100;
            ColIdadeLog.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            ColIdadeMat.HeaderText = "Matematica";
            ColIdadeMat.Name = "Matematica";
            ColIdadeMat.Width = 100;
            ColIdadeMat.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            this.dtGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            this.dtGrid.GridColor = Color.Black;
            this.dtGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            this.dtGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dtGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.dtGrid.EnableHeadersVisualStyles = false;
            this.dtGrid.RowHeadersVisible = false;

            this.dtGrid.Columns.Add(ColBt);
            this.dtGrid.Columns.Add(ColID);
            this.dtGrid.Columns.Add(ColMaterial);
            this.dtGrid.Columns.Add(ColDataSessao);

            this.dtGrid.Columns.Add(ColIdadeSessao);

            this.dtGrid.Columns.Add(ColIdadeLing);
            this.dtGrid.Columns.Add(ColIdadeLog);
            this.dtGrid.Columns.Add(ColIdadeMat);

            foreach (Sessao s in gridListaSessoes)
            {
                string[] rowValues = new string[] { "+", s.ID.ToString(), s.material, s.dataSessao.ToString("dd/MM/yyyy HH:mm"),
                    s.getIdadeSessaoPaciente(),
                    Biblioteca.getIdadeAprendizagem(s.idadeAprLing),
                    Biblioteca.getIdadeAprendizagem(s.idadeAprLog),
                    Biblioteca.getIdadeAprendizagem(s.idadeAprMat) };

                dtGrid.Rows.Add(rowValues);
            }

            foreach(DataGridViewRow row in dtGrid.Rows)
            {
                if ((row.Index < gridListaSessoes.Count) && !gridListaSessoes.ElementAt(row.Index).ieVerificado)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }                
            }

            this.dtGrid.Location = new System.Drawing.Point(0, 0);
            this.dtGrid.Name = "dataGridView1";
            this.dtGrid.RowTemplate.Height = 20;
            this.dtGrid.Size = pnlArea.Size;
            this.dtGrid.ReadOnly = true;
            this.dtGrid.AllowUserToAddRows = false;
            this.dtGrid.AllowUserToDeleteRows = false;
            this.dtGrid.AllowUserToResizeRows = false;

            if (this.dtGridCelContentClick != null)
            {
                this.dtGrid.CellContentClick += new DataGridViewCellEventHandler(this.dtGridCelContentClick);
            }
        }

        private void createEdit()
        {
            updateBtEsqState('V');
            updateBtDirState('S');

            this.btEsq.Text = "Voltar";
            this.btDir.Text = "Salvar";

            if (!this.novoRegistro)
            {
                this.btDel.Visible = true;
                this.btDel.Enabled = true;
            }
            else
            {
                this.btDel.Visible = false;
                this.btDel.Enabled = false;
            }

            int spacing = 6;

            this.pnlEdit = new Panel();
            this.pnlEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
            | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEdit.AutoScroll = true;
            this.pnlArea.AutoScroll = true;

            this.pnlArea.Controls.Add(pnlEdit);

            this.pnlEdit.BackColor = this.pnlArea.BackColor;
            this.pnlEdit.BorderStyle = this.pnlArea.BorderStyle;
            this.pnlEdit.Location = new System.Drawing.Point(-1, 0);
            this.pnlEdit.Name = "pnlArea";
            this.pnlEdit.Size = this.pnlArea.Size;
            this.pnlEdit.TabIndex = 0;

            Control videoControl = new Control();

            getFolderSessao();

            if (novoRegistro)
            {
                PictureBox videoRecorder = new PictureBox();
                videoRecorder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
               | System.Windows.Forms.AnchorStyles.Bottom)
               | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));

                videoRecorder.MinimumSize = new System.Drawing.Size(pnlEdit.Size.Width, 250);
                videoRecorder.Size = new Size(pnlEdit.Size.Width, 250);
                videoRecorder.Location = new System.Drawing.Point(0, 0);
                videoRecorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                videoRecorder.Name = "videoRecorder";
                videoRecorder.BackColor = System.Drawing.Color.Black;
                videoRecorder.Paint += new PaintEventHandler(videoRecorderPaint);
                videoRecorder.TabIndex = 0;
                videoRecorder.TabStop = false;

                this.pnlEdit.Controls.Add(videoRecorder);

                videoControl = videoRecorder;
            } else
            {
                videoPlayer = new VideoPlayer();
                videoPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
                videoPlayer.Enabled = true;
                videoPlayer.Name = "videoPlayer";
                videoPlayer.MinimumSize = new System.Drawing.Size(pnlEdit.Size.Width, 275);
                videoPlayer.Location = new System.Drawing.Point(0, 0);
                videoPlayer.setSize(new Size(pnlEdit.Size.Width, (int)(pnlEdit.Size.Height * 0.45)));

                this.pnlEdit.Controls.Add(videoPlayer);

                videoPlayer.setVideoURL(this.caminhoSessao + "\\video_sessao_" + this.sessaoSelecionada.ID + ".avi");
                videoPlayer.DisplayImagePipeline += videoPlayerDisplayHandler;

                videoControl = videoPlayer;
            }

            Panel pnlFields = new Panel();
            pnlFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            pnlFields.AutoScroll = true;

            pnlFields.BackColor = this.pnlArea.BackColor;
            pnlFields.BorderStyle = this.pnlArea.BorderStyle;
            pnlFields.Location = new System.Drawing.Point(-1, videoControl.Location.Y + videoControl.Size.Height);
            pnlFields.Name = "pnlFields";
            pnlFields.Size = new Size(this.pnlArea.Size.Width, this.pnlArea.Size.Height - videoControl.Size.Height);
            pnlFields.TabIndex = 0;

            this.pnlEdit.Controls.Add(pnlFields);

            if (novoRegistro)
            {
                //botoes para gravar
                this.btStartVideo = new Button();
                this.btStartVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right
                | System.Windows.Forms.AnchorStyles.Bottom)));
                pnlFields.Controls.Add(btStartVideo);

                this.btStartVideo.Size = new System.Drawing.Size(120, 30);
                this.btStartVideo.Location = new System.Drawing.Point(pnlFields.Size.Width - btStartVideo.Size.Width - 5, spacing);
                this.btStartVideo.Name = "btStartVideo";
                this.btStartVideo.TabIndex = 8;
                this.btStartVideo.Text = "Iniciar Gravação";
                this.btStartVideo.UseVisualStyleBackColor = true;
                this.btStartVideo.Click += new System.EventHandler(this.btIniciaRecordVideoClick);
                this.btStartVideo.Enabled = true;

                isVideoRecorded = false;
                setBrainReader(false);

                //botoes para gravar
                this.btConnectBrain = new Button();
                this.btConnectBrain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right
                | System.Windows.Forms.AnchorStyles.Bottom)));
                pnlFields.Controls.Add(btConnectBrain);

                this.btConnectBrain.Size = new System.Drawing.Size(120, 30);
                this.btConnectBrain.Location = new System.Drawing.Point(pnlFields.Size.Width - btConnectBrain.Size.Width - 5, 
                    btStartVideo.Location.Y + btStartVideo.Size.Height + spacing);
                this.btConnectBrain.Name = "btConnectBrain";
                this.btConnectBrain.TabIndex = 8;
                this.btConnectBrain.Text = "Conectar MindWave";
                this.btConnectBrain.UseVisualStyleBackColor = true;
                this.btConnectBrain.Click += ConnectBrainEvent;
            }
            else
            {
               // Button btPdfSessao = new Button();
               // btPdfSessao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right
               //| System.Windows.Forms.AnchorStyles.Bottom)));
               // pnlFields.Controls.Add(btPdfSessao);
               // btPdfSessao.Size = new System.Drawing.Size(80, 30);
               // btPdfSessao.Location = new System.Drawing.Point(pnlFields.Size.Width - btPdfSessao.Size.Width - 5, spacing);
               // btPdfSessao.Name = "btPdfSessao";
               // btPdfSessao.TabIndex = 8;
               // btPdfSessao.Text = "PDF";
               // btPdfSessao.UseVisualStyleBackColor = true;
               // btPdfSessao.Click += new System.EventHandler(this.btGeraPdfSessao);
               // btPdfSessao.Enabled = false;
               // btPdfSessao.Visible = false;
            }

            AnchorStyles anchorEdit = ((System.Windows.Forms.AnchorStyles)
                ((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top))
            );

            this.lbID = new System.Windows.Forms.Label();
            this.lbMaterial = new System.Windows.Forms.Label();
            this.tbMaterial = new System.Windows.Forms.TextBox();
            this.lbDataSessao = new System.Windows.Forms.Label();
            this.dtpDataSessao = new System.Windows.Forms.DateTimePicker();

            pnlFields.Controls.Add(lbID);
            pnlFields.Controls.Add(lbMaterial);
            pnlFields.Controls.Add(tbMaterial);
            pnlFields.Controls.Add(lbDataSessao);
            pnlFields.Controls.Add(dtpDataSessao);

            if (this.novoRegistro)
            {
                this.lbID.Text = "ID: " + Sessao.ProxID();
                this.tbMaterial.Text = "";
                this.dtpDataSessao.Value = DateTime.Now;
            }
            else
            {
                this.lbID.Text = "ID: " + this.sessaoSelecionada.ID;
                this.tbMaterial.Text = this.sessaoSelecionada.material;
                this.dtpDataSessao.Value = this.sessaoSelecionada.dataSessao;
            }

            this.lbID.AutoSize = false;
            this.lbID.Location = new System.Drawing.Point(4, 2);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(200, 20);
            this.lbID.TabIndex = 0;
            this.lbID.Anchor = anchorEdit;            

            this.lbMaterial.AutoSize = false;
            this.lbMaterial.Location = new System.Drawing.Point(lbID.Location.X, lbID.Location.Y + lbID.Size.Height + spacing);
            this.lbMaterial.Name = "lbNome";
            this.lbMaterial.Size = new System.Drawing.Size(65, 20);
            this.lbMaterial.TabIndex = 0;
            this.lbMaterial.Text = "Material:";
            this.lbMaterial.Anchor = anchorEdit;

            this.tbMaterial.Location = new System.Drawing.Point(lbMaterial.Location.X + lbMaterial.Size.Width, lbMaterial.Location.Y - spacing);
            this.tbMaterial.Name = "tbMaterial";
            this.tbMaterial.Size = new System.Drawing.Size(200, lbMaterial.Size.Height);
            this.tbMaterial.TabIndex = 1;
            this.tbMaterial.Anchor = anchorEdit;

            this.lbDataSessao.AutoSize = false;
            this.lbDataSessao.Location = new System.Drawing.Point(lbMaterial.Location.X, lbMaterial.Location.Y + lbMaterial.Size.Height + spacing);
            this.lbDataSessao.Name = "lbDataSessao";
            this.lbDataSessao.Size = new System.Drawing.Size(95, 20);
            this.lbDataSessao.TabIndex = 0;
            this.lbDataSessao.Text = "Data Sessao:";
            this.lbDataSessao.Anchor = anchorEdit;

            this.dtpDataSessao.Location = new System.Drawing.Point(lbDataSessao.Location.X + lbDataSessao.Size.Width, lbDataSessao.Location.Y - spacing);
            this.dtpDataSessao.Name = "dtpDataSessao";
            this.dtpDataSessao.Size = new System.Drawing.Size(150, 22);
            this.dtpDataSessao.TabIndex = 2;
            this.dtpDataSessao.MaxDate = DateTime.Now;
            this.dtpDataSessao.Format = DateTimePickerFormat.Custom;
            this.dtpDataSessao.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpDataSessao.Anchor = anchorEdit;

            this.lbEspecialista = new Label();
            this.listEspecialista = new ComboBox();

            pnlFields.Controls.Add(listEspecialista);
            pnlFields.Controls.Add(lbEspecialista);

            this.lbEspecialista.Text = "Especialista:";
            this.lbEspecialista.AutoSize = false;
            this.lbEspecialista.Location = new System.Drawing.Point(lbDataSessao.Location.X, lbDataSessao.Location.Y + lbDataSessao.Size.Height + spacing + 5);
            this.lbEspecialista.Name = "lbEspecialista";
            this.lbEspecialista.Size = new System.Drawing.Size(90, 20);
            this.lbEspecialista.TabIndex = 0;
            this.lbEspecialista.Anchor = anchorEdit;

            this.listEspecialista.ItemHeight = 16;
            this.listEspecialista.DrawMode = DrawMode.Normal;
            this.listEspecialista.Location = new System.Drawing.Point(lbEspecialista.Location.X + lbEspecialista.Size.Width, lbEspecialista.Location.Y - spacing + 2);
            this.listEspecialista.Name = "listEspecialista";
            this.listEspecialista.DropDownStyle = ComboBoxStyle.DropDownList;

            this.listEspecialista.Items.Clear();

            List<Especialista> listEsp = Biblioteca.getEspecialistas();

            if (listEsp.Count >= 1)
            {
                this.listEspecialista.DisplayMember = "nome";
                this.listEspecialista.ValueMember = "ID";

                foreach (Especialista esp in listEsp)
                {
                    listEspecialista.Items.Add(esp);                    
                }

                listEspecialista.SelectedIndex = 0;

                long ID = this.sessaoSelecionada != null ? this.sessaoSelecionada.especialista.ID : -1L;

                if(ID > -1)
                {
                    for (int i = 0; i < listEsp.Count; i++)
                    {
                        if (listEsp.ElementAt(i).ID == this.sessaoSelecionada.especialista.ID)
                        {
                            listEspecialista.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                listEspecialista.Items.Add("-----");
            }
            listEspecialista.SelectedIndex = 0;

            this.listEspecialista.Size = new System.Drawing.Size(300, 25 + (listEspecialista.Items.Count > 4 ? 60 : listEspecialista.Items.Count * 15));

            if (novoRegistro)
            {
                this.btAtualizaCamera = new Button();
                this.lbCamera = new Label();
                this.listCamera = new ComboBox();

                pnlFields.Controls.Add(btAtualizaCamera);
                pnlFields.Controls.Add(lbCamera);
                pnlFields.Controls.Add(listCamera);

                this.btAtualizaCamera.Location = new System.Drawing.Point(lbEspecialista.Location.X, listEspecialista.Location.Y + listEspecialista.Size.Height + spacing + 2);
                this.btAtualizaCamera.Name = "btAtualizaCamera";
                this.btAtualizaCamera.Size = new System.Drawing.Size(75, 23);
                this.btAtualizaCamera.TabIndex = 14;
                this.btAtualizaCamera.Text = "Atualizar";
                this.btAtualizaCamera.UseVisualStyleBackColor = true;
                this.btAtualizaCamera.Click += new System.EventHandler(this.btAtualizaCamera_Click);

                this.lbCamera.Text = "Camera:";
                this.lbCamera.AutoSize = false;
                this.lbCamera.Location = new System.Drawing.Point(btAtualizaCamera.Location.X + btAtualizaCamera.Size.Width, btAtualizaCamera.Location.Y + spacing - 2);
                this.lbCamera.Name = "lbCamera";
                this.lbCamera.Size = new System.Drawing.Size(65, 20);
                this.lbCamera.TabIndex = 0;
                this.lbCamera.Anchor = anchorEdit;

                this.listCamera.ItemHeight = 16;
                this.listCamera.DrawMode = DrawMode.Normal;
                this.listCamera.Location = new System.Drawing.Point(lbCamera.Location.X + lbCamera.Size.Width, lbCamera.Location.Y - spacing + 2);
                this.listCamera.Name = "listCamera";
                this.listCamera.DropDownStyle = ComboBoxStyle.DropDownList;
                this.listCamera.SelectedValueChanged += ListCamera_SelectedValueChanged;
                this.listCamera.Size = new Size((listEspecialista.Location.X + listEspecialista.Size.Width) - listCamera.Location.X, listEspecialista.Size.Height);
                
                updateCameras();
                
            } else
            {
                Label lbAprLing = new System.Windows.Forms.Label();
                ComboBox cbAprLing = new ComboBox();

                Label lbAprLog = new System.Windows.Forms.Label();
                ComboBox cbAprLog = new ComboBox();

                Label lbAprMat = new System.Windows.Forms.Label();
                ComboBox cbAprMat = new ComboBox();

                lbAprLing.AutoSize = false;
                lbAprLing.Location = new System.Drawing.Point(lbEspecialista.Location.X, listEspecialista.Location.Y + listEspecialista.Size.Height + spacing - 2);
                lbAprLing.Name = "lbAprLing";
                lbAprLing.Size = new System.Drawing.Size(123, 20);
                lbAprLing.TabIndex = 0;
                lbAprLing.Text = "Idade Linguagem:";
                lbAprLing.Anchor = anchorEdit;

                cbAprLing.ItemHeight = 16;
                cbAprLing.DrawMode = DrawMode.Normal;
                cbAprLing.Location = new System.Drawing.Point(lbAprLing.Location.X + lbAprLing.Size.Width, lbAprLing.Location.Y - (spacing / 2));
                cbAprLing.Size = new Size(listEspecialista.Size.Width/2, 20);
                cbAprLing.Name = "cbAprLing";
                cbAprLing.DropDownStyle = ComboBoxStyle.DropDownList;
                populaIdades(cbAprLing);

                lbAprLog.AutoSize = false;
                lbAprLog.Location = new System.Drawing.Point(lbAprLing.Location.X, lbAprLing.Location.Y + lbAprLing.Size.Height + spacing);
                lbAprLog.Name = "lbAprLog";
                lbAprLog.Size = new System.Drawing.Size(123, 20);
                lbAprLog.TabIndex = 0;
                lbAprLog.Text = "Idade Logica:";
                lbAprLog.Anchor = anchorEdit;

                cbAprLog.ItemHeight = 16;
                cbAprLog.DrawMode = DrawMode.Normal;
                cbAprLog.Location = new System.Drawing.Point(lbAprLog.Location.X + lbAprLog.Size.Width, lbAprLog.Location.Y - (spacing / 2));
                cbAprLog.Size = cbAprLing.Size;
                cbAprLog.Name = "cbAprLog";
                cbAprLog.DropDownStyle = ComboBoxStyle.DropDownList;
                populaIdades(cbAprLog);

                lbAprMat.AutoSize = false;
                lbAprMat.Location = new System.Drawing.Point(lbAprLog.Location.X, lbAprLog.Location.Y + lbAprLog.Size.Height + spacing);
                lbAprMat.Name = "lbAprMat";
                lbAprMat.Size = new System.Drawing.Size(123, 20);
                lbAprMat.TabIndex = 0;
                lbAprMat.Text = "Idade Matematica:";
                lbAprMat.Anchor = anchorEdit;

                cbAprMat.ItemHeight = 16;
                cbAprMat.DrawMode = DrawMode.Normal;
                cbAprMat.Location = new System.Drawing.Point(lbAprMat.Location.X + lbAprMat.Size.Width, lbAprMat.Location.Y - (spacing / 2));
                cbAprMat.Size = cbAprLing.Size;
                cbAprMat.Name = "cbAprMat";
                cbAprMat.DropDownStyle = ComboBoxStyle.DropDownList;
                populaIdades(cbAprMat);

                cbAprLing.SelectedIndex = ((int)this.sessaoSelecionada.idadeAprLing - 1);
                cbAprLog.SelectedIndex = ((int)this.sessaoSelecionada.idadeAprLog - 1);
                cbAprMat.SelectedIndex = ((int)this.sessaoSelecionada.idadeAprMat - 1);

                Label lbDescricao = new Label();
                lbDescricao.Text = "Descrição:";
                lbDescricao.AutoSize = false;
                lbDescricao.Location = new System.Drawing.Point(lbAprMat.Location.X, lbAprMat.Location.Y + lbAprMat.Size.Height + spacing - 2);
                lbDescricao.Name = "lbDescricao";
                lbDescricao.Size = new System.Drawing.Size(90, 20);
                lbDescricao.TabIndex = 0;
                lbDescricao.Anchor = anchorEdit;

                TextBox tbDescricao = new TextBox();
                tbDescricao.Location = new System.Drawing.Point(lbDescricao.Location.X, lbDescricao.Location.Y + lbDescricao.Size.Height + spacing);
                tbDescricao.Name = "tbDescricao";
                tbDescricao.Size = new System.Drawing.Size(pnlFields.Size.Width - tbDescricao.Location.X - spacing - 5, pnlFields.Size.Height - tbDescricao.Location.Y - spacing - 5);
                tbDescricao.Multiline = true;
                tbDescricao.ScrollBars = ScrollBars.Both;
                tbDescricao.TabIndex = 1;
                tbDescricao.Anchor = anchorEdit;

                pnlFields.Controls.Add(lbAprLing);
                pnlFields.Controls.Add(cbAprLing);

                pnlFields.Controls.Add(lbAprLog);
                pnlFields.Controls.Add(cbAprLog);

                pnlFields.Controls.Add(lbAprMat);
                pnlFields.Controls.Add(cbAprMat);

                pnlFields.Controls.Add(lbDescricao);
                pnlFields.Controls.Add(tbDescricao);
            }

            if (novoRegistro)
            {
                iniciaVideo(0);
            }
        }

        private void populaIdades(ComboBox cBox)
        {
            cBox.Items.Clear();
            cBox.DisplayMember = "_DS";
            cBox.ValueMember = "_ID";

            for (int i = 1; i <= 9; i++)
            {
                Idade id = new Idade(i);
                cBox.Items.Add(id);
            }
        }

        private void ListCamera_SelectedValueChanged(object sender, EventArgs e)
        {
            btStartVideo.Enabled = false;
            if (videoCamera != null)
            {
                videoCamera.SignalToStop();
                while (videoCamera.IsRunning)
                {
                    Thread.Sleep(0500);
                }
                videoCamera = null;
            }
            iniciaVideo(this.listCamera.SelectedIndex);
        }

        private void btAtualizaCamera_Click(object sender, EventArgs e)
        {
            if (listCamera != null)
            {
                updateCameras();
            }
        }

        private void updateCameras()
        {
            btStartVideo.Enabled = false;
            enableBtConnectBrain(false);

            this.listCamera.Items.Clear();
            FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (VideoDevices.Count >= 1)
            {

                int indexCamera = -1;
                int count = -1;
                foreach (FilterInfo VideoCaptureDevice in VideoDevices)
                {
                    listCamera.Items.Add(VideoCaptureDevice.Name);
                    if (VideoCaptureDevice.Name.Equals(Biblioteca.nomeCamera) && indexCamera < 0)
                    {
                        indexCamera = count;
                    }
                    else
                    {
                        count++;
                    }
                }
                if (indexCamera >= 0)
                {
                    listCamera.SelectedIndex = indexCamera;
                }
                else
                {
                    listCamera.SelectedIndex = 0;
                }
                if (videoCamera != null)
                {
                    videoCamera.SignalToStop();
                    while (videoCamera.IsRunning)
                    {
                        Thread.Sleep(0500);
                    }
                    videoCamera = null;
                }
                iniciaVideo(listCamera.SelectedIndex);
                btStartVideo.Enabled = true;
                if (!isBrainReader)
                {
                    enableBtConnectBrain(true);
                }
            }
            else
            {
                MessageBox.Show("Nenhuma câmera foi encontrada." +
                    "\nVerifique se há uma câmera conectada e pressione o botão \"Atualizar\"");
            }
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

                this.nomeCamera = "";

                createGrid();
            } else //modo == 1
            {
                if (novoRegistro)
                {
                    this.sessaoSelecionada = null;
                }
                else
                {
                    long ID = long.Parse(this.dtGrid.CurrentRow.Cells[1].Value.ToString());

                    this.sessaoSelecionada = gridListaSessoes.Where(s => s.ID == ID).First();
                }

                this.pnlArea.Controls.Remove(dtGrid);
                this.disposeGrid();

                this.nomeCamera = Biblioteca.nomeCamera;

                createEdit();
            }
        }

        public void btGeraPdfSessao(object sender, EventArgs e)
        {
            //Teste gera PDF
            Document doc = new Document(PageSize.A4);//criando e estipulando o tipo da folha usada
            doc.SetMargins(40, 40, 40, 80);//estibulando o espaçamento das margens que queremos
            doc.AddCreationDate();//adicionando as configuracoes

            //caminho onde sera criado o pdf + nome desejado
            //OBS: o nome sempre deve ser terminado com .pdf
            string caminho = @"teste_pdf.pdf";

            //criando o arquivo pdf embranco, passando como parametro a variavel                
            //doc criada acima e a variavel caminho 
            //tambem criada acima.
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));

            doc.Open();

            PdfPTable tbRow1 = new PdfPTable(new float[] { 40, 85, 15, 25 });
            tbRow1.WidthPercentage = 100;
            Paragraph[] row1 = { new Paragraph("Nome:"),
                new Paragraph(sessaoSelecionada.paciente.nome),
                new Paragraph("Sexo:"),
                new Paragraph(sessaoSelecionada.paciente.sexo) };
            foreach (Paragraph row in row1)
            {
                tbRow1.AddCell(row);
            }
            doc.Add(tbRow1);

            PdfPTable tbRow2 = new PdfPTable(new float[] { 40, 25, 35, 25, 15, 25 });
            tbRow2.WidthPercentage = 100;
            Paragraph[] row2 = { new Paragraph("Data de Nascimento:"),
                new Paragraph(sessaoSelecionada.paciente.dataNasc.ToString("dd/MM/yyyy")),
                new Paragraph("Data da Sessão:"),
                new Paragraph(sessaoSelecionada.dataSessao.ToString("dd/MM/yyyy")),
                new Paragraph("Idade:"),
                new Paragraph(sessaoSelecionada.paciente.getDataNascMeses(sessaoSelecionada.dataSessao).ToString() + " meses")
            };

            foreach (Paragraph row in row2)
            {
                tbRow2.AddCell(row);
            }
            doc.Add(tbRow2);

            //criando a variavel para paragrafo
            Paragraph pObsPaciente = new Paragraph("Observações do paciente:" +
                "\n" +
                sessaoSelecionada.paciente.descricao);
            //etipulando o alinhamneto
            pObsPaciente.Alignment = Element.ALIGN_JUSTIFIED; //Alinhamento Justificado
            doc.Add(pObsPaciente);

            //criando a variavel para paragrafo
            Paragraph pObsSessao = new Paragraph("Sessão: " + sessaoSelecionada.material +
                "\n" +
                "Observações:" +
                "\n" +
                sessaoSelecionada.descricao, new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14));
            //etipulando o alinhamneto
            pObsSessao.Alignment = Element.ALIGN_JUSTIFIED; //Alinhamento Justificado
            doc.Add(pObsSessao);
                        
            //criando a variavel para paragrafo
            Paragraph pNivelAtencaoMedia = new Paragraph("Nível médio de atenção: 45 (normal)" +
                "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14));
            //etipulando o alinhamneto
            pNivelAtencaoMedia.Alignment = Element.ALIGN_JUSTIFIED; //Alinhamento Justificado
            pNivelAtencaoMedia.PaddingTop = 10;
            doc.Add(pNivelAtencaoMedia);

            //criando a variavel para paragrafo
            Paragraph pNivelConcentracaoMedia = new Paragraph("Nível médio de concentração: 25 (baixo)" +
                "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14));
            //etipulando o alinhamneto
            pNivelConcentracaoMedia.Alignment = Element.ALIGN_JUSTIFIED; //Alinhamento Justificado
            doc.Add(pNivelConcentracaoMedia);

            //criando a variavel para paragrafo
            Paragraph pNivelRelaxMedia = new Paragraph("Nível médio de relaxamento: 80 (alto)" +
                "\n", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14));
            //etipulando o alinhamneto
            pNivelRelaxMedia.Alignment = Element.ALIGN_JUSTIFIED; //Alinhamento Justificado
            doc.Add(pNivelRelaxMedia);


            doc.Close();
            //Abrindo o arquivo após cria-lo.
            System.Diagnostics.Process.Start(caminho);
        }

        public void addNeuroDataVideo(NeuroData neuroData)
        {
            lock (NeuroDataVideo)
            {
                if (NeuroDataVideo != null)
                {
                    NeuroDataVideo.Enqueue(neuroData);
                }
            }
        }

        Queue<object[]> DataWriteFile = null;
        Queue<NeuroData> NeuroDataVideo = null;

        public void VideoCamera_OnNewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if(this.pnlEdit != null)
            {
                if (this.novoRegistro)
                {
                    System.Windows.Forms.PictureBox videoRecorder = (System.Windows.Forms.PictureBox)pnlEdit.Controls.Find("videoRecorder", true)[0];

                    Bitmap videoInput = (Bitmap)eventArgs.Frame.Clone();

                    int width = videoRecorder.Size.Width;
                    int height = videoRecorder.Size.Height;

                    Bitmap videoOutput = new Bitmap(width, height);

                    using (Graphics gr = Graphics.FromImage(videoOutput))
                    {
                        gr.SmoothingMode = SmoothingMode.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        gr.CompositingQuality = CompositingQuality.HighQuality;
                        gr.DrawImage(videoInput, new System.Drawing.Rectangle(0, 0, width, height));
                    }
                    videoRecorder.BackgroundImage = videoOutput;
                    if (isRecording)
                    {
                        Bitmap recordVideoOutput = (Bitmap)eventArgs.Frame.Clone();

                        object[] frameData = new object[] { recordVideoOutput, null };

                        if (NeuroDataVideo != null && NeuroDataVideo.Count > 0)
                        {
                            frameData[1] = NeuroDataVideo.Dequeue();
                        }
                        else
                        {
                            frameData[1] = new NeuroData(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        }

                        if (DataWriteFile == null)
                        {
                            DataWriteFile = new Queue<object[]>();
                        }
                        DataWriteFile.Enqueue(frameData);
                    }
                }
            }            
        }

        public void writeVideoFile()
        {
            if(DataWriteFile == null)
            {
                DataWriteFile = new Queue<object[]>();
            }
            while (isRecording)
            {
                object[] frameData = null;
                lock (DataWriteFile)
                {
                    if(DataWriteFile.Count > 0)
                    {
                        frameData = DataWriteFile.Peek();
                        //[0] -> Bitmap
                        //[1] -> NeuroData
                    }
                }
                    if (writer != null && writerOpen && frameData != null)
                    {
                        Bitmap frame = (Bitmap)frameData[0];
                        NeuroData nr = (NeuroData)frameData[1];

                        byte[] aux = null;

                        frame.SetPixel(0, 0, Color.FromArgb(nr.PoorSignal, nr.Meditation, nr.Attention, 0));

                        aux = BitConverter.GetBytes(nr.Alpha1);
                        frame.SetPixel(1, 0, Color.FromArgb(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]), Convert.ToInt32(aux[3])));

                        aux = BitConverter.GetBytes(nr.Alpha2);
                        frame.SetPixel(2, 0, Color.FromArgb(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]), Convert.ToInt32(aux[3])));

                        aux = BitConverter.GetBytes(nr.Beta1);
                        frame.SetPixel(3, 0, Color.FromArgb(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]), Convert.ToInt32(aux[3])));

                        aux = BitConverter.GetBytes(nr.Beta2);
                        frame.SetPixel(4, 0, Color.FromArgb(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]), Convert.ToInt32(aux[3])));

                        aux = BitConverter.GetBytes(nr.Gamma1);
                        frame.SetPixel(5, 0, Color.FromArgb(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]), Convert.ToInt32(aux[3])));

                        aux = BitConverter.GetBytes(nr.Gamma2);
                        frame.SetPixel(6, 0, Color.FromArgb(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]), Convert.ToInt32(aux[3])));

                        aux = BitConverter.GetBytes(nr.Delta);
                        frame.SetPixel(7, 0, Color.FromArgb(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]), Convert.ToInt32(aux[3])));

                        aux = BitConverter.GetBytes(nr.Theta);
                        frame.SetPixel(8, 0, Color.FromArgb(Convert.ToInt32(aux[0]), Convert.ToInt32(aux[1]), Convert.ToInt32(aux[2]), Convert.ToInt32(aux[3])));

                        if (writerOpen)
                        {
                            writer.WriteVideoFrame(frame);
                            writer.WriteVideoFrame(frame);
                            writer.WriteVideoFrame(frame);
                            writer.WriteVideoFrame(frame);
                        }

                        lock (DataWriteFile)
                        {
                            DataWriteFile.Dequeue();
                        }
                }
            }
        }
  
        public void iniciaVideo(int indexCamera)
        {
            if (videoCamera == null)
            {
                // enumerate video devices
                FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (VideoDevices.Count > 0)
                {                    
                    if (indexCamera < 0)
                    {
                        indexCamera = 0;
                    }

                    videoCamera = new VideoCaptureDevice(VideoDevices[indexCamera].MonikerString);
                    
                    try
                    {
                        videoCamera.NewFrame += new NewFrameEventHandler(VideoCamera_OnNewFrame);
                        videoCamera.Start();
                        btStartVideo.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocorreu um problema ao iniciar a câmera." +
                            "\nVerifique as configurações da câmera e tente novamente." +
                            "\nSituação técnica:" + ex.Message +
                            "\nOrigem:" + ex.StackTrace.Substring(0, 255), "Aviso");
                        videoCamera = null;
                    }                    
                }
                else
                {
                    MessageBox.Show("Nenhuma camera foi detectada, não é possível iniciar a gravação.", "Aviso");
                }
            }
        }

        VideoFileWriter writer = new VideoFileWriter();

        bool writerOpen = false;

        public void btIniciaRecordVideoClick(object sender, EventArgs e)
        {
            if (isRecording)
            {
                isRecording = false;
                bool stopFileWriter = false;
                lock (DataWriteFile)
                {
                    if(DataWriteFile.Count > 0)
                    {
                        stopFileWriter = true;
                    }
                }
                while (stopFileWriter)
                {
                    lock (DataWriteFile)
                    {
                        if (DataWriteFile.Count > 0)
                        {
                            stopFileWriter = true;
                        } else
                        {
                            stopFileWriter = false;
                        }
                    }
                }
                writerOpen = false;
                writer.Close();

                ativaBotoes();
                this.btAtualizaCamera.Enabled = true;
                this.listCamera.Enabled = true;
                this.btConnectBrain.Enabled = true;
                this.btStartVideo.Text = "Iniciar Gravação";
                this.isVideoRecorded = true;
            } else
            {
                bool continua = true;

                if (continua && this.listCamera.Items.Count == 0)
                {
                    continua = false;
                    MessageBox.Show("Nenhuma camera foi detectada, não é possível iniciar a gravação.", "Aviso");
                }

                if (continua && isVideoRecorded)
                {
                    if (DialogResult.No == MessageBox.Show("Já foi realizada uma gravação para esta sessão." +
                        "\nIniciar uma nova gravação irá sobreescrever a antiga." +
                        "\nDeseja iniciar a gravação mesmo assim?", "Aviso", MessageBoxButtons.YesNo))
                    {
                        continua = false;
                    }
                }

                if (continua && !isBrainReader)
                {
                    if(DialogResult.No == MessageBox.Show("Não há conexão com o equipamento de leitura de ondas cerebrais." +
                        "\nDeseja iniciar a gravação mesmo assim?", "Aviso", MessageBoxButtons.YesNo)){
                        continua = false;
                    }
                }

                if (continua)
                {
                    if (videoCamera == null)
                    {
                        this.iniciaVideo(this.listCamera.SelectedIndex);
                    }

                    if (videoCamera != null)
                    {
                        writer = new VideoFileWriter();

                        Size size = new Size(640, 480);
                        int fps = 30;

                        long IDSessao = Sessao.ProxID();

                        if (this.sessaoSelecionada != null)
                        {
                            IDSessao = this.sessaoSelecionada.ID;
                        }

                        string destinationfile = caminhoSessao + "\\video_sessao_" + IDSessao + ".avi"; // Output file
                                                                                                        //                    writer.FrameRate = fps;
                        writer.Open(destinationfile, size.Width, size.Height, fps, VideoCodec.MPEG4, 30);
                        writerOpen = true;

                        isRecording = true;
                        NeuroDataVideo = new Queue<NeuroData>();
                        Queue<object[]> DataWriteFile = new Queue<object[]>();

                        thWriteVideoFile = new Thread(writeVideoFile);
                        thWriteVideoFile.Start();

                        inativaBotoes();
                        this.btAtualizaCamera.Enabled = false;
                        this.listCamera.Enabled = false;
                        this.btConnectBrain.Enabled = false;
                        this.btStartVideo.Text = "Finalizar gravação";
                    }
                }                
            }
        }

        public void videoRecorderPaint(object sender, PaintEventArgs e)
        {
            if (isRecording)
            {
                ControlPaint.DrawBorder(e.Graphics, ((System.Windows.Forms.PictureBox)sender).ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
            } else
            {
                ControlPaint.DrawBorder(e.Graphics, ((System.Windows.Forms.PictureBox)sender).ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
            }
        }

        public void SelecionaLinha(int index)
        {
            this.selectedIndex = index;
        }

        public void setGridCelContentClick(DataGridViewCellEventHandler handler)
        {
            this.dtGridCelContentClick = new DataGridViewCellEventHandler(handler);
            if (this.dtGrid != null)
            {
                this.dtGrid.CellContentClick += this.dtGridCelContentClick;
            }
        }

        private FrameProcessEventHandler videoPlayerDisplayHandler = null;

        public void setVideoPlayerDisplayHandler(FrameProcessEventHandler handler)
        {
            videoPlayerDisplayHandler = new FrameProcessEventHandler(handler);
            if (this.videoPlayer != null)
            {
                this.videoPlayer.DisplayImagePipeline += videoPlayerDisplayHandler;
            }
        }

        public void GridCelContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                this.novoRegistro = false;
                alteraModo(1);
            }
        }

        public void setConnectBrainEvent(EventHandler handler)
        {
             ConnectBrainEvent = handler;
        }

        public void setBtEsqAction(EventHandler handler)
        {
            this.btEsq.Click += new System.EventHandler(handler);
        }

        public void setBtDirAction(EventHandler handler)
        {
            this.btDir.Click += new System.EventHandler(handler);
        }
        
        public void btEsq_Click()
        {
            executeBtClick(btEsqState);            
        }
        
        public void btDir_Click()
        {
            executeBtClick(btDirState);
        }

        protected void btDel_Click(object sender, EventArgs e)
        {            
            if(videoPlayer != null)
            {
                videoPlayer.SignalToStop();
            }

            long ID = this.sessaoSelecionada.ID;
            listaSessoes.Remove(listaSessoes.Where(p => p.ID == ID).First());
            gridListaSessoes.Remove(gridListaSessoes.Where(p => p.ID == ID).First());
            Biblioteca.excluiSessaoSelecionada(ID);

            if (this.gridListaSessoes.Count > 0)
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

        private void executeBtClick(char btState)
        {
            switch (Char.ToUpper(btState))
            {
                case 'N':
                    clickNovo();
                    break;
                case 'L':
//                    clickLocaliza();
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
            this.novoRegistro = true;
            this.alteraModo(1);
        }

        private void getFolderSessao()
        {
            string startFolder = @"MindsEye";

            string caminhoArquivos = Biblioteca.caminhoArquivos;

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            if (!Directory.Exists(startFolder + "\\Pacientes"))
            {
                Directory.CreateDirectory(startFolder + "\\Pacientes");
            }
            startFolder += "\\Pacientes\\P_" + IDPaciente;

            if (!Directory.Exists(startFolder))
            {
                Directory.CreateDirectory(startFolder);
            }

            long IDSessao = Sessao.ProxID();

            if(this.sessaoSelecionada != null)
            {
                IDSessao = this.sessaoSelecionada.ID;
            }

            startFolder += "\\S_" + IDSessao;

            if (!Directory.Exists(startFolder))
            {
                Directory.CreateDirectory(startFolder);
            }

            this.caminhoSessao = startFolder;
        }

        private void clickLocaliza()
        {
            MessageBox.Show("Localiza Sessao!");
        }

        protected void clickVolta()
        {
            if(videoCamera != null)
            {
                videoCamera.SignalToStop();
            }
            if (videoPlayer != null)
            {
                videoPlayer.SignalToStop();
            }
            alteraModo(0);
        }

        protected void clickSalva()
        {
            bool salva = true;

            string material = pnlEdit.Controls.Find("tbmaterial", true)[0].Text.Trim();

            if (material.Trim().Length < 1)
            {
                MessageBox.Show("O material da sessao não pode ser vazio!");
                salva = false;
            }

            DateTime dataSessao = ((DateTimePicker)(pnlEdit.Controls.Find("dtpDataSessao", true)[0])).Value;

            Especialista esp = (Especialista) listEspecialista.SelectedItem;

            if (salva)
            {
                if (this.sessaoSelecionada != null)
                {
                    string descricao = pnlEdit.Controls.Find("tbDescricao", true)[0].Text.Trim();

                    IdadeAprendizagem idadeAprLing = (IdadeAprendizagem)((ComboBox)pnlEdit.Controls.Find("cbAprLing", true)[0]).SelectedIndex + 1;
                    IdadeAprendizagem idadeAprLog = (IdadeAprendizagem)((ComboBox)pnlEdit.Controls.Find("cbAprLog", true)[0]).SelectedIndex + 1;
                    IdadeAprendizagem idadeAprMat = (IdadeAprendizagem)((ComboBox)pnlEdit.Controls.Find("cbAprMat", true)[0]).SelectedIndex + 1;

                    bool ieVerificado = false;
                    //                    bool ieVerificado = ((CheckBox)(pnlEdit.Controls.Find("cbVerificado", true)[0])).Checked;

                    if (!ieVerificado)
                    {
                        if (DialogResult.Yes == MessageBox.Show(this, "Deseja marcar essa sessão como verificada?", "Aviso", MessageBoxButtons.YesNo))
                        {
                            ieVerificado = true;
                        }
                    }

                    this.sessaoSelecionada.updateValues(esp, dataSessao, this.sessaoSelecionada.nomeVideo, material,
                        descricao, idadeAprLing, idadeAprLog, idadeAprMat, ieVerificado);

                    long ID = this.sessaoSelecionada.ID;
                    gridListaSessoes.Where(p => p.ID == ID).First().updateValues(esp, dataSessao, this.sessaoSelecionada.nomeVideo, material,
                        descricao, idadeAprLing, idadeAprLog, idadeAprMat, ieVerificado);

                    Biblioteca.updateSessaoSelecionada(ID, esp, dataSessao, this.sessaoSelecionada.nomeVideo, material,
                        descricao, idadeAprLing, idadeAprLog, idadeAprMat, ieVerificado);

//                    MessageBox.Show("Sessao Atualizada com sucesso!");
                }
                else
                {
                    if (isRecording)
                    {
                        isRecording = false;
                        bool stopFileWriter = false;
                        lock (DataWriteFile)
                        {
                            if (DataWriteFile.Count > 0)
                            {
                                stopFileWriter = true;
                            }
                        }
                        while (stopFileWriter)
                        {
                            lock (DataWriteFile)
                            {
                                if (DataWriteFile.Count > 0)
                                {
                                    stopFileWriter = true;
                                }
                                else
                                {
                                    stopFileWriter = false;
                                }
                            }
                        }
                        writerOpen = false;
                        writer.Close();

                        ativaBotoes();
                        this.btAtualizaCamera.Enabled = true;
                        this.listCamera.Enabled = true;
                        this.btConnectBrain.Enabled = true;
                        this.btStartVideo.Text = "Iniciar Gravação";
                    }

                    if (videoCamera != null && videoCamera.IsRunning)
                    {
                        videoCamera.SignalToStop();
                    }

                    string caminhoVideo = "video_sessao_" + Sessao.ProxID() + ".avi"; // Output file

                    Paciente pac = Biblioteca.getPacientes().Where(p => p.ID == this.IDPaciente).First();
                    Sessao ses = new Sessao(pac, esp, dataSessao, caminhoVideo, material, 
                        "", IdadeAprendizagem.Ano1, IdadeAprendizagem.Ano1, IdadeAprendizagem.Ano1, false);
                    this.addSessao(ses);

                    MessageBox.Show("Sessao Salva com sucesso!");
                }

                novoRegistro = false;
                alteraModo(0);
            }
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
            this.nomeCamera = null;
            this.btStartVideo = null;
            this.btConnectBrain = null;
            this.videoCamera = null;
            this.videoPlayer = null;
            setVideoPlayerCreated(false);
            this.writer = null;
        }

        public void dispose()
        {
            if (isRecording)
            {
                isRecording = false;
                bool stopFileWriter = false;
                lock (DataWriteFile)
                {
                    if (DataWriteFile.Count > 0)
                    {
                        stopFileWriter = true;
                    }
                }
                while (stopFileWriter)
                {
                    lock (DataWriteFile)
                    {
                        if (DataWriteFile.Count > 0)
                        {
                            stopFileWriter = true;
                        }
                        else
                        {
                            stopFileWriter = false;
                        }
                    }
                }
                writerOpen = false;
                writer.Close();                
            }

            if(this.videoCamera != null && this.videoCamera.IsRunning)
            {
                this.videoCamera.SignalToStop();
            }

            if (thWriteVideoFile != null)
            {
                thWriteVideoFile.Join();
            }
            
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

        private void addSessao(Sessao ses)
        {
            Biblioteca.addSessao(ses);

            this.listaSessoes.Add(ses);
            this.gridListaSessoes = this.listaSessoes.OrderBy(s => s.dataSessao).ToList();

            if (this.gridListaSessoes.Count > 0)
            {
                this.selectedIndex = 0;
            }
            else
            {
                this.selectedIndex = -1;
            }

            alteraModo(0);
        }

        bool VideoPlayerCreated = false;

        public bool isVideoPlayerCreated()
        {
            return this.VideoPlayerCreated;
        }

        private void setVideoPlayerCreated(bool value)
        {
            this.VideoPlayerCreated = value;
        }

        public Bitmap getCurrentVideoFrame()
        {
                if (this.videoPlayer != null && isVideoPlayerCreated())
                {
                    try
                    {
                        Bitmap bitmap = VideoPlayerCopyFrame();

                        return bitmap;                        
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        return null;
                    }
                } else
                {
                    return null;
                }
            
        }

        delegate void VoidDelegate();

        public void BringFormToFront()
        {
            if (this.InvokeRequired)
            {
                VoidDelegate del = new VoidDelegate(BringFormToFront);
                if (this != null && !this.IsDisposed)
                {
                    this.Invoke(del, new object[] { });
                }
            }
            else
            {
                this.BringToFront();
            }
        }

        delegate Bitmap BitmapDelegate();

        public Bitmap VideoPlayerCopyFrame()
        {
            Bitmap ret = null;
            if (this.videoPlayer.InvokeRequired)
            {
                BitmapDelegate del = new BitmapDelegate(VideoPlayerCopyFrame);
                if (this != null && !this.IsDisposed)
                {
                    this.Invoke(del, new object[] { });
                }
            } else
            {
                // take picture BEFORE saveFileDialog pops up!!
                Bitmap bitmap = new Bitmap(this.videoPlayer.Width, this.videoPlayer.Height);

                Graphics g = Graphics.FromImage(bitmap);

                Graphics gg = this.videoPlayer.CreateGraphics();

                //timerTakePicFromVideo.Start();
                g.CopyFromScreen(
                            this.videoPlayer.PointToScreen(
                                new System.Drawing.Point()).X,
                            this.videoPlayer.PointToScreen(
                                new System.Drawing.Point()).Y,
                            0, 0,
                            new System.Drawing.Size(
                                this.videoPlayer.Width,
                                this.videoPlayer.Height)
                            );
                ret = bitmap;
            }
            return ret;
        }
    }
}
