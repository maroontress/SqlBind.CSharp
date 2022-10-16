namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;

/// <summary>
/// The interface of <see cref="Metadata{T}"/> that provides non-generic
/// properties.
/// </summary>
public interface WildMetadata
{
    /// <summary>
    /// Gets the SQL statements to create the table and indexes.
    /// </summary>
    IEnumerable<string> CreateTableStatements { get; }

    /// <summary>
    /// Gets the SQL statements to drop the table and indexes.
    /// </summary>
    IEnumerable<string> DropTableStatements { get; }

    /// <summary>
    /// Gets the insert statement.
    /// </summary>
    string InsertStatement { get; }

    /// <summary>
    /// Gets the name of the table.
    /// </summary>
    string TableName { get; }

    /// <summary>
    /// Gets the delete statement.
    /// </summary>
    string DeleteStatement { get; }

    /// <summary>
    /// Gets a complete <c>SELECT</c> statement like <c>SELECT</c> ...
    /// <c>FROM</c> ... for all rows.
    /// </summary>
    string SelectAllStatement { get; }

    /// <summary>
    /// Gets a new string representing the <c>SELECT</c> statement to find the
    /// row where the specified column is equal to the specific value.
    /// </summary>
    /// <param name="columnName">
    /// The column name. This must contain the <see cref="Field{T}"/> object
    /// that has the same name.
    /// </param>
    /// <returns>
    /// The new string: <c>SELECT</c> ... <c>FROM</c> ... <c>WHERE columnName =
    /// $columnName</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Throws if this does not contain the <see cref="Field{T}"/> object where
    /// the column name is equal to <paramref name="columnName"/>.
    /// </exception>
    string NewSelectStatement(string columnName);

    /// <summary>
    /// Gets a new string representing the incomplete <c>select</c> statement.
    /// </summary>
    /// <param name="alias">
    /// The alias name for the table with which <c>this</c> is associated.
    /// represents.
    /// </param>
    /// <returns>
    /// The new string: <c>SELECT</c> ... <c>FROM</c> ... <c>alias</c>.
    /// </returns>
    string NewSelectAllStatement(string alias);

    /// <summary>
    /// Gets the column name associated with the specified parameter name.
    /// </summary>
    /// <param name="parameterName">
    /// The parameter name that the primary constructor of <c>T</c> contains.
    /// </param>
    /// <returns>
    /// The column name.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Throws if <paramref name="parameterName"/> is not found.
    /// </exception>
    string ToColumnName(string parameterName);
}
