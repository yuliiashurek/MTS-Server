using Microsoft.EntityFrameworkCore;
using Server.Data.Entities;

namespace Server.Data.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Category> Catgories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<MaterialItem> MaterialItems { get; set; }
        public DbSet<MaterialMovement> MaterialMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MaterialItem>(entity =>
            {
                entity.ToTable("MaterialItems");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.MinimumStock)
                      .HasColumnType("decimal(18,2)");

                entity.HasOne(e => e.Organization)
                      .WithMany()
                      .HasForeignKey(e => e.OrganizationId)
                      .OnDelete(DeleteBehavior.Restrict); // ✅

                entity.HasOne(e => e.MeasurementUnit)
                      .WithMany()
                      .HasForeignKey(e => e.MeasurementUnitId)
                      .OnDelete(DeleteBehavior.Restrict); // ✅

                entity.HasOne(e => e.Category)
                      .WithMany()
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict); // ✅

                entity.HasOne(e => e.Supplier)
                      .WithMany()
                      .HasForeignKey(e => e.SupplierId)
                      .OnDelete(DeleteBehavior.Restrict); // ✅
            });

            modelBuilder.Entity<MaterialMovement>(entity =>
            {
                entity.ToTable("MaterialMovements");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Quantity)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.PricePerUnit)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.MovementDate)
                      .IsRequired();

                entity.Property(e => e.ExpirationDate)
                      .IsRequired(false);

                entity.HasOne(e => e.MaterialItem)
                      .WithMany()
                      .HasForeignKey(e => e.MaterialItemId)
                      .OnDelete(DeleteBehavior.Restrict); // ✅

                entity.HasOne(e => e.Warehouse)
                      .WithMany()
                      .HasForeignKey(e => e.WarehouseId)
                      .OnDelete(DeleteBehavior.Restrict); // ✅
            });
        }
    }
}
