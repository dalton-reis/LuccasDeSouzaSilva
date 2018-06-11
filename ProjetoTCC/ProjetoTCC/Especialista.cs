using System;

namespace ProjetoTCC
{
    class Especialista : Pessoa
    {
        static private long Count = 0;
        
        public Especialista(string nome, DateTime dataNasc, string caminhoFoto, string cpf, string rg, string descricao, string sexo)
        {
            Especialista.Count++;
            this.ID = Especialista.Count;
            this.nome = nome;
            this.dataNasc = dataNasc;
            this.caminhoFoto = caminhoFoto;
            this.cpf = cpf;
            this.rg = rg;
            this.descricao = descricao;
            this.sexo = sexo;
        }

        static public long ProxID()
        {
            return Especialista.Count + 1;
        }

        private Especialista(long ID, string nome, DateTime dataNasc, string caminhoFoto, string cpf, string rg, string descricao, string sexo)
        {
            this.ID = ID;
            this.nome = nome;
            this.dataNasc = dataNasc;
            this.caminhoFoto = caminhoFoto;
            this.cpf = cpf;
            this.rg = rg;
            this.descricao = descricao;
            this.sexo = sexo;
        }

        public static Especialista create(long ID, string nome, DateTime dataNasc, string caminhoFoto, string cpf, string rg, string descricao, string sexo)
        {
            return new Especialista(ID, nome, dataNasc, caminhoFoto, cpf, rg, descricao, sexo);
        }

        public void updateValues(string nome, DateTime dataNasc, string caminhoFoto, string cpf, string rg, string descricao, string sexo)
        {
            this.nome = nome;
            this.dataNasc = dataNasc;
            this.caminhoFoto = caminhoFoto;
            this.cpf = cpf;
            this.rg = rg;
            this.descricao = descricao;
            this.sexo = sexo;
        }

    }
}
