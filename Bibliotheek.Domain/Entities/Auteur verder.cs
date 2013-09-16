using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bibliotheek.Domain.Entities
{
    public partial class Auteur
    {
        public object Tags { get; set; } //object of html tags ...
        [NotMapped]
        public bool IsSelected { get; set; } //bool value to select a checkbox on the list
    }
}