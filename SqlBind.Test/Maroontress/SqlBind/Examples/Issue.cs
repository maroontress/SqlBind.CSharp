namespace Maroontress.SqlBind.Examples;

[Table("Issues")]
public record class Issue(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("title")] string Title,
    [Column("state")] string State)
{
}
