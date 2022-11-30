using System;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using TinyHelpers.Dapper.TypeHandlers;

const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=MinimalDB;Integrated Security=True";

using var connection = new SqlConnection(ConnectionString);
DateOnlyTypeHandler.Configure();
TimeOnlyTypeHandler.Configure();

var data = await connection.QueryAsync<Sample>("SELECT * FROM Sample");
Console.WriteLine(data);

var d = data.FirstOrDefault();
var d2 = d with { Id = 2 };

await connection.ExecuteAsync("INSERT INTO Sample(Id, Date, Time) VALUES(@Id, @Date, @Time)", d2);

internal record class Sample(int Id, DateOnly Date, TimeOnly Time);

//JsonTypeHandler<IList<Review>>.Configure();
//StringEnumerableTypeHandler.Configure();

//var posts = await connection.QueryAsync<Post>("SELECT Id, Title, Content, Date, Authors, Reviews FROM Posts");

//var posts2 = await connection.QueryAsync<Post>("SELECT * FROM Posts WHERE Authors LIKE @authors", new { Authors = $"%Marco%" });

//var post = new Post
//{
//    Id = Guid.NewGuid(),
//    Title = "TinyHelpers3",
//    Content = "New Description",
//    Authors = new string[] { "Andrea", "Calogero" },
//    Date = DateTime.UtcNow
//};

//await connection.ExecuteAsync("INSERT INTO Posts(Id, Title, Content, Date, Authors, Reviews) VALUES(@Id, @Title, @Content, @Date, @Authors, @Reviews)",
//    post);
