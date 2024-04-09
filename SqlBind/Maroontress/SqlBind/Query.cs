namespace Maroontress.SqlBind;

using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a query.
/// </summary>
public interface Query
{
    /// <summary>
    /// Selects all columns of the specified table.
    /// </summary>
    /// <typeparam name="T">
    /// The type qualified with <see cref="TableAttribute"/>.
    /// </typeparam>
    /// <param name="alias">
    /// The alias of the table name.
    /// </param>
    /// <returns>
    /// The <see cref="SelectFrom{T}"/> object.
    /// </returns>
    SelectFrom<T> SelectAllFrom<T>(string alias)
        where T : notnull;

    /// <summary>
    /// Gets the <see cref="Select{T}"/> object to select the specified columns
    /// of the specified table.
    /// </summary>
    /// <typeparam name="T">
    /// The type representing the result of this query, which must have the
    /// single constructor accepting the parameters corresponding to <paramref
    /// name="columns"/>.
    /// </typeparam>
    /// <param name="columns">
    /// One or more column names. Each consists of the alias followed by a
    /// period (<c>.</c>) and the column name.
    /// </param>
    /// <returns>
    /// The <see cref="Select{T}"/> object.
    /// </returns>
    Select<T> Select<T>(params string[] columns)
        where T : notnull;

    /// <summary>
    /// Gets the <see cref="SqlBind.Update"/> object to update the specified
    /// table.
    /// </summary>
    /// <typeparam name="T">
    /// The type qualified with <see cref="TableAttribute"/>.
    /// </typeparam>
    /// <param name="alias">
    /// The alias of the table name, which can be used in the <c>Where</c>
    /// clause (see <see cref="UpdateSet.Where(string)"/>).
    /// </param>
    /// <returns>
    /// The <see cref="SqlBind.Update"/> object.
    /// </returns>
    /// <example>
    /// <code>
    /// var kit = new TransactionKit(
    ///     "update_example.db", m => Console.WriteLine(m()));
    /// var map = new Dictionary&lt;string, object>()
    /// {
    ///     ["$id"] = 2L,
    ///     ["$newState"] = "Closed",
    /// };
    /// kit.Execute(q =>
    /// {
    ///     q.Update&lt;Issue>("i")
    ///         .Set("state = $newState")
    ///         .Where("i.id = $id")
    ///         .Execute(map);
    /// });
    /// </code>
    /// </example>
    Update Update<T>(string alias)
        where T : notnull;

    /// <summary>
    /// Gets the <see cref="DeleteFrom{T}"/> object to delete the specified
    /// rows of the specified table.
    /// </summary>
    /// <typeparam name="T">
    /// The type qualified with <see cref="TableAttribute"/>.
    /// </typeparam>
    /// <returns>
    /// The <see cref="DeleteFrom{T}"/> object.
    /// </returns>
    DeleteFrom<T> DeleteFrom<T>()
        where T : notnull;

    /// <summary>
    /// Creates new tables.
    /// </summary>
    /// <remarks>
    /// This method drops the tables if they exist and then creates them newly.
    /// </remarks>
    /// <param name="tables">
    /// The types qualified with <see cref="TableAttribute"/> representing
    /// the tables to create.
    /// </param>
    void NewTables(params Type[] tables);

    /// <summary>
    /// Creates new tables.
    /// </summary>
    /// <remarks>
    /// This method drops the tables if they exist and then creates them newly.
    /// </remarks>
    /// <param name="allTables">
    /// The types qualified with <see cref="TableAttribute"/> representing
    /// the tables to create.
    /// </param>
    void NewTables(IEnumerable<Type> allTables);

    /// <summary>
    /// Inserts a new row into the table.
    /// </summary>
    /// <typeparam name="T">
    /// The type qualified with <see cref="TableAttribute"/> representing
    /// the table.
    /// </typeparam>
    /// <param name="row">
    /// The row of the table to add.
    /// </param>
    void Insert<T>(T row)
        where T : notnull;

    /// <summary>
    /// Inserts a new row into the table and gets its ID.
    /// </summary>
    /// <typeparam name="T">
    /// The type qualified with <see cref="TableAttribute"/> representing the
    /// table. It must have the field qualified with <see
    /// cref="AutoIncrementAttribute"/>.
    /// </typeparam>
    /// <param name="row">
    /// The row of the table to add.
    /// </param>
    /// <returns>
    /// The ID of the row newly inserted.
    /// </returns>
    long InsertAndGetRowId<T>(T row)
        where T : notnull;

    /// <summary>
    /// Gets the single row of the table in which the specified unique field
    /// matches the specified value.
    /// </summary>
    /// <typeparam name="T">
    /// The type qualified with <see cref="TableAttribute"/> representing the
    /// table. It must have the field qualified with <see
    /// cref="UniqueAttribute"/>.
    /// </typeparam>
    /// <param name="columnName">
    /// The name of the unique field.
    /// </param>
    /// <param name="value">
    /// The value that the unique field matches.
    /// </param>
    /// <returns>
    /// The row in which the field specified with <paramref name="columnName"/>
    /// matches the specified <paramref name="value"/> if it exists,
    /// <c>null</c> otherwise.
    /// </returns>
    T? SelectUnique<T>(string columnName, object value)
        where T : class;

    /// <summary>
    /// Gets all the rows of the specified table.
    /// </summary>
    /// <remarks>
    /// The <see cref="IEnumerable{T}"/> object returned by this method is
    /// valid as long as the transaction is in progress. If you need access to
    /// it after the transaction, you must use something like the
    /// <see cref="Enumerable.ToList{TSource}(IEnumerable{TSource})"/>
    /// method to get the list in the meantime and then use it afterward.
    /// </remarks>
    /// <typeparam name="T">
    /// The type represents the row of the table.
    /// </typeparam>
    /// <returns>
    /// All the rows of the specified table.
    /// </returns>
    IEnumerable<T> SelectAll<T>()
        where T : notnull;

    /// <summary>
    /// Gets the column name associated with the specified parameter.
    /// </summary>
    /// <typeparam name="T">
    /// The type represents the row of the table.
    /// </typeparam>
    /// <param name="parameterName">
    /// The parameter name that the constructor of <typeparamref name="T"/>
    /// contains.
    /// </param>
    /// <returns>
    /// The column name.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Throws if <paramref name="parameterName"/> is not found.
    /// </exception>
    string ColumnName<T>(string parameterName)
        where T : notnull;
}
