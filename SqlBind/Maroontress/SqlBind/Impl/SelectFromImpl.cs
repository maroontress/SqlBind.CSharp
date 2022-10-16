#pragma warning disable SingleTypeParameter

namespace Maroontress.SqlBind.Impl;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The default implementation of <see cref="SelectFrom{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
public sealed class SelectFromImpl<T> : SelectFrom<T>
    where T : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectFromImpl{T}"/>
    /// class.
    /// </summary>
    /// <param name="bank">
    /// The <see cref="Bank"/> object.
    /// </param>
    /// <param name="siphon">
    /// The <see cref="Siphon"/> object.
    /// </param>
    /// <param name="text">
    /// The prefix statement.
    /// </param>
    internal SelectFromImpl(MetadataBank bank, Siphon siphon, string text)
    {
        Bank = bank;
        Siphon = siphon;
        Text = text;
    }

    private MetadataBank Bank { get; }

    private Siphon Siphon { get; }

    private string Text { get; }

    /// <inheritdoc/>
    public SelectFrom<T> InnerJoin<U>(string alias, string constraint)
        where U : notnull
    {
        var tableName = Bank.GetMetadata<U>()
            .TableName;
        var newText = string.Join(
            ' ',
            Text,
            "INNER JOIN",
            tableName,
            alias,
            "ON",
            constraint);
        return new SelectFromImpl<T>(Bank, Siphon, newText);
    }

    /// <inheritdoc/>
    public Where<T> Where(
        string condition,
        IReadOnlyDictionary<string, object> parameters)
    {
        var text = Text + $" WHERE {condition}";
        return new WhereImpl<T>(Siphon, text, parameters);
    }

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
        using var reader = Siphon.ExecuteReader(text);
        return reader.NewInstances<T>()
            .ToList();
    }
}
