namespace Maroontress.SqlBind.Test;

using Maroontress.SqlBind.Impl;

public sealed class DecoyToolkit : Toolkit
{
    public DecoyToolkit(Func<string, DatabaseLink> toLink)
    {
        ToLink = toLink;
    }

    private Func<string, DatabaseLink> ToLink { get; }

    public DatabaseLink NewDatabaseLink(string databasePath)
    {
        return ToLink(databasePath);
    }
}