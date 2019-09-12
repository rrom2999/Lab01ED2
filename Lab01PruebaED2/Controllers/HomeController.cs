﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Lab01PruebaED2.Models;

namespace Lab01PruebaED2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        static int X = 0;
        public ActionResult CargarArchivo()
        {
            if(X > 0)
            {
                ViewBag.Msg = "Error al cargar el archivo";
            }
            X++;
            return View();
        }

        [HttpPost]
        public ActionResult CargarArchivo(HttpPostedFileBase AComprimir)
        {
            if (AComprimir != null)
            {
                Subir(AComprimir);
                return RedirectToAction("Subir");
            }
            else
            {
                ViewBag.Msg = "ERROR AL CARGAR EL ARCHIVO, INTENTE DE NUEVO";
                return View();
            }
        }

        public void Subir(HttpPostedFileBase AComprimir)
        {

            Dictionary<byte, Nodo> DMaster = new Dictionary<byte, Nodo>();
            List<Nodo> ListadoOrden = new List<Nodo>();

            if (AComprimir != null && AComprimir.ContentLength > 0)
            {
                const int TBuffer = 1024;
                var NombreArchivo = AComprimir.FileName;
                var DireccionArchivo = Server.MapPath($"~/ArchivoCargado/{NombreArchivo}");
                AComprimir.SaveAs(DireccionArchivo);

                using (var stream = new FileStream(DireccionArchivo, FileMode.Open))
                {
                    using (var Lector = new BinaryReader(stream))
                    {
                        var BytesBuffer = new byte[TBuffer];
                        while(Lector.BaseStream.Position != Lector.BaseStream.Length)
                        {
                            BytesBuffer = Lector.ReadBytes(TBuffer);
                            foreach(var ByteLeido in BytesBuffer)
                            {
                                if(DMaster.ContainsKey(ByteLeido) == true)
                                {
                                    DMaster[ByteLeido].Frecuencia++;
                                }
                                else
                                {
                                    Nodo NuevoN = new Nodo();
                                    NuevoN.Valor = ByteLeido;
                                    NuevoN.Frecuencia = 1;
                                    NuevoN.Derecho = null; NuevoN.Izquierdo = null;
                                    NuevoN.Padre = null;
                                    DMaster.Add(ByteLeido, NuevoN);
                                }
                            }
                        }
                    }
                }
                PasarALista(DMaster);
            }
            
        }

        public void PasarALista(Dictionary<byte, Nodo> AOrdenar)
        {
            Dictionary<byte, Nodo> AuxAOrdenar = AOrdenar;
            List<Nodo> DiccionarioEnlistado = new List<Nodo>();
            foreach(var ByteBase in AuxAOrdenar)
            {
                Nodo Auxiliar = new Nodo();
                Auxiliar.Valor = ByteBase.Key;
                Auxiliar.Frecuencia = ByteBase.Value.Frecuencia;
                DiccionarioEnlistado.Add(Auxiliar);
                //AOrdenar.Remove(Auxiliar.Valor);
            }

            IEnumerable<Nodo> ListadoOrdenado = DiccionarioEnlistado.OrderBy(x => x.Frecuencia);
            ArmarArbol(ListadoOrdenado.ToList());
        }

        public void ArmarArbol(List<Nodo> Listado)
        {
            //Guardar en otro list Auxiliar a Listado
            int NCaracteres = Listado.Count; //Para ver cuantas hojas se tienen
            Nodo Izquierdo = new Nodo();
            Nodo Derecho = new Nodo();
            while(Listado.Count != 1)
            {
                Nodo PadreNuevo = new Nodo();
                Izquierdo = Listado[0];
                Derecho = Listado[1];
                PadreNuevo.Izquierdo = Izquierdo;
                PadreNuevo.Derecho = Derecho;
                PadreNuevo.Frecuencia = Izquierdo.Frecuencia + Derecho.Frecuencia;
                Izquierdo.Padre = PadreNuevo; //Para heredar el prefijo
                Derecho.Padre = PadreNuevo; 
                Listado.Remove(Izquierdo);
                Listado.Remove(Derecho);
                Listado.Add(PadreNuevo);
                IEnumerable<Nodo> FrecuenciasOrdenadas = Listado.OrderBy(x => x.Frecuencia);
                Listado = FrecuenciasOrdenadas.ToList();
            }

            Dictionary<byte, string> DPrefijos = new Dictionary<byte, string>();
            AsignarPrefijos(Listado[0]);
            //Hacer un metodo que vea

            //while (NCaracteres > 0)
            //{
                //DPrefijos.Add(/*ObtenerPrefijos.Izq*/);
                //DPrefijos.Add(/*ObtenerPrefijos.Der*/);
                //NCaracteres -=2;
            //}

            //Llenar Dprefijos
            //Llamar a Escristuradelcomprimido(Dprefijos)
        }

        public void AsignarPrefijos(Nodo Raiz)
        {
            if (Raiz.Padre != null)
            {
                Raiz.Izquierdo.Prefijo = Raiz.Prefijo + "0";
                Raiz.Derecho.Prefijo = Raiz.Prefijo + "1";
            }
            else
            {
                Raiz.Izquierdo.Prefijo = "0";
                Raiz.Derecho.Prefijo = "1";
            }

            if (Raiz.Izquierdo.Izquierdo != null && Raiz.Izquierdo.Derecho != null)
            {
                AsignarPrefijos(Raiz.Izquierdo);
            }
            if (Raiz.Derecho.Izquierdo != null && Raiz.Derecho.Derecho != null)
            {
                AsignarPrefijos(Raiz.Derecho);
            }
        }
        //Hacer que devuelva Padre de hojas 
        /*public Nodo ObtenerDPrefijos(Nodo Raiz)
        {
            //Nodo PadreADevolver = new Nodo();
            if (Raiz.Izquierdo.Izquierdo == null && Raiz.Izquierdo.Derecho == null)
            {
                return Raiz; //Retorna raiz, un nivel anterior de la hoja, para en diccionario agregar Raiz.Izq y Raiz.Der
            }
            if (Raiz.Derecho.Izquierdo == null && Raiz.Derecho.Derecho == null)
            {
                return Raiz; //Agrega las hojas, con su prefijos, al diccionario 
            }
            else
            {
                ObtenerDPrefijos(Raiz.Izquierdo);
                ObtenerDPrefijos(Raiz.Derecho);
            }
            //Enviar Dprefijos
        }*/

        
        //public void EscribirComprimido( Dictionary<by)

        public ActionResult ErrorC()
        {
            return View();
        }

        public ActionResult ExitoC()
        {
            return View();
        }

    }
}