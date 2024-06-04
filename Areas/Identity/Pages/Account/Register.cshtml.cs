// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using SonseArt.Areas.Identity.Data;
using SonseArt.Models;

namespace SonseArt.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
        }

       
        [BindProperty]
        public InputModel Input { get; set; }

       
        public string ReturnUrl { get; set; }

     
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public Cart _cart = new Cart();
     
        public class InputModel
        {
          
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

         
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [MaxLength(30)]
            public string FirstName { get; set; }

            [Required]
            [MaxLength(40)]
            public string LastName { get; set; }
            [Required]
            [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Incorrect format")]
            public string Phone { get; set; }
            [Required]
            [MaxLength(30)]
            public string City { get; set; }
            [Required]
            [MaxLength(30)]
            public string Street { get; set; }
            [Required]
            [MaxLength(7)]
            public string House { get; set; }
            [MaxLength(5)]
            public string? Local { get; set; }

            [Required]
            [MaxLength(10)]
            public string PostalCode { get; set; }

        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.City = Input.City;
                user.Street = Input.Street;
                user.House = Input.House;
                user.Local = Input.Local;
                user.PostalCode = Input.PostalCode;
                user.Phone=Input.Phone;
                _cart.User = user;
                _cart.UserId = user.Id;
                _cart.Items = new();
                _cart.Id = Guid.NewGuid().ToString();
                user.ShoppingCart = _cart;
                user.CartId = _cart.Id;
                
                
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);


                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    await _signInManager.SignInAsync(user, isPersistent: false);


                    await _userManager.AddToRoleAsync(user, "Admin");
                    return LocalRedirect(returnUrl);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
