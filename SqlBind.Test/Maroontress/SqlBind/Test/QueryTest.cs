namespace Maroontress.SqlBind.Test;

using System.Collections.Generic;
using System.Collections.Immutable;
using Maroontress.SqlBind.Impl;

[TestClass]
public sealed class QueryTest
{
    private static CoffeeRow Bolivia { get; }
        = new("SOL DE LA MA\x00d1ANA", "BOLIVIA");

    private static CoffeeRow Zambia { get; }
        = new("ISANYA ESTATE", "ZAMBIA");

    private static CoffeeRow CostaRicaTresRios { get; }
        = new("TRES RIOS", "COSTA RICA");

    private static CoffeeRow SunDriedCostaRica { get; }
        = new("HACIENDA ALSACIA", "COSTA RICA");

    [TestMethod]
    public void NewTablesWithoutIndex()
    {
        var cache = new MetadataBank();
        var siphon = new TestSiphon();
        var q = new QueryImpl(siphon, cache);
        q.NewTables([typeof(CoffeeRow)]);
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "DROP TABLE IF EXISTS coffees",
            "CREATE TABLE coffees (name TEXT, country TEXT)",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        Assert.IsTrue(result.All(i => i.Parameters is null));
    }

    [TestMethod]
    public void NewTablesWithIndices()
    {
        var cache = new MetadataBank();
        var siphon = new TestSiphon();
        var q = new QueryImpl(siphon, cache);
        q.NewTables([typeof(PersonRow)]);
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "DROP TABLE IF EXISTS persons",
            "DROP INDEX IF EXISTS persons_Index_firstNameId",
            "DROP INDEX IF EXISTS persons_Index_lastNameId",
            "CREATE TABLE persons (id INTEGER PRIMARY KEY AUTOINCREMENT, "
                + "firstNameId INTEGER, lastNameId INTEGER)",
            "CREATE INDEX persons_Index_firstNameId on persons (firstNameId)",
            "CREATE INDEX persons_Index_lastNameId on persons (lastNameId)",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        Assert.IsTrue(result.All(i => i.Parameters is null));
    }

    [TestMethod]
    public void Insert()
    {
        var cache = new MetadataBank();
        var siphon = new TestSiphon([]);
        var q = new QueryImpl(siphon, cache);
        q.Insert(Bolivia);
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "INSERT INTO coffees (name, country) VALUES ($name, $country)",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(p["$name"], Bolivia.Name);
        Assert.AreEqual(p["$country"], Bolivia.Country);
    }

