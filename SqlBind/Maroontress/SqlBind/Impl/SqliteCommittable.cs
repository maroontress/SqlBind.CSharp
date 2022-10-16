namespace Maroontress.SqlBind.Impl;

using Microsoft.Data.Sqlite;

/// <summary>
/// The implementation with Sqlite.
/// </summary>
public sealed class SqliteCommittable : Committable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqliteCommittable"/>
    /// class.
    /// </summary>
    /// <param name="transaction">
    /// The Sqlite's transaction.
    /// </param>
    public SqliteCommittable(SqliteTransaction transaction)
    {
        Transaction = transaction;
    }

    private SqliteTransaction Transaction { get; }

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
