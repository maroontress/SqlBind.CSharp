namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public record IntParameterRow([Column("id")] int Id)
{
}
