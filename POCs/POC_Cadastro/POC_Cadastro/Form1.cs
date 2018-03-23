using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAlura
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 450);

            this.Name = "POC Cadastro";
            this.Text = "POC Cadastro";

            this.ResumeLayout(false);
            this.PerformLayout();

            dtDataNasc.Value = DateTime.Now;

        }

        private void btShow_Click(object sender, EventArgs e)
        {
            lbUsuario.Visible = true;
            tbUsuario.Visible = true;
        }

        private void btHide_Click(object sender, EventArgs e)
        {
            lbUsuario.Visible = false;
            tbUsuario.Visible = false;
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario(tbNome.Text, 
                tbCpf.Text, 
                tbRg.Text, 
                tbTel1.Text, 
                tbTel2.Text, 
                dtDataNasc.Value,
                tbEmail.Text,
                tbUsuario.Text);

            MessageBox.Show(usuario.getExibeValores());

            Boolean criar = true;

            if (File.Exists("poc_cadastro.txt"))
            {
                //Ja existe arquivo. Substituir?
                DialogResult dlgResult = MessageBox.Show("Já existe um cadastro salvo. Deseja substituí-lo?", "Cadastro encontrado", MessageBoxButtons.YesNo);
                if(dlgResult == DialogResult.No)
                {
                    criar = false;
                }
            }
            if (criar)
            {
                Stream saida = File.Open("poc_cadastro.txt", FileMode.Create);
                StreamWriter escritor = new StreamWriter(saida);
                escritor.WriteLine(usuario.getValores());
                escritor.Close();
                saida.Close();

                MessageBox.Show("Cadastro salvo com sucesso!", "Sucesso!");
            }
            else
            {
                MessageBox.Show("Operação de salvar abortada!");
            }
        }

        private void btAbrir_Click(object sender, EventArgs e)
        {
            if (File.Exists("poc_cadastro.txt"))
            {
                MessageBox.Show("Cadastro encontrado!", "Sucesso!");

                List<String> txt = new List<String>();

                Stream entrada = File.Open("poc_cadastro.txt", FileMode.Open);
                StreamReader leitor = new StreamReader(entrada);
                string linha = leitor.ReadLine();
                while (linha != null)
                {
                    //Faz alguma coisa com a linha

                    txt.Add(linha);
                    linha = leitor.ReadLine();
                }
                leitor.Close();
                entrada.Close();

                tbNome.Text = txt.ElementAt(0);
                tbCpf.Text = txt.ElementAt(1);
                tbRg.Text = txt.ElementAt(2);
                tbTel1.Text = txt.ElementAt(3);
                tbTel2.Text = txt.ElementAt(4);
                dtDataNasc.Text = txt.ElementAt(5);
                tbEmail.Text = txt.ElementAt(6);
                tbUsuario.Text = txt.ElementAt(7);

            } else
            {
                MessageBox.Show("Nenhum cadastro encontrado!", "Erro!");
            }
        }
    }
}
