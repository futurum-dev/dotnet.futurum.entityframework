using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Futurum.Test.Result;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Futurum.EntityFramework.Tests;

public class EntityFrameworkResultExtensionsTryAsyncEnumerableTests
{
    [Fact]
    public async Task success()
    {
        var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                               .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryAsyncEnumerable.Success")
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

        var retrievedNumberEntities = dbContext.Numbers.TryAsyncEnumerable();

        retrievedNumberEntities.ShouldBeSuccessWithValueEquivalentToAsync(x => x,AsyncEnumerable(numberEntities));
    }

    private static async IAsyncEnumerable<TestEntity> AsyncEnumerable(IEnumerable<TestEntity> numbers)
    {
        await Task.Yield();

        foreach (var number in numbers)
        {
            yield return number;
        }
    }
}