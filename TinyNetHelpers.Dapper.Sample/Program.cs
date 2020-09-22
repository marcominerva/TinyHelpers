using Microsoft.Data.SqlClient;

namespace TinyNetHelpers.Dapper.Sample
{
    internal class Program
    {
        public const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Sample;Integrated Security=True";

        private static void Main(string[] args)
        {
            using var connection = new SqlConnection(ConnectionString);
        }
    }
}
