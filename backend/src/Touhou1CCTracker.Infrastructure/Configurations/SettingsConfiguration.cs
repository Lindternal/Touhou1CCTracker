using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Infrastructure.Configurations;

public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
{
    public void Configure(EntityTypeBuilder<Settings> builder)
    {
        builder.ToTable("Settings").HasKey(s => s.Id);
        
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        
        builder.Property(s => s.SettingName).IsRequired();
        
        builder.Property(s => s.SettingValue).IsRequired();
        
        builder.HasIndex(s => s.SettingName).IsUnique();
    }
}