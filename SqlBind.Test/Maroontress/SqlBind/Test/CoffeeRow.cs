namespace Maroontress.SqlBind.Test;

[Table("coffees")]
public record CoffeeRow(
    [Column("name")] string Name,
    [Column("country")] string Country)
{
}
