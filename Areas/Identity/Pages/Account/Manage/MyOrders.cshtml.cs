using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SonseArt.Areas.Identity.Data;
using SonseArt.Data;
using SonseArt.Models;

namespace SonseArt.Areas.Identity.Pages.Account.Manage
{
    public class MyOrdersModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SonseArtContext _context;
        public MyOrdersModel(UserManager<User> userManager,SonseArtContext context) 
        {
            _userManager = userManager;
            _context = context;
        }
        public List<Order> MyOrders { get; set; } = default!;
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            MyOrders = await (from u in _context.Order
                              where u.UserId == user.Id
                              select u).ToListAsync();
            foreach(var order in MyOrders)
            {
                order.Items= await (from u in _context.OrderItem
                                    where u.OrderId==order.Id
                                    select u).ToListAsync();
            }
        }
    }
}
