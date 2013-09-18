namespace Bibliotheek.Domain.Migrations
{
    using Bibliotheek.Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<Bibliotheek.Domain.Concrete.EFBibliotheekRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Bibliotheek.Domain.Concrete.EFBibliotheekRepository context)
        {

            Auteur julia = new Auteur { Familienaam = "Lerman", Voornaam = "Julia" };
            Auteur adam = new Auteur { Familienaam = "Freeman", Voornaam = "Adam" };
            Auteur henry = new Auteur { Familienaam = "Lee", Voornaam = "Henry" };
            Auteur eugene = new Auteur { Familienaam = "Chuvyrov", Voornaam = "Eugene" };

            Uitgever apress = new Uitgever { Naam = "Apress" };
            Uitgever oreilly = new Uitgever { Naam = "O'Reilly" };

            Isbn efIsbn = new Isbn { Nummer = "9780596807269" };
            Isbn mvc4Isbn = new Isbn { Nummer = "9781430242469" };
            Isbn wpIsbn = new Isbn { Nummer = "9781430241348" };

            Boek ef = new Boek
            {
                Isbn = efIsbn,
                Titel = "Programming Entity Framework",
                Samenvatting = "Get a thorough ...",
                Auteurs = new List<Auteur> { 
                    julia
                },
                Uitgever = oreilly
            };
            Boek mvc4 = new Boek
            {
                Isbn = mvc4Isbn,
                Titel = "Pro ASP.NET MVC 4",
                Samenvatting = "The ASPNET MVC 4  framework ...",
                Auteurs = new List<Auteur> { 
                    adam
                },
                Uitgever = apress
            };
            Boek wp = new Boek
            {
                Isbn = wpIsbn,
                Titel = "Beginning Windows Phone App Development",
                Samenvatting = "Learn the skills ...",
                Auteurs = new List<Auteur> { 
                    adam
                },
                Uitgever = apress
            };

            Etiket et1 = new Etiket { Tekst = ".Net" };

            Exemplaar mvc4Ex1 = new Exemplaar
            {
                Boek = mvc4,
                Druk = "1ste test", //enkel voor in ontwikkeling omdat ik deze kolom als identifierexpression gebruik in de AddOrUpdate 
                Commentaar = "In slechte staat",
                Etiket = et1
            };
            Exemplaar mvc4Ex2 = new Exemplaar
            {
                Boek = mvc4,
                Druk = "2de test",
                Commentaar = "In goede staat",
                Etiket = et1
            };
            Exemplaar efEx1 = new Exemplaar
            {
                Boek = ef,
                Druk = "3de test",
                Commentaar = "In goede staat",
                Etiket = et1
            };
            Exemplaar wpEx1 = new Exemplaar
            {
                Boek = wp,
                Druk = "4de test",
                Commentaar = "In goede staat",
                Etiket = et1
            };



            context.Exemplaren.AddOrUpdate(ex => ex.Druk,
                mvc4Ex1, mvc4Ex2, efEx1, wpEx1);

            context.SaveChanges();
        }
    }

}
