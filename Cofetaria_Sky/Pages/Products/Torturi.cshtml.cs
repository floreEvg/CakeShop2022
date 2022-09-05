using System.Collections.Generic;
using System.Linq;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagedList;

namespace Cofetaria_Sky.Pages
{
    public class TorturiModel : PageModel
    {
        public List<Product> Torturi { get; set; }

        private readonly SkyContext _db;

        public int page;
        public int size = 3;
        public int count;

        public TorturiModel (SkyContext db)
        {
            _db = db;
        }
        public void OnGet(int current_page)
        {
            page = current_page;
            count = _db.Products.Count(p => p.Category == "Tort" && p.Stock == true);

            Torturi = _db.Products.Where(p => p.Category == "Tort" && p.Stock == true).OrderBy(p => p.Name).ToPagedList(current_page, size).ToList();
            
        }
    }
}
