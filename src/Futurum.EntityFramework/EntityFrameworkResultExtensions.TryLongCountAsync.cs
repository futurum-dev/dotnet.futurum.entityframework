using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static partial class EntityFrameworkResultExtensions
{
    /// <summary>
    ///     <para>
    ///     Asynchronously returns the number of elements in a sequence as an <see cref="long" />.
    ///     </para>
    /// </summary>
    public static Task<Result<long>> TryLongCountAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
    {
        async Task<long> ExecuteAsync() =>
            await source.LongCountAsync(cancellationToken);

        return Result.TryAsync(ExecuteAsync, () => $"Failed to {nameof(TryLongCountAsync)} on '{typeof(TSource).FullName}'");
    }
}