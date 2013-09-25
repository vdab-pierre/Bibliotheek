using Bibliotheek.Domain.Concrete;
using Bibliotheek.Domain.Entities;
using MvcBib.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBib.Controllers
{
    public class AuteurController : Controller
    {
        private EFBibliotheekRepository _db;

        public AuteurController()
        {
            _db = new EFBibliotheekRepository();
        }


        public ActionResult CreateVoorBoek(int boekId) {
            var boekInDb = _db.Boeken.Find(boekId);
            if (boekInDb != null){
                var auteur = new Auteur();
                auteur.Boeken = new HashSet<Boek>();
                auteur.Boeken.Add(boekInDb);
                
                return View(auteur);
            }
            throw new HttpException(404, "Boek niet gevonden");
            
        }

        [HttpPost]
        public ActionResult CreateVoorBoek(Auteur auteur) {
            if (ModelState.IsValid) {
                _db.Auteurs.Add(auteur);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
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
