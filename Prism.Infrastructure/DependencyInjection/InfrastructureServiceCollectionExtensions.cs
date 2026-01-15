using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prism.Application.Interfaces;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Data;
using Prism.Infrastructure.Identity;
using Prism.Infrastructure.Repositories;


namespace Prism.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Identity by Microsoft.AspNetCore.Identity
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
                {
                    // opções de senha, lockout etc.
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // HttpContextAccessor by Microsoft.AspNetCore.Http
            services.AddHttpContextAccessor();

            // Repositórios / UoW / serviços de infra
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICurrentUser, IdentityCurrentUser>();
            return services;
        }
    }
}
