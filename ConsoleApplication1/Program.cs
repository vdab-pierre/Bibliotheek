using Bibliotheek.Domain.Concrete;
using Bibliotheek.Domain.Entities;
using Bibliotheek.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var boek =WebZoekBoek.ZoekBoek(new Isbn { Nummer = "9780297870470" } );
            //boek.Auteurs.Add(new Auteur { Familienaam="Joske",Voornaam="Vermeulen"});
            //boek.Auteurs.Add(new Auteur { Familienaam = "Joske", Voornaam = "Vermeulen" });
            foreach (Auteur a in boek.Auteurs) {
                Console.WriteLine(string.Format("{0} {1}",a.Voornaam,a.Familienaam));
            }
        }
    }
}
