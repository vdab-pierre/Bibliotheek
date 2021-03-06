﻿using Bibliotheek.Domain.Concrete;
using Bibliotheek.Domain.Entities;
using MvcBib.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBib.Controllers
{
    public class HomeController : Controller
    {
        private EFBibliotheekRepository _db;
        public HomeController()
        {
            _db = new EFBibliotheekRepository();
        }

        [CustomHandleError]
        public ActionResult Index(string searchTerm=null)
        {
            //try
            //{
                
                var boeken = (from b in _db.Boeken
                              from a in b.Auteurs
                              where
                              searchTerm == null
                              || a.Familienaam.StartsWith(searchTerm)
                              || a.Voornaam.StartsWith(searchTerm)
                              || b.Titel.Contains(searchTerm)
                              || b.Isbn.Nummer.StartsWith(searchTerm)
                              || b.Uitgever.Naam.StartsWith(searchTerm)
                              select b).GroupBy(b => b.Id)
                                .Select(g => g.FirstOrDefault())
                                .ToList();

                //nog geen searchTerm
                //var boeken = _db.Boeken.ToList();
                return View(boeken);
            //}
            //catch (DataException) {
            //    throw new Exception("Er is een probleem met de database.");
            //}
            
            
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
