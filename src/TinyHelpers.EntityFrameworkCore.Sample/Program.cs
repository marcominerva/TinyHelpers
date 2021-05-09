using Microsoft.EntityFrameworkCore;
using System.Linq;
using TinyHelpers.EntityFrameworkCore.Extensions;
using TinyHelpers.EntityFrameworkCore.Sample;

using var dataContext = new DataContext();

//var post = new Post
//{
//    Title = "TinyHelpers",
//    Content = "A collection of helper methods and classes that I use everyday. I have packed them in a single library to avoid code duplication.",
//    Authors = new string[] { "Marco", "Bot" },
//    Date = DateTime.UtcNow,
//    Reviews = new List<Review>
//    {
//        new Review { User = "Pippo", Date = DateTime.UtcNow, Score = 5 },
//        new Review { User = "Pluto", Date = DateTime.UtcNow, Score = 4 }
//    }
//};

//dataContext.Posts.Add(post);

//await dataContext.SaveChangesAsync();

await dataContext.ExecuteTransactionAsync(async () =>
{
    var posts = await dataContext.Posts.ToListAsync();
    var post = posts.First();
    post.Reviews.First().User = "Topolino";
    await dataContext.SaveChangesAsync();
});

// Exception
//var posts = await dataContext.Posts.Where(p => p.Reviews.Any())
//    .ToListAsync();