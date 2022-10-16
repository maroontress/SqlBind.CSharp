namespace Maroontress.SqlBind.Test;

[Table("persons")]
[IndexedColumns("firstNameId")]
[IndexedColumns("lastNameId")]
public record PersonRow(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("firstNameId")] long FirstNameId,
    [Column("lastNameId")] long LastNameId)
{
}
