using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjetoTCC
{
    public class Idade
    {
        int id { get; set; }
        string ds { get; set; }

        public string _DS
        {
            get
            {
                return ds;
            }
            set
            {
                ds = value;
            }
        }

        public int _ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public Idade(int ano)
        {
            this.id = (int) ano;
            this.ds = ano + "o ano";
        }
    }

    public enum IdadeAprendizagem {
        Ano1 = 1,
        Ano2 = 2,
        Ano3 = 3,
        Ano4 = 4,
        Ano5 = 5,
        Ano6 = 6,
        Ano7 = 7,
        Ano8 = 8,
        Ano9 = 9
    };
}