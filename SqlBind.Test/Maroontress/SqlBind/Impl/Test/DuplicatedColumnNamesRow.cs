namespace Maroontress.SqlBind.Impl.Test;

[Table("duplicatedColumnNames")]
public record DuplicatedColumnNamesRow(
    [Column("name")] string Name,
    [Column("name")] string Country)
{
}
