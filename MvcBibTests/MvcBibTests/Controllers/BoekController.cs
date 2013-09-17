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
    public class BoekController : Controller
    {
        private EFBibliotheekRepository _db;
        
        public BoekController()
        {
            _db = new EFBibliotheekRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var boek = _db.Boeken.Find(id);
            if (boek != null)
            {
                return View(boek);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Boek boek)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(boek).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(boek);
        }

        public ActionResult Details(int id)
        {
            var boek = _db.Boeken.Where(b => b.Id == id).SingleOrDefault();
            if (boek != null)
            {
                return View(boek);
            }

            return RedirectToAction("Index");

        }

        public ActionResult WisAlleExemplarenVanBoek(int id)
        {
            var boek = _db.Boeken.Find(id);
            if (boek != null)
            {
                return View(boek);
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("WisAlleExemplarenVanBoek")]
        [ValidateAntiForgeryToken]
        public ActionResult WisAlleExemplarenVanBoekConfirmed(int id) {
            Boek boek = _db.Boeken.Find(id);
            var exemplaren = boek.Exemplaren;
            boek.Exemplaren = null;
            foreach(var ex in exemplaren){
                _db.Exemplaren.Remove(ex);
                _db.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }

        public ActionResult ExemplarenVanBoek(int id) {
            Boek boek = _db.Boeken.Find(id);
            return View(boek);
        }

        public ActionResult WisExemplaarVanBoek(int exId) {
            var exemplaar = _db.Exemplaren.Find(exId);
            if (exemplaar != null) {
                return View(exemplaar);
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing) {
                _db.Dispose();
            }
        }
    }
}
