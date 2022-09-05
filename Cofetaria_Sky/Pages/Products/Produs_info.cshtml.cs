using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cofetaria_Sky.Pages
{
    [AllowAnonymous]
    public class Produs_infoModel : PageModel
    {
        public string Name { get; set; }
        [BindProperty]
        public Product Produs { get; set; }

        [BindProperty]
        public float cantitate { get; set; }

        [BindProperty]
        public string detalii { get; set; }

        public User MyUser { get; set; }

        private readonly SkyContext _db;

        private readonly UserManager<User> _userManager;

        public Produs_infoModel(SkyContext db, UserManager<User> userManager)
        {
            _db = db; 
            _userManager = userManager;
        }

        public async Task OnGetAsync(string id)
        {
            int nr_id = Convert.ToInt32(id);
            Produs = _db.Products.SingleOrDefault(p => p.Id == nr_id);
            if(Produs != null) Name = Produs.Name;

            var userName = User.Identity.Name;

            if (userName != null)
            {
                MyUser = await _userManager.FindByNameAsync(userName);
            }
        }

        public IActionResult OnPost(int id)
        {
            if (ModelState.IsValid)
            {
                Product produs = _db.Products.SingleOrDefault(p => p.Id == id);
                if (produs != null)
                {
                    string session = GetString(HttpContext.Session, "cart");

                    if (produs.Category != "Tort" || detalii == null)
                    {
                        detalii = "-";
                    }

                    if (session == null)
                    {
                        SetString(HttpContext.Session, "cart", produs.Id.ToString() + "~" + cantitate.ToString() + "~" + detalii);
                    }
                    else
                    {
                        if (produs.Category == "Tort")
                        {
                            session = session + "~" + produs.Id + "~" + cantitate + "~" + detalii;
                            SetString(HttpContext.Session, "cart", session);
                        }
                        else
                        {
                            var x = session.Split("~");
                            string x_new = null;
                            bool ok = false;

                            for (int i = 0; i < x.Length; i += 3)
                            {
                                if (x[i] == produs.Id.ToString())
                                {
                                    ok = true;
                                    x[i + 1] = (Convert.ToDouble(x[i + 1]) + cantitate).ToString();
                                }
                                if (x_new == null)
                                {
                                    x_new = x[i] + "~" + x[i + 1] + "~" + x[i + 2];
                                }
                                else
                                {
                                    x_new = x_new + "~" + x[i] + "~" + x[i + 1] + "~" + x[i + 2];
                                }
                            }
                            if (ok == false)
                            {
                                x_new = x_new + "~" + produs.Id + "~" + cantitate + "~" + detalii;
                            }

                            SetString(HttpContext.Session, "cart", x_new);
                        }

                    }
                    TempData["Message"] = "Produsul a fost adăugat în coș";
                    return RedirectToPage("/Products/Produse");
                }
                else ModelState.AddModelError("", "Produs inexistent");
                
            }
            return Page();
        }

        public IActionResult OnPostAddStock()
        {
            var produs = _db.Products.SingleOrDefault(p => p.Id == Produs.Id);

            if(produs != null )
            {
                produs.Stock = true;

                _db.SaveChanges();

                TempData["Message"] = "Produsul a fost adăugat în stoc";
                return RedirectToPage("/Products/Produse");
            }
            return Page();
        }

        private void SetString(ISession session, string key, string value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
        }

        private string GetString(ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(data);
        }

    }
}