// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SonseArt.Areas.Identity.Data;
using SonseArt.Data;

namespace SonseArt.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [MaxLength(30)]
            public string FirstName { get; set; }

            [MaxLength(40)]
            public string LastName { get; set; }

            [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Incorrect format")]
            public string Phone { get; set; }

            [MaxLength(30)]
            public string City { get; set; }
            [MaxLength(30)]
            public string Street { get; set; }
            [MaxLength(7)]
            public string House { get; set; }
            [MaxLength(5)]
            public string? Local { get; set; }

            [MaxLength(10)]
            public string PostalCode { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = user.Phone;
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var street = user.Street;
            var city = user.City;
            var house = user.House;
            var local = user.Local;
            var postalCode = user.PostalCode;
            Username = userName;

            Input = new InputModel
            {
                Phone = phoneNumber, 
                FirstName = firstName,
                LastName = lastName,
                Street = street,
                City = city,
                House = house,
                Local = local,
                PostalCode = postalCode
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = user.Phone;
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var street = user.Street;
            var city = user.City;
            var house = user.House;
            var local = user.Local;
            var postalCode = user.PostalCode;
            if (Input.Phone != phoneNumber)
            {
                user.Phone = Input.Phone;
            }
            if (Input.FirstName != firstName)
            {
                user.FirstName = Input.FirstName;
            }
            if(Input.LastName != lastName) 
            {
                user.LastName = Input.LastName;
            }
            if(Input.City!= city) 
            {
                user.City = Input.City;
            }
            if(Input.Street != street) 
            {
                user.Street= Input.Street;
            }
            if(Input.House != house) 
            {
                user.House = Input.House;
            }
            if(Input.Local != local)
            {
                user.Local = Input.Local;
            }
            if (Input.PostalCode != postalCode)
            {
                user.PostalCode = Input.PostalCode;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
