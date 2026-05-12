using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transit.Application.Abstractions.Identity;
using Transit.Application.Abstractions.Persistence;
using Transit.Application.Abstractions.Routing;
using Transit.Application.Abstractions.Time;
using Transit.Infrastructure.Identity;
using Transit.Infrastructure.Persistence;
using Transit.Infrastructure.Routing;
using Transit.Infrastructure.Time;

namespace Transit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<TransitDbContext>(opts =>
            opts.UseNpgsql(config.GetConnectionString("DefaultConnection"),
                npg => npg.MigrationsAssembly(typeof(TransitDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<TransitDbContext>());

        services.AddIdentity<ApplicationUser, ApplicationRole>(opts =>
        {
            opts.Password.RequireDigit = true;
            opts.Password.RequiredLength = 8;
            opts.Password.RequireUppercase = true;
            opts.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<TransitDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<IWyszukiwaczPolaczen, WyszukiwaczPolaczenJednoliniowy>();

        return services;
    }
}
