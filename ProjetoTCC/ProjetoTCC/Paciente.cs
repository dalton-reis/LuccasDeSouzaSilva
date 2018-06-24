using System;

namespace ProjetoTCC
{
    class Paciente : Pessoa
    {
        static private long Count = 0;

        public IdadeAprendizagem idadeAprLing { get; private set; }
        public IdadeAprendizagem idadeAprLog { get; private set; }
        public IdadeAprendizagem idadeAprMat { get; private set; }

        public DateTime dataCadastro { get; private set; }

        public Paciente(string nome, DateTime dataNasc, string cpf, string rg, string descricao, string sexo, 
            IdadeAprendizagem idadeAprLing, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat, DateTime dataCadastro)
        {
            Paciente.Count++;
            this.ID = Paciente.Count;
            this.nome = nome;
            this.dataNasc = dataNasc;
            this.cpf = cpf;
            this.rg = rg;
            this.descricao = descricao;
            this.sexo = sexo;
            this.idadeAprLing = idadeAprLing;
            this.idadeAprLog = idadeAprLog;
            this.idadeAprMat = idadeAprMat;
            this.dataCadastro = dataCadastro;
        }

        private int[] getDataNascAnoMeses(DateTime dataBase)
        {
            int[] ret = new int[] { 0, 0 };

            int totMeses = getDataNascMeses(dataBase);

            ret[0] = (int) Math.Floor((double)(totMeses / 12));
            ret[1] = totMeses % 12;

            return ret;
        }

        public string getIdadeAnoMesesC()
        {
            int[] ret = getDataNascAnoMeses(this.dataCadastro);
            return ret[0] + " a. " + ret[1] + " m.";
        }

        public string getIdadeAnoMesesA()
        {
            int[] ret = getDataNascAnoMeses(DateTime.Now);
            return ret[0] + " a. " + ret[1] + " m.";
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

        private Paciente(long ID, string nome, DateTime dataNasc, string cpf, string rg, string descricao, string sexo,
            IdadeAprendizagem idadeAprLing, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat, DateTime dataCadastro)
        {
            this.ID = ID;
            this.nome = nome;
            this.dataNasc = dataNasc;
            this.cpf = cpf;
            this.rg = rg;
            this.descricao = descricao;
            this.sexo = sexo;
            this.idadeAprLing = idadeAprLing;
            this.idadeAprLog = idadeAprLog;
            this.idadeAprMat = idadeAprMat;
            this.dataCadastro = dataCadastro;
        }

        public static Paciente create(long ID, string nome, DateTime dataNasc, string cpf, string rg, string descricao, string sexo,
            IdadeAprendizagem idadeAprLing, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat, DateTime dataCadastro)
        {
            if(ID > Paciente.Count)
            {
                Paciente.Count = ID;
            }
            return new Paciente(ID, nome, dataNasc, cpf, rg, descricao, sexo, idadeAprLing, idadeAprLog, idadeAprMat, dataCadastro);
        }

        public void updateValues(string nome, DateTime dataNasc, string cpf, string rg, string descricao, string sexo,
            IdadeAprendizagem idadeAprLing, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat, DateTime dataCadastro)
        {
            this.nome = nome;
            this.dataNasc = dataNasc;
            this.cpf = cpf;
            this.rg = rg;
            this.descricao = descricao;
            this.sexo = sexo;
            this.idadeAprLing = idadeAprLing;
            this.idadeAprLog = idadeAprLog;
            this.idadeAprMat = idadeAprMat;
            this.dataCadastro = dataCadastro;
        }
    }
}
