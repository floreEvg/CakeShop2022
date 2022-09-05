using System.Threading.Tasks;
using Cofetaria_Sky.Entities.Models;
using Cofetaria_Sky.Payloads.AccountPayloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace Cofetaria_Sky.Pages
{
    [AllowAnonymous]
    [BindProperties]
    public class LoginModel : PageModel
    {
        public LoginPayload LoginPayload { get; set; }

        private readonly SignInManager<User> _signInManager;

        public LoginModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
               var identityResult = await _signInManager.PasswordSignInAsync(LoginPayload.Email, LoginPayload.Password, true, false);
               if (identityResult.Succeeded)
               {
                    return RedirectToPage("/Account/Cont");
               }
                else
                {
                    ModelState.AddModelError("", "Email sau parolă incorecte");
                }
            }
            return Page();
        }
    }
}
