namespace TinyHelpers.EntityFrameworkCore.Sample.Entities;

public class Post
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public IEnumerable<string> Authors { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public IList<Review> Reviews { get; set; }
}

public class Review
{
    public string User { get; set; }

    public DateTime Date { get; set; }

    public int Score { get; set; }
}
