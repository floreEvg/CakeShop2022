using System.Collections.Generic;
using System.Linq;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList;

namespace Cofetaria_Sky.Pages
{
    public class PatiserieModel : PageModel
    {
        public List<Product> Patiserie { get; set; }

        private readonly SkyContext _db;

        public int page;
        public int size = 3; //6
        public int count;

        public PatiserieModel(SkyContext db)
        {
            _db = db;
        }
        public void OnGet(int current_page)
        {
            page = current_page;
            count = _db.Products.Count(p => p.Category == "Patiserie" && p.Stock == true);

            Patiserie = _db.Products.Where(p => p.Category == "Patiserie" && p.Stock == true).OrderBy(p => p.Name).ToPagedList(current_page, size).ToList();
        }
    }
}
