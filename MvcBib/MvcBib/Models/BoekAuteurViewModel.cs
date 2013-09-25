using Bibliotheek.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBib.Models
{
    public class BoekAuteurViewModel
    {

        public Boek Boek { get; set; }
        public int BoekId { get; set; }
        public Auteur Auteur { get; set; }
    }
}