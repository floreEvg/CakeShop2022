using System;
using System.IO;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cofetaria_Sky.Pages.Account
{
    [Authorize]
    public class UpdateModel : PageModel
    {
        public User MyUser { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        private readonly UserManager<User> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]

        public string FirstName { get; set; }

        [BindProperty]

        public string LastName { get; set; }

        [BindProperty]

        public string Address { get; set; }

        [BindProperty]

        public string Phone { get; set; }

        public UpdateModel(UserManager<User> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = User.Identity.Name;

            MyUser = await _userManager.FindByNameAsync(userName);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userName = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);

            if(ModelState.IsValid)
            {
                if (Photo != null && 
                    (Photo.ContentType.Equals("image/jpg") || 
                    Photo.ContentType.Equals("image/jpeg") ||
                    Photo.ContentType.Equals("image/png")))
                {
                    if (user.Image != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                            "imagini/profil", user.Image);
                        System.IO.File.Delete(filePath);
                    }
                    user.Image = ProcessUploadedFile();
                }

                if(!FirstName.Equals(null) && !FirstName.Equals("") && !FirstName.Equals(user.FirstName))
                {
                    user.FirstName = FirstName;
                }

                if(LastName != null && LastName != "" && LastName != user.LastName)
                {
                    user.LastName = LastName;
                }

                if(Address != null && Address != "" && Address != user.Address)
                {
                    user.Address = Address;
                }

                if (Phone != null && Phone != "" && Phone != user.PhoneNumber)
                {
                    user.PhoneNumber = Phone;
                }

                var result = await _userManager.UpdateAsync(user);

                if(result.Succeeded)
                {
                    TempData["Message"] = "Informațiile contului au fost actualizate";
                    return RedirectToPage("/Account/Cont");
                }
                
            }

            return Page();     
        }

        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (Photo != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "imagini/profil");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
