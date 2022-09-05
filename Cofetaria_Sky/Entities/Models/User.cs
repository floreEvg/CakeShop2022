using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cofetaria_Sky.Entities.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public string Role { get; set; }

    }
}
