using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Transit.Infrastructure.Persistence;

public class TransitDbContextFactory : IDesignTimeDbContextFactory<TransitDbContext>
{
    public TransitDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = ConnectionStringResolver.Resolve(
            config.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=transit_db;Username=transit_user;Password=transit_pass");

        var opts = new DbContextOptionsBuilder<TransitDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new TransitDbContext(opts);
    }
}
