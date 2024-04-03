namespace Maroontress.SqlBind.Test;

using Maroontress.SqlBind.Impl;

public sealed class DecoyCommittable(List<string> trace)
    : Committable
{
    private List<string> Trace { get; } = trace;

    public void Commit()
    {
        Trace.Add($"{nameof(Committable)}#{nameof(Commit)}");
    }

    public void Dispose()
    {
        Trace.Add($"{nameof(Committable)}#{nameof(Dispose)}");
    }

    public void Rollback()
    {
        Trace.Add($"{nameof(Committable)}#{nameof(Rollback)}");
    }
}
