using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek.Domain.Entities
{
    public class Exemplaar
    {
        public int Id { get; set; }
        public virtual Boek Boek { get; set; }
        public string Druk { get; set; }
        public string Commentaar { get; set; }
        public virtual ICollection<Uitlening> Uitleningen { get; set; }
        public virtual Etiket Etiket { get; set; }
        public Exemplaar()
        {
            Uitleningen = new HashSet<Uitlening>();
        }
    }
}
