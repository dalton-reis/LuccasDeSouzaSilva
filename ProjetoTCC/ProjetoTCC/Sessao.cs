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
        public string titulo { get; private set; }
        public string descricao { get; private set; }

        public Sessao(Paciente paciente, Especialista especialista, DateTime dataSessao, string caminhoVideo, string titulo, string descricao)
        {
            Sessao.Count++;
            this.ID = Sessao.Count;
            this.paciente = paciente;
            this.especialista = especialista;
            this.dataSessao = dataSessao;
            this.nomeVideo = caminhoVideo;
            this.titulo = titulo;
            this.descricao = descricao;
        }

        static public long ProxID()
        {
            return Sessao.Count+1;
        }

        private Sessao(long ID, Paciente paciente, Especialista especialista, DateTime dataSessao, string caminhoVideo, string titulo, string descricao)
        {
            this.ID = ID;
            this.paciente = paciente;
            this.especialista = especialista;
            this.dataSessao = dataSessao;
            this.nomeVideo = caminhoVideo;
            this.titulo = titulo;
            this.descricao = descricao;
        }

        public static Sessao create(long ID, Paciente paciente, Especialista especialista, DateTime dataSessao, string caminhoVideo, string titulo, string descricao)
        {
            if (ID > Sessao.Count)
            {
                Sessao.Count = ID;
            }
            return new Sessao(ID, paciente, especialista, dataSessao, caminhoVideo, titulo, descricao);
        }

        public void updateValues(Especialista especialista, DateTime dataSessao, string caminhoVideo, string titulo, string descricao)
        {
            this.especialista = especialista;
            this.dataSessao = dataSessao;
            this.nomeVideo = caminhoVideo;
            this.titulo = titulo;
            this.descricao = descricao;
        }

    }
}
