using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SonseArt.Data;
using SonseArt.Models;

namespace SonseArt.Pages.Orders
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly SonseArt.Data.SonseArtContext _context;

        public IndexModel(SonseArt.Data.SonseArtContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Order = await _context.Order
                .Include(o => o.User).ToListAsync();
        }
    }
}
