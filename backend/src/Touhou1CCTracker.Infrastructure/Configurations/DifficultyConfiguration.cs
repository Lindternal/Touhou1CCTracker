using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Infrastructure.Configurations;

public class DifficultyConfiguration : IEntityTypeConfiguration<Difficulty>
{
    public void Configure(EntityTypeBuilder<Difficulty> builder)
    {
        builder.ToTable("Difficulties").HasKey(d => d.Id);
        builder.Property(d => d.Id).ValueGeneratedOnAdd();
        builder.Property(d => d.Name).IsRequired().HasMaxLength(20);
        builder.HasMany(d => d.Records).WithOne(r => r.Difficulty);
    }
}