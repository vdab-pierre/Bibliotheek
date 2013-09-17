using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bibliotheek.Domain.Entities;
using Bibliotheek.Domain.Concrete;

namespace MvcBibTests.Controllers
{
    public class BrolController : Controller
    {
        private EFBibliotheekRepository db = new EFBibliotheekRepository();

        //
        // GET: /Brol/

        public ActionResult Index()
        {
            return View(db.Boeken.ToList());
        }

        //
        // GET: /Brol/Details/5

        public ActionResult Details(int id = 0)
        {
            Boek boek = db.Boeken.Find(id);
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(boek);
        }

        //
        // GET: /Brol/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Brol/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Boek boek)
        {
            if (ModelState.IsValid)
            {
                db.Boeken.Add(boek);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(boek);
        }

        //
        // GET: /Brol/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Boek boek = db.Boeken.Find(id);
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(boek);
        }

        //
        // POST: /Brol/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Boek boek)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boek).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(boek);
        }

        //
        // GET: /Brol/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Boek boek = db.Boeken.Find(id);
            if (boek == null)
            {
                return HttpNotFound();
            }
            return View(boek);
        }

        //
        // POST: /Brol/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Boek boek = db.Boeken.Find(id);
            db.Boeken.Remove(boek);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}