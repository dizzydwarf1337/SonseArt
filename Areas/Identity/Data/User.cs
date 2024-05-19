using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SonseArt.Models;

namespace SonseArt.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(40)]
    public string LastName { get; set; }

    [Required]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Incorrect format")]
    public string Phone {  get; set; }

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
    public string CartId {  get; set; }
    [InverseProperty("User")]
    public Cart ShoppingCart { get; set; } = new Cart();
    [InverseProperty("User")]
    public List<Order> MyOrders { get; set; } = new();
}

