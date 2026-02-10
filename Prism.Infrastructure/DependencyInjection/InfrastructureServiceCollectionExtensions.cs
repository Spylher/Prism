using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Prism.Application.Interfaces;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Data;
using Prism.Infrastructure.Identity;
using Prism.Infrastructure.Repositories;
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
        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                // pw, lockout etc.☺
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // configure o cookie do Identity explicitamente
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = ".Prism.Identity.Application";

            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.Path = "/";

            options.ExpireTimeSpan = TimeSpan.FromDays(3);
            options.SlidingExpiration = true;
        });

        // Identity options configuration (alternative to the one in AddIdentity)
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
        return services;
    }
}
