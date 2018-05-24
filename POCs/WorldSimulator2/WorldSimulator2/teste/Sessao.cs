using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldSimulator
{
    class Sessao
    {
        static private long Count = 0;

        public long ID { get; private set; }
        public Paciente paciente { get; private set; }
        public DateTime dataSessao { get; private set; }
        public string caminhoVideo { get; private set; }
        public string titulo { get; private set; }
        public string descricao { get; private set; }

        public Sessao(Paciente paciente, DateTime dataSessao, string caminhoVideo, string titulo, string descricao)
        {
            Sessao.Count++;
            this.ID = Sessao.Count;
            this.paciente = paciente;
            this.dataSessao = dataSessao;
            this.caminhoVideo = caminhoVideo;
            this.titulo = titulo;
            this.descricao = descricao;
        }
        
    }
}
