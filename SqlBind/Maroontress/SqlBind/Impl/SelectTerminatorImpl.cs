namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;

/// <summary>
/// Represents the default implementation of <see cref="SelectTerminator{T}"/>
/// interface.
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
public sealed class SelectTerminatorImpl<T>(
        string text,
        Func<string, IEnumerable<T>> executor) : SelectTerminator<T>
    where T : notnull
{
    private string Text { get; } = text;

    private Func<string, IEnumerable<T>> Executor { get; } = executor;

    /// <inheritdoc/>
    public IEnumerable<T> Execute()
        => Executor(Text);

    /// <inheritdoc/>
    public IEnumerable<T> Limit(int limit)
        => Executor($"{Text} LIMIT {limit}");

    /// <inheritdoc/>
    public IEnumerable<T> LimitOffset(int limit, int offset)
        => Executor($"{Text} LIMIT {limit} OFFSET {offset}");
}