    [TestMethod]
    public void Update()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir();
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var map = new Dictionary<string, object>()
        {
            ["$newLastNameId"] = 123L,
            ["$id"] = 456L,
        };
        q.Update<PersonRow>("p").Set("lastNameId = $newLastNameId")
            .Where("p.id = $id")
            .Execute(map);
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "UPDATE persons AS p "
                + "SET lastNameId = $newLastNameId "
                + "WHERE p.id = $id",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(p["$newLastNameId"], 123L);
        Assert.AreEqual(p["$id"], 456L);
    }

    [TestMethod]
    public void SelectAll()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([
            Bolivia,
            Zambia,
        ]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var all = q.SelectAll<CoffeeRow>().ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT name, country FROM coffees",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNull(p);
        Assert.AreEqual(2, all.Length);
        Assert.AreSame(Bolivia, all[0]);
        Assert.AreSame(Zambia, all[1]);
    }

    [TestMethod]
    public void SelectAllFrom_Where_Execute()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([Zambia]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var parameters = ImmutableDictionary.CreateRange(
            [KeyValuePair.Create("$country", (object)Zambia.Name)]);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .Where("c.country = $country", parameters)
            .Execute();
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c "
                + "WHERE c.country = $country",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(1, array.Length);
        Assert.AreSame(Zambia, array[0]);
    }

    [TestMethod]
    public void SelectAllFrom_Where_OrderBy_Execute()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([Zambia]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var parameters = ImmutableDictionary.CreateRange(
            [KeyValuePair.Create("$country", (object)Zambia.Name)]);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .Where("c.country = $country", parameters)
            .OrderBy("c.country")
            .Execute();
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c "
                + "WHERE c.country = $country "
                + "ORDER BY c.country",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(1, array.Length);
        Assert.AreSame(Zambia, array[0]);
    }

    [TestMethod]
    public void SelectAllFrom_Where_OrderBy_Limit()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([
            SunDriedCostaRica,
            CostaRicaTresRios,
        ]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var parameters = ImmutableDictionary.CreateRange(
            [KeyValuePair.Create("$country", (object)"COSTA RICA")]);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .Where("c.country = $country", parameters)
            .OrderBy("c.name")
            .Limit(2);
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c "
                + "WHERE c.country = $country "
                + "ORDER BY c.name "
                + "LIMIT 2",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(2, array.Length);
        Assert.AreSame(SunDriedCostaRica, array[0]);
        Assert.AreSame(CostaRicaTresRios, array[1]);
    }

    [TestMethod]
    public void SelectAllFrom_Where_OrderBy_LimitOffset()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([
            SunDriedCostaRica,
            CostaRicaTresRios,
        ]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var parameters = ImmutableDictionary.CreateRange(
            [KeyValuePair.Create("$country", (object)"COSTA RICA")]);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .Where("c.country = $country", parameters)
            .OrderBy("c.name")
            .LimitOffset(2, 3);
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c "
                + "WHERE c.country = $country "
                + "ORDER BY c.name "
                + "LIMIT 2 OFFSET 3",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(2, array.Length);
        Assert.AreSame(SunDriedCostaRica, array[0]);
        Assert.AreSame(CostaRicaTresRios, array[1]);
    }

    [TestMethod]
    public void SelectAllFrom_Execute()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([
            Zambia,
            Bolivia,
        ]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .Execute();
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNull(p);
        Assert.AreEqual(2, array.Length);
        Assert.AreSame(Zambia, array[0]);
        Assert.AreSame(Bolivia, array[1]);
    }

    [TestMethod]
    public void SelectAllFrom_Limit()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([
            Zambia,
            Bolivia,
        ]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .Limit(2);
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c LIMIT 2",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNull(p);
        Assert.AreEqual(2, array.Length);
        Assert.AreSame(Zambia, array[0]);
        Assert.AreSame(Bolivia, array[1]);
    }

    [TestMethod]
    public void SelectAllFrom_LimitOffset()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([
            Zambia,
            Bolivia,
        ]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .LimitOffset(2, 3);
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c LIMIT 2 OFFSET 3",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNull(p);
        Assert.AreEqual(2, array.Length);
        Assert.AreSame(Zambia, array[0]);
        Assert.AreSame(Bolivia, array[1]);
    }

    [TestMethod]
    public void SelectAllFrom_OrderBy_Execute()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([
            Zambia,
            Bolivia,
        ]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .OrderBy("c.name")
            .Execute();
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c ORDER BY c.name",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNull(p);
        Assert.AreEqual(2, array.Length);
        Assert.AreSame(Zambia, array[0]);
        Assert.AreSame(Bolivia, array[1]);
    }

    [TestMethod]
    public void SelectAllFrom_InnerJoin_Where_Execute()
    {
        var personRow = new PersonRow(1, 2, 3);
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([personRow]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var parameters = ImmutableDictionary.CreateRange(
            [KeyValuePair.Create("$firstName", (object)"Yuri")]);
        var all = q.SelectAllFrom<PersonRow>("p")
            .InnerJoin<StringRow>("s", "s.id = p.firstNameId")
            .Where("s.value = $firstName", parameters)
            .Execute();
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT p.id, p.firstNameId, p.lastNameId FROM persons p "
                + "INNER JOIN strings s ON s.id = p.firstNameId "
                + "WHERE s.value = $firstName",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreSame("Yuri", p["$firstName"]);
        Assert.AreEqual(1, array.Length);
        Assert.AreSame(personRow, array[0]);
    }

    [TestMethod]
    public void Select()
    {
        var cache = new MetadataBank();
        var rows = new[]
        {
            Zambia,
            Bolivia,
        };
        var reservoir = new TestReservoir(
            rows.Select(i => new StringView(i.Name)));
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var all = q.Select<StringView>("c.country")
            .From<CoffeeRow>("c")
            .OrderBy("c.name")
            .Execute();
        var array = all.ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.country FROM coffees c ORDER BY c.name",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNull(p);
        Assert.AreEqual(2, array.Length);
        Assert.AreEqual(array[0].Value, Zambia.Name);
        Assert.AreEqual(array[1].Value, Bolivia.Name);
    }

    [TestMethod]
    public void DeleteFrom()
    {
        var cache = new MetadataBank();
        var siphon = new TestSiphon();
        var parameters = ImmutableDictionary.CreateRange(
            [KeyValuePair.Create("$country", (object)Bolivia.Name)]);
        var q = new QueryImpl(siphon, cache);
        q.DeleteFrom<CoffeeRow>()
            .Where("country = $country", parameters);
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "DELETE FROM coffees WHERE country = $country",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(p["$country"], Bolivia.Name);
    }

    [TestMethod]
    public void InsertAndGetRowId()
    {
        var cache = new MetadataBank();
        var siphon = new TestSiphon([]);
        var q = new QueryImpl(siphon, cache);
        var row = new PersonRow(0, 1, 2);
        var id = q.InsertAndGetRowId(row);
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "INSERT INTO persons (firstNameId, lastNameId) VALUES "
                + "($firstNameId, $lastNameId)",
            "select last_insert_rowid()",
        };
        Assert.AreEqual(id, 1);
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(1L, p["$firstNameId"]);
        Assert.AreEqual(2L, p["$lastNameId"]);
    }

    [TestMethod]
    public void SelectUnique()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir([Zambia]);
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var maybeRow = q.SelectUnique<CoffeeRow>("name", Zambia.Name);
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT name, country FROM coffees WHERE name = $name",
        };
        Assert.IsNotNull(maybeRow);
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(p["$name"], Zambia.Name);
        Assert.AreEqual(Zambia, maybeRow);
    }

    [TestMethod]
    public void SelectUniqueButNotFound()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir();
        var siphon = new TestSiphon([reservoir]);
        var q = new QueryImpl(siphon, cache);
        var maybeRow = q.SelectUnique<CoffeeRow>("name", Zambia.Name);
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT name, country FROM coffees WHERE name = $name",
        };
        Assert.IsNull(maybeRow);
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNotNull(p);
        Assert.AreEqual(p["$name"], Zambia.Name);
    }

    public sealed class TestReservoir(IEnumerable<object> list)
        : Reservoir
    {
        public TestReservoir()
            : this([])
        {
        }

        private Queue<object> InstanceQueue { get; } = new(list);

        private object? Current { get; set; } = null;

        public void Dispose()
        {
        }

        public T NewInstance<T>()
        {
            if (Current is null)
            {
                throw new NullReferenceException();
            }
            if (Current is not T)
            {
                throw new InvalidOperationException();
            }
            return (T)Current;
        }

        public IEnumerable<T> NewInstances<T>()
        {
            while (Read())
            {
                yield return NewInstance<T>();
            }
        }

        public bool Read()
        {
            if (!InstanceQueue.TryDequeue(out var instance))
            {
                return false;
            }
            Current = instance;
            return true;
        }
    }

    public sealed class TestSiphon(IEnumerable<Reservoir> reservoirs)
        : Siphon
    {
        private long scalar = 0;

        public TestSiphon()
            : this([])
        {
        }

        public List<Statement> StatementList { get; } = [];

        private IEnumerator<Reservoir> Reservoirs { get; }
            = reservoirs.GetEnumerator();

        public long ExecuteLong(
            string text,
            IReadOnlyDictionary<string, object>? parameters = null)
        {
            StatementList.Add(new Statement(text, parameters));
            return ++scalar;
        }

        public void ExecuteNonQuery(
            string text,
            IReadOnlyDictionary<string, object>? parameters = null)
        {
            StatementList.Add(new Statement(text, parameters));
        }

        public Reservoir ExecuteReader(
            string text,
            IReadOnlyDictionary<string, object>? parameters)
        {
            StatementList.Add(new Statement(text, parameters));
            if (!Reservoirs.MoveNext())
            {
                throw new InvalidOperationException("invalid reservoirs");
            }
            return Reservoirs.Current;
        }
    }

    public sealed class Statement(
        string text,
        IReadOnlyDictionary<string, object>? parameters)
    {
        public string Text { get; } = text;

        public IReadOnlyDictionary<string, object>? Parameters { get; }
            = parameters?.ToImmutableDictionary();
    }
}
