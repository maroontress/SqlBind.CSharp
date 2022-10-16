namespace Maroontress.SqlBind.Examples;

[Table("Actors")]
public record class Actor(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("name")] string Name)
{
}
