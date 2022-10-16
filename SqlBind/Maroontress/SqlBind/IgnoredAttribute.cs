namespace Maroontress.SqlBind;

using System;

/// <summary>
/// An attribute that qualifies the constructor that SqlBind does not use to
/// instantiate when the type has two or more constructors.
/// </summary>
[AttributeUsage(
    AttributeTargets.Constructor,
    Inherited = false,
    AllowMultiple = false)]
public sealed class IgnoredAttribute : Attribute
{
}
