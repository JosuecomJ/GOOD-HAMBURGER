using GOOD_HAMBURGER.Model;
using Microsoft.EntityFrameworkCore;

namespace GOOD_HAMBURGER.Data.SeedData
{
    public static class DbInitializer
    {
        public static void Initialize(AppDBContext context)
        {
            // Check if the database is already populated
            if (context.MenuItems.Any() || context.OrderRequests.Any())
            {
                return; // DB is already populated
            }

            // Create and add MenuItems
            var menuItems = new List<MenuItemModel>
            {
                new() { Name = "Good Hamburger", Price = 10.99, Type = ItemType.Sandwich, IsExtra = false },
                new() { Name = "Good Fries", Price = 4.99, Type = ItemType.Fries, IsExtra = true },
                new() { Name = "Good Soda", Price = 2.99, Type = ItemType.Drink, IsExtra = true }
            };


            context.MenuItems.AddRange(menuItems);
            context.SaveChanges(); // Save the MenuItems


            // Create and add OrderRequests]
            var orderRequests = new List<OrderRequestModel>
            {

              new() { Name = "jhon", MenuItems = new List<MenuItemModel> { menuItems[0] , menuItems[1] } }


            };

            context.OrderRequests.AddRange(orderRequests);
            context.SaveChanges(); // Save the OrderRequests


        }
    }
}