using System;

namespace ProjetoTCC
{
    abstract class Pessoa
    {
        public long ID { get; protected set; }
        public string nome { get; protected set; }
        public DateTime dataNasc { get; protected set; }
        public string cpf { get; protected set; }
        public string rg { get; protected set; }
        public string descricao { get; protected set; }
        public string sexo { get; protected set; }
    }
}
