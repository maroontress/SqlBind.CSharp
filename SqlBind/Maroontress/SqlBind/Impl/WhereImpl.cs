namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;

/// <summary>
/// The default implementation of <see cref="Where{T}"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
/// <param name="siphon">
/// The <see cref="Siphon"/> object.
/// </param>
/// <param name="text">
/// The prefix statement.
/// </param>
/// <param name="parameters">
/// Immutable key-value pairs.
/// </param>
public sealed class WhereImpl<T>(
        Siphon siphon,
        string text,
        IReadOnlyDictionary<string, object> parameters)
    : AbstractSelectSorter<T>(text, ToExecutor(siphon, parameters)),
        Where<T>
    where T : notnull
{
    private static Func<string, IEnumerable<T>> ToExecutor(
            Siphon siphon,
            IReadOnlyDictionary<string, object> parameters)
    {
        IEnumerable<T> Executor(string s)
        {
            using var reader = siphon.ExecuteReader(s, parameters);
            while (reader.Read())
            {
                yield return reader.NewInstance<T>();
            }
        }

        return Executor;
    }
}
