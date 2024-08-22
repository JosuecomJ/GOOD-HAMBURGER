﻿using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services.OrderItem;

namespace GOOD_HAMBURGER.Data.SeedData
{
    //Seed data for the database
    public static class DbInitializer
    {

        //Initialize the database with some data if it is empty
        public static async Task InitializeAsync(AppDBContext context, IOrderService orderService)
        {
            //Check if there are any menu items or order requests
            if (context.MenuItems.Any() || context.OrderRequests.Any())
            {
                return;
            }
            //Create some menu items
            var menuItems = new List<MenuItemModel>
            {
                new() { Name = "X Burguer", Price = 5.00m, Type = ItemType.Sandwich, IsExtra = false },
                new() { Name = "X Egg", Price = 4.50m, Type = ItemType.Sandwich, IsExtra = false },
                new() { Name = "X Bacon", Price = 7.00m, Type = ItemType.Sandwich, IsExtra = false },
                new() { Name = "Fries", Price = 2.00m, Type = ItemType.Fries, IsExtra = true },
                new() { Name = "Soft Drink", Price = 2.50m, Type = ItemType.Drink, IsExtra = true },
            };

            //Add the menu items to the database
            context.MenuItems.AddRange(menuItems);
            await context.SaveChangesAsync();

            //Create an order request
            var orderRequests = new List<OrderRequestModel>
            {
                new() { Name = "John" }
            };

            //Add the order request to the database and save the changes to get the OrderRequestId  generated by the database for the order request 
            context.OrderRequests.AddRange(orderRequests);
            await context.SaveChangesAsync();

            //Create some order menu items and associate them with the order request
            var orderMenuItems = new List<OrderMenuItem>
            {
                new() {OrderRequestId = orderRequests[0].OrderRequestId ?? 0 , MenuItemId = menuItems[0].MenuItemID },
                new() {OrderRequestId = orderRequests[0].OrderRequestId ?? 0 , MenuItemId = menuItems[3].MenuItemID },
                new() {OrderRequestId = orderRequests[0].OrderRequestId ?? 0 , MenuItemId = menuItems[4].MenuItemID },
            };

            //Add the order menu items to the database
            context.OrderMenuItems.AddRange(orderMenuItems);
            await context.SaveChangesAsync();

            //Associate the order menu items with the order request
            orderRequests[0].OrderMenuItems = orderMenuItems;

            
            await orderService.CalculateAndSaveOrderTotalAsync(orderRequests[0]);
        }
    }
}
