using App1.Entities;
using Microsoft.EntityFrameworkCore;

namespace App1.Data
{
    public class App1DbContext : DbContext
    {
        public App1DbContext(DbContextOptions<App1DbContext> options)
            : base(options) { }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>(entity =>
            {
                entity.ToTable("Expenses", "dbo");

                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Code)
                      .HasMaxLength(10);

                entity.Property(e => e.Description)
                      .HasMaxLength(200);

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                      .HasColumnType("datetime");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
