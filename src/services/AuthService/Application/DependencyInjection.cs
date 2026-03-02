

using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {

            // =============================
            // CONFIGURATION JWT
            // (Authentification par token)
            // =============================
            var jwtSettings = configuration.GetSection("JwtSettings");
            // Récupère la clé secrète pour signer les tokens
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("SecretKey is not configured in appsettings.json");

            services.AddAuthentication(options =>
            {
                // Définit JWT comme méthode d’authentification par défaut
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Configuration du middleware JWT
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,                                                                  // Vérifie qui a émis le token
                    ValidateAudience = true,                                                                // Vérifie à qui le token est destiné
                    ValidateLifetime = true,                                                                // Vérifie expiration
                    ValidateIssuerSigningKey = true,                                                        // Vérifie signature du token
                    ValidIssuer = jwtSettings["Issuer"],                                                    // Doit correspondre au token
                    ValidAudience = jwtSettings["Audience"],                                                // Doit correspondre au token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),         // Clé utilisée pour signer et valider le token
                    ClockSkew = TimeSpan.Zero                                                               // Pas de tolérance sur l'expiration (plus strict)
                };
            });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, UserService>();
            return services;
        }
    }
}
