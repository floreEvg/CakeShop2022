using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cofetaria_Sky.Pages
{
    public class CodResetModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Codul este obligatoriu")]
        public string Cod { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Parola este obligatorie")]
        public string Parola { get; set; }

        private readonly SkyContext _db;

        private readonly UserManager<User> _userManager;

        public CodResetModel(SkyContext db, UserManager<User> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var found_user = _userManager.Users.SingleOrDefault(u => u.Email == Email);

                if (found_user != null)
                {
                    var code = _db.Codes.SingleOrDefault(c => c.UserId == found_user.Id
                    && c.Code == Cod);

                    if (code != null)
                    {
                        _db.Codes.Remove(code);

                        await _userManager.RemovePasswordAsync(found_user);

                        await _userManager.AddPasswordAsync(found_user, Parola);

                        _db.SaveChanges();

                        TempData["Message"] = "Parola a fost resetată";
                        return RedirectToPage("/Account/Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cod incorect");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email incorect");
                }
            }
            return Page();
        }
    }
}
