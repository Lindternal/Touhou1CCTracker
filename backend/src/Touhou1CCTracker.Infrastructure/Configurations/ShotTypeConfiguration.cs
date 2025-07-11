using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Infrastructure.Configurations;

public class ShotTypeConfiguration : IEntityTypeConfiguration<ShotType>
{
    public void Configure(EntityTypeBuilder<ShotType> builder)
    {
        builder.ToTable("ShotTypes").HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.Property(s => s.CharacterName).IsRequired().HasMaxLength(20);
        builder.Property(s => s.ShotName).HasMaxLength(20);
        builder.HasMany(s => s.Records).WithOne(r => r.ShotType);
    }
}