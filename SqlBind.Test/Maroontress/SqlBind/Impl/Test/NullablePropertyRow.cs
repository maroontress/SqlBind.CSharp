namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public record class NullablePropertyRow(
    [Column("id")] long Id,
    [Column("value")] string? Value)
{
}
