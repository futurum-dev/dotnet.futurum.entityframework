using System.Linq;
using System.Threading.Tasks;

using Futurum.Test.Option;
using Futurum.Test.Result;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Futurum.EntityFramework.Tests;

public class EntityFrameworkResultExtensionsTryFirstAsyncTests
{
    public class Result
    {
        private const string ERROR_MESSAGE = "Error Message";

        public class WithoutPredicate
        {
            [Fact]
            public async Task success_found_item()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryFirstAsync.Success.FoundItem")
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

                var retrievedNumberEntity = await dbContext.Numbers.TryFirstAsync(ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeSuccessWithValueEquivalentTo(numberEntities.First());
            }

            [Fact]
            public async Task success_not_found_item()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryFirstAsync.Success.NotFoundItem")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                await dbContext.SaveChangesAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TryFirstAsync(ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeFailureWithErrorContaining(ERROR_MESSAGE);
            }
        }
        
        public class WithPredicate
        {
            [Fact]
            public async Task success_found_item()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryFirstAsync.Success.FoundItem")
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

                var retrievedNumberEntity = await dbContext.Numbers.TryFirstAsync(x => x.Id == 1, ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeSuccessWithValueEquivalentTo(numberEntities.First());
            }

            [Fact]
            public async Task success_not_found_item()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryFirstAsync.Success.NotFoundItem")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                await dbContext.SaveChangesAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TryFirstAsync(x => x.Id == 1, ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeFailureWithErrorContaining(ERROR_MESSAGE);
            }
        }
    }
    
    public class ResultOption
    {
        public class WithoutPredicate
        {
            [Fact]
            public async Task success_found_item()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryFirstAsync.Success.FoundItem")
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

                var retrievedNumberEntity = await dbContext.Numbers.TryFirstAsync();

                retrievedNumberEntity.ShouldBeSuccessWithValueAssertion(x => x.ShouldBeHasValueWithValueEquivalentTo(numberEntities.First()));
            }

            [Fact]
            public async Task success_not_found_item()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryFirstAsync.Success.NotFoundItem")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                await dbContext.SaveChangesAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TryFirstAsync();

                retrievedNumberEntity.ShouldBeSuccessWithValueAssertion(x => x.ShouldBeHasNoValue());
            }
        }
        
        public class WithPredicate
        {
            [Fact]
            public async Task success_found_item()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryFirstAsync.Success.FoundItem")
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

                var retrievedNumberEntity = await dbContext.Numbers.TryFirstAsync(x => x.Id == 1);

                retrievedNumberEntity.ShouldBeSuccessWithValueAssertion(x => x.ShouldBeHasValueWithValueEquivalentTo(numberEntities.First()));
            }

            [Fact]
            public async Task success_not_found_item()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TryFirstAsync.Success.NotFoundItem")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                await dbContext.SaveChangesAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TryFirstAsync(x => x.Id == 1);

                retrievedNumberEntity.ShouldBeSuccessWithValueAssertion(x => x.ShouldBeHasNoValue());
            }
        }
    }
}