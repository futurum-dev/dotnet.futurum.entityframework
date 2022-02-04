using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;

namespace Futurum.EntityFramework;

public static class EntityFrameworkSaveUpdateResultErrorExtensions
{
    public static IResultError ToResultError(this DbUpdateException exception) =>
        ResultErrorCompositeExtensions.ToResultError(new EntityFrameworkSaveUpdateResultError(exception), ((Exception)exception).ToResultError());
}