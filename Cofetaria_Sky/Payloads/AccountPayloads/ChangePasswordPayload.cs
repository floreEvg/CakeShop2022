using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cofetaria_Sky.Payloads.AccountPayloads
{
    public class ChangePasswordPayload
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
