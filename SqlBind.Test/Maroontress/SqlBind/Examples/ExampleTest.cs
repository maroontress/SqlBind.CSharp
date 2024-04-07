namespace Maroontress.SqlBind.Examples;

[TestClass]
public sealed class ExampleTest
{
    [TestMethod]
    public void RunExample()
    {
        var e = new Example();
        e.CreateTableAndInsertRows();
        e.SelectAllRows();
        e.CreateTables();
        e.ListActorNames("Peripheral");
        e.ListActorNamesV2("Peripheral");
    }

    [TestMethod]
    public void RunUpdateExample()
    {
        var u = new UpdateExample();
        u.CreateTable();
        u.SelectAllRows();
        u.UpdateState();
        u.SelectAllRows();
    }
}
