namespace Maroontress.SqlBind.Examples;

[Table("Books")]
public record class Book(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("title")] string Title)
{
}
