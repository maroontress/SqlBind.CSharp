namespace Maroontress.SqlBind.Impl.Test;

using Maroontress.SqlBind.Test;

[TestClass]
public sealed class FieldTest
{
    [TestMethod]
    public void New_TypeMismatchRow()
    {
        var t = typeof(CoffeeRow);
        var ctor = t.GetConstructor(
        [
            typeof(string),
            typeof(string),
        ]);
        var all = ctor!.GetParameters();
        var country = all[1];

        Assert.ThrowsException<ArgumentException>(
            () => _ = new Field<PersonRow>(country));
    }
}
