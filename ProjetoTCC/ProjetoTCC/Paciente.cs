using System;

namespace ProjetoTCC
{
    class Paciente : Pessoa
    {
        static private long Count = 0;

        public Paciente(string nome, DateTime dataNasc, string caminhoFoto, string cpf, string rg, string descricao, string sexo)
        {
            Paciente.Count++;
            this.ID = Paciente.Count;
            this.nome = nome;
            this.dataNasc = dataNasc;
            this.caminhoFoto = caminhoFoto;
            this.cpf = cpf;
            this.rg = rg;
            this.descricao = descricao;
            this.sexo = sexo;
        }

        public int getDataNascMeses(DateTime dataBase)
        {
            if(dataBase.Year >= this.dataNasc.Year)
            {
                int meses = (dataBase.Year - this.dataNasc.Year) * 12;
                meses += dataBase.Month - this.dataNasc.Month;
                if (dataBase.Day < this.dataNasc.Day)
                {
                    meses += -1;
                }
                return meses;
            } else
            {
                return -1;
            }
        }

        static public long ProxID()
        {
            return Paciente.Count + 1;
        }

        private Paciente(long ID, string nome, DateTime dataNasc, string caminhoFoto, string cpf, string rg, string descricao, string sexo)
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

        public static Paciente create(long ID, string nome, DateTime dataNasc, string caminhoFoto, string cpf, string rg, string descricao, string sexo)
        {
            if(ID > Paciente.Count)
            {
                Paciente.Count = ID;
            }
            return new Paciente(ID, nome, dataNasc, caminhoFoto, cpf, rg, descricao, sexo);
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
