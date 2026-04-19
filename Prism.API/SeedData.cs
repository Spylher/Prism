using Microsoft.AspNetCore.Identity;
using Prism.Domain.Entities;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Identity;

namespace Prism.API;

public static class SeedData
{
    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var clientRepo = services.GetRequiredService<IClientRepository>();

        const string adminEmail = "sphylher@gmail.com";
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
        const string adminRole = "Admin";
        const string clientRole = "Client";
        const string adminName = "Spylher";

        if (string.IsNullOrWhiteSpace(adminPassword))
            throw new Exception("ADMIN_PASSWORD not set");

        // 1. Criar role se não existir
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(adminRole));
            await roleManager.CreateAsync(new IdentityRole<Guid>(clientRole));
        }

        // 2. Criar usuário se não existir
        var user = await userManager.FindByEmailAsync(adminEmail);
        if (user == null)
        {
            var client = new Client(adminEmail, "22");
            client.AddDaysToExpiration(365);
            await clientRepo.AddAsync(client);

            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                UserName = adminEmail,
                FullName = adminName,
                NormalizedUserName = adminEmail.ToUpperInvariant(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpperInvariant(),
            };
            
            var result = await userManager.CreateAsync(user, adminPassword);

            if (!result.Succeeded)
                throw new Exception("Erro ao criar admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // 3. Adicionar role se não tiver
        if (!await userManager.IsInRoleAsync(user, adminRole))
            await userManager.AddToRoleAsync(user, adminRole);
    }
}