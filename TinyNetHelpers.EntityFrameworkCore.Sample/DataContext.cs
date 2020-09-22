using Microsoft.EntityFrameworkCore;
using System;
using TinyNetHelpers.EntityFrameworkCore.Sample.Configurations;
using TinyNetHelpers.EntityFrameworkCore.Sample.Models;

namespace TinyNetHelpers.EntityFrameworkCore.Sample
{
    public class DataContext : DbContext
    {
        public const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Sample;Integrated Security=True";

        public DataContext()
        {
        }


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
            modelBuilder.ApplyConfiguration(new PostConfiguration());
        }
    }
}
