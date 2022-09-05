using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cofetaria_Sky.Payloads.AccountPayloads
{
    public class ChangeEmailPayload
    {
        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
