using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Sample;
using TinyHelpers.EntityFrameworkCore.Sample.Entities;

using var dataContext = new DataContext();

var list = new List<Review>
    {
        new Review { User = "Pippo", Score = 5 },
        new Review { User = "Pluto", Score = 4 }
    };

var posts = await dataContext.Posts.Where(p => p.Reviews == list).ToListAsync();

var post = new Post
{
    Title = "TinyHelpers",
    Content = "A collection of helper methods and classes that I use every day. I have packed them in a single library to avoid code duplication.",
    Authors = new string[] { "Marco", "Bot" },
    Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
    Reviews = new List<Review>
    {
        new Review { User = "Pippo", Score = 5, Time=TimeOnly.FromDateTime(DateTime.Now) },
        new Review { User = "Pluto", Score = 4 }
    }
};

dataContext.Posts.Add(post);
await dataContext.SaveChangesAsync();

Console.ReadLine();

//dataContext.Posts.Add(post);

//await dataContext.SaveChangesAsync();

//await dataContext.ExecuteTransactionAsync(async () =>
//{
//    var posts = await dataContext.Posts.ToListAsync();
//    var post = posts.First();
//    post.Reviews.First().User = "Topolino";
//    await dataContext.SaveChangesAsync();
//});

// Exception
//var posts = await dataContext.Posts.Where(p => p.Reviews.Any())
//    .ToListAsync();