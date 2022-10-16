#pragma warning disable SingleTypeParameter

namespace Maroontress.SqlBind;

using System.Collections.Generic;
using Maroontress.SqlBind.Impl;

/// <summary>
/// Represents the <c>SELECT</c> statement in SQL without a <c>WHERE</c>
/// clause. It can end with a <c>INNER JOIN</c> clause.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
public interface SelectFrom<T> : TerminalOperation<T>
    where T : notnull
{
    /// <summary>
    /// Gets a new <see cref="SelectFrom{T}"/> object, which represents
    /// the combination of <c>this</c> (<c>SELECT</c> ... <c>From</c> ...)
    /// and the <c>INNER JOIN</c> ... <c>ON</c> ... clause with the table that
    /// <typeparamref name="U"/> represents.
    /// </summary>
    /// <remarks>
    /// The object that this method returns represents <c>SELECT ... FROM ...
    /// INNER JOIN</c> "the table name of U" <paramref name="alias"/> <c>ON</c>
    /// <paramref name="constraint"/>.
    /// </remarks>
    /// <typeparam name="U">
    /// The type of the class qualified with the <see cref="TableAttribute"/>.
    /// </typeparam>
    /// <param name="alias">
    /// The alias name of the table that <typeparamref name="U"/> represents.
    /// </param>
    /// <param name="constraint">
    /// The expression following <c>ON</c>.
    /// </param>
    /// <returns>
    /// The new <see cref="SelectFrom{T}"/> object.
    /// </returns>
    SelectFrom<T> InnerJoin<U>(string alias, string constraint)
        where U : notnull;

    /// <summary>
    /// Gets a new <see cref="WhereImpl{T}"/> object, which represents
    /// the combination of <c>this</c> (<c>SELECT</c> ... <c>From</c> ...)
    /// and the <c>WHERE</c> ... clause.
    /// </summary>
    /// <remarks>
    /// The object that this method returns represents <c>SELECT ... FROM ...
    /// WHERE</c> <paramref name="condition"/>.
    /// </remarks>
    /// <param name="condition">
    /// The condition of the <c>WHERE</c> clause.
    /// </param>
    /// <param name="parameters">
    /// Immutable key-value pairs. The <paramref name="condition"/> must
    /// contain all the keys. Each value must be of the appropriate type.
    /// </param>
    /// <returns>
    /// The new <see cref="WhereImpl{T}"/> object.
    /// </returns>
    Where<T> Where(
        string condition,
        IReadOnlyDictionary<string, object> parameters);
}
