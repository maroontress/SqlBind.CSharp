namespace Maroontress.SqlBind.Impl;

using System;

/// <summary>
/// Represents an atomic transaction.
/// </summary>
public interface Committable : IDisposable
{
    /// <summary>
    /// Applies the changes made in the transaction.
    /// </summary>
    void Commit();

    /// <summary>
    /// Reverts the changes made in the transaction.
    /// </summary>
    void Rollback();
}
