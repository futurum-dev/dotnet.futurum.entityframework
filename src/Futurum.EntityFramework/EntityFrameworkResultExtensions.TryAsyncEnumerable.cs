using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static partial class EntityFrameworkResultExtensions
{
    /// <summary>
    ///     <para>
    ///         Returns an IAsyncEnumerable{T} which can be enumerated asynchronously.
    ///     </para>
    /// </summary>
    public static Result<IAsyncEnumerable<TSource>> TryAsyncEnumerable<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
    {
        IAsyncEnumerable<TSource> ExecuteAsync() =>
            source.AsAsyncEnumerable();

        return Result.Try(ExecuteAsync, () => $"Failed to {nameof(TryAsyncEnumerable)} on '{typeof(TSource).FullName}'");
    }
}