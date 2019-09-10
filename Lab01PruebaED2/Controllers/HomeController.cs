using System;
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
        Dictionary<byte, Nodo> DMaster = new Dictionary<byte, Nodo>();

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

        public ActionResult Subir(HttpPostedFileBase AComprimir)
        {
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
                                    NuevoN.EsHoja = true;
                                    NuevoN.Derecho = null; NuevoN.Izquierdo = null;
                                    DMaster.Add(ByteLeido, NuevoN);
                                }
                            }
                        }
                    }
                }

                return RedirectToAction("ExitoC");
            }
            else
            {
                return RedirectToAction("ErrorC");
            }
            
        }
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