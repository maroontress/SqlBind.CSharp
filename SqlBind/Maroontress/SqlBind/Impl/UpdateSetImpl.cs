namespace Maroontress.SqlBind.Impl;

using System.Collections.Generic;

/// <summary>
/// The default implementation of <see cref="UpdateSet"/>.
/// </summary>
/// <param name="siphon">
/// The siphon to be used in the update operation.
/// </param>
/// <param name="text">
/// The SQL update statement.
/// </param>
public sealed class UpdateSetImpl(Siphon siphon, string text)
    : UpdateSet
{
    private Siphon Siphon { get; } = siphon;

    private string Text { get; } = text;

    /// <inheritdoc/>
    public void Execute(IReadOnlyDictionary<string, object> parameters)
    {
        Siphon.ExecuteNonQuery(Text, parameters);
    }

    /// <inheritdoc/>
    public UpdateWhere Where(string expr)
    {
        var text = Text + $" WHERE {expr}";
        return new UpdateWhereImpl(Siphon, text);
    }
}
