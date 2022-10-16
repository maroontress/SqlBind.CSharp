#pragma warning disable SingleTypeParameter

namespace Maroontress.SqlBind.Impl;

/// <summary>
/// The default implementation of <see cref="Select{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
public sealed class SelectImpl<T> : Select<T>
    where T : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectImpl{T}"/>
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
    internal SelectImpl(MetadataBank bank, Siphon siphon, string text)
    {
        Bank = bank;
        Siphon = siphon;
        Text = text;
    }

    private MetadataBank Bank { get; }

    private Siphon Siphon { get; }

    private string Text { get; }

    /// <inheritdoc/>
    public SelectFrom<T> From<U>(string alias)
        where U : notnull
    {
        var tableName = Bank.GetMetadata<U>()
            .TableName;
        var newText = string.Join(
            ' ',
            Text,
            "FROM",
            tableName,
            alias);
        return new SelectFromImpl<T>(Bank, Siphon, newText);
    }
}
