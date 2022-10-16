namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public record PrimaryAndNonPublicConstructorsRow(
    [Column("id")] long Id,
    [Column("value")] string Value)
{
    private PrimaryAndNonPublicConstructorsRow()
        : this(0, "(default)")
    {
    }

    public static PrimaryAndNonPublicConstructorsRow Of()
    {
        return new PrimaryAndNonPublicConstructorsRow();
    }
}
