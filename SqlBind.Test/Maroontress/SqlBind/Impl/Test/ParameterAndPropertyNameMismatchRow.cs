namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public sealed class ParameterAndPropertyNameMismatchRow
{
    public ParameterAndPropertyNameMismatchRow(
        [Column("one")] string one,
        [Column("two")] string two)
    {
        One = one;
        Two = two;
    }

    public string One { get; }

    public string Two { get; }
}
