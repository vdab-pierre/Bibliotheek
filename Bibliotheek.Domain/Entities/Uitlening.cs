using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bibliotheek.Abstract;

namespace Bibliotheek.Domain.Entities
{
    public class Uitlening
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public DateTime Terug { get; set; }
        public virtual Exemplaar Exemplaar { get; set; }
        public IGebruiker Gebruiker{ get; set; }
    }
}
