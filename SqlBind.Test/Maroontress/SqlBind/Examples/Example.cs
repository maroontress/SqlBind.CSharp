namespace Maroontress.SqlBind.Examples;

public sealed class Example
{
    private TransactionKit Kit { get; } = new TransactionKit(
        "example.db",
        m => Console.WriteLine(m()));

    public void CreateTableAndInsertRows()
    {
        var allActorNames = new[]
        {
            "Chloë Grace Moretz",
            "Gary Carr",
            "Jack Reynor",
        };

        Kit.Execute(q =>
        {
            q.NewTables(typeof(Actor));
            foreach (var i in allActorNames.Select(n => new Actor(0, n)))
            {
                q.Insert(i);
            }
        });
    }

    public void SelectAllRows()
    {
        Kit.Execute(q =>
        {
            var all = q.SelectAll<Actor>();
            foreach (var i in all)
            {
                Console.WriteLine(i);
            }
        });
    }

    public void CreateTables()
    {
        Kit.Execute(q =>
        {
            q.NewTables(typeof(Title));
            q.NewTables(typeof(Actor));
            q.NewTables(typeof(Cast));
            var titleId = q.InsertAndGetRowId(new Title(0, "Peripheral"));
            var allCasts = new (string Name, string Role)[]
            {
                ("Chloë Grace Moretz", "Flynne Fisher"),
                ("Gary Carr", "Wilf Netherton"),
                ("Jack Reynor", "Burton Fisher"),
            };
            foreach (var (name, role) in allCasts)
            {
                var actorId = q.InsertAndGetRowId(new Actor(0, name));
                q.Insert(new Cast(0, titleId, actorId, role));
            }
        });
    }

    public void ListActorNames(string title)
    {
        Kit.Execute(q =>
        {
            var map = new Dictionary<string, object>
            {
                ["$name"] = title,
            };
            var all = q.SelectAllFrom<Actor>("a")
                .InnerJoin<Cast>("c", "a.id = c.actorId")
                .InnerJoin<Title>("t", "t.id = c.titleId")
                .Where("t.name = $name", map)
                .Execute();
            foreach (var i in all)
            {
                Console.WriteLine(i.Name);
            }
        });
    }

    public void ListActorNamesV2(string title)
    {
        Kit.Execute(q =>
        {
            var map = new Dictionary<string, object>
            {
                ["$name"] = title,
            };
            var cActorId = q.ColumnName<Cast>(nameof(Cast.ActorId));
            var cTitleId = q.ColumnName<Cast>(nameof(Cast.TitleId));
            var aId = q.ColumnName<Actor>(nameof(Actor.Id));
            var tId = q.ColumnName<Title>(nameof(Title.Id));
            var tName = q.ColumnName<Title>(nameof(Title.Name));
            var all = q.SelectAllFrom<Actor>("a")
                .InnerJoin<Cast>("c", $"a.{aId} = c.{cActorId}")
                .InnerJoin<Title>("t", $"t.{tId} = c.{cTitleId}")
                .Where($"t.{tName} = $name", map)
                .Execute();
            foreach (var i in all)
            {
                Console.WriteLine(i.Name);
            }
        });
    }
}
