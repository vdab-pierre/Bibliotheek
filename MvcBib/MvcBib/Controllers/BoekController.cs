﻿using Bibliotheek.Domain.Concrete;
using Bibliotheek.Domain.Entities;
using Bibliotheek.Domain.Utility;
using Newtonsoft.Json;
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
        public JsonResult Edit(string jsonBoek,int aantalEx)
        {
            if (ModelState.IsValid){
                //boek entity maken van jsonboek
                Boek boek = JsonConvert.DeserializeObject<Boek>(jsonBoek);
                if(boek!=null){
                    //auteurs
                    var auteurs = new HashSet<Auteur>();
                    foreach (var auteur in boek.Auteurs)
                    {
                        auteurs.Add(CreatAuteurOrUseExisting(auteur));
                    }
                    boek.Auteurs = auteurs;

                    //uitgever
                    var uitgever = CreateUitgeverOrUseExisting(boek.Uitgever);
                    boek.Uitgever = uitgever;
                    //exemplaren
                    var boekInDb = _db.Boeken.Where(b => b.Id == boek.Id).SingleOrDefault();
                    long aantalExInDb = 0;
                    if (boek != null)
                    {
                        aantalExInDb = boekInDb.Exemplaren.LongCount();
                    }
                    if (aantalExInDb < aantalEx) {
                        ExemplarenToevoegen(boek, aantalEx-aantalExInDb);
                    }
                    else if (aantalEx < aantalExInDb) {
                        ExemplarenVerwijderen(boek, aantalExInDb - aantalEx);
                    }
                    //boek
                    _db.Boeken.Attach(boek);
                    _db.Entry(boek).State = EntityState.Modified;
                    _db.SaveChanges();
                    
                    
                }    
            }
            

            return Json(new {result="ok" });
        }

        private void ExemplarenVerwijderen(Boek boek, long aantalEx)
        {
            _db.Exemplaren.Remove(boek.Exemplaren.First());
            _db.SaveChanges();
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
        public ActionResult WisAlleExemplarenVanBoekConfirmed(int id)
        {
            Boek boek = _db.Boeken.Find(id);
            var exemplaren = boek.Exemplaren;
            boek.Exemplaren = null;
            foreach (var ex in exemplaren)
            {
                _db.Exemplaren.Remove(ex);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ExemplarenVanBoek(int id)
        {
            Boek boek = _db.Boeken.Find(id);
            return View(boek);
        }

        public ActionResult WisExemplaarVanBoek(int exId)
        {
            var exemplaar = _db.Exemplaren.Find(exId);
            if (exemplaar != null)
            {
                return View(exemplaar);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("WisExemplaarVanBoek")]
        public ActionResult WisExemplaarVanBoekConfirmed(int exId)
        {
            var exemplaar = _db.Exemplaren.Find(exId);
            if (exemplaar != null)
            {
                _db.Exemplaren.Remove(exemplaar);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {
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
                    //boek gevonden => edit boek
                    return RedirectToAction("Edit", "Boek", new { Id = boekInDb.Id });
                }
                else
                {
                    //boek bestaat niet in db
                    // op isbndb.org zoeken
                    //niet doen in testfase
                    //var webBoek = WebZoekBoek.ZoekBoek(isbn);
                    //if (webBoek != null)
                    //{
                    //    _db.Boeken.Add(webBoek);
                    //    _db.SaveChanges();
                    //    return View("NewBoek", webBoek);
                    //}
                    //boek niet op het net gevonden
                    var leegBoek = new Boek();
                    leegBoek.Isbn = isbn;

                    leegBoek.Uitgever = new Uitgever();
                    return View("NewBoek", leegBoek);
                }
            }



            return View("Create", isbn);
        }

        
        [HttpPost]
        public JsonResult CreateBoekOrExemplaar(string jsonBoek, int aantalEx)
        {

            //boek entity maken van jsonboek
            Boek boek = JsonConvert.DeserializeObject<Boek>(jsonBoek);

            //is het boek in de db dan aantal exemplaren toevoegen
            //anders nieuw boek aanmaken
            if (boek != null)
            {
                //nieuw boek maken en exemplaren eraan toevoegen
                if (ModelState.IsValid)
                {
                    //uitgever
                    var uitgever = CreateUitgeverOrUseExisting(boek.Uitgever);
                    boek.Uitgever = uitgever;
                    //auteurs
                    var auteurs = new HashSet<Auteur>();
                    foreach (var auteur in boek.Auteurs) {
                        auteurs.Add(CreatAuteurOrUseExisting(auteur));
                    }
                    boek.Auteurs = auteurs;
                    _db.Boeken.Add(boek);
                    _db.SaveChanges();

                    //exemplaren
                    ExemplarenToevoegen(boek, aantalEx);
                }
            }

            return Json(new{result="ok"});
        }

        private Auteur CreatAuteurOrUseExisting(Auteur auteur)
        {
            var auteurInDb = _db.Auteurs.Where(a => a.Familienaam.Contains(auteur.Familienaam)&&a.Voornaam.Contains(auteur.Voornaam)).FirstOrDefault();
            if (auteurInDb != null)
            {
                return auteurInDb;
            }
            else
            {
                return auteur;
            }
        }

        private Uitgever CreateUitgeverOrUseExisting(Uitgever uitgever)
        {
            var uitgeverInDb = _db.Uitgevers.Where(u => u.Naam.Contains(uitgever.Naam)).FirstOrDefault();
            if (uitgeverInDb != null)
            {
                return  uitgeverInDb;
            }
            else {
                return uitgever;
            }
        }

        private void ExemplarenToevoegen(Boek boek, long aantalEx)
        {
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
            if (disposing)
            {
                _db.Dispose();
            }
        }
    }
}
