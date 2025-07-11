using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Infrastructure.Data;

public class Touhou1CCTrackerDbContext(IConfiguration configuration) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
         optionsBuilder.UseNpgsql(configuration.GetConnectionString(nameof(Touhou1CCTrackerDbContext)));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Touhou1CCTrackerDbContext).Assembly);
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Difficulty> Difficulties { get; set; }
    public DbSet<ShotType> ShotTypes { get; set; }
    public DbSet<ReplayFile> ReplayFiles { get; set; }
    public DbSet<Record> Records { get; set; }
}