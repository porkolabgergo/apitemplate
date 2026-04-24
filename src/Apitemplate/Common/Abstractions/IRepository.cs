namespace Apitemplate.Common.Abstractions;

/// <summary>
/// Generic repository abstraction for basic CRUD operations.
/// Implement a concrete version backed by IBM Db2 (or any other driver) at the infrastructure layer.
/// </summary>
/// <typeparam name="TEntity">The aggregate root / entity type.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>Retrieves an entity by its primary key.</summary>
    Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

    /// <summary>Returns all entities (use with caution on large tables).</summary>
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Adds a new entity to the data store.</summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing entity in the data store.</summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>Deletes an entity from the data store.</summary>
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
