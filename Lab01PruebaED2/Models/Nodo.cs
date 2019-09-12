using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Lab01PruebaED2.Models
{
    public class Nodo
    {
        public Nodo Izquierdo { get; set; }
        public Nodo Derecho { get; set; }
        public Nodo Padre { get; set; }
        public byte Valor { get; set; }
        public int Frecuencia { get; set; }
        public bool EsRaiz { get; set; }
        public string Prefijo { get; set; }

        public int CompareTo(Nodo other)
        {
            return this.Frecuencia.CompareTo(other.Frecuencia);
        }

        /*public Nodo()
        {
            Nodo Izq = new Nodo();
            Nodo Der = new Nodo();
            byte Val = new byte();
            int Frec = new int();
            bool EH = new bool();
        }*/
    }
}