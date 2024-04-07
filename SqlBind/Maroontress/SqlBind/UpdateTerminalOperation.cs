namespace Maroontress.SqlBind;

using System.Collections.Generic;

/// <summary>
/// Represents the executable <c>UPDATE</c> statement in SQL.
/// </summary>
public interface UpdateTerminalOperation
{
    /// <summary>
    /// Executes the update operation with the specified parameters.
    /// </summary>
    /// <param name="parameters">
    /// Immutable key-value pairs. The parameters in the <c>Set</c> and
    /// <c>Where</c> clauses must contain all the keys. Each value must be of
    /// the appropriate type.
    /// </param>
    void Execute(IReadOnlyDictionary<string, object> parameters);
}
