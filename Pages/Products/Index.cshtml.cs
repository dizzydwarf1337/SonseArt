using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SonseArt.Data;
using SonseArt.Models;

namespace SonseArt.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly SonseArt.Data.SonseArtContext _context;

        public IndexModel(SonseArt.Data.SonseArtContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;
        [BindProperty]
        public string SearchString { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Product = await _context.Product.ToListAsync();
        }
        public async Task OnPostFindAsync()
        {
            Product= await _context.Product.Where(x=>x.Name.Contains(SearchString)).ToListAsync();
        }
    }
}
