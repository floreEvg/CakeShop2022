using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Cofetaria_Sky.Pages.Products
{
    public class AddProductModel : PageModel
    {
        [BindProperty]
        public User MyUser { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        private readonly UserManager<User> _userManager;

        private readonly SkyContext _db;

        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        [Required(ErrorMessage = "Numele este obligatoriu")]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Detaliile sunt obligatorii")]
        public string Info { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Categoria este obligatorie")]
        public string Category { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Prețul este obligatoriu")]
        public float Price { get; set; }

        public AddProductModel(UserManager<User> userManager, SkyContext db, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var userName = User.Identity.Name;

            if (userName != null)
            {   
                MyUser = await _userManager.FindByNameAsync(userName);

                if (MyUser.Role == "admin")
                {
                    return Page();
                }
            }
            return RedirectToPage("/Account/AccessDenied");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userName = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);

            if (ModelState.IsValid)
            {
                if(user.Role == "admin")
                {
                    Product p = _db.Products.SingleOrDefault(p => p.Name == Name);

                    if(p == null)
                    {
                        string Image = null;

                        if (Photo != null &&
                            (Photo.ContentType.Equals("image/jpg") ||
                            Photo.ContentType.Equals("image/jpeg") ||
                            Photo.ContentType.Equals("image/png")))
                        {
                            Image = ProcessUploadedFile();
                        }

                        if (Image != null)
                        {
                            _db.Products.Add(new Product
                            {
                                Name = Name,
                                Category = Category,
                                Info = Info,
                                Image = Image,
                                Price = Price
                            });

                            _db.SaveChanges();


                            TempData["Message"] = "Produs adăugat cu succes!";
                            return RedirectToPage("/Products/Produse");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Imaginea este obligatorie");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Există un produs cu acest nume");
                    }
                }
                else
                {
                    return RedirectToPage("/Account/AccessDenied");
                }
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
