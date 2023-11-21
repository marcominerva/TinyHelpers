using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Sample.Entities;

namespace TinyHelpers.EntityFrameworkCore.Sample;

public class DataContext : DbContext
{
    public const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=SampleDB;Integrated Security=True";

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
    {
        modelBuilder.Entity<Post>(builder =>
        {
            builder.Property(x => x.Title).HasMaxLength(80).IsRequired();
            builder.Property(x => x.Content).IsRequired();

            // Date is a DateOnly property (.NET 6 or 7).
            //builder.Property(x => x.Date).HasDateOnlyConversion();

            // Time is a TimeOnly property (.NET 6 or 7).
            //builder.Property(x => x.Time).HasTimeOnlyConversion();

            /* JSON SUPPORT */

            // For .NET 6:
            // Reviews is a complex type, this Converter will automatically JSON-de/serialize it
            // in a string column.
            // builder.Property(x => x.Reviews).HasJsonConversion();

            // For .NET 7 or higher:
            builder.OwnsMany(x => x.Reviews).ToJson();

            /* COLLECTION OF PRIMITIVE TYPES */

            // For .NET 6 and 7:
            //builder.Property(x => x.Authors).HasArrayConversion();

            // For .NET 8
            // The support for collection of primitive types is built-in.
        });
    }
}
