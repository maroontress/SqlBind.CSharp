namespace Maroontress.SqlBind;

/// <summary>
/// Represents the <c>UPDATE</c> statement in SQL.
/// </summary>
public interface Update
{
    /// <summary>
    /// Gets a new <see cref="UpdateSet"/> object.
    /// </summary>
    /// <param name="expr">
    /// The expression in the <c>SET</c> clause.
    /// </param>
    /// <returns>
    /// The new <see cref="UpdateSet"/> object.
    /// </returns>
    UpdateSet Set(string expr);
}
