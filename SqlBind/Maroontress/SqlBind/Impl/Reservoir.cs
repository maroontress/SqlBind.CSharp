namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Generic;

/// <summary>
/// Retrieves the result of the query.
/// </summary>
public interface Reservoir : IDisposable
{
    /// <summary>
    /// Advances to the next row in the result set.
    /// </summary>
    /// <returns>
    /// <c>true</c> if there are more rows; otherwise, <c>false</c>.
    /// </returns>
    bool Read();

    /// <summary>
    /// Creates an instance with the current row.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the instance to be created.
    /// </typeparam>
    /// <returns>
    /// The new instance.
    /// </returns>
    T NewInstance<T>();

    /// <summary>
    /// Creates new instances with the current and susequent rows.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the instances to be created.
    /// </typeparam>
    /// <returns>
    /// The new instances.
    /// </returns>
    IEnumerable<T> NewInstances<T>();
}
