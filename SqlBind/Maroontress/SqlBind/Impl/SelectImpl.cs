#pragma warning disable SingleTypeParameter

namespace Maroontress.SqlBind.Impl;

/// <summary>
/// The default implementation of <see cref="Select{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
/// <param name="bank">
/// The <see cref="Bank"/> object.
/// </param>
/// <param name="siphon">
/// The <see cref="Siphon"/> object.
/// </param>
/// <param name="text">
/// The prefix statement.
/// </param>
public sealed class SelectImpl<T>(
        MetadataBank bank, Siphon siphon, string text) : Select<T>
    where T : notnull
{
    private MetadataBank Bank { get; } = bank;

    private Siphon Siphon { get; } = siphon;

    private string Text { get; } = text;

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
