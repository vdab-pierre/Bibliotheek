using Bibliotheek.Domain.Concrete;
using Bibliotheek.Domain.Entities;
using Bibliotheek.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBib.Controllers
{
    public class BoekController : Controller
    {
        private EFBibliotheekRepository _db;
        
        public BoekController()
        {
            _db = new EFBibliotheekRepository();
        }

        public ActionResult TestNewBoek()
        {
            return View("NewBoek_z_layout");
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

        
        
        public ActionResult CreateOrLookUp(Isbn isbn)
        {
            //boek bestaat in de db*
            // boek opzoeken*
            // boekinfo uit db weergeven, vragen hoeveel exemplaren en ze toevoegen*

            // bestaat niet in db
            // zoeken op het net
            //   gevonden: boekinfo van website weergeven, vragen hoeveel exemplaar toevoegen

            //   niet gevonden: lege boekvelden laten invullen, # exemplaren, toevoegen

            
            if (ModelState.IsValid)
            {
                var boekInDb = _db.Boeken.Where(b => b.Isbn.Nummer == isbn.Nummer).FirstOrDefault();

                if (boekInDb != null)
                {
                    return View("NewBoek", boekInDb);
                }
                else
                {
                    //boek bestaat niet in db
                    // op isbndb.org zoeken
                    //niet doen in testfase
                    //var webBoek = WebZoekBoek.ZoekBoek(isbn);
                    //if (webBoek != null) {
                    //    _db.Boeken.Add(webBoek);
                    //    _db.SaveChanges();
                    //    return View("NewBoek",webBoek);
                    //}
                    //boek niet op het net gevonden
                    var leegBoek = new Boek();
                    leegBoek.Isbn = isbn;
                    
                    leegBoek.Uitgever = new Uitgever();
                    return View("NewBoek", leegBoek);
                }
            }
        
        

            return View("Create",isbn);
        }


        public ActionResult CreateBoekOrExemplaar(Boek boek, int aantalEx) { 
            //is het boek in de db dan aantal exemplaren toevoegen
            //anders nieuw boek aanmaken
            
            if (boek != null) {
                var boekInDb = _db.Boeken.Where(b => b.Isbn.Nummer == boek.Isbn.Nummer).FirstOrDefault();
                if (boekInDb != null)
                {
                    //boek gevonden in db
                    //exemplaren toevoegen aan bestaand boek
                    ExemplarenToevoegen(boekInDb, aantalEx);
                    return RedirectToAction("Index", "Home");
                }
                else { 
                    //boek niet gevonden in db
                    //nieuw boek maken en exemplaren eraan toevoegen
                    if (ModelState.IsValid)
                    {
                        var uitgeverInDb = _db.Uitgevers.Where(u => u.Naam.Contains(boek.Uitgever.Naam)).FirstOrDefault();
                        if (uitgeverInDb != null) {
                            boek.Uitgever = uitgeverInDb;
                        }
                        _db.Boeken.Add(boek);
                        _db.SaveChanges();
                        ExemplarenToevoegen(boek, aantalEx);
                        
                        if(boek.Auteurs.LongCount()!=0){
                            return RedirectToAction("Index", "Home");
                        }
                        else{
                            return RedirectToAction("CreateVoorBoek", "Auteur", new { boekId = boek.Id });
                        }
                    }
                }
            }
            return View();
        }

        private void ExemplarenToevoegen(Boek boek, int aantalEx) {
            if (boek != null)
            {
                for (int i = 0; i < aantalEx; i++)
                {
                    Exemplaar ex = new Exemplaar { Commentaar = "ex" + (i + 1) };
                    
                    boek.Exemplaren.Add(ex);
                    _db.SaveChanges();
                }
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
