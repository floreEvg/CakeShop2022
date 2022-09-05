using System.Collections.Generic;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using PagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Cofetaria_Sky.Pages
{
    [AllowAnonymous]
    public class ProduseModel : PageModel
    {
        [BindProperty]
        public List<Product> Torturi { get; set; }

        [BindProperty]
        public List<Product> Prajituri { get; set; }

        [BindProperty]
        public List<Product> Patiserie { get; set; }

        [BindProperty]
        public List<Product> Indisponibile { get; set; }


        public User MyUser { get; set; }

        public int page = 1;
        public int size = 3;
        
        private readonly SkyContext _db;

        private readonly UserManager<User> _userManager;

        public ProduseModel(SkyContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;

        }

        public async Task OnGetAsync()
        {
            Torturi = _db.Products.Where(p => p.Category == "Tort" && p.Stock == true).OrderBy(p => p.Name).ToPagedList(page, size).ToList();
            

            Prajituri = _db.Products.Where(p => p.Category == "Prajitura" && p.Stock == true).OrderBy(p => p.Name).ToPagedList(page, size).ToList();
            
            Patiserie = _db.Products.Where(p => p.Category == "Patiserie" && p.Stock == true).OrderBy(p => p.Name).ToPagedList(page, size).ToList();
           
            Indisponibile = _db.Products.Where(p => p.Stock == false).OrderBy(p => p.Category).OrderBy(p => p.Name).ToPagedList(page, size).ToList();

            var userName = User.Identity.Name;

            if(userName != null)
            {
                MyUser = await _userManager.FindByNameAsync(userName);
            }
        }  
    }
}
