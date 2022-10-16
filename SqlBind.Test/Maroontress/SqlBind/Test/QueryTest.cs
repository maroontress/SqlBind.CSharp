namespace Maroontress.SqlBind.Test;

using System.Collections.Generic;
using System.Collections.Immutable;
using Maroontress.SqlBind.Impl;

[TestClass]
public sealed class QueryTest
{
    private static readonly CoffeeRow Bolivia
        = new("SOL DE LA MA\x00d1ANA", "BOLIVIA");

    private static readonly CoffeeRow Zambia = new("ISANYA ESTATE", "ZAMBIA");

    [TestMethod]
    public void NewTablesWithoutIndex()
    {
        var cache = new MetadataBank();
        var siphon = new TestSiphon();
        var q = new QueryImpl(siphon, cache);
        q.NewTables(new[] { typeof(CoffeeRow) });
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
        q.NewTables(new[] { typeof(PersonRow) });
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
        var siphon = new TestSiphon(Array.Empty<Reservoir>());
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
    public void SelectAll()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir(new[]
        {
            Bolivia,
            Zambia,
        });
        var siphon = new TestSiphon(new[] { reservoir });
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
        var reservoir = new TestReservoir(new[]
        {
            Zambia,
        });
        var siphon = new TestSiphon(new[] { reservoir });
        var q = new QueryImpl(siphon, cache);
        var parameters = ImmutableDictionary.CreateRange(
            ImmutableArray.Create(
                KeyValuePair.Create("$country", (object)Zambia.Name)));
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .Where("c.country = $country", parameters)
            .Execute()
            .ToArray();
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
        Assert.AreEqual(1, all.Length);
        Assert.AreSame(Zambia, all[0]);
    }

    [TestMethod]
    public void SelectAllFrom_Where_OrderBy()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir(new[]
        {
            Zambia,
        });
        var siphon = new TestSiphon(new[] { reservoir });
        var q = new QueryImpl(siphon, cache);
        var parameters = ImmutableDictionary.CreateRange(
            ImmutableArray.Create(
                KeyValuePair.Create("$country", (object)Zambia.Name)));
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .Where("c.country = $country", parameters)
            .OrderBy("c.country")
            .ToArray();
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
        Assert.AreEqual(1, all.Length);
        Assert.AreSame(Zambia, all[0]);
    }

    [TestMethod]
    public void SelectAllFrom_Execute()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir(new[]
        {
            Zambia,
            Bolivia,
        });
        var siphon = new TestSiphon(new[] { reservoir });
        var q = new QueryImpl(siphon, cache);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .Execute()
            .ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNull(p);
        Assert.AreEqual(2, all.Length);
        Assert.AreSame(Zambia, all[0]);
        Assert.AreSame(Bolivia, all[1]);
    }

    [TestMethod]
    public void SelectAllFrom_OrderBy()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir(new[]
        {
            Zambia,
            Bolivia,
        });
        var siphon = new TestSiphon(new[] { reservoir });
        var q = new QueryImpl(siphon, cache);
        var all = q.SelectAllFrom<CoffeeRow>("c")
            .OrderBy("c.name")
            .ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.name, c.country FROM coffees c ORDER BY c.name",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNull(p);
        Assert.AreEqual(2, all.Length);
        Assert.AreSame(Zambia, all[0]);
        Assert.AreSame(Bolivia, all[1]);
    }

    [TestMethod]
    public void SelectAllFrom_InnerJoin_Where()
    {
        var personRow = new PersonRow(1, 2, 3);
        var cache = new MetadataBank();
        var reservoir = new TestReservoir(new[]
        {
            personRow,
        });
        var siphon = new TestSiphon(new[] { reservoir });
        var q = new QueryImpl(siphon, cache);
        var parameters = ImmutableDictionary.CreateRange(
            ImmutableArray.Create(
                KeyValuePair.Create("$firstName", (object)"Yuri")));
        var all = q.SelectAllFrom<PersonRow>("p")
            .InnerJoin<StringRow>("s", "s.id = p.firstNameId")
            .Where("s.value = $firstName", parameters)
            .Execute()
            .ToArray();
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
        Assert.AreEqual(1, all.Length);
        Assert.AreSame(personRow, all[0]);
    }

    [TestMethod]
    public void Select()
    {
        var cache = new MetadataBank();
        var reservoir = new TestReservoir(
            new[]
            {
                Zambia,
                Bolivia,
            }.Select(i => new StringView(i.Name)));
        var siphon = new TestSiphon(new[] { reservoir });
        var q = new QueryImpl(siphon, cache);
        var all = q.Select<StringView>("c.country")
            .From<CoffeeRow>("c")
            .OrderBy("c.name")
            .ToArray();
        var result = siphon.StatementList;
        var resultTexts = result.Select(i => i.Text).ToArray();
        var expectedTexts = new[]
        {
            "SELECT c.country FROM coffees c ORDER BY c.name",
        };
        CollectionAssert.AreEqual(expectedTexts, resultTexts);
        var p = result[0].Parameters;
        Assert.IsNull(p);
        Assert.AreEqual(2, all.Length);
        Assert.AreEqual(all[0].Value, Zambia.Name);
        Assert.AreEqual(all[1].Value, Bolivia.Name);
    }

    [TestMethod]
    public void DeleteFrom()
    {
        var cache = new MetadataBank();
        var siphon = new TestSiphon();
        var parameters = ImmutableDictionary.CreateRange(
            ImmutableArray.Create(
                KeyValuePair.Create("$country", (object)Bolivia.Name)));
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
        var siphon = new TestSiphon(Array.Empty<Reservoir>());
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
        var reservoir = new TestReservoir(
            new[]
            {
                Zambia,
            });
        var siphon = new TestSiphon(new[] { reservoir });
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
        var siphon = new TestSiphon(new[] { reservoir });
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

    public sealed class TestReservoir : Reservoir
    {
        public TestReservoir()
            : this(Array.Empty<object>())
        {
        }

        public TestReservoir(IEnumerable<object> list)
        {
            InstanceQueue = new Queue<object>(list);
            Current = null;
        }

        private Queue<object> InstanceQueue { get; }

        private object? Current { get; set; }

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

    public sealed class TestSiphon : Siphon
    {
        private long scalar = 0;

        public TestSiphon()
            : this(Array.Empty<Reservoir>())
        {
        }

        public TestSiphon(IEnumerable<Reservoir> reservoirs)
        {
            Reservoirs = reservoirs.GetEnumerator();
        }

        public List<Statement> StatementList { get; } = new();

        private IEnumerator<Reservoir> Reservoirs { get; }

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

    public sealed class Statement
    {
        public Statement(
            string text,
            IReadOnlyDictionary<string, object>? parameters)
        {
            Text = text;
            Parameters = parameters?.ToImmutableDictionary();
        }

        public string Text { get; }

        public IReadOnlyDictionary<string, object>? Parameters { get; }
    }
}
