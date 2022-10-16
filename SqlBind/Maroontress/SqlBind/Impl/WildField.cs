namespace Maroontress.SqlBind.Impl;

using System.Reflection;

/// <summary>
/// The interface of <see cref="Field{T}"/> that provides non-generic
/// properties.
/// </summary>
public interface WildField
{
    /// <summary>
    /// Gets the parameter name.
    /// </summary>
    string ParameterName { get; }

    /// <summary>
    /// Gets a string representing the column definition.
    /// </summary>
    string ColumnDefinition { get; }

    /// <summary>
    /// Gets the column definition.
    /// </summary>
    string ColumnName { get; }

    /// <summary>
    /// Gets the property.
    /// </summary>
    PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// Gets a value indicating whether the field is qualified with
    /// <c>AUTOINCREMENT</c>.
    /// </summary>
    bool IsAutoIncrement { get; }
}
