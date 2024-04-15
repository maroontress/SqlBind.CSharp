namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;

/// <summary>
/// Abstract base class for SELECT sorters.
/// </summary>
/// <typeparam name="T">
/// The type of the result set.
/// </typeparam>
/// <param name="text">
/// The SQL query text.
/// </param>
/// <param name="executor">
/// The executor function.
/// </param>
public abstract class AbstractSelectSorter<T>(
        string text,
        Func<string, IEnumerable<T>> executor) : SelectSorter<T>
    where T : notnull
{
    /// <summary>
    /// Gets the SQL query text.
    /// </summary>
    protected string Text { get; } = text;

    /// <inheritdoc/>
    public SelectTerminator<T> OrderBy(params string[] columns)
    {
        var orderBy = string.Join(", ", columns);
        var newText = $"{Text} ORDER BY {orderBy}";
        return new SelectTerminatorImpl<T>(newText, executor);
    }

    /// <inheritdoc/>
    public IEnumerable<T> Execute()
        => ToTerminator().Execute();

    /// <inheritdoc/>
    public IEnumerable<T> Limit(int limit)
        => ToTerminator().Limit(limit);

    /// <inheritdoc/>
    public IEnumerable<T> LimitOffset(int limit, int offset)
        => ToTerminator().LimitOffset(limit, offset);

    private SelectTerminator<T> ToTerminator()
        => new SelectTerminatorImpl<T>(Text, executor);
}
