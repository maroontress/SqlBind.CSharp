namespace Maroontress.SqlBind;

using System;

/// <summary>
/// An attribute that qualifies any class representing a row of a table,
/// associating the class with the table that has the specified name.
/// </summary>
/// <remarks>
/// <para>The class that this attribute qualifies must have a single
/// constructor, every parameter of which must be qualified with <see
/// cref="ColumnAttribute"/>. And the class must have the properties
/// corresponding to those parameters. Each property must have the same name
/// as the corresponding parameter, in the simular way of a <c>record</c>
/// class.</para>
/// <para>See:
/// <a href="https://www.sqlite.org/lang_createtable.html">SQLite CREATE
/// TABLE</a>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// using Maroontress.SqlBind;
///
/// [Table("properties")]
/// public record class PropertyRow(
///     [Column("key")][Unique] string Key,
///     [Column("value")] string Value)
/// {
/// }
/// </code>
/// </example>
/// <seealso cref="ColumnAttribute"/>
[AttributeUsage(
    AttributeTargets.Class,
    Inherited = false,
    AllowMultiple = false)]
public sealed class TableAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TableAttribute"/> class.
    /// </summary>
    /// <param name="name">
    /// The table name.
    /// </param>
    public TableAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the table name.
    /// </summary>
    public string Name { get; }
}
