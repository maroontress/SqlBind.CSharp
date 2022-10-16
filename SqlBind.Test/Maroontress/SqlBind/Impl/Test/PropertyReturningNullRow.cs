#pragma warning disable SA1313

namespace Maroontress.SqlBind.Impl.Test;

[Table("foo")]
public sealed class PropertyReturningNullRow
{
    public PropertyReturningNullRow(
        [Column("id")] long Id,
        [Column("value")] string Value)
    {
        _ = Value;
        this.Id = Id;
        this.Value = null;
    }

    public long Id { get; }

    public string? Value { get; }
}
