using Bibliotheek.Domain.Concrete;
using Bibliotheek.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBibTests.Controllers
{
    public class HomeController : Controller
    {
        private EFBibliotheekRepository _db;
        public HomeController()
        {
            _db = new EFBibliotheekRepository();
        }

        public ActionResult TestTooltip() {
            return View();
        }

        public ActionResult Index(string searchTerm=null)
        {

            var boeken = (from b in _db.Boeken
                         from a in b.Auteurs
                         where 
                         searchTerm==null
                         || a.Familienaam.StartsWith(searchTerm)
                         || b.Titel.Contains(searchTerm)
                         || b.Isbn.StartsWith(searchTerm)
                         || b.Uitgever.Naam.StartsWith(searchTerm)
                         || a.Voornaam.StartsWith(searchTerm)
                         select b).ToList();

            return View(boeken);
        }

        [HttpPost]
        public ActionResult WisExemplaar(int boekId,int exId) {
            var exemplaar = _db.Boeken.Find(boekId).Exemplaren.Where(ex=>ex.Id==exId).FirstOrDefault();
            if (exemplaar == null)
            {
                return HttpNotFound();
            }
            _db.Exemplaren.Remove(exemplaar);
            _db.SaveChanges();
            return RedirectToAction("Index","Home");
            //return View("Index", _db.Boeken.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _db.Dispose();
            }
        }
    }
}
