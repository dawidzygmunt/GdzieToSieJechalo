using Microsoft.EntityFrameworkCore;
using Transit.Infrastructure.Persistence;

namespace Transit.Application.UnitTests.Helpers;

public static class InMemoryDbContextFactory
{
    public static TransitDbContext Create(string? dbName = null)
    {
        var opts = new DbContextOptionsBuilder<TransitDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;
        return new TransitDbContext(opts);
    }
}
