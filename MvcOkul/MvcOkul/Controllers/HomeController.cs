using MvcOkul.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MvcOkul.Controllers
{
    public class HomeController : Controller
    {
        OsbAkilliTahtaEntities db = new OsbAkilliTahtaEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TBL_OGRETMENLER p)
        {
            MesajGonder mg = new MesajGonder();
            mg.Mail = p.Gmail;
            var _id = db.TBL_OGRETMENLER.FirstOrDefault(x => x.Gmail == p.Gmail);
            int id = _id.ID;
            var table = db.TBL_OGRETMENLER.Find(id);
            mg.Giris();
            mg.Gonder();
            ViewBag.a = "Mesaj Gönderildi";
            int _sifre = mg.sifre;
            ViewBag.hata = _sifre;
            ViewBag.hata1 = id;
            table.TekKulanımlıkŞifre = _sifre;
            db.SaveChanges();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}