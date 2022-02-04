using Futurum.Core.Linq;
using Futurum.Core.Result;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Futurum.EntityFramework;

public class EntityFrameworkSaveConcurrencyResultError : IResultErrorNonComposite
{
    private readonly DbUpdateConcurrencyException _dbUpdateConcurrencyException;

    internal EntityFrameworkSaveConcurrencyResultError(DbUpdateConcurrencyException dbUpdateConcurrencyException)
    {
        _dbUpdateConcurrencyException = dbUpdateConcurrencyException;
    }

    public string GetErrorString() =>
        _dbUpdateConcurrencyException.Entries
                                     .Select(TransformEntityEntryToErrorMessage)
                                     .StringJoin(",");

    public ResultErrorStructure GetErrorStructure()
    {
        var children = _dbUpdateConcurrencyException.Entries
                                                    .Select(TransformEntityEntryToErrorMessage)
                                                    .Select(ResultErrorStructureExtensions.ToResultErrorStructure);

        return new ResultErrorStructure("Entity Framework Concurrency conflicts", children);
    }

    private static string TransformEntityEntryToErrorMessage(EntityEntry entityEntry) =>
        $"Entity Framework Concurrency conflicts for '{entityEntry.Metadata.Name}'. Original value : '{entityEntry.OriginalValues}'. New value : '{entityEntry.CurrentValues}'";
}