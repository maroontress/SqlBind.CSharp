namespace Maroontress.SqlBind.Examples;

[Table("Titles")]
public record class Title(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("name")] string Name)
{
}
