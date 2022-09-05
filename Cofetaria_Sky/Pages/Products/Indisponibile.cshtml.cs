using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList;

namespace Cofetaria_Sky.Pages.Products
{
    public class IndisponibileModel : PageModel
    {
        public List<Product> Indisponibile { get; set; }

        private readonly SkyContext _db;

        public int page;
        public int size = 3;
        public int count;

        public User MyUser { get; set; }

        public UserManager<User> _userManager;
        public IndisponibileModel(SkyContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(int current_page)
        {
            var userName = User.Identity.Name;

            if (userName != null)
            {
                MyUser = await _userManager.FindByNameAsync(userName);
            }

            if(MyUser == null)
            {
                return RedirectToPage("/Account/Login");
            }
            else
            {
                if (MyUser.Role != "admin")
                {
                    return RedirectToPage("/Account/AccessDenied");
                }
            }

            page = current_page;
            count = _db.Products.Count(p => p.Stock == false);

            Indisponibile = _db.Products.Where(p => p.Stock == false).OrderBy(p => p.Name).ToPagedList(current_page, size).ToList();

            return Page();
        }
    }
}
