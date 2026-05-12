using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Transit.Infrastructure.Identity;
using Transit.Infrastructure.Persistence;
using Transit.Infrastructure.Persistence.Seeding;

namespace Transit.Api.IntegrationTests;

public class CustomWebApplicationFactory : IAsyncLifetime
{
    private readonly PostgreSqlContainer _pgContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("transit_test")
        .WithUsername("test_user")
        .WithPassword("test_pass")
        .Build();

    private WebApplicationFactory<Program>? _factory;
    public HttpClient Client { get; private set; } = null!;
    public IServiceProvider Services => _factory!.Server.Services;

    public async Task InitializeAsync()
    {
        await _pgContainer.StartAsync();
        var connectionString = _pgContainer.GetConnectionString();

        // Krok 1: migracje i seed bezpośrednio przez EF Core (omijamy IoC factory)
        var opts = new DbContextOptionsBuilder<TransitDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using (var db = new TransitDbContext(opts))
        {
            await db.Database.MigrateAsync();
            await SeedSlownikow.SeedAsync(db);
        }

        // Krok 2: wystartuj factory z prawidłowym connection stringiem przez env var
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", connectionString);
        Environment.SetEnvironmentVariable("Jwt__SecretKey", "INTEGRATION_TEST_SECRET_KEY_32CH!!");
        Environment.SetEnvironmentVariable("Jwt__Issuer", "TransitApi");
        Environment.SetEnvironmentVariable("Jwt__Audience", "TransitClients");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b =>
            {
                b.UseSetting("ASPNETCORE_ENVIRONMENT", "Testing");
            });

        // Task.Run ucieka z synchronization context xUnit (konieczne dla HostFactoryResolver)
        Client = await Task.Run(() => _factory.CreateClient());

        // Krok 3: utwórz role przez Identity (wymaga uruchomionego serwisu)
        using var scope = _factory.Server.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        foreach (var role in Roles.All)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new ApplicationRole(role));
    }

    public async Task DisposeAsync()
    {
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", null);
        Environment.SetEnvironmentVariable("Jwt__SecretKey", null);
        Environment.SetEnvironmentVariable("Jwt__Issuer", null);
        Environment.SetEnvironmentVariable("Jwt__Audience", null);
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);

        Client?.Dispose();
        if (_factory is not null) await _factory.DisposeAsync();
        await _pgContainer.StopAsync();
    }
}
