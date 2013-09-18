using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek.Domain.Entities
{
    public class Isbn
    {
        [Key,ForeignKey("Boek")]
        public int Id { get; set; }
        [IsIsbn]
        public string Nummer { get; set; }
        public virtual Boek Boek { get; set; }
    }
}
