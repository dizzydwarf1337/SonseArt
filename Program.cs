using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SonseArt.Areas.Identity.Data;
using SonseArt.Data;
using SonseArt.Models;




var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SonseArtContextConnection") ?? throw new InvalidOperationException("Connection string 'SonseArtContextConnection' not found.");

builder.Services.AddDbContext<SonseArtContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).
        AddEntityFrameworkStores<SonseArtContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User"};

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization("en-UY", "fr-FR");
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapGet("/", async context =>
{
    context.Response.Redirect("/Products");
});


app.Run();
