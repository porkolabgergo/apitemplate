using System.Data;

namespace Apitemplate.Common.Abstractions;

/// <summary>
/// Abstraction for creating database connections.
/// Swap the concrete implementation to change the underlying database driver
/// (e.g. IBM Db2, SQL Server, PostgreSQL).
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>Opens and returns a new database connection.</summary>
    IDbConnection CreateConnection();

    /// <summary>Opens and returns a new database connection asynchronously.</summary>
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}
