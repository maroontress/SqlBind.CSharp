namespace Maroontress.SqlBind.Impl;

/// <summary>
/// Provides the database connection.
/// </summary>
public interface Toolkit
{
    /// <summary>
    /// Gets or sets the toolkit instance.
    /// </summary>
    static Toolkit Instance { get; set; } = new DefaultToolkit();

    /// <summary>
    /// Gets a new database connection.
    /// </summary>
    /// <param name="databasePath">
    /// The database path.
    /// </param>
    /// <returns>
    /// The new database connection.
    /// </returns>
    DatabaseLink NewDatabaseLink(string databasePath);
}
