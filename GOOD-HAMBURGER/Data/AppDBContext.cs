using GOOD_HAMBURGER.Model;
using Microsoft.EntityFrameworkCore;

namespace GOOD_HAMBURGER.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<OrderRequestModel> OrderRequests { get; set; }
        public DbSet<MenuItemModel> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderRequestModel>()
                .HasMany(order => order.MenuItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict); // Configura o comportamento de exclusão para Restrict

            base.OnModelCreating(modelBuilder);
        }
    }
}
