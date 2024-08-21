﻿using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class CreateOrderRequestDTO
{
    public string? Name { get; set; }
    public List<int> MenuItemIds { get; set; } = new List<int>();

    public decimal TotalPrice { get; private set; }
    public decimal Discount { get; private set; }

    public async Task CalculateTotalPriceAsync(AppDBContext context, IOrderService orderService)
    {
        var menuItems = await context.MenuItems.Where(item => MenuItemIds.Contains(item.Id)).ToListAsync();
        Discount = orderService.CalculateDiscount(menuItems);
        TotalPrice = menuItems.Sum(item => item.Price) - Discount;
    }
}
