using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek.Domain.Entities
{
    public partial class Boek
    {
        
        public int Id { get; set; }
        public virtual Isbn Isbn { get; set; }
        public string Titel { get; set; }
        public string Samenvatting { get; set; }
        public virtual ICollection<Auteur> Auteurs { get; set; }
        public virtual ICollection<Exemplaar> Exemplaren { get; set; }
        public virtual Uitgever Uitgever { get; set; }
        public Boek()
        {
            Auteurs = new HashSet<Auteur>();
            Exemplaren = new HashSet<Exemplaar>();
        }
    }
}
