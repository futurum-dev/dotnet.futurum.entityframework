using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static partial class EntityFrameworkResultExtensions
{
    /// <summary>
    ///     <para>
    ///     Asynchronously returns the number of elements in a sequence.
    ///     </para>
    /// </summary>
    public static Task<Result<int>> TryCountAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
    {
        async Task<int> ExecuteAsync() =>
            await source.CountAsync(cancellationToken);

        return Result.TryAsync(ExecuteAsync, () => $"Failed to {nameof(TryCountAsync)} on '{typeof(TSource).FullName}'");
    }
}