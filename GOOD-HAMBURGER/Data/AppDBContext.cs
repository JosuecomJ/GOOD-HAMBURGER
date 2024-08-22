using Microsoft.EntityFrameworkCore;
using GOOD_HAMBURGER.Model;

namespace GOOD_HAMBURGER.Data
{
    // This class is used to connect to the database and create the tables
    public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
    {
        //Create the tables
        public DbSet<MenuItemModel> MenuItems { get; set; }
        public DbSet<OrderRequestModel> OrderRequests { get; set; }
        public DbSet<OrderMenuItem> OrderMenuItems { get; set; }

        //This method is used to create the relationship between the tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderMenuItem>()
                .HasKey(om => om.OrderMenuItemID);

            modelBuilder.Entity<OrderMenuItem>()
                .HasOne(om => om.OrderRequest)
                .WithMany(o => o.OrderMenuItems)
                .HasForeignKey(om => om.OrderRequestId);

            modelBuilder.Entity<OrderMenuItem>()
                .HasOne(om => om.MenuItem)
                .WithMany(m => m.OrderMenuItems)
                .HasForeignKey(om => om.MenuItemId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
