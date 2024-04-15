namespace Maroontress.SqlBind.Impl;

using Microsoft.Data.Sqlite;

/// <summary>
/// The implementation with Sqlite.
/// </summary>
/// <param name="transaction">
/// The Sqlite's transaction.
/// </param>
public sealed class SqliteCommittable(SqliteTransaction transaction)
    : Committable
{
    private SqliteTransaction Transaction { get; } = transaction;

    /// <inheritdoc/>
    public void Commit()
        => Transaction.Commit();

    /// <inheritdoc/>
    public void Dispose()
        => Transaction.Dispose();

    /// <inheritdoc/>
    public void Rollback()
        => Transaction.Rollback();
}
