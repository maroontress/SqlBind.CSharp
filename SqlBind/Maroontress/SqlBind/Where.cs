namespace Maroontress.SqlBind;

/// <summary>
/// Represents the <c>WHERE</c> clause in a <c>SELECT</c> statement.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
public interface Where<T> : SelectSorter<T>
    where T : notnull
{
}
