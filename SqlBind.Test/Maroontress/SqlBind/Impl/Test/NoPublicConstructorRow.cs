namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public sealed class NoPublicConstructorRow
{
    private NoPublicConstructorRow()
    {
    }
}
