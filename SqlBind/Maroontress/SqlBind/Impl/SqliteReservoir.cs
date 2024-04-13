namespace Maroontress.SqlBind.Impl;

using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The <see cref="Reservoir"/> implementation wrapping SQLite.
/// </summary>
/// <param name="reader">
/// The <see cref="SqliteDataReader"/> object.
/// </param>
public sealed class SqliteReservoir(SqliteDataReader reader) : Reservoir
{
    private SqliteDataReader Reader { get; } = reader;

    /// <inheritdoc/>
    public void Dispose()
    {
        Reader.Dispose();
    }

    /// <inheritdoc/>
    public T NewInstance<T>()
    {
        var type = typeof(T);
        var ctor = type.GetConstructors().First();
        var n = Reader.FieldCount;
        var args = new object[n];
        Reader.GetValues(args);
        return (T)ctor.Invoke(args);
    }

    /// <inheritdoc/>
    public IEnumerable<T> NewInstances<T>()
    {
        var type = typeof(T);
        var ctor = type.GetConstructors().First();
        var n = ctor.GetParameters().Length;
        var args = new object[n];
        while (Reader.Read())
        {
            if (Reader.FieldCount != n)
            {
                throw new ArgumentException(
                    "The number of constructor parameters does not match",
                    nameof(T));
            }
            Reader.GetValues(args);
            var instance = (T)ctor.Invoke(args);
            yield return instance;
        }
    }

    /// <inheritdoc/>
    public bool Read()
    {
        return Reader.Read();
    }
}
