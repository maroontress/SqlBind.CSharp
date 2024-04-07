namespace Maroontress.SqlBind.Impl;

using System.Collections.Generic;

/// <summary>
/// The default implementation of <see cref="UpdateWhere"/>.
/// </summary>
/// <param name="siphon">
/// The siphon to be used in the update operation.
/// </param>
/// <param name="text">
/// The SQL update statement.
/// </param>
public sealed class UpdateWhereImpl(Siphon siphon, string text)
    : UpdateWhere
{
    private Siphon Siphon { get; } = siphon;

    private string Text { get; } = text;

    /// <inheritdoc/>
    public void Execute(IReadOnlyDictionary<string, object> parameters)
    {
        Siphon.ExecuteNonQuery(Text, parameters);
    }
}
