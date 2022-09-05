using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList;

namespace Cofetaria_Sky.Pages
{
    [Authorize]
    public class ContModel : PageModel
    {
        [BindProperty]
        public User MyUser { get; set; }

        public int page = 1;
        public int size = 3;

        [BindProperty]
        public List<Entities.Models.Order> Orders { get; set; }

        private readonly SignInManager<User> _signInManager;

        private readonly UserManager<User> _userManager;

        private readonly SkyContext _db;

        public ContModel(SignInManager<User> signInManager, UserManager<User> userManager, SkyContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userName = User.Identity.Name;

            MyUser = await _userManager.FindByNameAsync(userName);

            var t = _db.Orders.Where(o => o.UserId == MyUser.Id).OrderByDescending(o => o.Date).ToPagedList(page, size);

            var list = new List<Entities.Models.Order>();

            for (int i = 0; i < t.Count; i++)
            {
                var x = t[i];
                list.Add(x);
            }
            Orders = list;

            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }
        
        public RedirectToPageResult OnPostUpdate()
        {
            return RedirectToPage("/Account/Update");
        }

        public RedirectToPageResult OnPostChangePassword()
        {
            return RedirectToPage("/Account/ChangePassword");
        }

        public RedirectToPageResult OnPostChangeEmail()
        {
            return RedirectToPage("/Account/ChangeEmail");
        }
    }
}
