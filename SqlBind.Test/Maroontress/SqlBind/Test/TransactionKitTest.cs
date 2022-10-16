namespace Maroontress.SqlBind.Test;

using System.Collections.Immutable;
using Maroontress.SqlBind.Impl;
using StyleChecker.Annotations;

[TestClass]
public sealed class TransactionKitTest
{
    private static ImmutableArray<string> ExpectedCommitTrace { get; }
        = ImmutableArray.Create(
            "decoy.db",
            "DatabaseLink#BeginTransaction",
            "DatabaseLink#NewSiphon",
            "Committable#Commit",
            "Committable#Dispose",
            "DatabaseLink#Dispose");

    private static ImmutableArray<string> ExpectedRollbackTrace { get; }
        = ImmutableArray.Create(
            "decoy.db",
            "DatabaseLink#BeginTransaction",
            "DatabaseLink#NewSiphon",
            "Committable#Rollback",
            "Committable#Dispose",
            "DatabaseLink#Dispose");

    private List<string> Trace { get; } = new();

    [TestInitialize]
    public void Initialize()
    {
        Toolkit.Instance = new DecoyToolkit(s =>
        {
            Trace.Add(s);
            return new DecoyDatabaseLink(Trace);
        });
    }

    [TestCleanup]
    public void Cleanup()
    {
        Toolkit.Instance = new DefaultToolkit();
    }

    [TestMethod]
    public void Execute_Action_Commit()
    {
        Trace.Clear();
        var kit = new TransactionKit("decoy.db", m => {});
        kit.Execute(q => {});
        CollectionAssert.AreEqual(ExpectedCommitTrace, Trace);
    }

    [TestMethod]
    public void Execute_Action_Rollback()
    {
        Trace.Clear();
        var kit = new TransactionKit("decoy.db", m => {});
        static void Function([Unused] Query q)
        {
            throw new Exception("!");
        }
        Assert.ThrowsException<Exception>(
            () => kit.Execute(Function),
            "!");
        CollectionAssert.AreEqual(ExpectedRollbackTrace, Trace);
    }

    [TestMethod]
    public void Execute_Func_Commit()
    {
        Trace.Clear();
        var kit = new TransactionKit("decoy.db", m => {});
        var result = kit.Execute(q => "foo");
        Assert.IsNotNull(result);
        Assert.AreEqual("foo", result);
        CollectionAssert.AreEqual(ExpectedCommitTrace, Trace);
    }

    [TestMethod]
    public void Execute_Func_Rollback()
    {
        Trace.Clear();
        var kit = new TransactionKit("decoy.db", m => {});
        static string Function([Unused] Query q)
        {
            throw new Exception("!");
        }
        Assert.ThrowsException<Exception>(
            () => _ = kit.Execute(Function),
            "!");
        CollectionAssert.AreEqual(ExpectedRollbackTrace, Trace);
    }
}
