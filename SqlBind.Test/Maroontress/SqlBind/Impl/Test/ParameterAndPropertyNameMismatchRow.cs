namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public sealed class ParameterAndPropertyNameMismatchRow(
    [Column("one")] string one,
    [Column("two")] string two)
{
    public string One { get; } = one;

    public string Two { get; } = two;
}
