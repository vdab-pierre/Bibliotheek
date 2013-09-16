using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek.Domain.Entities
{
    public class Etiket
    {
        public int Id { get; set; }
        public string Tekst { get; set; }
        public virtual ICollection<Exemplaar> Exemplaren { get; set; }
    }
}
