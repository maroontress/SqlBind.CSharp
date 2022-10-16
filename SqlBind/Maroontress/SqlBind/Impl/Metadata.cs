namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

/// <summary>
/// The cache of the metadata for reflection.
/// </summary>
/// <typeparam name="T">
/// The type qualified with <see cref="TableAttribute"/>.
/// </typeparam>
public sealed class Metadata<T> : WildMetadata
    where T : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata{T}"/> class.
    /// </summary>
    public Metadata()
    {
        var type = typeof(T);
        var tableName = ToTableName(type);
        var fields = ToFields(type).ToImmutableArray();
        var columnNameSet = new HashSet<string>(fields.Length)
        {
            fields[0].ColumnName,
        };
        for (var k = 1; k < fields.Length; ++k)
        {
            var it = fields[k];
            var columnName = it.ColumnName;
            if (columnNameSet.Add(columnName))
            {
                continue;
            }
            throw new ArgumentException(
                $"the column name '{columnName}' associated with "
                + $"the parameter '{it.ParameterName}' is duplicated",
                nameof(type));
        }
        var insertColumns = fields.Where(p => !p.IsAutoIncrement)
            .ToImmutableArray();
        var indexedColumns = ToIndexedColumns(type)
            .Select(i => (Name: ToIndexName(tableName, i),
                Columns: string.Join(", ", i)))
            .ToImmutableArray();

        string ToInsertStatement()
        {
            var all = insertColumns.Select(p => p.ColumnName)
                .ToArray();
            var columns = string.Join(", ", all);
            var args = string.Join(", ", all.Select(i => "$" + i));
            return $"INSERT INTO {tableName} ({columns}) VALUES ({args})";
        }

        Func<T, IReadOnlyDictionary<string, object>>
            NewInsertParameterMap()
        {
            var all = insertColumns.Select(
                    i => (Name: "$" + i.ColumnName, ToValue: ToParameter(i)))
                .ToImmutableArray();
            return o =>
            {
                return all.ToImmutableDictionary(
                    i => i.Name,
                    i => i.ToValue(o));
            };
        }

        string ToCreateIndex(string name, string columns)
        {
            return $"CREATE INDEX {name} on {tableName} ({columns})";
        }

        string ToDropIndex(string name)
        {
            return $"DROP INDEX IF EXISTS {name}";
        }

        IEnumerable<string> ToCreateTableStatements()
        {
            var definitions = fields.Select(i => i.ColumnDefinition);
            var columns = string.Join(", ", definitions);
            var first = $"CREATE TABLE {tableName} ({columns})";
            var list = ImmutableArray.Create(first);
            return !indexedColumns.Any()
                ? list
                : list.Concat(
                    indexedColumns.Select(
                        i => ToCreateIndex(i.Name, i.Columns)));
        }

        IEnumerable<string> ToDropTableStatements()
        {
            var first = $"DROP TABLE IF EXISTS {tableName}";
            var list = ImmutableArray.Create(first);
            return !indexedColumns.Any()
                ? (IEnumerable<string>)list
                : list.Concat(
                    indexedColumns.Select(i => ToDropIndex(i.Name)));
        }

        string ToSelectAllStatement()
        {
            var columns = string.Join(", ", fields.Select(i => i.ColumnName));
            return $"SELECT {columns} FROM {tableName}";
        }

        TableName = tableName;
        Fields = fields;
        InsertStatement = ToInsertStatement();
        DeleteStatement = $"DELETE FROM {tableName}";
        CreateTableStatements = ToCreateTableStatements().ToImmutableArray();
        DropTableStatements = ToDropTableStatements().ToImmutableArray();
        ToInsertParameterMap = NewInsertParameterMap();
        SelectAllStatement = ToSelectAllStatement();
        ParameterToColumnNameMap
            = fields.ToDictionary(i => i.ParameterName, i => i.ColumnName);
    }

    /// <inheritdoc/>
    public IEnumerable<string> CreateTableStatements { get; }

    /// <inheritdoc/>
    public IEnumerable<string> DropTableStatements { get; }

    /// <inheritdoc/>
    public string InsertStatement { get; }

    /// <inheritdoc/>
    public string TableName { get; }

    /// <inheritdoc/>
    public string DeleteStatement { get; }

    /// <inheritdoc/>
    public string SelectAllStatement { get; }

    /// <summary>
    /// Gets all the fields of <typeparamref name="T"/>.
    /// </summary>
    public IEnumerable<Field<T>> Fields { get; }

    private Func<T, IReadOnlyDictionary<string, object>>
        ToInsertParameterMap { get; }

    private IReadOnlyDictionary<string, string>
        ParameterToColumnNameMap { get; }

    /// <inheritdoc/>
    public string NewSelectStatement(string columnName)
    {
        if (Fields.All(i => i.ColumnName != columnName))
        {
            throw new ArgumentException(
                "does not contain the field of that name",
                nameof(columnName));
        }
        var columns = string.Join(
            ", ",
            Fields.Select(i => i.ColumnName));
        return $"SELECT {columns} FROM {TableName} "
            + $"WHERE {columnName} = ${columnName}";
    }

    /// <inheritdoc/>
    public string NewSelectAllStatement(string alias)
    {
        var columns = string.Join(
            ", ",
            Fields.Select(i => $"{alias}.{i.ColumnName}"));
        return string.Join(
            ' ',
            "SELECT",
            columns,
            "FROM",
            TableName,
            alias);
    }

    /// <inheritdoc/>
    public string ToColumnName(string parameterName)
    {
        if (!ParameterToColumnNameMap.TryGetValue(
            parameterName, out var columnName))
        {
            throw new ArgumentException(
                $"parameter name '{parameterName}' is not found.");
        }
        return columnName;
    }

    /// <summary>
    /// Gets a new map of the parameter name to the parameter value.
    /// </summary>
    /// <param name="row">
    /// The row object containing the all field values.
    /// </param>
    /// <returns>
    /// The new dictionary containing the parameter name-value pairs.
    /// </returns>
    public IReadOnlyDictionary<string, object> NewInsertParameterMap(T row)
    {
        return ToInsertParameterMap(row);
    }

    private static string ToTableName(Type type)
    {
        var a = type.GetCustomAttribute<TableAttribute>();
        if (a is null)
        {
            throw new ArgumentException(
                $"not annotated with {nameof(TableAttribute)}",
                nameof(type));
        }
        return a.Name;
    }

    private static IEnumerable<Field<T>> ToFields(Type type)
    {
        var allPublicConstructors = type.GetConstructors();
        var ctors = allPublicConstructors.Where(
                i => i.GetCustomAttribute<IgnoredAttribute>() is null)
            .ToArray();
        if (ctors.Length is not 1)
        {
            throw new ArgumentException(
                "must have the single public constructor without [Ignored] "
                    + "attribute.",
                nameof(type));
        }
        var ctor = ctors[0];
        var parameters = ctor.GetParameters();
        if (parameters.Length is 0)
        {
            throw new ArgumentException(
                "the constructor has no parameters",
                nameof(type));
        }
        return parameters.Select(i => new Field<T>(i));
    }

    private static Func<T, object> ToParameter(Field<T> field)
    {
        return i =>
        {
            var o = field.GetPropertyValue(i);
            if (o is null)
            {
                throw new ArgumentException(
                    $"'{field.ParameterName}' must be non-null",
                    nameof(field));
            }
            return o;
        };
    }

    private static IEnumerable<IEnumerable<string>> ToIndexedColumns(Type type)
    {
        var a = type.GetCustomAttributes<IndexedColumnsAttribute>();
        foreach (var i in a)
        {
            if (i.Names.Count is 0)
            {
                var name = nameof(IndexedColumnsAttribute);
                throw new ArgumentException(
                    $"missing parameters of {name}", type.ToString());
            }
        }
        return a.Select(i => i.Names.AsEnumerable());
    }

    private static string ToIndexName(
        string tableName, IEnumerable<string> names)
    {
        var list = ImmutableArray.Create(tableName, "Index")
            .Concat(names);
        return string.Join('_', list);
    }
}
