namespace Maroontress.SqlBind.Impl;

using System;
using Microsoft.Data.Sqlite;

/// <summary>
/// The implementation with Sqlite.
/// </summary>
public sealed class SqliteDatabaseLink : DatabaseLink
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqliteDatabaseLink"/>
    /// class.
    /// </summary>
    /// <param name="connection">
    /// The Sqlite's connection.
    /// </param>
    public SqliteDatabaseLink(SqliteConnection connection)
    {
        Connection = connection;
    }

    private SqliteConnection Connection { get; }

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
