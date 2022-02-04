using System.Linq.Expressions;

using Futurum.Core.Option;
using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static partial class EntityFrameworkResultExtensions
{
    /// <summary>
    ///     <para>
    ///         Returns the only element of a sequence.
    ///     </para>
    ///     <para>
    ///         If the sequence is empty, then <see cref="Result.Fail" /> is returned with the <paramref name="errorMessageIfOptionNone"/>.
    ///     </para>
    ///     <para>
    ///         If there is more than one element in the sequence, then <see cref="Result.Fail{TSource}" /> is returned.
    ///     </para>
    /// </summary>
    public static Task<Result<TSource>> TrySingleAsync<TSource>(this IQueryable<TSource> source, string errorMessageIfOptionNone, CancellationToken cancellationToken = default) =>
        source.TrySingleAsync(cancellationToken)
              .ToResultAsync(() => errorMessageIfOptionNone);

    /// <summary>
    ///     <para>
    ///         Returns the only element of a sequence.
    ///     </para>
    ///     <para>
    ///         If the sequence is empty, then <see cref="Result.Fail" /> is returned with the <paramref name="errorMessageIfOptionNone"/>.
    ///     </para>
    ///     <para>
    ///         If there is more than one element in the sequence, then <see cref="Result.Fail{TSource}" /> is returned.
    ///     </para>
    /// </summary>
    public static Task<Result<TSource>> TrySingleAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, string errorMessageIfOptionNone,
                                                                CancellationToken cancellationToken = default) =>
        source.TrySingleAsync(predicate, cancellationToken)
              .ToResultAsync(() => errorMessageIfOptionNone);

    /// <summary>
    ///     <para>
    ///         Returns the only element of a sequence.
    ///     </para>
    ///     <para>
    ///         If the sequence is empty, then <see cref="Option.None{TSource}" /> is returned.
    ///     </para>
    ///     <para>
    ///         If there is more than one element in the sequence, then <see cref="Result.Fail{TSource}" /> is returned.
    ///     </para>
    /// </summary>
    public static Task<Result<Option<TSource>>> TrySingleAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
    {
        async Task<Option<TSource>> ExecuteAsync()
        {
            var value = await source.SingleOrDefaultAsync(cancellationToken);

            return value.ToOption();
        }

        return Result.TryAsync(ExecuteAsync, () => $"Failed to {nameof(TrySingleAsync)} on '{typeof(TSource).FullName}'");
    }

    /// <summary>
    ///     <para>
    ///         Returns the only element of a sequence.
    ///     </para>
    ///     <para>
    ///         If the sequence is empty, then <see cref="Option.None{TSource}" /> is returned.
    ///     </para>
    ///     <para>
    ///         If there is more than one element in the sequence, then <see cref="Result.Fail{TSource}" /> is returned.
    ///     </para>
    /// </summary>
    public static Task<Result<Option<TSource>>> TrySingleAsync<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
    {
        async Task<Option<TSource>> ExecuteAsync()
        {
            var value = await source.SingleOrDefaultAsync(predicate, cancellationToken);

            return value.ToOption();
        }

        return Result.TryAsync(ExecuteAsync, () => $"Failed to {nameof(TrySingleAsync)} on '{typeof(TSource).FullName}'");
    }
}