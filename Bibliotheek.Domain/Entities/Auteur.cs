using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek.Domain.Entities
{
    public partial class Auteur
    {
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Familienaam { get; set; }
        public virtual ICollection<Boek> Boeken { get; set; }
        public Auteur()
        {
            Boeken = new HashSet<Boek>();
        }
    }
}
