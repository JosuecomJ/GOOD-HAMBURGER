using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services;
using GOOD_HAMBURGER.Services.OrderItem;
using Microsoft.EntityFrameworkCore;

namespace GOOD_HAMBURGER.Data.SeedData
{
    public static class DbInitializer
    {
        public static void Initialize(AppDBContext context, IOrderService orderService)
        {
            // Check if the database is already populated
            if (context.MenuItems.Any() || context.OrderRequests.Any())
            {
                return; // DB is already populated
            }

            // Create and add MenuItems
            var menuItems = new List<MenuItemModel>
            {
                // The customer can choose three sandwich options:
                new() { Name = "X Burguer", Price = 5.00m, Type = ItemType.Sandwich, IsExtra = false },
                new() { Name = "X Egg", Price = 4.50m, Type = ItemType.Sandwich, IsExtra = false },
                new() { Name = "X Bacon", Price = 7.00m, Type = ItemType.Sandwich, IsExtra = false },

                // Customer can also add Extras:
                new() { Name = "Fries", Price = 2.00m, Type = ItemType.Fries, IsExtra = true },
                new() { Name = "Good Fries", Price = 2.00m, Type = ItemType.Fries, IsExtra = true },
                new() { Name = "Soft Drink", Price = 2.50m, Type = ItemType.Drink, IsExtra = true },
                new() { Name = "Good Soft Drink", Price = 2.50m, Type = ItemType.Drink, IsExtra = true },
            };

            context.MenuItems.AddRange(menuItems);
            context.SaveChanges(); // Save the MenuItems

            // Create and add OrderRequests
            var orderRequests = new List<OrderRequestModel>
            {
                new() { Name = "jhon", MenuItems =  [ menuItems[0] , menuItems[3] , menuItems[6]  ]}
            };

            // Calculate and set TotalPrice and Discount for each order
            foreach (var order in orderRequests)
            {
                order.CalculateTotalPrice(orderService);
                orderService.CalculateDiscount(order.MenuItems);
            }

            context.OrderRequests.AddRange(orderRequests);
            context.SaveChanges(); // Save the OrderRequests
        }
    }
}
