namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

/// <summary>
/// The <see cref="Reservoir"/> implementation wrapping SQLite.
/// </summary>
internal sealed class SqliteReservoir : Reservoir
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqliteReservoir"/> class.
    /// </summary>
    /// <param name="reader">
    /// The <see cref="SqliteDataReader"/> object.
    /// </param>
    public SqliteReservoir(SqliteDataReader reader)
    {
        Reader = reader;
    }

    private SqliteDataReader Reader { get; }

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
                throw new ArgumentException("T");
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
