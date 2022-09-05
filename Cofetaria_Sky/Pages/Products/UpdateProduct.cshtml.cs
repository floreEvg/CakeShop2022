using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cofetaria_Sky.Pages.Products
{
    public class UpdateProductModel : PageModel
    {
        private readonly SkyContext _db;

        private readonly UserManager<User> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public string Name { get; set; }

        public Product Produs { get; set; }

        public User MyUser { get; set; }

        public IFormFile Photo { get; set; }

        [BindProperty]
        public string Info { get; set; }

        [BindProperty]
        public string Category { get; set; }

        [BindProperty]
        public float Price { get; set; }

        public UpdateProductModel(SkyContext db, UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
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

                if(MyUser.Role == "admin")
                {
                    int nr_id = Convert.ToInt32(id);
                    Produs = _db.Products.SingleOrDefault(p => p.Id == nr_id);
                    if (Produs != null)
                    {
                        Name = Produs.Name;
                        Info = Produs.Info;
                        return Page();
                    }
                }
            }
            return RedirectToPage("/Account/AccessDenied");
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userName = User.Identity.Name;

            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (ModelState.IsValid)
                {
                    if (user.Role == "admin")
                    {
                        var p = _db.Products.SingleOrDefault(p => p.Id == id);

                        if (p != null)
                        {
                            if (Photo != null &&
                            (Photo.ContentType.Equals("image/jpg") ||
                            Photo.ContentType.Equals("image/jpeg") ||
                            Photo.ContentType.Equals("image/png")))
                            {
                                if (p.Image != null)
                                {
                                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                        "", p.Image);
                                    System.IO.File.Delete(filePath);
                                }
                                p.Image = ProcessUploadedFile();
                            }

                            p.Category = Category;

                            if (!Price.Equals(null) && !Price.Equals(p.Price))
                            {
                                p.Price = Price;
                            }

                            if (!Info.Equals(null) && !Info.Equals(p.Info))
                            {
                                p.Info = Info;
                            }

                            _db.SaveChanges();

                            TempData["Message"] = "Produs actualizat cu succes!";
                            return RedirectToPage("/Products/Produse");
                        }
                        else
                        {
                            TempData["Message"] = "Produs inexistent!";

                        }
                    }
                    else
                    {
                        TempData["Message"] = "Nu aveți access!";

                    }
                }
            }
            else
            {
                TempData["Message"] = "Este nevoie să vă logați!";

            }
            return Page();  
        }

        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (Photo != null)
            {
                string uploadsFolder = null;

                if (Category == "Tort")
                {
                    uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imagini/torturi");
                }
                else
                {
                    if (Category == "Prajitura")
                    {
                        uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imagini/prajituri");
                    }
                    else
                    {
                        if (Category == "Patiserie")
                        {
                            uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imagini/patiserie");
                        }
                    }
                }
                if (uploadsFolder != null)
                {
                    uniqueFileName = Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        Photo.CopyTo(fileStream);
                    }
                }
            }
            if (Category == "Tort")
            {
                return "imagini/torturi/" + uniqueFileName;
            }
            if (Category == "Prajitura")
            {
                return "imagini/prajituri/" + uniqueFileName;
            }
            if (Category == "Patiserie")
            {
                return "imagini/patiserie/" + uniqueFileName;
            }
            return uniqueFileName;
        }
    }
}
