using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SonseArt.Data;
using SonseArt.Migrations;
using SonseArt.Models;

namespace SonseArt.Pages.Products
{
    [Authorize(Policy = "Admin")]
    public class EditModel : PageModel
    {
        private readonly SonseArt.Data.SonseArtContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EditModel(SonseArt.Data.SonseArtContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        [BindProperty]
        public Models.Product Product { get; set; } = default!;
        public IFormFile? ImageFile;
        public string ImgDel;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            TempData["ImgDel"] = product.Image;
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? ImageFile)
        {
            ImgDel = TempData["ImgDel"] as string;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (ImageFile != null)
            {
                DeleteImage(ImgDel);
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                Product.Image = "/images/" + uniqueFileName;
            }
            else
            {
                Product.Image = ImgDel;
            }
            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }



        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }


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
        public string SaveImage (IFormFile ImageFile)
        {

            return " ";
        }
    }
}
