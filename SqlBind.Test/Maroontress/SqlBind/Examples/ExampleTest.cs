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
        var e = new UpdateExample();
        e.CreateTable();
        e.SelectAllRows();
        e.UpdateState();
        e.SelectAllRows();
    }

    [TestMethod]
    public void RunLimitExample()
    {
        var e = new LimitExample();
        e.CreateTable();
        e.SelectAllRows();
        e.Limit();
        e.LimitOffset();
    }
}
