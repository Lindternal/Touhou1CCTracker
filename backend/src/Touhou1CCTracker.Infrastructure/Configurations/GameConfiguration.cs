using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Infrastructure.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games").HasKey(g => g.Id);
        builder.Property(g => g.Id).ValueGeneratedOnAdd();
        builder.Property(g => g.Name).IsRequired().HasMaxLength(50);
        builder.HasMany(g => g.Records).WithOne(r => r.Game);
    }
}