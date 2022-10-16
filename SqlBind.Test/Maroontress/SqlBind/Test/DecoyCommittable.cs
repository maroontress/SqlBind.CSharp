namespace Maroontress.SqlBind.Test;

using Maroontress.SqlBind.Impl;

public sealed class DecoyCommittable : Committable
{
    public DecoyCommittable(List<string> trace)
    {
        Trace = trace;
    }

    private List<string> Trace { get; }

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
