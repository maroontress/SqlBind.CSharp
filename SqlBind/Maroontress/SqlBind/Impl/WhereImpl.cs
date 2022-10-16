namespace Maroontress.SqlBind.Impl;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The default implementation of <see cref="Where{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
public sealed class WhereImpl<T> : Where<T>
    where T : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WhereImpl{T}"/> class.
    /// </summary>
    /// <param name="siphon">
    /// The <see cref="Siphon"/> object.
    /// </param>
    /// <param name="text">
    /// The prefix statement.
    /// </param>
    /// <param name="parameters">
    /// Immutable key-value pairs.
    /// </param>
    internal WhereImpl(
        Siphon siphon,
        string text,
        IReadOnlyDictionary<string, object> parameters)
    {
        Siphon = siphon;
        Text = text;
        Parameters = parameters;
    }

    private Siphon Siphon { get; }

    private string Text { get; }

    private IReadOnlyDictionary<string, object> Parameters { get; }

    /// <inheritdoc/>
    public IEnumerable<T> Execute()
    {
        return Execute(Text);
    }

    /// <inheritdoc/>
    public IEnumerable<T> OrderBy(params string[] columns)
    {
        var orderBy = string.Join(", ", columns);
        var text = $"{Text} ORDER BY {orderBy}";
        return Execute(text);
    }

    private IEnumerable<T> Execute(string text)
    {
        using var reader = Siphon.ExecuteReader(text, Parameters);
        return reader.NewInstances<T>()
            .ToList();
    }
}
