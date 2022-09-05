using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cofetaria_Sky.Payloads.AccountPayloads
{
    public class LoginPayload
    {
        [Required(ErrorMessage ="Adresa de email este obligatorie")]
        [EmailAddress(ErrorMessage ="Adresa de email este invalidă")]
        public string Email { get; set; }


        [Required(ErrorMessage ="Parola este obligatorie")]
        public string Password { get; set; }
    }
}
