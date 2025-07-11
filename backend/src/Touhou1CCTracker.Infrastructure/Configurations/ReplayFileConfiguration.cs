using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Infrastructure.Configurations;

public class ReplayFileConfiguration : IEntityTypeConfiguration<ReplayFile>
{
    public void Configure(EntityTypeBuilder<ReplayFile> builder)
    {
        builder.ToTable("ReplayFiles").HasKey(f => f.Id);
        
        builder.Property(f => f.Id).ValueGeneratedOnAdd();
        
        builder.Property(f => f.Name).HasMaxLength(100).IsRequired();
        
        builder.Property(f => f.Path).HasMaxLength(120).IsRequired();
        
        builder.Property(f => f.Size).IsRequired();
        
        builder.HasOne(f => f.Record)
            .WithOne(r => r.ReplayFile)
            .OnDelete(DeleteBehavior.Cascade);
    }
}