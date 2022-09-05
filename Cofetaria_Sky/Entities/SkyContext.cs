using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cofetaria_Sky.Entities
{
    
    public class SkyContext : IdentityDbContext
    {
        public SkyContext(DbContextOptions<SkyContext> options) : base(options)
        { 

        }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CodePassword> Codes { get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }


    }

}
