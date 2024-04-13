#pragma warning disable SingleTypeParameter

namespace Maroontress.SqlBind;

/// <summary>
/// Represents the <c>SELECT</c> statement.
/// </summary>
/// <typeparam name="T">
/// The type of the class representing any row of the result of the query.
/// </typeparam>
public interface Select<T>
    where T : notnull
{
    /// <summary>
    /// Gets a new <see cref="SelectFrom{T}"/> object.
    /// </summary>
    /// <typeparam name="U">
    /// The type of the class qualified with the <see cref="TableAttribute"/>.
    /// </typeparam>
    /// <param name="alias">
    /// The alias name of the table that <typeparamref name="U"/> represents.
    /// </param>
    /// <returns>
    /// The new <see cref="SelectFrom{T}"/> object.
    /// </returns>
    SelectFrom<T> From<U>(string alias)
        where U : notnull;
}
