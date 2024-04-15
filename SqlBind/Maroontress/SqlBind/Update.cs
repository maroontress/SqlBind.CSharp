namespace Maroontress.SqlBind;

using System.Collections.Generic;

/// <summary>
/// Represents the <c>UPDATE</c> statement.
/// </summary>
public interface Update
{
    /// <summary>
    /// Gets a new <see cref="UpdateSet"/> object.
    /// </summary>
    /// <param name="expr">
    /// The expression in the <c>SET</c> clause. Note that this expression must
    /// not contain the <c>alias</c> specified in the <see
    /// cref="Query.Update{T}(string)"/> method. The values of the parameters
    /// in the expression can be specified with the <see
    /// cref="UpdateTerminator.Execute(IReadOnlyDictionary{string, object})"/>
    /// method.
    /// </param>
    /// <returns>
    /// The new <see cref="UpdateSet"/> object.
    /// </returns>
    UpdateSet Set(string expr);
}
