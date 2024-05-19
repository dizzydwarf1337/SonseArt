using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SonseArt.Data;
using SonseArt.Models;
using Microsoft.AspNetCore.Hosting;

namespace SonseArt.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly SonseArt.Data.SonseArtContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DeleteModel(SonseArt.Data.SonseArtContext context, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public bool DeleteImage(string fileName)
        {
            try
            {
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, fileName.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No existing file {ex.Message}");
                return false;
            }
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                Product = product;
                try
                {
                    DeleteImage(product.Image);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Image problem {ex.Message}");
                    return NotFound();
                }
                
                    _context.Product.Remove(Product);
                    await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
