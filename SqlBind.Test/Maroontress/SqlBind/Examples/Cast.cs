namespace Maroontress.SqlBind.Examples;

[Table("Casts")]
public record class Cast(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("titleId")] long TitleId,
    [Column("actorId")] long ActorId,
    [Column("role")] string Role)
{
}
