namespace Maroontress.SqlBind.Examples;

[TestClass]
public sealed class ExampleTest
{
    [TestMethod]
    public void Run()
    {
        var e = new Example();
        e.CreateTableAndInsertRows();
        e.SelectAllRows();
        e.CreateTables();
        e.ListActorNames("Peripheral");
        e.ListActorNamesV2("Peripheral");
    }
}
