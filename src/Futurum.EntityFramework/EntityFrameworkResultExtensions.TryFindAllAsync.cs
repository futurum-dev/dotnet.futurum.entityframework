using System.Linq.Expressions;
using System.Reflection;

using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static partial class EntityFrameworkResultExtensions
{
    private static readonly MethodInfo ContainsMethod = typeof(Enumerable).GetMethods()
                                                                          .First(methodInfo => methodInfo.Name == "Contains" &&
                                                                                               methodInfo.GetParameters().Length == 2)
                                                                          .MakeGenericMethod(typeof(object));

    /// <summary>
    ///     <para>
    ///         Finds all entities with the given primary key values. If an entity with one of the given primary key values
    ///         is being tracked by the context, then it is returned immediately without making a request to the
    ///         database. Otherwise, a query is made to the database for an entity with the given primary key values
    ///         and this entity, if found, is attached to the context and returned.
    ///     </para>
    ///     <para>
    ///         If no entities are found, then Result.Fail of TEntity is returned.
    ///     </para>
    /// </summary>
    public static Task<Result<IEnumerable<TSource>>> TryFindAllAsync<TSource>(this DbContext dbContext, IEnumerable<Guid> keyValues, CancellationToken cancellationToken = default)
        where TSource : class =>
        Result.TryAsync(async () => await dbContext.FindAllAsync<Guid, TSource>(keyValues, cancellationToken),
                        () => $"Failed to {nameof(TryFindAllAsync)} on '{typeof(TSource).FullName}'")
              .MapAsync(Enumerable.AsEnumerable);

    private static async Task<Result<IEnumerable<TSource>>> FindAllAsync<TKey, TSource>(this DbContext dbContext, IEnumerable<TKey> keyValues, CancellationToken cancellationToken = default)
        where TSource : class
    {
        var entityType = dbContext.Model.FindEntityType(typeof(TSource));
        var primaryKey = entityType.FindPrimaryKey();
        if (primaryKey.Properties.Count != 1)
            throw new NotSupportedException("Only a single primary key is supported");

        var primaryKeyProperty = primaryKey.Properties[0];
        var primaryKeyPropertyType = primaryKeyProperty.ClrType;

        // validate key type against primary key type
        if (!primaryKeyPropertyType.IsInstanceOfType(typeof(TKey)))
            throw new ArgumentException($"Keys are not of the right type");

        // retrieve member info for primary key
        var primaryKeyMemberInfo = typeof(TSource).GetProperty(primaryKeyProperty.Name);
        if (primaryKeyMemberInfo == null)
            throw new ArgumentException("Type does not contain the primary key as an accessible property");

        // build lambda expression
        var parameter = Expression.Parameter(typeof(TSource), "e");
        var body = Expression.Call(null, ContainsMethod,
                                   Expression.Constant(keyValues),
                                   Expression.Convert(Expression.MakeMemberAccess(parameter, primaryKeyMemberInfo), typeof(object)));
        var predicateExpression = Expression.Lambda<Func<TSource, bool>>(body, parameter);

        // run query
        return await dbContext.Set<TSource>()
                              .Where(predicateExpression)
                              .TryToListAsync(cancellationToken);
    }
}