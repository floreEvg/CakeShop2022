using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Cofetaria_Sky.Pages.Order
{
    [Authorize]
    public class ComandaModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        private readonly SkyContext _db;

        private IConfiguration _config { get; }

        public User MyUser { get; set; }
        public List<float> Quantities { get; set; }

        public List<string> Details { get; set; }

        public List<Entities.Models.Product> Products { get; set; }


        [BindProperty]
        public string Key { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Adresa este obligatorie")]
        public string Address { get; set; }

        [BindProperty]
        public float sum { get; set; }
      

        public ComandaModel(UserManager<User> userManager, SkyContext db, IConfiguration config)
        {
            _userManager = userManager;
            _db = db;
            _config = config;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Key = _config.GetSection("Stripe")["PublishableKey"];
            var list = new List<Entities.Models.Product>();
            var qnt = new List<float>();
            var dtl = new List<string>();
            string session = GetString(HttpContext.Session, "cart");

            if (session != null)
            {
                var x = session.Split("~");

                for (int i = 0; i < x.Length; i += 3)
                {
                    Entities.Models.Product produs = _db.Products.SingleOrDefault(p => p.Id == Convert.ToInt32(x[i]));

                    if (produs != null)
                    {
                        list.Add(produs);
                        qnt.Add((float)Convert.ToDecimal(x[i + 1]));
                        dtl.Add(x[i + 2]);

                        if(produs.Category == "Tort")
                        {
                            sum += produs.Price * (float)Convert.ToDecimal(x[i+1]);
                        }
                        else
                        {
                            sum += produs.Price * (float)Convert.ToDecimal(x[i + 1]) / 100;
                        }
                    }
                }
                Products = list;
                Quantities = qnt;
                Details = dtl;
            }
            else
            {
                return RedirectToPage("/Order/Cos");
            }

            var userName = User.Identity.Name;

            MyUser = await _userManager.FindByNameAsync(userName);

            return Page();
        }

        public async Task<IActionResult> OnPostRambursAsync()
        {
            var userName = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);

            _db.Orders.Add(new Entities.Models.Order
            {
                UserId = user.Id,
                Total = sum,
                Address = Address,
                Date = DateTime.Now,
                Payment = "ramburs"
            });

            _db.SaveChanges();

            var order = _db.Orders.SingleOrDefault(o => o.UserId == user.Id && o.Total == sum && o.Address == Address);

            if(order != null)
            {
                string session = GetString(HttpContext.Session, "cart");

                if (session != null)
                {
                    var x = session.Split("~");

                    for (int i = 0; i < x.Length; i += 3)
                    {
                        float total = 0;
                        float cantitate = 0;

                        Entities.Models.Product produs = _db.Products.SingleOrDefault(p => p.Id == Convert.ToInt32(x[i]));

                        if (produs != null)
                        {
                            if (produs.Category == "Tort")
                            {
                                total = (float)Convert.ToDecimal(x[i + 1]) * produs.Price;
                                cantitate = (float)Convert.ToDecimal(x[i + 1]) * 1000;
                            }
                            else
                            {
                                total = (float)Convert.ToDecimal(x[i + 1]) * produs.Price / 100;
                                cantitate = (float)Convert.ToDecimal(x[i + 1]);
                            }

                            _db.OrderProducts.Add(new OrderProduct
                            {
                                OrderId = order.Id,
                                ProductId = produs.Id,
                                Details = x[i + 2],
                                Quantity = cantitate,
                                Total = total
                            });
  
                        }
                    }
                    _db.SaveChanges();
                    HttpContext.Session.Remove("cart");
                    TempData["Message"] = "Comanda a fost inregistrata";
                    return RedirectToPage("/Account/Cont");
                }
                else
                {
                    ModelState.AddModelError("", "Nu există produse în coș");
                }
            }
            else
            {
                ModelState.AddModelError("", "Comanda nu a putut fi înregistrată");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostOnlineAsync(string stripeToken, string stripeEmail)
        {
            var userName = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(userName);

            try
            {
                var optionsCust = new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Name = user.LastName + " " + user.FirstName,
                    Phone = user.PhoneNumber

                };
                var serviceCust = new CustomerService();
                Customer customer = serviceCust.Create(optionsCust);
                var optionsCharge = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt64(sum * 100 / 4.7),
                    Currency = "RON",
                    Description = "Cofetăria Sky",
                    Source = stripeToken,
                    ReceiptEmail = stripeEmail
                };
                var service = new ChargeService();
                Charge charge = service.Create(optionsCharge);
                if (charge.Status == "succeeded")
                {
                    _db.Orders.Add(new Entities.Models.Order
                    {
                        UserId = user.Id,
                        Total = sum,
                        Address = Address,
                        Date = DateTime.Now,
                        Payment = "online"
                    });

                    _db.SaveChanges();

                    var order = _db.Orders.SingleOrDefault(o => o.UserId == user.Id && o.Total == sum && o.Address == Address);

                    if (order != null)
                    {
                        string session = GetString(HttpContext.Session, "cart");

                        if (session != null)
                        {
                            var x = session.Split("~");

                            for (int i = 0; i < x.Length; i += 3)
                            {
                                float total = 0;
                                float cantitate = 0;

                                Entities.Models.Product produs = _db.Products.SingleOrDefault(p => p.Id == Convert.ToInt32(x[i]));

                                if (produs != null)
                                {
                                    if (produs.Category == "Tort")
                                    {
                                        total = (float)Convert.ToDecimal(x[i + 1]) * produs.Price;
                                        cantitate = (float)Convert.ToDecimal(x[i + 1]) * 1000;
                                    }
                                    else
                                    {
                                        total = (float)Convert.ToDecimal(x[i + 1]) * produs.Price / 100;
                                        cantitate = (float)Convert.ToDecimal(x[i + 1]);
                                    }

                                    _db.OrderProducts.Add(new OrderProduct
                                    {
                                        OrderId = order.Id,
                                        ProductId = produs.Id,
                                        Details = x[i + 2],
                                        Quantity = cantitate,
                                        Total = total
                                    });

                                }
                            }
                            _db.SaveChanges();
                            HttpContext.Session.Remove("cart");
                            TempData["Message"] = "Comanda a fost înregistrată";
                            return RedirectToPage("/Account/Cont");
                        }
                        else
                        {
                            _db.Orders.Remove(order);
                            _db.SaveChanges();
                            TempData["Message"] = "Nu există produse în coș";
                        }
                    }
                    else
                    {
                        TempData["Message"] = "Comanda nu a putut fi înregistrată";
                    }
                }
                else
                {
                    TempData["Message"] = "Plata nu a putut fi procesată";

                }
            }
            catch(Exception ex)
            {
                TempData["Message"] = "Comanda nu a putut fi procesată";
            }
            
            return RedirectToPage("/Order/Comanda");
        }

        private static string GetString(ISession session, string key)
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

