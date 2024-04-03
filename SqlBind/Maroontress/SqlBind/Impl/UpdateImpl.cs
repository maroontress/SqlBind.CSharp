namespace Maroontress.SqlBind.Impl;

/// <summary>
/// The default implementation of <see cref="Update"/>.
/// </summary>
/// <param name="siphon">
/// The siphon to be used in the update operation.
/// </param>
/// <param name="updateStatement">
/// The SQL update statement.
/// </param>
/// <param name="alias">
/// The alias for the table in the update statement.
/// </param>
public sealed class UpdateImpl(
        Siphon siphon, string updateStatement, string alias)
    : Update
{
    private Siphon Siphon { get; } = siphon;

    private string Text { get; } = $"{updateStatement} AS {alias}";

    /// <inheritdoc/>
    public UpdateSet Set(string expr)
    {
        var text = Text + $" SET {expr}";
        return new UpdateSetImpl(Siphon, text);
    }
}
