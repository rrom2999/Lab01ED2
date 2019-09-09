using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult Subir(HttpPostedFileBase AComprimir)
        {
            string direccion = "";
            if (AComprimir != null && AComprimir.ContentLength > 0)
            {
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