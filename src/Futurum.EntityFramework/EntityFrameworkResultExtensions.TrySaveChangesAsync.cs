using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Futurum.EntityFramework;

public static partial class EntityFrameworkResultExtensions
{
    /// <summary>
    ///     <para>
    ///         Saves all changes made in this context to the database.
    ///     </para>
    ///     <para>
    ///         This method will automatically call <see cref="ChangeTracker.DetectChanges" /> to discover any
    ///         changes to entity instances before saving to the underlying database. This can be disabled via
    ///         <see cref="ChangeTracker.AutoDetectChangesEnabled" />.
    ///     </para>
    /// </summary>
    public static Task<Result> TrySaveChangesAsync<TDbContext>(this TDbContext dbContext, CancellationToken cancellationToken = default)
        where TDbContext : DbContext
    {
        async Task<Result> ExecuteAsync()
        {
            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);

                return Result.Ok();
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                return Result.Fail(dbUpdateConcurrencyException.ToResultError());
            }
            catch (DbUpdateException dbUpdateException)
            {
                return Result.Fail(dbUpdateException.ToResultError());
            }
        }

        return Result.TryAsync(ExecuteAsync, () => $"Failed to {nameof(TrySaveChangesAsync)} on '{typeof(TDbContext).FullName}'");
    }
}