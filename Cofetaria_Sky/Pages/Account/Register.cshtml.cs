using System.Linq;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Cofetaria_Sky.Payloads.AccountPayloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Cofetaria_Sky.Pages
{
    [BindProperties]
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        public RegisterPayload RegisterPayload { get; set; }

        private readonly SkyContext _db;

        public RegisterModel(SkyContext db,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var found_user = _db.Users.SingleOrDefault(u => u.Email == RegisterPayload.Email);

                if (found_user != null)
                {
                    ModelState.AddModelError("", "Există un cont cu adresa de email introdusă!");
                }
                else
                {
                    var user = new User()
                    {
                        UserName = RegisterPayload.Email,
                        FirstName = RegisterPayload.FirstName,
                        LastName = RegisterPayload.LastName,
                        Email = RegisterPayload.Email,
                        PhoneNumber = RegisterPayload.Phone,
                        Address = RegisterPayload.Address,
                        Role = "default"
                    };

                    var result = await _userManager.CreateAsync(user, RegisterPayload.Password);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToPage("/Account/Cont");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("" , error.Description);
                        }
                    }
                }
            }
            return Page();
        }

    }
}
