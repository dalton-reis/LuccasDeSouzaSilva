using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAlura
{
    class Usuario : Pessoa
    {
        public string login { get; private set; }
        private string senha;

        public Usuario() { }

        public Usuario(string nome, string cpf, string rg, string tel1, string tel2, DateTime dataNasc,
            string email, string login)
        {
            nome = nome.Trim();
            this.Id = 0;
            this.NomeCompleto = nome;

            string[] s = nome.Split(' ');

            if (s.Length < 2)
            {
                this.NomeCurto = s[0];
            }
            else
            {
                this.NomeCurto = s[0] + " " + s[s.Length - 1];
            }
            this.DataNasc = dataNasc;
            this.Cpf = cpf.Trim();
            this.Rg = rg.Trim();
            this.Tel1 = tel1.Trim();
            this.Tel2 = tel2.Trim();
            this.Email = email.Trim();
            this.login = login.Trim();
            this.senha = "";
        }

        public string getExibeValores()
        {
            StringBuilder str = new StringBuilder();
            str.Append("Nome completo: ").Append(this.NomeCompleto).Append("\n");
            str.Append("Nome curto: ").Append(this.NomeCurto).Append("\n");
            str.Append("Cpf: ").Append(this.Cpf).Append("\n");
            str.Append("Rg: ").Append(this.Rg).Append("\n");
            str.Append("Telefone 1: ").Append(this.Tel1).Append("\n");
            str.Append("Telefone 2: ").Append(this.Tel2).Append("\n");
            str.Append("Data de Nascimento: ").Append(this.DataNasc.ToShortDateString()).Append("\n");
            str.Append("Email: ").Append(this.Email).Append("\n");
            str.Append("Usuario: ").Append(this.login);
            
            return str.ToString();
        }

        public string getValores()
        {
            StringBuilder str = new StringBuilder();
            str.Append(this.NomeCompleto).Append("\n");
            str.Append(this.Cpf).Append("\n");
            str.Append(this.Rg).Append("\n");
            str.Append(this.Tel1).Append("\n");
            str.Append(this.Tel2).Append("\n");
            str.Append(this.DataNasc.ToShortDateString()).Append("\n");
            str.Append(this.Email).Append("\n");
            str.Append(this.login);

            return str.ToString();
        }


    }
}
