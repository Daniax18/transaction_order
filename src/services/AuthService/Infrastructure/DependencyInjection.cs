using AuthService.Domain;
using AuthService.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register infrastructure services here
            // For example, you can register your database context, repositories, etc.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DockerDb")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Règles de sécurité des mots de passe
                options.Password.RequireDigit = true;                       // doit contenir un chiffre
                options.Password.RequiredLength = 6;                        // longueur minimale
                options.Password.RequireNonAlphanumeric = false;            // caractères spéciaux non obligatoires
                options.Password.RequireUppercase = true;                   // au moins une majuscule
                options.Password.RequireLowercase = true;                   // au moins une minuscule

                options.User.RequireUniqueEmail = true;                     // Email unique pour chaque utilisateur
            })                
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
