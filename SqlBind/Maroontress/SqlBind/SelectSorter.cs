namespace Maroontress.SqlBind;

/// <summary>
/// Represents the executable <c>SELECT</c> statement.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
public interface SelectSorter<T> : SelectTerminator<T>
    where T : notnull
{
    /// <summary>
    /// Executes the query and gets the result in the order sorted by the
    /// specified columns.
    /// </summary>
    /// <remarks>
    /// The result that this method returns represents that of <c>SELECT
    /// ... FROM ... ORDER BY</c> <paramref name="columns"/>.
    /// </remarks>
    /// <param name="columns">
    /// The columns to sort the rows of the result by.
    /// </param>
    /// <returns>
    /// The <typeparamref name="T"/> objects representing the result of the
    /// query.
    /// </returns>
    SelectTerminator<T> OrderBy(params string[] columns);
}
