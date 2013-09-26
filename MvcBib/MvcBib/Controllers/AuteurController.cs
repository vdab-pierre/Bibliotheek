using Bibliotheek.Domain.Concrete;
using Bibliotheek.Domain.Entities;
using MvcBib.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
                
                BoekAuteurViewModel vm = new BoekAuteurViewModel { Auteur=auteur,Boek=boekInDb,BoekId=boekId};

                return View(vm);
            }
            throw new HttpException(404, "Boek niet gevonden");
            
        }

        [HttpPost]
        public ActionResult CreateVoorBoek(BoekAuteurViewModel vm) {
            if (ModelState.IsValid) {
                //eerst zoeken in db ...
                var deAuteurInDb = _db.Auteurs.Where(a => a.Familienaam.Contains(vm.Auteur.Familienaam) && a.Voornaam.Contains(vm.Auteur.Voornaam)).FirstOrDefault();
                var boekInDb = _db.Boeken.Find(vm.BoekId);
                if (boekInDb != null)
                {
                    if (deAuteurInDb != null)
                    {
                        
                        boekInDb.Auteurs.Add(deAuteurInDb);
                        _db.SaveChanges();
                    }
                    else
                    {
                        boekInDb.Auteurs.Add(vm.Auteur);
                        _db.SaveChanges();
                    }
                }
                else {
                    throw new HttpException(404, "Boek niet gevonden");
                }
                
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
