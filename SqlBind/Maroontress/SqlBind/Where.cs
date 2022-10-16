namespace Maroontress.SqlBind;

/// <summary>
/// Represents the <c>SELECT</c> ... <c>WHERE</c> ... statement in SQL.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
public interface Where<T> : TerminalOperation<T>
    where T : notnull
{
}
