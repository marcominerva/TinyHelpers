using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyNetHelpers.Dapper.Sample.Models;
using TinyNetHelpers.Dapper.SqlTypeHandlers;

namespace TinyNetHelpers.Dapper.Sample
{
    internal class Program
    {
        public const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Sample;Integrated Security=True";

        private static async Task Main(string[] args)
        {
            using var connection = new SqlConnection(ConnectionString);
            JsonTypeHandler<IList<Review>>.Configure();
            ArrayTypeHandler.Configure();

            var posts = await connection.QueryAsync<Post>("SELECT Id, Title, Content, Date, Authors, Reviews FROM Posts");

            var posts2 = await connection.QueryAsync<Post>("SELECT * FROM Posts WHERE Authors LIKE @authors", new { Authors = $"%Marco%" });

            var post = new Post
            {
                Id = Guid.NewGuid(),
                Title = "TinyNetHelpers3",
                Content = "New Description",
                Authors = new string[] { "Andrea", "Calogero" },
                Date = DateTime.UtcNow
            };

            await connection.ExecuteAsync("INSERT INTO Posts(Id, Title, Content, Date, Authors, Reviews) VALUES(@Id, @Title, @Content, @Date, @Authors, @Reviews)",
                post);
        }
    }
}
