using System.Linq.Expressions;

using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static partial class EntityFrameworkResultExtensions
{
    /// <summary>
    ///     <para>
    ///         Returns the first element of a sequence.
    ///     </para>
    ///     <para>
    ///         If the sequence is empty, then <see cref="Result.Fail{TSource}" /> is returned with the <paramref name="errorMessageIfOptionNone"/>.
    ///     </para>
    /// </summary>
    public static Task<Result<TSource>> TryFirstAsync<TSource>(this IQueryable<TSource> source, string errorMessageIfOptionNone, CancellationToken cancellationToken = default) =>
        source.TryFirstAsync(cancellationToken)
              .ToResultAsync(() => errorMessageIfOptionNone);
        
    /// <summary>
    ///     <para>
    ///         Returns the first element of a sequence.
    ///     </para>
    ///     <para>
    ///         If the sequence is empty, then <see cref="Result.Fail{TSource}" /> is returned with the <paramref name="errorMessageIfOptionNone"/>.
    ///     </para>
    /// </summary>
    public static Task<Result<TSource>> TryFirstAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, string errorMessageIfOptionNone, CancellationToken cancellationToken = default) =>
        source.TryFirstAsync(predicate, cancellationToken)
              .ToResultAsync(() => errorMessageIfOptionNone);

    /// <summary>
    ///     <para>
    ///         Returns the first element of a sequence.
    ///     </para>
    ///     <para>
    ///         If the sequence is empty, then <see cref="Option.None{TSource}" /> is returned.
    ///     </para>
    /// </summary>
    public static Task<Result<Option<TSource>>> TryFirstAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
    {
        async Task<Option<TSource>> ExecuteAsync()
        {
            var value = await source.FirstOrDefaultAsync(cancellationToken);

            return value.ToOption();
        }

        return Result.TryAsync(ExecuteAsync, () => $"Failed to {nameof(TryFirstAsync)} on '{typeof(TSource).FullName}'");
    }

    /// <summary>
    ///     <para>
    ///         Returns the first element of a sequence.
    ///     </para>
    ///     <para>
    ///         If the sequence is empty, then <see cref="Option.None{TSource}" /> is returned.
    ///     </para>
    /// </summary>
    public static Task<Result<Option<TSource>>> TryFirstAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
    {
        async Task<Option<TSource>> ExecuteAsync()
        {
            var value = await source.FirstOrDefaultAsync(predicate, cancellationToken);

            return value.ToOption();
        }

        return Result.TryAsync(ExecuteAsync, () => $"Failed to {nameof(TryFirstAsync)} on '{typeof(TSource).FullName}'");
    }
}