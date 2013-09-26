using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek.Domain.Entities
{
    public class Uitgever
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public virtual ICollection<Boek> Boeken { get; set; }
        public Uitgever()
        {
            Boeken = new HashSet<Boek>();
        }
    }
}
