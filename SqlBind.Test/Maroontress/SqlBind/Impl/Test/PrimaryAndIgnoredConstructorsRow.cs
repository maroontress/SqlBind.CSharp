namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public record PrimaryAndIgnoredConstructorsRow(
    [Column("id")] long Id,
    [Column("value")] string Value)
{
    [Ignored]
    public PrimaryAndIgnoredConstructorsRow()
        : this(0, "(default)")
    {
    }
}
