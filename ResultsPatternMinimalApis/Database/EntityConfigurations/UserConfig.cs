using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResultsPatternMinimalApis.Models;

namespace ResultsPatternMinimalApis.Database.EntityConfigurations;

public class UserConfig: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(e => e.Id).HasColumnType("uniqueidentifier");

        builder.Property(o => o.Name)
            .HasMaxLength(256)
            .IsRequired()
            .HasColumnType("nvarchar");

        builder.Property(o => o.Email)
            .HasMaxLength(256)
            .IsRequired()
            .HasColumnType("nvarchar");
    }
}