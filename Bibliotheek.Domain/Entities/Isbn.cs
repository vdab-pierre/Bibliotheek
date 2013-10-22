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
        [IsIsbn(ErrorMessage="Tik of scan een correct isbn.")]
        [Required(ErrorMessage="Isbn moet ingevuld worden.")]
        public string Nummer { get; set; }
        public virtual Boek Boek { get; set; }
    }
}
