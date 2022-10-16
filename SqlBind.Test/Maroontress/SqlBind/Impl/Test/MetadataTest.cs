namespace Maroontress.SqlBind.Impl.Test;

using Maroontress.SqlBind.Test;

[TestClass]
public sealed class MetadataTest
{
    [TestMethod]
    public void New_DuplicatedColumnNamesRow()
    {
        NewThrowArgumentException<DuplicatedColumnNamesRow>();
    }

    [TestMethod]
    public void New_ParameterAndPropertyNameMismatchRow()
    {
        NewThrowArgumentException<ParameterAndPropertyNameMismatchRow>();
    }

    [TestMethod]
    public void New_IntParameterRow()
    {
        NewThrowArgumentException<IntParameterRow>();
    }

    [TestMethod]
    public void New_EmptyIndexedColumnsRow()
    {
        NewThrowArgumentException<EmptyIndexedColumnsRow>();
    }

    [TestMethod]
    public void New_TwoOrMoreConstructorRow()
    {
        NewThrowArgumentException<TwoOrMoreConstructorRow>();
    }

    [TestMethod]
    public void New_PrimaryAndNonPublicConstructorsRow()
    {
        var m = new Metadata<PrimaryAndNonPublicConstructorsRow>();
        CheckFooTable(m);
    }

    [TestMethod]
    public void New_PrimaryAndIgnoredConstructorsRow()
    {
        var m = new Metadata<PrimaryAndIgnoredConstructorsRow>();
        CheckFooTable(m);
    }

    [TestMethod]
    public void New_NoPublicConstructorRow()
    {
        NewThrowArgumentException<NoPublicConstructorRow>();
    }

    [TestMethod]
    public void New_DefaultConstructorRow()
    {
        NewThrowArgumentException<DefaultConstructorRow>();
    }

    [TestMethod]
    public void New_ParameterMissingColumnAttributeRow()
    {
        NewThrowArgumentException<ParameterMissingColumnAttributeRow>();
    }

    [TestMethod]
    public void New_NoTableAttributeRow()
    {
        NewThrowArgumentException<NoTableAttributeRow>();
    }

    [TestMethod]
    public void NewSelectStatement()
    {
        var m = new Metadata<CoffeeRow>();
        Assert.ThrowsException<ArgumentException>(
            () => _ = m.NewSelectStatement("price"));
    }

    [TestMethod]
    public void NewInsertParameterMap_NullValue()
    {
        var row = new NullablePropertyRow(1, null);
        var m = new Metadata<NullablePropertyRow>();
        Assert.ThrowsException<ArgumentException>(
            () => _ = m.NewInsertParameterMap(row));
    }

    [TestMethod]
    public void ToColumnName_ParameterNameNotFound()
    {
        var m = new Metadata<CoffeeRow>();
        Assert.ThrowsException<ArgumentException>(
            () => _ = m.ToColumnName("Price"));
    }

    [TestMethod]
    public void ToColumnName()
    {
        var m = new Metadata<CoffeeRow>();
        Assert.AreEqual("name", m.ToColumnName(nameof(CoffeeRow.Name)));
        Assert.AreEqual("country", m.ToColumnName(nameof(CoffeeRow.Country)));
    }

    private static void NewThrowArgumentException<T>()
        where T : notnull
    {
        Assert.ThrowsException<ArgumentException>(() => _ = new Metadata<T>());
    }

    private static void CheckFooTable<T>(Metadata<T> m)
        where T : notnull
    {
        Assert.AreEqual("foo", m.TableName);
        var allFields = m.Fields.ToArray();
        Assert.AreEqual(2, allFields.Length);
        Assert.AreEqual("id", allFields[0].ColumnName);
        Assert.AreEqual("Id", allFields[0].ParameterName);
        Assert.AreEqual("value", allFields[1].ColumnName);
        Assert.AreEqual("Value", allFields[1].ParameterName);
    }
}
