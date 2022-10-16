namespace Maroontress.SqlBind;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

/// <summary>
/// An attribute that qualifies any class representing a table, creating the
/// index of the table and using the specified column names for the index key.
/// </summary>
/// <remarks>
/// See: <a href="https://www.sqlite.org/lang_createindex.html">SQLite CREATE
/// INDEX</a>.
/// </remarks>
/// <example>
/// <code>
/// [Table("persons")]
/// [IndexedColumns("firstNameId")]
/// [IndexedColumns("lastNameId")]
/// [IndexedColumns("firstNameId", "lastNameId")]
/// public record class PersonRow(
///     [Column("id")][PrimaryKey][AutoIncrement] long Id,
///     [Column("firstNameId")] long firstNameId,
///     [Column("lastNameId")] long lastNameId)
/// {
/// }
///
/// [Table("string")]
/// public record class StringRow(
///     [Column("id")][PrimaryKey][AutoIncrement] long Id,
///     [Column("value")][Unique] string Value)
/// {
/// }{
/// </code>
/// </example>
[AttributeUsage(
    AttributeTargets.Class,
    Inherited = false,
    AllowMultiple = true)]
public sealed class IndexedColumnsAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IndexedColumnsAttribute"/>
    /// class.
    /// </summary>
    /// <param name="names">
    /// The column names used for the index key.
    /// </param>
    public IndexedColumnsAttribute(params string[] names)
    {
        Names = names.ToImmutableArray();
    }

    /// <summary>
    /// Gets the column names used for the index key.
    /// </summary>
    public IReadOnlyList<string> Names { get; }
}
