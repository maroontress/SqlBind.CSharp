namespace Maroontress.SqlBind;

/// <summary>
/// Represents an operation that sets the values in an update statement.
/// </summary>
public interface UpdateSet
    : UpdateTerminalOperation
{
    /// <summary>
    /// Gets a new <see cref="UpdateWhere"/> object, which represents the
    /// combination of <c>this</c> (<c>UPDATE</c> ... <c>SET</c> ...) and the
    /// <c>WHERE</c> ... clause.
    /// </summary>
    /// <param name="expr">
    /// The expression of the <c>WHERE</c> clause. Note that this expression
    /// should not contain <c>alias</c> specified in the <see
    /// cref="Query.Update{T}(string)"/>.
    /// </param>
    /// <returns>
    /// The new <see cref="UpdateWhere"/> object.
    /// </returns>
    UpdateWhere Where(string expr);
}
