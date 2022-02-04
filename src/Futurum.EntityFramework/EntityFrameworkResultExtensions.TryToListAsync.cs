using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static partial class EntityFrameworkResultExtensions
{
    /// <summary>
    ///     <para>
    ///         Returns a <see cref="List{T}" /> from an <see cref="IQueryable{T}" /> by enumerating it asynchronously.
    ///     </para>
    /// </summary>
    public static Task<Result<IEnumerable<TSource>>> TryToListAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
    {
        async Task<List<TSource>> ExecuteAsync() =>
            await source.ToListAsync(cancellationToken);

        return Result.TryAsync(ExecuteAsync, () => $"Failed to {nameof(TryToListAsync)} on '{typeof(TSource).FullName}'")
                     .MapAsync(Enumerable.AsEnumerable);
    }
}