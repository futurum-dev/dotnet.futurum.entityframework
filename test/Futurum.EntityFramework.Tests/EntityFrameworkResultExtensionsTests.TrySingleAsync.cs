using System.Linq;
using System.Threading.Tasks;

using Futurum.Test.Option;
using Futurum.Test.Result;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Futurum.EntityFramework.Tests;

public class EntityFrameworkResultExtensionsTrySingleAsyncTests
{
    public class Result
    {
        private const string ERROR_MESSAGE = "Error Message";
        
        public class WithoutPredicate
        {
            [Fact]
            public async Task success()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Success")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                var numberEntity = new TestEntity { Id = 1, Numeric = 1 };

                dbContext.Numbers.Add(numberEntity);

                await dbContext.SaveChangesAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync(ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeSuccessWithValueEquivalentTo(numberEntity);
            }

            [Fact]
            public async Task failure_no_value()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Failure")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync(ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeFailureWithErrorContaining(ERROR_MESSAGE);
            }

            [Fact]
            public async Task failure()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Failure")
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

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync(ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeFailure();
            }
        }

        public class WithPredicate
        {
            [Fact]
            public async Task success()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Success")
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

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync(x => x.Id == 1, ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeSuccessWithValueEquivalentTo(numberEntities.Single(x => x.Id == 1));
            }

            [Fact]
            public async Task failure_no_value()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Failure")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync(x => x.Id == 1, ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeFailureWithErrorContaining(ERROR_MESSAGE);
            }

            [Fact]
            public async Task failure()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Failure")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                var numberEntities = Enumerable.Range(1, 10)
                                               .Select(i => new TestEntity { Id = i, Numeric = 1 })
                                               .ToList();

                dbContext.Numbers.AddRange(numberEntities);

                await dbContext.SaveChangesAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync(x => x.Numeric == 1, ERROR_MESSAGE);

                retrievedNumberEntity.ShouldBeFailure();
            }
        }
    }
    
    public class ResultOption
    {
        public class WithoutPredicate
        {
            [Fact]
            public async Task success()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Success")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                var numberEntity = new TestEntity { Id = 1, Numeric = 1 };

                dbContext.Numbers.Add(numberEntity);

                await dbContext.SaveChangesAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync();

                retrievedNumberEntity.ShouldBeSuccessWithValueAssertion(x => x.ShouldBeHasValueWithValueEquivalentTo(numberEntity));
            }

            [Fact]
            public async Task failure()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Failure")
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

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync();

                retrievedNumberEntity.ShouldBeFailure();
            }
        }

        public class WithPredicate
        {
            [Fact]
            public async Task success()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Success")
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

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync(x => x.Id == 1);

                retrievedNumberEntity.ShouldBeSuccessWithValueAssertion(x => x.ShouldBeHasValueWithValueEquivalentTo(numberEntities.Single(x => x.Id == 1)));
            }

            [Fact]
            public async Task failure()
            {
                var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
                                       .UseInMemoryDatabase($"Futurum.EntityFramework.Tests.TrySingleAsync.Failure")
                                       .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                                       .EnableSensitiveDataLogging()
                                       .Options;

                await using var dbContext = new TestDbContext(dbContextOptions);

                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.EnsureCreatedAsync();

                var numberEntities = Enumerable.Range(1, 10)
                                               .Select(i => new TestEntity { Id = i, Numeric = 1 })
                                               .ToList();

                dbContext.Numbers.AddRange(numberEntities);

                await dbContext.SaveChangesAsync();

                var retrievedNumberEntity = await dbContext.Numbers.TrySingleAsync(x => x.Numeric == 1);

                retrievedNumberEntity.ShouldBeFailure();
            }
        }
    }
}