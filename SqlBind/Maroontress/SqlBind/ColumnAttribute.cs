namespace Maroontress.SqlBind;

using System;

/// <summary>
/// An attribute that qualifies any parameter in the constructor, associating
/// the parameter with the column that has the specified name.
/// </summary>
/// <seealso cref="TableAttribute"/>
[AttributeUsage(
    AttributeTargets.Parameter,
    Inherited = false,
    AllowMultiple = false)]
public sealed class ColumnAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnAttribute"/> class.
    /// </summary>
    /// <param name="name">
    /// The column name.
    /// </param>
    public ColumnAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the column name.
    /// </summary>
    public string Name { get; }
}
