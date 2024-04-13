namespace Maroontress.SqlBind;

using System.Collections.Generic;

/// <summary>
/// Represents the executable <c>UPDATE</c> statement.
/// </summary>
public interface UpdateTerminalOperation
{
    /// <summary>
    /// Executes the <c>UPDATE</c> statement with the specified parameters.
    /// </summary>
    /// <param name="parameters">
    /// Immutable key-value pairs. All parameter names in the <c>SET</c> and
    /// <c>WHERE</c> clauses must be included in <c>parameters</c> as keys.
    /// Each value must be of the appropriate type.
    /// </param>
    /// <seealso cref="Update.Set(string)"/>
    /// <seealso cref="UpdateSet.Where(string)"/>
    void Execute(IReadOnlyDictionary<string, object> parameters);
}
