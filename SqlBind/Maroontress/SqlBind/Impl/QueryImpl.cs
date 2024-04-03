namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

/// <summary>
/// The default implementation of <see cref="Query"/>.
/// </summary>
/// <param name="siphon">
/// The abstraction of the database connection.
/// </param>
/// <param name="bank">
/// The cache for the reflection.
/// </param>
public sealed class QueryImpl(Siphon siphon, MetadataBank bank)
    : Query
{
    private Siphon Siphon { get; } = siphon;

    private MetadataBank Bank { get; } = bank;

    /// <inheritdoc/>
    public SelectFrom<T> SelectAllFrom<T>(string alias)
        where T : notnull
    {
        var text = Bank.GetMetadata<T>()
            .NewSelectAllStatement(alias);
        return new SelectFromImpl<T>(Bank, Siphon, text);
    }

    /// <inheritdoc/>
    public Select<T> Select<T>(params string[] columns)
        where T : notnull
    {
        var resultColumn = string.Join(", ", columns);
        var text = $"SELECT {resultColumn}";
        return new SelectImpl<T>(Bank, Siphon, text);
    }

    /// <inheritdoc/>
    public DeleteFrom<T> DeleteFrom<T>()
        where T : notnull
    {
        var text = Bank.GetMetadata<T>().DeleteStatement;
        return new DeleteFromImpl<T>(Siphon, text);
    }

    /// <inheritdoc/>
    public void NewTables(params Type[] tables)
        => NewTables(tables.AsEnumerable());

    /// <inheritdoc/>
    public void NewTables(IEnumerable<Type> allTables)
    {
        void Execute(Func<Type, IEnumerable<string>> typeToQuery)
        {
            var all = allTables.SelectMany(typeToQuery);
            foreach (var s in all)
            {
                Siphon.ExecuteNonQuery(s);
            }
        }

        Execute(DropTableStatements);
        Execute(CreateTableStatements);
    }

    /// <inheritdoc/>
    public void Insert<T>(T row)
        where T : notnull
    {
        var (text, parameters) = InsertStatement(row);
        Siphon.ExecuteNonQuery(text, parameters);
    }

    /// <inheritdoc/>
    public long InsertAndGetRowId<T>(T row)
        where T : notnull
    {
        Insert(row);
        return GetLastInsertRowId();
    }

    /// <inheritdoc/>
    public T? SelectUnique<T>(string columnName, object value)
        where T : class
    {
        var text = Bank.GetMetadata<T>()
            .NewSelectStatement(columnName);
        var parameters = ImmutableArray.Create(
                KeyValuePair.Create("$" + columnName, value))
            .ToImmutableDictionary();
        using var reader = Siphon.ExecuteReader(text, parameters);
        return reader.Read()
            ? reader.NewInstance<T>()
            : null;
    }

    /// <inheritdoc/>
    public IEnumerable<T> SelectAll<T>()
        where T : notnull
    {
        var text = Bank.GetMetadata<T>()
            .SelectAllStatement;
        using var reader = Siphon.ExecuteReader(text);
        while (reader.Read())
        {
            yield return reader.NewInstance<T>();
        }
    }

    /// <inheritdoc/>
    public string ColumnName<T>(string parameterName)
        where T : notnull
    {
        return Bank.GetMetadata<T>()
            .ToColumnName(parameterName);
    }

    /// <inheritdoc/>
    public Update Update<T>(string alias)
        where T : notnull
    {
        var m = Bank.GetMetadata<T>();
        return new UpdateImpl(Siphon, m.UpdateStatement, alias);
    }

    private long GetLastInsertRowId()
    {
        var text = "select last_insert_rowid()";
        return Siphon.ExecuteLong(text);
    }

    private IEnumerable<string> CreateTableStatements(Type type)
    {
        return Bank.GetMetadata(type)
            .CreateTableStatements;
    }

    private IEnumerable<string> DropTableStatements(Type type)
    {
        return Bank.GetMetadata(type)
            .DropTableStatements;
    }

    private (string Text, IReadOnlyDictionary<string, object> Parameters)
        InsertStatement<T>(T row)
        where T : notnull
    {
        var m = Bank.GetMetadata<T>();
        var text = m.InsertStatement;
        var parameters = m.NewInsertParameterMap(row);
        return (text, parameters);
    }
}
