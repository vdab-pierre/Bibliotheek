using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MvcBib.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="GebruikersNaam moet ingevuld worden.")]
        [Display(Name = "Gebruikersnaam")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Wachtwoord moet ingevuld worden.")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
    }
}
