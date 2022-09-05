using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cofetaria_Sky.Pages.Products
{
    public class DeleteProductModel : PageModel
    {
        private readonly SkyContext _db;

        private readonly UserManager<User> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public string Name { get; set; }

        public Product Produs { get; set; }

        public User MyUser { get; set; }

        public DeleteProductModel(SkyContext db, UserManager<User> userManager,IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userName = User.Identity.Name;

            if (userName != null)
            {
                MyUser = await _userManager.FindByNameAsync(userName);

                if (MyUser.Role == "admin")
                {
                    int nr_id = Convert.ToInt32(id);
                    Produs = _db.Products.SingleOrDefault(p => p.Id == nr_id);
                    if (Produs != null)
                    {
                        Name = Produs.Name;
                        return Page();
                    }
                }
            }
            return RedirectToPage("/Account/AccessDenied");
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userName = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);

            if (ModelState.IsValid)
            {
                if (user.Role == "admin")
                {
                    var produs = _db.Products.SingleOrDefault(p => p.Id == id);

                    produs.Stock = false;

                    _db.SaveChanges();

                    TempData["Message"] = "Produsul a fost scos din stoc";
                    return RedirectToPage("/Products/Produse");
                }
                else
                {
                    return RedirectToPage("/Account/AccessDenied");
                }
            }
            return Page();
        }
    }
}
