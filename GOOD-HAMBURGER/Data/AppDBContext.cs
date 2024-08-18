using GOOD_HAMBURGER.Model;
using Microsoft.EntityFrameworkCore;

namespace GOOD_HAMBURGER.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<MenuItemModel> MenuItems { get; set; }
        public DbSet<OrderRequestModel> OrderRequests { get; set; }
        // public DbSet<OrderRequestMenuItem> OrderRequestMenuItems { get; set; } // Adicione esta linha

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderRequestModel>()
                .HasMany(o => o.MenuItems)
                .WithOne(m => m.OrderRequest)
                .HasForeignKey(m => m.OrderRequestId)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
