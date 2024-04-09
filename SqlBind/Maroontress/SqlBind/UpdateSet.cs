namespace Maroontress.SqlBind;

using System.Collections.Generic;

/// <summary>
/// Represents the <c>SET</c> clause in an <c>UPDATE</c> statement.
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
    /// The expression of the <c>WHERE</c> clause. This expression can contain
    /// the <c>alias</c> specified in the <see cref="Query.Update{T}(string)"/>
    /// method. The values of the parameters in the expression can be specified
    /// with the <see
    /// cref="UpdateTerminalOperation.Execute(IReadOnlyDictionary{string,
    /// object})"/> method.
    /// </param>
    /// <returns>
    /// The new <see cref="UpdateWhere"/> object.
    /// </returns>
    UpdateWhere Where(string expr);
}
