namespace Maroontress.SqlBind.Test;

using System;
using Maroontress.SqlBind.Impl;
using StyleChecker.Annotations;

public sealed class DecoyDatabaseLink : DatabaseLink
{
    public DecoyDatabaseLink(List<string> trace)
    {
        Trace = trace;
    }

    private List<string> Trace { get; }

    public Committable BeginTransaction()
    {
        Trace.Add($"{nameof(DatabaseLink)}#{nameof(BeginTransaction)}");
        return new DecoyCommittable(Trace);
    }

    public void Dispose()
    {
        Trace.Add($"{nameof(DatabaseLink)}#{nameof(Dispose)}");
    }

    public Siphon NewSiphon([Unused] Action<Func<string>> logger)
    {
        Trace.Add($"{nameof(DatabaseLink)}#{nameof(NewSiphon)}");
        return new DecoySiphon();
    }
}
