using AuthService.Application;
using AuthService.Domain;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddControllers();  

// Add services to the container.

var app = builder.Build();

// =============================
// INITIALISATION DES ROLES
// (Créés automatiquement au démarrage)
// =============================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    // Ajout des rôles dans la base de données s’ils n’existent pas déjà
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = new[] { RoleUser.ADMIN.ToString(), RoleUser.USER.ToString() };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role)) // Si le role n’existe pas, on le crée
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Ajout d’un utilisateur admin par défaut s’il n’existe pas déjà
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var adminEmail = "admin@gmail.com";
    var adminMdp = "Admin123!";
    if (await userManager.FindByEmailAsync(adminEmail) == null) // Si l’utilisateur n’existe pas, on le crée
    {
        var adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = adminEmail,
            IsFirstLogin = false
        };
        var result = await userManager.CreateAsync(adminUser, adminMdp);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, RoleUser.ADMIN.ToString());
        }
    }
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();        // Active l’authentification (lecture du token JWT)
app.UseAuthorization();         // Active l’autorisation ([Authorize])

app.MapControllers();           // Mappe les routes des controllers

app.Run();