using System;

namespace ProjetoTCC
{
    class Sessao
    {
        static private long Count = 0;

        public long ID { get; private set; }
        public Paciente paciente { get; private set; }
        public Especialista especialista { get; private set; }
        public DateTime dataSessao { get; private set; }
        public string nomeVideo { get; private set; }
        public string material { get; private set; }
        public string descricao { get; private set; }

        public IdadeAprendizagem idadeAprLing { get; private set; }
        public IdadeAprendizagem idadeAprLog { get; private set; }
        public IdadeAprendizagem idadeAprMat { get; private set; }

        public bool ieVerificado = false;

        public Sessao(Paciente paciente, Especialista especialista, DateTime dataSessao, string caminhoVideo, string titulo, string descricao,
            IdadeAprendizagem idadeAprLing, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat, bool ieVerificado)
        {
            Sessao.Count++;
            this.ID = Sessao.Count;
            this.paciente = paciente;
            this.especialista = especialista;
            this.dataSessao = dataSessao;
            this.nomeVideo = caminhoVideo;
            this.material = titulo;
            this.descricao = descricao;
            this.idadeAprLing = idadeAprLing;
            this.idadeAprLog = idadeAprLing;
            this.idadeAprMat = idadeAprLing;
            this.ieVerificado = ieVerificado;
        }

        public string getIdadeSessaoPaciente()
        {
            int[] ret = getDataNascAnoMeses(this.dataSessao);
            return ret[0] + " a. " + ret[1] + " m.";
        }

        private int[] getDataNascAnoMeses(DateTime dataBase)
        {
            int[] ret = new int[] { 0, 0 };

            int totMeses = this.paciente.getDataNascMeses(dataBase);

            ret[0] = (int)Math.Floor((double)(totMeses / 12));
            ret[1] = totMeses % 12;

            return ret;
        }

        public String getStringVerificado()
        {
            if (this.ieVerificado)
            {
                return "V";
            } else
            {
                return "F";
            }
        }

        static public long ProxID()
        {
            return Sessao.Count+1;
        }

        private Sessao(long ID, Paciente paciente, Especialista especialista, DateTime dataSessao, string caminhoVideo, string titulo, string descricao,
            IdadeAprendizagem idadeAprLing, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat, bool ieVerificado)
        {
            this.ID = ID;
            this.paciente = paciente;
            this.especialista = especialista;
            this.dataSessao = dataSessao;
            this.nomeVideo = caminhoVideo;
            this.material = titulo;
            this.descricao = descricao;
            this.idadeAprLing = idadeAprLing;
            this.idadeAprLog = idadeAprLing;
            this.idadeAprMat = idadeAprLing;
            this.ieVerificado = ieVerificado;
        }

        public static Sessao create(long ID, Paciente paciente, Especialista especialista, DateTime dataSessao, string caminhoVideo, string titulo, string descricao,
            IdadeAprendizagem idadeAprLing, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat, bool ieVerificado)
        {
            if (ID > Sessao.Count)
            {
                Sessao.Count = ID;
            }
            return new Sessao(ID, paciente, especialista, dataSessao, caminhoVideo, titulo, descricao, idadeAprLing, idadeAprLog, idadeAprMat, ieVerificado);
        }

        public void updateValues(Especialista especialista, DateTime dataSessao, string caminhoVideo, string titulo, string descricao,
            IdadeAprendizagem idadeAprLing, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat,
            bool ieVerificado)
        {
            this.especialista = especialista;
            this.dataSessao = dataSessao;
            this.nomeVideo = caminhoVideo;
            this.material = titulo;
            this.descricao = descricao;
            this.idadeAprLing = idadeAprLing;
            this.idadeAprLog = idadeAprLing;
            this.idadeAprMat = idadeAprLing;
            this.ieVerificado = ieVerificado;
        }

    }
}
