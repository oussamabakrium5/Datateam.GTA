using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Datateam.Security
{
    public static class SecurityDependencyInjection
    {
        public static IServiceCollection AddDatateamSecurity(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<SecurityDbContext>(options =>
            {
                options.UseSqlServer(config.GetSection("ConnectionStrings:SecurityConnection").Value);
            });




            /*services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();*/



            /*services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.GetSection("Jwt:Issuer").Value,
                    ValidAudience = config.GetSection("Jwt:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                    (config.GetSection("Jwt:Key").Value))
                };
            });*/

            /*services.AddIdentityCore<ApplicationUser>(cfg =>{});*/

            /*services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<SecurityDbContext>()
            .AddDefaultTokenProviders();*/

            /*services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();*/

            services.AddIdentityApiEndpoints<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<SecurityDbContext>();
                   /* .AddClaimsPrincipalFactory<AppClaimsFactory>();*/

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "MyIdentityAuth";
                options.LoginPath = "/auth/login";
            });

            /*services.AddAuthentication().AddCookie(options =>
            {
                options.LoginPath = "/login"; // Path to your login endpoint
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set cookie expiration time
                options.SlidingExpiration = true; // Refresh cookie expiration on each request
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401; // Send 401 Unauthorized on redirect
                    return Task.CompletedTask;
                };
                options.ClaimsIssuer = "YourAPI";
            });*/

            services.AddAuthorization();

            services.AddScoped<IIAMService, IAMService>();

            /*services.AddHttpContextAccessor();*/

            return services;
        }
    }
}
