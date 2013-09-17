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

        [HttpPost,ActionName("WisExemplaarVanBoek")]
        public ActionResult WisExemplaarVanBoekConfirmed(int exId) {
            var exemplaar = _db.Exemplaren.Find(exId);
            if (exemplaar != null)
            {
                _db.Exemplaren.Remove(exemplaar);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create() { 
            //inputveld voor isbn weergeven
            return View();
        }

        
        
        public ActionResult CreateOrLookUp(string isbn)
        {
            //boek bestaat in de db
            // boek opzoeken
            // boekinfo uit db weergeven, vragen hoeveel exemplaren toevoegen

            // bestaat niet in db
            // zoeken op het net
            //   gevonden: boekinfo van website weergeven, vragen hoeveel exemplaar toevoegen

            //   niet gevonden: lege boekvelden laten invullen, # exemplaren, toevoegen

            if (string.IsNullOrEmpty(isbn))
            {
                return View("Create");
            }
            //correct isbn?
            //else if() { 
                
            //}
            var boek = _db.Boeken.Where(b => b.Isbn == isbn).FirstOrDefault();

            if (boek != null)
            {
                return View("NewBoek", boek);
            }
            else { 
                // op isbndb.org zoeken
            }
        
        

            return View();
        }


        public ActionResult CreateBoekOrExemplaar(Boek boek, int aantalEx) { 
            //is het boek in de db dan aantal exemplaren toevoegen
            //anders nieuw boek aanmaken
            
            if (boek != null) {
                var boekInDb = _db.Boeken.Where(b => b.Isbn == boek.Isbn).FirstOrDefault();
                if (boekInDb != null)
                {
                    //exemplaren toevoegen aan bestaand boek
                    ExemplarenToevoegen(boekInDb, aantalEx);
                    return RedirectToAction("Index", "Home");
                }
                else { 
                    //nieuw boek maken en exemplaren eraan toevoegen
                    if (ModelState.IsValid)
                    {
                        _db.Boeken.Add(boek);
                        _db.SaveChanges();
                        ExemplarenToevoegen(boek, aantalEx);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        private void ExemplarenToevoegen(Boek boek, int aantalEx) {
            for (int i = 0; i < aantalEx; i++)
            {
                Exemplaar ex = new Exemplaar { Commentaar = "ex" + (i + 1)};
                boek.Exemplaren.Add(ex);
                _db.SaveChanges();
            }
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
