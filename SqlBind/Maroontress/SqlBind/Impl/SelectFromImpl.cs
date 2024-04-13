#pragma warning disable SingleTypeParameter

namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;

/// <summary>
/// The default implementation of <see cref="SelectFrom{T}"/>.
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
public sealed class SelectFromImpl<T>(
        MetadataBank bank, Siphon siphon, string text)
    : AbstractSelectSorter<T>(text, ToExecutor(siphon)),
        SelectFrom<T>
    where T : notnull
{
    private MetadataBank Bank { get; } = bank;

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
        return new SelectFromImpl<T>(Bank, siphon, newText);
    }

    /// <inheritdoc/>
    public Where<T> Where(
        string condition,
        IReadOnlyDictionary<string, object> parameters)
    {
        var newText = Text + $" WHERE {condition}";
        return new WhereImpl<T>(siphon, newText, parameters);
    }

    private static Func<string, IEnumerable<T>> ToExecutor(Siphon siphon)
    {
        IEnumerable<T> Executor(string s)
        {
            using var reader = siphon.ExecuteReader(s);
            while (reader.Read())
            {
                yield return reader.NewInstance<T>();
            }
        }

        return Executor;
    }
}
