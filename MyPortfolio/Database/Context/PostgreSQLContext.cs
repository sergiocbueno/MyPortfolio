using Microsoft.EntityFrameworkCore;
using MyPortfolio.Database.Models;

namespace PostgreSQLIntegration.Context
{
    public class PostgreSQLContext : DbContext
    {
        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessMap>(entity =>
            {
                entity.ToTable("accessmap");
                entity.Property(e => e.Id)
                      .IsRequired()
                      .HasColumnName("id");
                entity.Property(e => e.City)
                      .IsRequired()
                      .HasColumnName("city");
            });
        }
    }
}
