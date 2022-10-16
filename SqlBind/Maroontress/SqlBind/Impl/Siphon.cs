namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;

/// <summary>
/// Abstraction of the database connection.
/// </summary>
public interface Siphon
{
    /// <summary>
    /// Executes the SQL statement without the results.
    /// </summary>
    /// <param name="text">
    /// The SQL statement.
    /// </param>
    /// <param name="parameters">
    /// The parameters of the statement. This can be <c>null</c>
    /// if the statement contains no parameters.
    /// </param>
    void ExecuteNonQuery(
        string text,
        IReadOnlyDictionary<string, object>? parameters = null);

    /// <summary>
    /// Executes the SQL statement and returns the reader of the results.
    /// </summary>
    /// <param name="text">
    /// The SQL statement.
    /// </param>
    /// <param name="parameters">
    /// The parameters of the statement. This can be <c>null</c>
    /// if the statement contains no parameters.
    /// </param>
    /// <returns>
    /// The reader of the results.
    /// </returns>
    public Reservoir ExecuteReader(
        string text,
        IReadOnlyDictionary<string, object>? parameters = null);

    /// <summary>
    /// Executes the SQL statement and returns the scalar result.
    /// </summary>
    /// <param name="text">
    /// The SQL statement.
    /// </param>
    /// <param name="parameters">
    /// The parameters of the statement. This can be <c>null</c>
    /// if the statement contains no parameters.
    /// </param>
    /// <returns>
    /// The scalar result.
    /// </returns>
    /// <exception cref="NullReferenceException">
    /// If no results.
    /// </exception>
    public long ExecuteLong(
        string text,
        IReadOnlyDictionary<string, object>? parameters = null);
}
