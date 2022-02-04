using System.Linq;
using System.Threading.Tasks;

using Futurum.Test.Result;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Futurum.EntityFramework.Tests;

public class EntityFrameworkResultExtensionsTryToListAsyncTests
{
    [Fact]
    public async Task success()
    {
        var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                               .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryToListAsync.Success")
                               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                               .EnableSensitiveDataLogging()
                               .Options;

        await using var dbContext = new TestDbContext(dbContextOptions);

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var numberEntities = Enumerable.Range(1, 10)
                                       .Select(i => new TestEntity { Id = i, Numeric = i })
                                       .ToList();

        dbContext.Numbers.AddRange(numberEntities);

        await dbContext.SaveChangesAsync();

        var retrievedNumberEntities = await dbContext.Numbers.TryToListAsync();

        retrievedNumberEntities.ShouldBeSuccessWithValueEquivalentTo(numberEntities);
    }
}