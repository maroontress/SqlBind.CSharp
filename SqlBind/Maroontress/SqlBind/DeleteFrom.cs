namespace Maroontress.SqlBind;

using System.Collections.Generic;

/// <summary>
/// Represents the <c>DELETE</c> statement.
/// </summary>
/// <typeparam name="T">
/// The type of the class qualified with the <see cref="TableAttribute"/>.
/// </typeparam>
public interface DeleteFrom<T>
{
    /// <summary>
    /// Executes the delete statement with the specified condition and
    /// parameters.
    /// </summary>
    /// <param name="condition">
    /// The condition of the <c>WHERE</c> clause.
    /// </param>
    /// <param name="parameters">
    /// Immutable key-value pairs. The <paramref name="condition"/> must
    /// contain all the keys. Each value must be of the appropriate type.
    /// </param>
    public void Where(
        string condition,
        IReadOnlyDictionary<string, object> parameters);
}
