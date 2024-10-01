using Microsoft.EntityFrameworkCore;
using utgiftsoversikt.Models;

namespace utgiftsoversikt.Data;

public class CosmosContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User>? Users { get; set; }
    public DbSet<Expense>? Expenses { get; set; }
    public DbSet<Month>? Month { get; set; }
    public DbSet<Budget>? Budget { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToContainer("Users") // To container
            .HasPartitionKey(u => u.Id); // Partition key

        modelBuilder.Entity<Expense>()
            .ToContainer("Expenses") // To container
            .HasPartitionKey(e => e.Id); // Partition key

        modelBuilder.Entity<Budget>()
            .ToContainer("Budgets") // To container
            .HasPartitionKey(b => b.Id); // Partition key

        modelBuilder.Entity<Month>()
            .ToContainer("Months") // To container
            .HasPartitionKey(m => m.Id); // Partition key
    }
}
