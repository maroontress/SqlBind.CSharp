namespace Maroontress.SqlBind.Test;

using Maroontress.SqlBind.Impl;

public sealed class DecoyToolkit(Func<string, DatabaseLink> toLink)
    : Toolkit
{
    private Func<string, DatabaseLink> ToLink { get; } = toLink;

    public DatabaseLink NewDatabaseLink(string databasePath)
    {
        return ToLink(databasePath);
    }
}
