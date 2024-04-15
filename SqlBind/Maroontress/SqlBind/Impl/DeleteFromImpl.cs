namespace Maroontress.SqlBind.Impl;

using System.Collections.Generic;

/// <summary>
/// The default implementation of <see cref="DeleteFrom{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the class qualified with the <see cref="TableAttribute"/>.
/// </typeparam>
/// <param name="siphon">
/// The <see cref="Siphon"/> object.
/// </param>
/// <param name="text">
/// The prefix statement.
/// </param>
public sealed class DeleteFromImpl<T>(Siphon siphon, string text)
    : DeleteFrom<T>
{
    private Siphon Siphon { get; } = siphon;

    private string Text { get; } = text;

    /// <inheritdoc/>
    public void Where(
        string condition,
        IReadOnlyDictionary<string, object> parameters)
    {
        var text = Text + $" WHERE {condition}";
        Siphon.ExecuteNonQuery(text, parameters);
    }
}
