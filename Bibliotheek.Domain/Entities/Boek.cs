using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek.Domain.Entities
{
    public class Boek
    {
        //blah
        //nogis blah
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Titel { get; set; }
        public string Samenvatting { get; set; }
        public virtual ICollection<Auteur> Auteurs { get; set; }
        public virtual ICollection<Exemplaar> Exemplaren { get; set; }
        public virtual Uitgever Uitgever { get; set; }
        
    }
}
