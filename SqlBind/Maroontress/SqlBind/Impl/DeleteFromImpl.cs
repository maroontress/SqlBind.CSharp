namespace Maroontress.SqlBind.Impl;

using System.Collections.Generic;

/// <summary>
/// The default implementation of <see cref="DeleteFrom{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the class qualified with the <see cref="TableAttribute"/>.
/// </typeparam>
public sealed class DeleteFromImpl<T> : DeleteFrom<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteFromImpl{T}"/>
    /// class.
    /// </summary>
    /// <param name="siphon">
    /// The <see cref="Siphon"/> object.
    /// </param>
    /// <param name="text">
    /// The prefix statement.
    /// </param>
    internal DeleteFromImpl(Siphon siphon, string text)
    {
        Siphon = siphon;
        Text = text;
    }

    private Siphon Siphon { get; }

    private string Text { get; }

    /// <inheritdoc/>
    public void Where(
        string condition,
        IReadOnlyDictionary<string, object> parameters)
    {
        var text = Text + $" WHERE {condition}";
        Siphon.ExecuteNonQuery(text, parameters);
    }
}
