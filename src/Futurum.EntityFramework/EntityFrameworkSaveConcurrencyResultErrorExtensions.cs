using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static class EntityFrameworkSaveConcurrencyResultErrorExtensions
{
    public static IResultError ToResultError(this DbUpdateConcurrencyException exception) =>
        ResultErrorCompositeExtensions.ToResultError(new EntityFrameworkSaveConcurrencyResultError(exception), ((Exception)exception).ToResultError());
}