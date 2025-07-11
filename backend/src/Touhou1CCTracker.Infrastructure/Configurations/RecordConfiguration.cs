using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Infrastructure.Configurations;

public class RecordConfiguration : IEntityTypeConfiguration<Record>
{
    public void Configure(EntityTypeBuilder<Record> builder)
    {
        builder.ToTable("Records").HasKey(r => r.Id);
        
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        
        builder.Property(r => r.Rank).IsRequired().HasMaxLength(70);
        
        builder.HasOne(r => r.Game)
            .WithMany(g => g.Records)
            .HasForeignKey(r => r.GameId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(r => r.Difficulty)
            .WithMany(d => d.Records)
            .HasForeignKey(r => r.DifficultyId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.HasOne(r => r.ShotType)
            .WithMany(s => s.Records)
            .HasForeignKey(r => r.ShotTypeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(r => r.ReplayFile)
            .WithOne(f => f.Record)
            .HasForeignKey<ReplayFile>(f => f.RecordId);
        
        builder.Property(r => r.Date);
        
        builder.Property(r => r.VideoUrl).HasMaxLength(100);
    }
}