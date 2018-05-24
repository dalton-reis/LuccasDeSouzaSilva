using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSimulator
{
    class Paciente
    {
        static private long Count = 0;

        public long ID { get; private set; }
        public string nome { get; private set; }
        public DateTime dataNasc { get; private set; }
        public string caminhoFoto { get; private set; }
        public string cpf { get; private set; }
        public string rg { get; private set; }
        public string descricao { get; private set; }

        public Paciente(string nome, DateTime dataNasc, string caminhoFoto, string cpf, string rg, string descricao)
        {
            Paciente.Count++;
            this.ID = Paciente.Count;
            this.nome = nome;
            this.dataNasc = dataNasc;
            this.caminhoFoto = caminhoFoto;
            this.cpf = cpf;
            this.rg = rg;
            this.descricao = descricao;
        }
        
    }
}
