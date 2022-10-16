namespace Maroontress.SqlBind.Test;

[Table("strings")]
[IndexedColumns("value")]
public record StringRow(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("value")][Unique] string Value)
{
}
