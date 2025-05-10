using Microsoft.EntityFrameworkCore;

namespace Shipping.Model
{
    public class ShippingDbContext : DbContext
    {
        public ShippingDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ShippingProduct> Products { get; set; }
        public DbSet<ShippingRequest> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShippingProduct>()
                .HasOne(p => p.ShippingRequest)
                .WithMany(r => r.Products)
                .HasForeignKey(p => p.RequestId)
                .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
