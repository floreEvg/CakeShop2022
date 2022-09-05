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

namespace Cofetaria_Sky.Pages.Account
{
    [Authorize]
    [BindProperties]
    public class ComenziModel : PageModel
    {
        public List<Entities.Models.Order> Orders { get; set; }

        private readonly SkyContext _db;

        private readonly UserManager<User> _userManager;

        public User MyUser { get; set; }

        public int page;
        public int size = 6;
        public int count;

        public ComenziModel(UserManager<User> userManager, SkyContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task OnGetAsync(int current_page)
        {
            page = current_page;

            var userName = User.Identity.Name;

            MyUser = await _userManager.FindByNameAsync(userName);


            count = _db.Orders.Count(o => o.UserId == MyUser.Id);

            Orders = _db.Orders.Where(o => o.UserId == MyUser.Id).OrderByDescending(o => o.Date).ToPagedList(page, size).ToList();

        }

    }
}
