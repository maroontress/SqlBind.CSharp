namespace Maroontress.SqlBind.Impl;

using Microsoft.Data.Sqlite;

/// <summary>
/// The default implementation of the <see cref="Toolkit"/> interface.
/// </summary>
public sealed class DefaultToolkit : Toolkit
{
    /// <inheritdoc/>
    public DatabaseLink NewDatabaseLink(string databasePath)
    {
        var c = new SqliteConnection($"Data Source={databasePath}");
        c.Open();
        return new SqliteDatabaseLink(c);
    }
}
