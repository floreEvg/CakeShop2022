using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cofetaria_Sky.Payloads.AccountPayloads
{
    public class RegisterPayload
    {
        [Required(ErrorMessage = "Prenumele este obligatoriu")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Adresa de email este obligatorie")]
        [EmailAddress(ErrorMessage = "Adresă de email invalidă")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Numărul de telefon este obligatoriu")]
        [Phone(ErrorMessage = "Număr de telefon invalid")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Adresa este obligatorie")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie")]
        public string Password { get; set; }
    }
}
