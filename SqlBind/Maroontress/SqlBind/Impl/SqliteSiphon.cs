namespace Maroontress.SqlBind.Impl;

using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

/// <summary>
/// The implementation with Sqlite that has the logging facility.
/// </summary>
/// <param name="connection">
/// The connection to the Sqlite.
/// </param>
/// <param name="logger">
/// The logger.
/// </param>
internal sealed class SqliteSiphon(
        SqliteConnection connection, @Action<Func<string>> logger)
    : Siphon
{
    private SqliteConnection Connection { get; } = connection;

    private Action<Func<string>> Logger { get; } = logger;

    /// <inheritdoc/>
    public void ExecuteNonQuery(
        string text,
        IReadOnlyDictionary<string, object>? parameters = null)
    {
        var command = NewCommand(text, parameters);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public Reservoir ExecuteReader(
        string text,
        IReadOnlyDictionary<string, object>? parameters = null)
    {
        var command = NewCommand(text, parameters);
        return new SqliteReservoir(command.ExecuteReader());
    }

    /// <inheritdoc/>
    public long ExecuteLong(
        string text,
        IReadOnlyDictionary<string, object>? parameters = null)
    {
        var command = NewCommand(text, parameters);
        if (!(command.ExecuteScalar() is {} rowId))
        {
            throw new NullReferenceException();
        }
        return (long)rowId;
    }

    private SqliteCommand NewCommand(
        string text,
        IReadOnlyDictionary<string, object>? parameters = null)
    {
        var command = Connection.CreateCommand();
        Logger(() => text);
        command.CommandText = text;
        if (parameters is not null)
        {
            foreach (var (key, value) in parameters)
            {
                command.Parameters.AddWithValue(key, value);
                Logger(() => $"  ({key}, {value})");
            }
        }
        return command;
    }
}
