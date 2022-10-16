namespace Maroontress.SqlBind;

using System;

/// <summary>
/// An attribute that qualifies any parameter in the constructor, making the
/// column associated with the parameter an <c>AUTOINCREMENT</c> field.
/// </summary>
/// <remarks>
/// <para>
/// This attribute must be specified together with the <see
/// cref="PrimaryKeyAttribute"/>.
/// </para>
/// <para>
/// See: <a href="https://www.sqlite.org/autoinc.html">SQLite
/// Autoincrement</a>.
/// </para>
/// </remarks>
[AttributeUsage(
    AttributeTargets.Parameter,
    Inherited = false,
    AllowMultiple = false)]
public sealed class AutoIncrementAttribute : Attribute
{
}
