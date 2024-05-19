using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SonseArt.Data;
using SonseArt.Models;

namespace SonseArt.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly SonseArtContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CreateModel(SonseArtContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public Product product { get; set; }

        public IFormFile ImageFile;
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile ImageFile)
        {
            if (ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }
                product.Image = "/images/" + uniqueFileName;
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
