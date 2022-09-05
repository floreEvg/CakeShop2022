using System.Collections.Generic;
using System.Linq;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList;

namespace Cofetaria_Sky.Pages
{
    public class PrajituriModel : PageModel
    {
        public List<Product> Prajituri { get; set; }

        private readonly SkyContext _db;

        public int page;
        public int size = 3;
        public int count;

        public PrajituriModel(SkyContext db)
        {
            _db = db;
        }
        public void OnGet(int current_page)
        {
            page = current_page;
            count = _db.Products.Count(p => p.Category == "Prajitura" && p.Stock == true);

            Prajituri = _db.Products.Where(p => p.Category == "Prajitura" && p.Stock == true).OrderBy(p => p.Name).ToPagedList(current_page, size).ToList();
        }
    }
}
