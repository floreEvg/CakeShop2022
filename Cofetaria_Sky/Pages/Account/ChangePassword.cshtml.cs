using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cofetaria_Sky.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        [Required(ErrorMessage = "Parola veche este obligatorie")]
        [BindProperty]
        public string Password { get; set; }

        [Required(ErrorMessage = "Parola nouă este obligatorie")]
        [BindProperty]
        public string New_Password { get; set; }


        private readonly UserManager<User> _userManager;


        public ChangePasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;

                var user = await _userManager.FindByNameAsync(userName);

                var change = await _userManager.ChangePasswordAsync(user, Password, New_Password);

                if (change.Succeeded)
                {
                    TempData["Message"] = "Parola a fost actualizată";
                    return RedirectToPage("/Account/Cont");
                }
                else
                {
                    ModelState.AddModelError("", "Parolă incorectă");
                }

            }
            return Page();
        }
    }
}
