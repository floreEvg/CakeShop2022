using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cofetaria_Sky.Pages.Order
{
    public class CosModel : PageModel
    {
        [BindProperty]
        public List<Product> Products { get; set; }

        [BindProperty]
        public List<float> Quantities { get; set; }

        [BindProperty]

        public List<string> Details { get; set; }

        public float sum = 0;

        private readonly SkyContext _db;

        public CosModel(SkyContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            var list = new List<Product>();
            var qnt = new List<float>();
            var dtl = new List<string>();
            string session = GetString(HttpContext.Session, "cart");

            if (session != null)
            {
                var x = session.Split("~");

                for (int i = 0; i < x.Length; i += 3)
                {
                    Product produs = _db.Products.SingleOrDefault(p => p.Id == Convert.ToInt32(x[i]));

                    if (produs != null)
                    {
                        list.Add(produs);
                        qnt.Add((float)Convert.ToDecimal(x[i + 1]));
                        dtl.Add(x[i + 2]);
                    }
                }
                Products = list;
                Quantities = qnt;
                Details = dtl;
            }
        }

        public IActionResult OnPostSterge(int id, string cantitate, string info)
        {
            string list = null;

            string session = GetString(HttpContext.Session, "cart");

            if (session != null)
            {
                var x = session.Split("~");

                for (int i = 0; i < x.Length; i += 3)
                {
                   if(x[i] == id.ToString() && x[i+2] == info && x[i+1] == cantitate)
                    {
                        continue;
                    }
                    else
                    {
                        if (list == null)
                        {
                            list = x[i] + "~" + x[i + 1] + "~" + x[i + 2];
                        }
                        else
                        {
                            list = list + "~" + x[i] + "~" + x[i + 1] + "~" + x[i + 2];
                        }
                    }
                   
                }
                if (list != null)
                {
                    SetString(HttpContext.Session, "cart", list);
                }
                else
                {
                    HttpContext.Session.Remove("cart");
                }
            }
            return RedirectToPage("/Order/Cos");
        }

        public IActionResult OnPostStergeTot()
        {
            HttpContext.Session.Remove("cart");
            
            return RedirectToPage("/Order/Cos");
        }

        private static void SetString(ISession session, string key, string value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
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
