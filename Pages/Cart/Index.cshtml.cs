using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SonseArt.Areas.Identity.Data;
using SonseArt.Data;
using SonseArt.Models;

namespace SonseArt.Pages.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly SonseArt.Data.SonseArtContext _context;
        private readonly SignInManager<User>  _signInManager;
        private readonly UserManager<User> _userManager;
        public IndexModel(SonseArt.Data.SonseArtContext context, SignInManager<User> signInManager,UserManager<User> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public User user { get; set; } = default!;
        public Models.Cart Cart { get;set; } = default!;
        public List<Product> _products { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? cartId)
        {
            user = await _userManager.GetUserAsync(User);
            Cart = await _context.Cart.FirstOrDefaultAsync(c => c.Id == user.CartId);
            Cart.Items = (from i in _context.CartItem
                          where i.CartId == Cart.Id
                          select i).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteItemAsync(string CartItemId)
        {
            var CartItemToDelete = await _context.CartItem.FirstOrDefaultAsync(x=>x.Id == CartItemId);
            if (CartItemToDelete == null) return Page();
            else _context.CartItem.Remove(CartItemToDelete);
            await _context.SaveChangesAsync();
            return await OnGetAsync(CartItemToDelete.CartId);
            
        }

        public async Task<IActionResult> OnPostMakeAnOrderAsync(string CartId)
        {
                var user = await _userManager.GetUserAsync(User);
                List<CartItem> cartItems = await _context.CartItem.Where(x => x.CartId == CartId).ToListAsync();

                if (cartItems.Count < 1)
                {
                    return await OnGetAsync(CartId);
                }

                Order orderToAdd = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    User = user,
                    Status = "Waiting",
                    Adres = $"City: {user.City}, Street: {user.Street}, House: {user.House}",
                    Items = new List<OrderItem>(),
                    UserFirstName = user.FirstName,
                    UserLastName = user.LastName,
                    UserPhone = user.Phone,
                    date = DateTime.Now
                };

                foreach (var item in cartItems)
                {
                    OrderItem itemToAdd = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = item.ProductId,
                        OrderId = orderToAdd.Id,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        ProductPrice = item.ProductPrice
                    };

                    orderToAdd.Items.Add(itemToAdd);
                    await _context.OrderItem.AddAsync(itemToAdd);
                    _context.CartItem.Remove(item);
                }

                orderToAdd.OrderPrice = orderToAdd.Total();
                user.MyOrders.Add(orderToAdd);
                //await _context.Order.AddAsync(orderToAdd);
                await _context.SaveChangesAsync();

                return Redirect("/Products");
            
        }
    }
}
