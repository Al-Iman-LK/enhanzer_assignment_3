using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LocationDetail> Location_Details { get; set; }
        public DbSet<PurchaseBillItem> PurchaseBillItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<LocationDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Location_Code).IsRequired();
                entity.Property(e => e.Location_Name).IsRequired();
            });

            modelBuilder.Entity<PurchaseBillItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Item).IsRequired();
                entity.Property(e => e.Batch).IsRequired();
                entity.Property(e => e.StandardCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.StandardPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Discount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalSelling).HasColumnType("decimal(18,2)");
            });
        }
    }
}
