using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace TinyHelpers.EntityFrameworkCore.EnhancedDbContexts;

public class SqlServerDbContext : DbContext {
  private string? connectionString;

  public SqlServerDbContext(string? connectionString) {
    this.connectionString = connectionString;
    
    try {
      Database.EnsureCreated();
    } catch (Exception ex) {
      throw new SqlException("An error occurred when connecting to the database.", ex);
    }
  }
  
  public SqlServerDbContext() : this(null) { }
  
  protected virtual void OnConnected(DbContextOptionsBuilder optionsBuilder) { }
  
  protected sealed override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    if (connectionString != null) optionsBuilder.UseSqlServer(connectionString);
    
    OnConnected(optionsBuilder);
  }
}
