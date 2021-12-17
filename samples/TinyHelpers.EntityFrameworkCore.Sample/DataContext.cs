using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Sample.Entities;

namespace TinyHelpers.EntityFrameworkCore.Sample;

public class DataContext : DbContext
{
    public const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=SampleDatabase;Integrated Security=True";

    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionString, options =>
        {
            options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null);
        });

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
