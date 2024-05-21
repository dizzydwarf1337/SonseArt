using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SonseArt.Areas.Identity.Data;
using SonseArt.Data;
using SonseArt.Models;

namespace SonseArt.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly SonseArt.Data.SonseArtContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public DetailsModel(UserManager<User> userManager, SonseArt.Data.SonseArtContext context,SignInManager<User> signInManager)
        {
            _context = context;
            _userManager=userManager;
            _signInManager=signInManager;
        }
        [BindProperty]
        public Product Product { get; set; } = default!;
        [BindProperty]
        public List<Comment> _comments { get; set; } = default!;
        [BindProperty]
        public Comment comment { get; set; } = default!;
        public List<Product> RelatedProducts { get; set; } = default!;
        public Models.Cart cart { get; set; } = default!;
        public int Quantity { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            var comments = from c in _context.Comment
                           where c.ProductId == id
                           select c;
            _comments = comments.ToList();
            RelatedProducts = GetRandomObjects<Product>(await _context.Product.ToListAsync(), 4, product.Id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
                _comments = comments.ToList();
            }
            TempData["ProductId"] = Product.Id;
            return Page();
        }
        public async Task<IActionResult> OnPostAddCommentAsync()
        {
            int? productId = TempData["ProductId"] as int?;
            var user = await _userManager.GetUserAsync(User);
            var product = await _context.Product.Include(m=>m._comments).FirstOrDefaultAsync(m=>m.Id == productId);
            if (comment.Text != null)
            {
                comment.Author = user.FirstName + " " + user.LastName;
                comment.Created= DateTime.Now;
                comment.ProductId = product.Id;
                comment.AuthorId = user.Id;
                product._comments.Add(comment);
                await _context.SaveChangesAsync();
            }
            return await OnGetAsync(productId);
        }
        public async Task<IActionResult> OnPostDeleteAsync(int commentId)
        {
            var commentToDelete = await _context.Comment.FindAsync(commentId);
            if (commentToDelete == null)
            {
                return NotFound();
            }

            _context.Comment.Remove(commentToDelete);
            await _context.SaveChangesAsync();

            return await OnGetAsync(commentToDelete.ProductId); 
        }
        public async Task<IActionResult> OnPostAddToCartAsync(int Quantity)
        {
            if (!_signInManager.IsSignedIn(User)) 
            {
                return Redirect("~/Identity/Account/Login");
            } 
            int? productId = TempData["ProductId"] as int?;
            var user =await _userManager.GetUserAsync(User);
            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == productId);
            cart = await _context.Cart.FirstOrDefaultAsync(m => m.Id == user.CartId);
            foreach(var item in _context.CartItem)
            {
                if (item.CartId == cart.Id)
                {
                    cart.Items.Add(item);
                }
            }
            Models.CartItem _cartItem;
            if (cart.Items.Count > 0)
            {
                foreach (var item in cart.Items)
                {
                    _cartItem = item;
                    if (_cartItem.ProductId == product.Id)
                    {
                        item.Quantity += Quantity;
                        await _context.SaveChangesAsync();
                        return await OnGetAsync(productId);
                    }
                }
            }

            
                _cartItem = new();
                _cartItem.CartId = cart.Id;
                _cartItem.ProductId = product.Id;
                _cartItem.Quantity = 1;
                _cartItem.Id = Guid.NewGuid().ToString();
                _cartItem.ProductName = product.Name;
                _cartItem.ProductPrice= product.Price;
                _cartItem.ImgSrc = product.Image;
                _cartItem.Quantity = Quantity;
                cart.Items.Add(_cartItem);
            
            
            await _context.SaveChangesAsync();
            return await OnGetAsync(productId);
        }


        static List<Product> GetRandomObjects<T>(List<Product> list, int count,int productId)
        {
            List<Product> randomObjects = new List<Product>();
            Random rand = new Random();

            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);
                Product temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }

            for (int i = 0; i < count && i < list.Count; i++)
            {
                randomObjects.Add(list[i]);
            }

            return randomObjects;
        }
    }
}
