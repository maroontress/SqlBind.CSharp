namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
[IndexedColumns]
public record EmptyIndexedColumnsRow(
    [Column("id")] long Id,
    [Column("value")] string Value)
{
}
