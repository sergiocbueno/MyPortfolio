using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyPortfolio.Database.Models;

namespace MyPortfolio.Database.Configurations
{
    public class AccessMapConfiguration : IEntityTypeConfiguration<AccessMap>
    {
        public void Configure(EntityTypeBuilder<AccessMap> builder)
        {
            builder
                .ToTable(nameof(AccessMap))
                .HasKey(entity => entity.Id);

            builder
                .Property(entity => entity.City)
                .HasMaxLength(100);
        }
    }
}