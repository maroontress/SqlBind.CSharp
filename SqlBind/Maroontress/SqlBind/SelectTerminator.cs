namespace Maroontress.SqlBind;

using System.Collections.Generic;

/// <summary>
/// Represents the executable <c>SELECT</c> statement.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
public interface SelectTerminator<T>
    where T : notnull
{
    /// <summary>
    /// Executes the query and gets the result.
    /// </summary>
    /// <returns>
    /// The <typeparamref name="T"/> objects representing the result of the
    /// query.
    /// </returns>
    IEnumerable<T> Execute();

    /// <summary>
    /// Executes the query and limits the number of rows that the query
    /// returns.
    /// </summary>
    /// <param name="limit">
    /// The maximum number of rows to return.
    /// </param>
    /// <returns>
    /// The <typeparamref name="T"/> objects representing the limited result of
    /// the query.
    /// </returns>
    IEnumerable<T> Limit(int limit);

    /// <summary>
    /// Executes the query and limits the number of rows that the query returns
    /// and specifies the starting offset.
    /// </summary>
    /// <param name="limit">
    /// The maximum number of rows to return.
    /// </param>
    /// <param name="offset">
    /// The number of rows to skip before starting to return rows.
    /// </param>
    /// <returns>
    /// The <typeparamref name="T"/> objects representing the limited result of
    /// the query.
    /// </returns>
    IEnumerable<T> LimitOffset(int limit, int offset);
}
