using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class DetailsModel : PageModel
    {
        private readonly SonseArt.Data.SonseArtContext _context;

        public DetailsModel(SonseArt.Data.SonseArtContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = default!;
        public List<OrderItem> orderItems { get; set; } = default!;
        [BindProperty]
        public string orderItemId { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
                orderItems = await (from u in _context.OrderItem
                                    where u.OrderId == id
                                    select u).ToListAsync();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteItemAsync()
        {
            var item = await _context.OrderItem.FirstOrDefaultAsync(x => x.Id == orderItemId);
            _context.OrderItem.Remove(item);
            await _context.SaveChangesAsync();
            return await OnGetAsync(item.OrderId);
        }
    }
}
