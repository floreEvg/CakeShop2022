using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cofetaria_Sky.Pages.Account
{
    [BindProperties]
    public class DetaliiComandaModel : PageModel
    {
        private readonly SkyContext _db;

        private readonly UserManager<User> _userManager;

        public User MyUser { get; set; }

        public Entities.Models.Order MyOrder { get; set; }

        public List<OrderProduct> MyProducts { get; set; }
        
        public List<Product> Products { get; set; }
        public DetaliiComandaModel(SkyContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int idComanda)
        {
            var userName = User.Identity.Name;

            MyUser = await _userManager.FindByNameAsync(userName);

            MyOrder = _db.Orders.SingleOrDefault(o => o.Id == idComanda && o.UserId == MyUser.Id);

            if(MyOrder != null)
            {
                MyProducts  = _db.OrderProducts.Where(op => op.OrderId == MyOrder.Id).ToList();

                var prds = new List<Product>();

                for(int i = 0; i < MyProducts.Count; i ++)
                {
                    var p = _db.Products.SingleOrDefault(p => p.Id == MyProducts[i].ProductId);

                    prds.Add(p);
                }
                Products = prds;
                
                return Page();
            }

            return RedirectToPage("/Account/AccessDenied");
        }
    }
}
