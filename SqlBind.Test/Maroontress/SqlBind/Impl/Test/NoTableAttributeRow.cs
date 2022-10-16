namespace Maroontress.SqlBind.Impl.Test;

public record class NoTableAttributeRow(
    [Column("id")] long Id,
    [Column("value")] string Value)
{
}
