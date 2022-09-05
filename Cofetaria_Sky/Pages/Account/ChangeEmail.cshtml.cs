using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cofetaria_Sky.Pages.Account
{   
    [Authorize]
    public class ChangeEmailModel : PageModel
    {
        [Required(ErrorMessage = "Parola este obligatorie")]
        [BindProperty]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu")]
        [EmailAddress(ErrorMessage = "Adresă de email invalidă")]
        [BindProperty]
        public string Email { get; set; }


        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public ChangeEmailModel( UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;

                var user = await _userManager.FindByNameAsync(userName);

                var result = await _userManager.CheckPasswordAsync(user, Password);

                if(result)
                {
                    user.Email = Email;

                    var userNameResult = await _userManager.SetUserNameAsync(user, Email);

                    var change = await _userManager.UpdateAsync(user);

                    if(change.Succeeded && userNameResult.Succeeded)
                    {

                        await _userManager.UpdateNormalizedUserNameAsync(user);

                        await _signInManager.SignOutAsync();

                        await _signInManager.PasswordSignInAsync(Email, Password, true, false);

                        TempData["Message"] = "Adresa de email a fost actualizată";
                        return RedirectToPage("/Account/Cont");
                    }
                    else
                    {
                        foreach (var error in change.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        foreach (var error in userNameResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
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
