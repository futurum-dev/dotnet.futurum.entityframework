using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Futurum.Test.Result;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Futurum.EntityFramework.Tests;

public class EntityFrameworkResultExtensionsTrySaveChangesAsyncTests
{
    [Fact]
    public async Task success()
    {
        var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                               .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryCountAsync.Success")
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

        var action = () => dbContext.TrySaveChangesAsync();

        await action.Should().NotThrowAsync();

        var retrievedEntityCount = await dbContext.Numbers.TryCountAsync();

        retrievedEntityCount.ShouldBeSuccessWithValue(numberEntities.Count);
    }
}