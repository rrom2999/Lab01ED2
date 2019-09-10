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
        public byte Valor { get; set; }
        public int Frecuencia { get; set; }
        public bool EsHoja { get; set; }

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