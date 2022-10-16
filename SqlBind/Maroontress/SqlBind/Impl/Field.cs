#pragma warning disable SingleTypeParameter

namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

/// <summary>
/// Represents a column of the table's row. Each instance corresponds to the
/// parameter in the constructor that the class qualified with <see
/// cref="TableAttribute"/> has.
/// </summary>
/// <typeparam name="T">
/// The type qualified with <see cref="TableAttribute"/>.
/// </typeparam>
public sealed class Field<T> : WildField
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Field{T}"/> class.
    /// </summary>
    /// <param name="parameterInfo">
    /// The constructor's parameter corresponding to this field.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Throws if the <paramref name="parameterInfo"/> is invalid.
    /// </exception>
    public Field(ParameterInfo parameterInfo)
    {
        var type = parameterInfo.Member.DeclaringType;
        if (typeof(T) != type)
        {
            throw new ArgumentException(
                "type mismatch", nameof(parameterInfo));
        }
        var column = parameterInfo.GetCustomAttribute<ColumnAttribute>();
        if (column is null)
        {
            throw new ArgumentException(
                "all parameters of the constructor must be annotated"
                    + $" with [Column]: {type}",
                nameof(parameterInfo));
        }
        ColumnName = column.Name;
        IsAutoIncrement = parameterInfo.GetCustomAttribute<
            AutoIncrementAttribute>() is not null;

        ParameterName = parameterInfo.Name;
        var parameterType = parameterInfo.ParameterType;
        if (!SqlTypeMap.TryGetValue(parameterType, out var sqlType))
        {
            throw new ArgumentException(
                "unsupported parameter type", ParameterName);
        }
        var propertyInfo = type.GetProperty(ParameterName);
        if (propertyInfo is null)
        {
            throw new ArgumentException(
                $"invalid type '{type}': no property found corresponding "
                    + $"to the paramter '{ParameterName}' of the "
                    + "constructor");
        }
        PropertyInfo = propertyInfo;
        var flags = SqlColumnFlags.Where(
                p => (parameterInfo.GetCustomAttribute(p.Key) is not null))
            .Select(p => p.Value);
        var list = ImmutableArray.Create(ColumnName, sqlType)
            .Concat(flags);
        ColumnDefinition = string.Join(' ', list);
    }

    /// <inheritdoc/>
    public string ParameterName { get; }

    /// <inheritdoc/>
    public string ColumnDefinition { get; }

    /// <inheritdoc/>
    public string ColumnName { get; }

    /// <inheritdoc/>
    public PropertyInfo PropertyInfo { get; }

    /// <inheritdoc/>
    public bool IsAutoIncrement { get; }

    private static IReadOnlyDictionary<Type, string>
            SqlTypeMap
    { get; } = ImmutableDictionary.CreateRange(
        ImmutableArray.Create(
            ToTypePair<string>("TEXT"),
            ToTypePair<long>("INTEGER")));

    private static IEnumerable<KeyValuePair<Type, string>>
            SqlColumnFlags
    { get; } = ImmutableArray.Create(
        ToTypePair<PrimaryKeyAttribute>("PRIMARY KEY"),
        ToTypePair<AutoIncrementAttribute>("AUTOINCREMENT"),
        ToTypePair<UniqueAttribute>("UNIQUE"));

    /// <summary>
    /// Gets the value of the property associated with the field that the
    /// specified row object contains.
    /// </summary>
    /// <param name="row">
    /// The row object that contains this field.
    /// </param>
    /// <returns>
    /// The property value.
    /// </returns>
    public object? GetPropertyValue(T row)
    {
        return PropertyInfo.GetValue(row);
    }

    private static KeyValuePair<Type, string> ToTypePair<U>(string name)
        => KeyValuePair.Create(typeof(U), name);
}
