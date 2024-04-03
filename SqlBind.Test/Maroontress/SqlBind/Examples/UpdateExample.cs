namespace Maroontress.SqlBind.Examples;

public sealed class UpdateExample
{
    private TransactionKit Kit { get; } = new TransactionKit(
        "update_example.db",
        m => Console.WriteLine(m()));

    public void CreateTable()
    {
        var issueTitles = new[]
        {
            "Does not cover all SQLite grammars",
            "Cannot execute Update statement",
            "Cannot see the SQL statement that was executed when the "
                + "exception was thrown",
        };
        var all = issueTitles.Select(t => new Issue(0, t, "Open"));

        Kit.Execute(q =>
        {
            q.NewTables(typeof(Issue));
            foreach (var i in all)
            {
                q.Insert(i);
            }
        });
    }

    public void SelectAllRows()
    {
        Kit.Execute(q =>
        {
            var all = q.SelectAll<Issue>();
            foreach (var i in all)
            {
                Console.WriteLine(i);
            }
        });
    }

    public void UpdateState()
    {
        var map = new Dictionary<string, object>()
        {
            ["$id"] = 2L,
            ["$newState"] = "Closed",
        };
        Kit.Execute(q =>
        {
            q.Update<Issue>("i")
                .Set("state = $newState")
                .Where("i.id = $id")
                .Execute(map);
        });
    }
}
