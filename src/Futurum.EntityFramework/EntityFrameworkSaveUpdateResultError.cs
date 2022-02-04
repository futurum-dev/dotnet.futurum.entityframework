using Futurum.Core.Linq;
using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Futurum.EntityFramework;

public class EntityFrameworkSaveUpdateResultError : IResultErrorNonComposite
{
    private readonly DbUpdateException _dbUpdateException;

    internal EntityFrameworkSaveUpdateResultError(DbUpdateException dbUpdateException)
    {
        _dbUpdateException = dbUpdateException;
    }

    public string GetErrorString() =>
        _dbUpdateException.Entries
                          .Select(TransformEntityEntryToErrorMessage)
                          .StringJoin(",");

    public ResultErrorStructure GetErrorStructure()
    {
        var children = _dbUpdateException.Entries
                                         .Select(TransformEntityEntryToErrorMessage)
                                         .Select(ResultErrorStructureExtensions.ToResultErrorStructure);

        return new ResultErrorStructure("Entity Framework Save errors", children);
    }

    private static string TransformEntityEntryToErrorMessage(EntityEntry entityEntry) =>
        $"Entity Framework Save errors for '{entityEntry.Metadata.Name}'";
}