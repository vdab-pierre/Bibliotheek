﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheek.Domain.Entities
{
    public partial class Auteur
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string author_id { get; set; }
        public virtual ICollection<Boek> Boeken { get; set; }
    }
}
