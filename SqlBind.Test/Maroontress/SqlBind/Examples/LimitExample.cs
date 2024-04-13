namespace Maroontress.SqlBind.Examples;

public sealed class LimitExample
{
    private TransactionKit Kit { get; } = new TransactionKit(
        "limit_example.db",
        m => Console.WriteLine(m()));

    public void CreateTable()
    {
        var bookTitles = new[]
        {
            "The Great Gatsby",
            "To Kill a Mockingbird",
            "1984",
            "Pride and Prejudice",
            "The Catcher in the Rye",
            "The Hobbit",
            "Fahrenheit",
            "The Lord of the Rings",
            "Animal Farm",
            "Brave New World",
        };
        var all = bookTitles.Select(t => new Book(0, t));

        Kit.Execute(q =>
        {
            q.NewTables(typeof(Book));
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
            var all = q.SelectAll<Book>();
            foreach (var i in all)
            {
                Console.WriteLine(i);
            }
        });
    }

    public void Limit()
    {
        Kit.Execute(q =>
        {
            var topThree = q.SelectAllFrom<Book>("b")
                .Limit(3);
            foreach (var i in topThree)
            {
                Console.WriteLine(i);
            }
        });
    }

    public void LimitOffset()
    {
        Kit.Execute(q =>
        {
            var nextFive = q.SelectAllFrom<Book>("b")
                .LimitOffset(5, 3);
            foreach (var i in nextFive)
            {
                Console.WriteLine(i);
            }
        });
    }
}
