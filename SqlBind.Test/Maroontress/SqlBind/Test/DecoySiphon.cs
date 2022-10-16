namespace Maroontress.SqlBind.Test;

using System.Collections.Generic;
using Maroontress.SqlBind.Impl;
using StyleChecker.Annotations;

public sealed class DecoySiphon : Siphon
{
    public long ExecuteLong(
        [Unused] string text,
        [Unused] IReadOnlyDictionary<string, object>? parameters = null)
    {
        throw new NotImplementedException();
    }

    public void ExecuteNonQuery(
        [Unused] string text,
        [Unused] IReadOnlyDictionary<string, object>? parameters = null)
    {
        throw new NotImplementedException();
    }

    public Reservoir ExecuteReader(
        [Unused] string text,
        [Unused] IReadOnlyDictionary<string, object>? parameters = null)
    {
        throw new NotImplementedException();
    }
}
