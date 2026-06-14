using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transit.Infrastructure.Identity;

namespace Transit.Infrastructure.Persistence.Seeding;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, bool seedDemo)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TransitDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await db.Database.MigrateAsync();

        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new ApplicationRole(role));
        }

        await SeedSlownikow.SeedAsync(db);

        await SeedAdminUser(userManager);

        if (seedDemo)
        {
            await SeedDemo.SeedAsync(db);
        }
    }

    private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@transit.local";
        const string adminPassword = "Admin123!";

        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin is null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, adminPassword);
            await userManager.AddToRoleAsync(admin, Roles.Admin);
        }
        else
        {
            // Ensure existing admin has Admin role
            if (!await userManager.IsInRoleAsync(existingAdmin, Roles.Admin))
            {
                await userManager.AddToRoleAsync(existingAdmin, Roles.Admin);
            }
        }
    }
}
