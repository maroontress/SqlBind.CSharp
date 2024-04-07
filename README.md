# SqlBind

SqlBind.CSharp is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rows

Let's consider creating the Actors table as follows:

> ### _Actors_
>
> | <u>id</u> | name |
> | ---: | :--- |
> | 1 | Chloë Grace Moretz |
> | 2 | Gary Carr |
> | 3 | Jack Reynor |

Create the following class to represent this table:

```csharp
[Table("Actors")]
public record class Actor(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("name")] string Name)
{
}
```

Each parameter in the constructor of the `Actor` class corresponds to each column in the Actors table in the same order. The type of each parameter must be either `long` or `string`.

Note that you can implement the `Actor` class without a `record` class. However, the parameter names of the constructor must start with an _uppercase_ letter if you create a regular one according to the naming conventions of the `record` class. This is inconsistent with general naming conventions. Therefore, we recommend that you use `record` classes.

The following code from the `Example` class uses the `Actor` class to create the Actors table and add three rows of data to the table:

```csharp
public sealed class Example
{
    private TransactionKit Kit { get; } = new TransactionKit(
        "example.db",
        m => Console.WriteLine(m()));

    public void CreateTableAndInsertRows()
    {
        Kit.Execute(q =>
        {
            q.NewTables(typeof(Actor));
            q.Insert(new Actor(0, "Chloë Grace Moretz"));
            q.Insert(new Actor(0, "Gary Carr"));
            q.Insert(new Actor(0, "Jack Reynor"));
        });
    }
    ...
```

The `Kit` property has the `TransactionKit` instance, which uses the `example.db` file as a database backend and writes log messages to the console. The `Execute` method executes the queries that the lambda expression of its parameter performs atomically (as a single transaction).

Note that calling the `Insert(object)` method with the `Actor` instance ignores its `Id` property, which is specified with the first parameter of the constructor of the `Actor` class, because it is qualified with the `AutoIncrement` attribute.

The log messages that the `CreateTableAndInsertRows()` method prints to the console are as follows:

```plaintext
DROP TABLE IF EXISTS Actors
CREATE TABLE Actors (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT)
INSERT INTO Actors (name) VALUES ($name)
  ($name, Chloë Grace Moretz)
INSERT INTO Actors (name) VALUES ($name)
  ($name, Gary Carr)
INSERT INTO Actors (name) VALUES ($name)
  ($name, Jack Reynor)
```

The non-indented lines are actual SQL statements that were automatically generated and executed.

## How to select a table and get rows

Then run the `SelectAllRows()` method as follows:

```csharp
public sealed class Example
{
    ...
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
    ...
```

The `SelectAllRows()` method outputs:

```plaintext
SELECT id, name FROM Actors
Actor { Id = 1, Name = Chloë Grace Moretz }
Actor { Id = 2, Name = Gary Carr }
Actor { Id = 3, Name = Jack Reynor }
```

The first line is the log message that the `TransactionKit` instance prints. The `SelectAll<T>()` method generates this statement.

The next three lines are the messages that the `WriteLine(object)` method outputs within the `foreach` block.

## Inner join with two or more tables

Consider the following Titles table:

> ### _Titles_
>
> | id | name |
> | ---: | :--- |
> | 1 | Peripheral  |

And the following Casts table:

> ### _Casts_
>
> | id | titleId | actorId | role |
> | ---: | ---: | ---: | :--- |
> | 1 | 1 | 1 | Flynne Fisher |
> | 2 | 1 | 2 | Wilf Netherton |
> | 3 | 1 | 3 | Burton Fisher |

The classes that correspond to these tables are:

```csharp
[Table("Titles")]
public record class Title(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("name")] string Name)
{
}

[Table("Casts")]
public record class Cast(
    [Column("id")][PrimaryKey][AutoIncrement] long Id,
    [Column("titleId")] long TitleId,
    [Column("actorId")] long ActorId,
    [Column("role")] string Role)
{
}
```

The following code creates the tables and inserts the rows:

```csharp
public sealed class Example
{
    ...
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
    ...
```

The log messages that the `CreateTables()` method prints to the console are as follows:

```
DROP TABLE IF EXISTS Titles
CREATE TABLE Titles (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT)
DROP TABLE IF EXISTS Actors
CREATE TABLE Actors (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT)
DROP TABLE IF EXISTS Casts
CREATE TABLE Casts (id INTEGER PRIMARY KEY AUTOINCREMENT, titleId INTEGER, actorId INTEGER, role TEXT)
INSERT INTO Titles (name) VALUES ($name)
  ($name, Peripheral)
select last_insert_rowid()
INSERT INTO Actors (name) VALUES ($name)
  ($name, Chloë Grace Moretz)
select last_insert_rowid()
INSERT INTO Casts (titleId, actorId, role) VALUES ($titleId, $actorId, $role)
  ($titleId, 1)
  ($role, Flynne Fisher)
  ($actorId, 1)
INSERT INTO Actors (name) VALUES ($name)
  ($name, Gary Carr)
select last_insert_rowid()
INSERT INTO Casts (titleId, actorId, role) VALUES ($titleId, $actorId, $role)
  ($titleId, 1)
  ($role, Wilf Netherton)
  ($actorId, 2)
INSERT INTO Actors (name) VALUES ($name)
  ($name, Jack Reynor)
select last_insert_rowid()
INSERT INTO Casts (titleId, actorId, role) VALUES ($titleId, $actorId, $role)
  ($titleId, 1)
  ($role, Burton Fisher)
  ($actorId, 3)
```

Let's suppose that you would like to get a list of the names of the actors who performed in the specified title. To do this, use the APIs as follows:

```csharp
public sealed class Example
{
    ...
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
    ...

```

Calling `ListActorNames("Peripheral");` results in the following output:

```
SELECT a.id, a.name FROM Actors a INNER JOIN Casts c ON a.id = c.actorId INNER JOIN Titles t ON t.id = c.titleId WHERE t.name = $name
  ($name, Peripheral)
Chloë Grace Moretz
Gary Carr
Jack Reynor
```

<!--
## Get started

SqlBind.CSharp is available as
[the ![NuGet-logo][nuget-logo] NuGet package][nuget-maroontress.sqlbind].
-->

## API Reference

- [Maroontress.SqlBind][apiref-maroontress.sqlbind] namespace

## How to build

### Requirements for build

- Visual Studio 2022 (Version 17.9)
  or [.NET 8.0 SDK (SDK 8.0.203)][dotnet-sdk]

### Build

```plaintext
git clone URL
cd SqlBind.CSharp
dotnet build
```

### Get the test coverage report with Coverlet

Install [ReportGenerator][report-generator] as follows:

```plaintext
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Run all tests and get the report in the file `Coverlet-html/index.html`:

```plaintext
rm -rf MsTestResults
dotnet test --collect:"XPlat Code Coverage" --results-directory MsTestResults \
  && reportgenerator -reports:MsTestResults/*/coverage.cobertura.xml \
    -targetdir:Coverlet-html
```

[report-generator]:
  https://github.com/danielpalme/ReportGenerator
[dotnet-sdk]:
  https://dotnet.microsoft.com/en-us/download
[apiref-maroontress.sqlbind]:
  https://maroontress.github.io/SqlBind-CSharp/api/latest/html/Maroontress.SqlBind.html
[nuget-maroontress.sqlbind]:
  https://www.nuget.org/packages/Maroontress.SqlBind/
[nuget-logo]:
  https://maroontress.github.io/images/NuGet-logo.png
