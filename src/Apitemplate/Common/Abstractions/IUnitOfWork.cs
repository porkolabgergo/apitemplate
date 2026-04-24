namespace Apitemplate.Common.Abstractions;

/// <summary>
/// Abstraction for the Unit of Work pattern.
/// Ensures that multiple repository operations are committed atomically.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>Commits all pending changes to the data store.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
