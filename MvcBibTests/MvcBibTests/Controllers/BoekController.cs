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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing) {
                _db.Dispose();
            }
        }
    }
}
