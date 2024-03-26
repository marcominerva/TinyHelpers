using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Sample;
using TinyHelpers.EntityFrameworkCore.Sample.Entities;

using var dataContext = new DataContext();

var post = new Post
{
    Title = "TinyHelpers",
    Content = "A collection of helper methods and classes that I use every day. I have packed them in a single library to avoid code duplication.",
    Authors = ["Marco", "Bot"],
    Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
    Reviews =
    [
        new() { User = "Pippo", Score = 5, Time = TimeOnly.FromDateTime(DateTime.Now) },
        new() { User = "Pluto", Score = 4 }
    ]
};

dataContext.Posts.Add(post);
await dataContext.SaveChangesAsync();

var posts = await dataContext.Posts.Where(p => p.Authors.Contains("Marco")).ToListAsync();

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