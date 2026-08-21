using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prism.Application.Interfaces;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Data;
using Prism.Infrastructure.Identity;
using Prism.Infrastructure.Repositories;
using Prism.Infrastructure.Security;
using Prism.Infrastructure.Settings;
using System.Security.Claims;
using System.Text;

namespace Prism.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        //DbContext
        services.AddDbContext<AppDbContext>(options =>
        {
            // add-migration by Microsoft.EntityFrameworkCore.Tools requires Sqlite or SqlServer provider
            if (env.IsDevelopment())
                options.UseSqlite("Data Source=app.db");
            else
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        
        // Identity by Microsoft.AspNetCore.Identity
        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager();

        // configure jwt settings from configuration
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = ClaimTypes.Role,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = jwtSettings!.Issuer,
                    ValidAudience = jwtSettings.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };

                // vey beautiful
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/minimap"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection("JwtSettings"))
            .Validate(s => !string.IsNullOrEmpty(s.SecretKey), "SecretKey is required")
            .ValidateOnStart();

        // Identity by Microsoft.AspNetCore.Identity for cookie-based auth (alternative to JWT)
        //services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        //    {
        //        // pw, lockout etc.☺
        //        options.User.RequireUniqueEmail = true;
        //        options.Password.RequiredLength = 8;
        //        options.Password.RequireDigit = true;
        //        options.Password.RequireNonAlphanumeric = true;
        //        options.Password.RequireUppercase = true;
        //        options.Password.RequireLowercase = true;
        //    })
        //    .AddEntityFrameworkStores<AppDbContext>()
        //    .AddDefaultTokenProviders();

        // configure Cookie Identity
        //services.ConfigureApplicationCookie(options =>
        //{
        //    options.Cookie.Name = ".Prism.Identity.Application";

        //    options.Cookie.HttpOnly = true;
        //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //    options.Cookie.SameSite = SameSiteMode.Strict;
        //    options.Cookie.Path = "/";

        //    options.ExpireTimeSpan = TimeSpan.FromDays(3);
        //    options.SlidingExpiration = true;
        //});

        // Identity options configuration (alternative to th
        // e one in AddIdentity)
        //services.Configure<IdentityOptions>(options =>
        //{
        //    options.Password.RequireDigit = true;
        //    options.Password.RequireUppercase = true;
        //    options.Password.RequiredLength = 8;
        //});

        // HttpContextAccessor by Microsoft.AspNetCore.Http
        services.AddHttpContextAccessor();

        // Repositories / infra services
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICurrentUser, IdentityCurrentUser>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITokenHashService, TokenHashService>();
        services.AddScoped<ICurrentRequest, CurrentRequest>();
        services.AddScoped<IPlayerTagRepository, PlayerTagRepository>();
        services.AddScoped<IDiscordProfileRepository, DiscordProfileRepository>();
        services.AddScoped<IAppProfileRepository, AppProfileRepository>();

        return services;
    }
}
