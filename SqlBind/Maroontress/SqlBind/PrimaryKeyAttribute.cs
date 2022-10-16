namespace Maroontress.SqlBind;

using System;

/// <summary>
/// An attribute that qualifies any parameter in the constructor, making the
/// column associated with the parameter a <c>PRIMARY KEY</c> field.
/// </summary>
/// <remarks>
/// <para>
/// This attribute can be specified for at most one of the parameters of the
/// constructor.
/// </para>
/// <para>
/// See: <a
/// href="https://www.sqlite.org/lang_createtable.html#primkeyconst">SQLite
/// CREATE TABLE</a>.
/// </para>
/// </remarks>
[AttributeUsage(
    AttributeTargets.Parameter,
    Inherited = false,
    AllowMultiple = false)]
public sealed class PrimaryKeyAttribute : Attribute
{
}
