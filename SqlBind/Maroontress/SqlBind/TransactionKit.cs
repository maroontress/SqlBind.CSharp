namespace Maroontress.SqlBind;

using System;
using Maroontress.SqlBind.Impl;

/// <summary>
/// The factory that creates queries.
/// </summary>
public sealed class TransactionKit
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionKit"/> class.
    /// </summary>
    /// <param name="databasePath">
    /// The path of the database file.
    /// </param>
    /// <param name="logger">
    /// The logger.
    /// </param>
    public TransactionKit(string databasePath, Action<Func<string>> logger)
    {
        DatabasePath = databasePath;
        Logger = logger;
        Cache = new();
    }

    private string DatabasePath { get; }

    private Action<Func<string>> Logger { get; }

    private MetadataBank Cache { get; }

    /// <summary>
    /// Executes queries within a single transaction.
    /// </summary>
    /// <remarks>
    /// If the <paramref name="action"/> throws an exception, this method
    /// performs the rollback.
    /// </remarks>
    /// <param name="action">
    /// The action that takes a <see cref="Query"/> object.
    /// </param>
    public void Execute(Action<Query> action)
    {
        var kit = Toolkit.Instance;
        using var link = kit.NewDatabaseLink(DatabasePath);
        using var x = link.BeginTransaction();
        try
        {
            var s = link.NewSiphon(Logger);
            action(new QueryImpl(s, Cache));
            x.Commit();
        }
        catch (Exception)
        {
            x.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Executes queries within a single transaction and returns the result.
    /// </summary>
    /// <remarks>
    /// If the <paramref name="apply"/> throws an exception, this method
    /// performs the rollback.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the result.
    /// </typeparam>
    /// <param name="apply">
    /// The function that takes a <see cref="Query"/> object and returns the
    /// result.
    /// </param>
    /// <returns>
    /// The result.
    /// </returns>
    public T Execute<T>(Func<Query, T> apply)
    {
        var kit = Toolkit.Instance;
        using var link = kit.NewDatabaseLink(DatabasePath);
        using var x = link.BeginTransaction();
        try
        {
            var s = link.NewSiphon(Logger);
            var o = apply(new QueryImpl(s, Cache));
            x.Commit();
            return o;
        }
        catch (Exception)
        {
            x.Rollback();
            throw;
        }
    }
}
