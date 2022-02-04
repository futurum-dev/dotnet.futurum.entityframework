using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static partial class EntityFrameworkResultExtensions
{
    /// <summary>
    ///     <para>
    ///         Finds an entity with the given primary key values. If an entity with the given primary key values
    ///         is being tracked by the context, then it is returned immediately without making a request to the
    ///         database. Otherwise, a query is made to the database for an entity with the given primary key values
    ///         and this entity, if found, is attached to the context and returned.
    ///     </para>
    ///     <para>
    ///         If no entity is found, then Result.Fail of TEntity is returned with the <paramref name="errorMessageIfOptionNone"/>.
    ///     </para>
    /// </summary>
    public static Task<Result<TSource>> TryFindAsync<TSource, TId>(this DbSet<TSource> source, TId keyValue, string errorMessageIfOptionNone, CancellationToken cancellationToken = default)
        where TSource : class =>
        source.TryFindAsync(keyValue, cancellationToken)
              .ToResultAsync(() => errorMessageIfOptionNone);

    /// <summary>
    ///     <para>
    ///         Finds an entity with the given primary key values. If an entity with the given primary key values
    ///         is being tracked by the context, then it is returned immediately without making a request to the
    ///         database. Otherwise, a query is made to the database for an entity with the given primary key values
    ///         and this entity, if found, is attached to the context and returned.
    ///     </para>
    ///     <para>
    ///         If no entity is found, then Option.None of TEntity is returned.
    ///     </para>
    /// </summary>
    public static Task<Result<Option<TSource>>> TryFindAsync<TSource, TId>(this DbSet<TSource> source, TId keyValue, CancellationToken cancellationToken = default)
        where TSource : class
    {
        async Task<Option<TSource>> ExecuteAsync()
        {
            var value = await source.FindAsync(new object[] { keyValue }, cancellationToken);

            return value.ToOption();
        }

        return Result.TryAsync(ExecuteAsync, () => $"Failed to {nameof(TryFindAsync)} on '{typeof(TSource).FullName}'");
    }
}