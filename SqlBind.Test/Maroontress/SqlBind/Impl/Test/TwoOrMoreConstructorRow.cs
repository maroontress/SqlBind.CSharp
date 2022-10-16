namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public record TwoOrMoreConstructorRow(
    [Column("id")] long Id,
    [Column("value")] string Value)
{
    public TwoOrMoreConstructorRow()
        : this(0, "(default)")
    {
    }
}
