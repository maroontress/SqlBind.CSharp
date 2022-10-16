namespace Maroontress.SqlBind.Impl;

using System;

/// <summary>
/// Represents connection to the database.
/// </summary>
public interface DatabaseLink : IDisposable
{
    /// <summary>
    /// Begins the transaction.
    /// </summary>
    /// <returns>
    /// The abstract transaction.
    /// </returns>
    Committable BeginTransaction();

    /// <summary>
    /// Gets a new siphon with the specified logger.
    /// </summary>
    /// <param name="logger">
    /// The logger to record statements.
    /// </param>
    /// <returns>
    /// The new siphon.
    /// </returns>
    Siphon NewSiphon(Action<Func<string>> logger);
}
