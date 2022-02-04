using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework.Tests;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
    }

    public DbSet<TestEntity> Numbers { get; set; }
}