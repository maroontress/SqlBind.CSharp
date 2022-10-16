namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public record class ParameterMissingColumnAttributeRow(
    long Id,
    string Value)
{
}
