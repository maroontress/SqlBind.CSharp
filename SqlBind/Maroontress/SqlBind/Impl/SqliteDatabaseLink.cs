namespace Maroontress.SqlBind.Impl;

using Microsoft.Data.Sqlite;
using System;

/// <summary>
/// The implementation with Sqlite.
/// </summary>
/// <param name="connection">
/// The Sqlite's connection.
/// </param>
public sealed class SqliteDatabaseLink(SqliteConnection connection)
    : DatabaseLink
{
    private SqliteConnection Connection { get; } = connection;

    /// <inheritdoc/>
    public Committable BeginTransaction()
    {
        var t = Connection.BeginTransaction();
        return new SqliteCommittable(t);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Connection.Dispose();
    }

    /// <inheritdoc/>
    public Siphon NewSiphon(Action<Func<string>> logger)
    {
        return new SqliteSiphon(Connection, logger);
    }
}
