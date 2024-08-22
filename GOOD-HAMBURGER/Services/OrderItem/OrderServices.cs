using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.DTOs;
using GOOD_HAMBURGER.Model;
using Microsoft.EntityFrameworkCore;


namespace GOOD_HAMBURGER.Services.OrderItem
{
    // This class is used to implement the IOrderService interface 
    public class OrderService(AppDBContext context) : IOrderService
    {
        private readonly AppDBContext _context = context ?? throw new ArgumentNullException(nameof(context));


        // Method to calculate and save the total price of the order
        public async Task CalculateAndSaveOrderTotalAsync(OrderRequestModel order)
        {
            // Check if the order or the order items are null
            if (order == null || order.OrderMenuItems == null)
            {
                throw new ArgumentNullException(nameof(order), "The order can't be null");
            }

            // Get the menu items from the database 
            var menuItems = await _context.MenuItems
                .Where(mi => order.OrderMenuItems.Select(omi => omi.MenuItemId).Contains(mi.MenuItemID))
                .ToListAsync();
            // Check if the menu items are null
            if (menuItems.Count == 0)
            {
                throw new InvalidOperationException("The order must have some item");
            }

            // Check if the order has a sandwich, fries or drink item 
            bool hasSandwich = menuItems.Any(item => item.Type == ItemType.Sandwich);
            bool hasFries = menuItems.Any(item => item.Type == ItemType.Fries);
            bool hasDrink = menuItems.Any(item => item.Type == ItemType.Drink);

            // Calculate the discount based on the items selected
            decimal discount = (hasSandwich, hasFries, hasDrink) switch
            {
                (true, true, true) => menuItems.Sum(item => item.Price) * 0.20m,
                (true, false, true) => menuItems.Sum(item => item.Price) * 0.15m,
                (true, true, false) => menuItems.Sum(item => item.Price) * 0.10m,
                _ => 0,
            };
            // Calculate the total price of the order and apply the discount 
            decimal total = menuItems.Sum(item => item.Price);
            order.Discount = discount;
            order.TotalPrice = total - discount;

            _context.OrderRequests.Update(order);
            await _context.SaveChangesAsync();
        }


        // Method to get all the orders from the database
        public async Task<ResponseModel<object>> GetOrdersAsync()
        {
            var response = new ResponseModel<object>();

            try
            {
                // Get all the orders from the database
                var orders = await _context.OrderRequests
                    .Include(order => order.OrderMenuItems)
                    .ThenInclude(orderMenuItem => orderMenuItem.MenuItem)
                    .ToListAsync();

                // Format information
                var formattedOrders = orders.Select(order => new
                {
                    order.OrderRequestId,
                    order.Name,
                    order.TotalPrice,
                    order.Discount,
                    order.FormattedTotalPrice,
                    order.FormattedDiscount,
                    MenuItems = order.OrderMenuItems?.Select(omi => new
                    {
                        omi.MenuItem?.MenuItemID,
                        omi.MenuItem?.Name,
                        omi.MenuItem?.Price,
                        omi.MenuItem?.IsExtra,
                        omi.MenuItem?.Type
                    }).ToList()
                }).ToList();

                
                response.Message = orders.Count > 0 ? "Order items retrieved successfully" : "Despite the lack of data, the method is working";
                response.Status = true;
                response.Data = formattedOrders;
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                response.Status = false;
            }

            return response;
        }


        // Method to get an order by its ID
        public async Task<ResponseModel<object>> GetOrderByIdAsync(int id)
        {
            var response = new ResponseModel<object>();

            try
            {
                // Get the order from the database by its ID
                var order = await _context.OrderRequests
                    .Include(o => o.OrderMenuItems)
                    .ThenInclude(omi => omi.MenuItem)
                    .FirstOrDefaultAsync(o => o.OrderRequestId == id);

                // Check if the order is null
                if (order == null)
                {
                    response.Message = "Order not found";
                    response.Status = false;
                    return response;
                }

                // Format the information
                var orderItems = order.OrderMenuItems?.Select(omi => new
                {
                    omi.MenuItem?.MenuItemID,
                    omi.MenuItem?.Name,
                    omi.MenuItem?.Price,
                    omi.MenuItem?.IsExtra,
                    omi.MenuItem?.Type
                }).ToList();

                response.Message = "Order retrieved successfully";
                response.Status = true;
                response.Data = new
                {
                    order.OrderRequestId,
                    order.Name,
                    Items = orderItems
                };
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"Internal server error: {ex.Message}";
            }

            return response;
        }


        // Method to create a new order 
        public async Task<ResponseModel<object>> CreateOrderAsync(CreateOrderRequestDTO orderRequestDto)
        {
            var response = new ResponseModel<object>();

            try
            {
               
                // Get the menu items from the database
                var menuItems = await _context.MenuItems
                    .Where(item => orderRequestDto.MenuItemIds.Contains(item.MenuItemID))
                    .ToListAsync();

                // Check if the menu items are null
                if (menuItems.Count == 0)
                {
                    response.Status = false;
                    response.Message = "Check if the menu items exist.";
                    return response;
                }

                // Check if the order has a sandwich, fries or drink item
                if (menuItems.Count(item => item.Type == ItemType.Sandwich) > 1 ||
                    menuItems.Count(item => item.Type == ItemType.Fries) > 1 ||
                    menuItems.Count(item => item.Type == ItemType.Drink) > 1)
                {
                    throw new InvalidOperationException("Each order can't have more than one sandwich, fries or drink item.");
                }

                // Create a new order
                var order = new OrderRequestModel
                {
                    Name = orderRequestDto.Name,
                    OrderMenuItems = menuItems.Select(mi => new OrderMenuItem
                    {
                        MenuItemId = mi.MenuItemID,
                        MenuItem = mi
                    }).ToList()
                };

                // Calculate and save the total price of the order
                await CalculateAndSaveOrderTotalAsync(order);

                response.Message = "Order created successfully";
                response.Status = true;
                response.Data = new
                {
                    // Format the information
                    order.OrderRequestId,
                    order.Name,
                    order.TotalPrice,
                    order.Discount,
                    order.FormattedTotalPrice,
                    order.FormattedDiscount,
                    MenuItems = order.OrderMenuItems.Select(omi => new
                    {
                        omi.MenuItem?.MenuItemID,
                        omi.MenuItem?.Name,
                        omi.MenuItem?.Price,
                        omi.MenuItem?.IsExtra,
                        omi.MenuItem?.Type
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                response.Message = $"Erro interno do servidor: {ex.Message}";
                response.Status = false;
            }

            return response;
        }


        // Method to update an order
        public async Task<ResponseModel<object>> UpdateOrderAsync(int id, CreateOrderRequestDTO orderRequestDto)
        {
            var response = new ResponseModel<object>();

            try
            {
                //  Get order from the database
                var existingOrder = await _context.OrderRequests
                    .Include(o => o.OrderMenuItems)
                    .ThenInclude(omi => omi.MenuItem)
                    .FirstOrDefaultAsync(o => o.OrderRequestId == id);

                // Check if the order exists
                if (existingOrder == null)
                {
                    response.Status = false;
                    response.Message = "Pedido não encontrado.";
                    return response;
                }
                // Get the menu items from the database
                var menuItems = await _context.MenuItems
                    .Where(item => orderRequestDto.MenuItemIds.Contains(item.MenuItemID))
                    .ToListAsync();

                // Check if the menu items are null
                if (menuItems.Count == 0)
                {
                    response.Status = false;
                    response.Message = "Nenhum item de menu encontrado para os IDs fornecidos.";
                    return response;
                }

                // Check if the order has more than one sandwich, fries or drink item
                if (menuItems.Count(item => item.Type == ItemType.Sandwich) > 1 ||
                    menuItems.Count(item => item.Type == ItemType.Fries) > 1 ||
                    menuItems.Count(item => item.Type == ItemType.Drink) > 1)
                {
                    throw new InvalidOperationException("Each order can't have more than one sandwich, fries or drink item.");
                }

                // Update the order
                existingOrder.Name = orderRequestDto.Name;
                existingOrder.OrderMenuItems = menuItems.Select(mi => new OrderMenuItem
                {
                    MenuItemId = mi.MenuItemID,
                    MenuItem = mi,
                    OrderRequest = existingOrder
                }).ToList();

                // Calculate and save the total price of the order
                await CalculateAndSaveOrderTotalAsync(existingOrder);
                _context.OrderRequests.Update(existingOrder);
                await _context.SaveChangesAsync();

                response.Message = "Order updated successfully.";
                response.Status = true;
                response.Data = existingOrder;
            }
            catch (Exception ex)
            {
                response.Message = $"Internal Server error: {ex.Message}";
                response.Status = false;
            }

            return response;
        }


        // Method to delete an order
        public async Task<ResponseModel<object>> DeleteOrderAsync(int id)
        {
            var response = new ResponseModel<object>();

            try
            {
                // Get the order from the database by its ID
                var order = await _context.OrderRequests
                    .Include(o => o.OrderMenuItems)
                    .ThenInclude(omi => omi.MenuItem)
                    .FirstOrDefaultAsync(o => o.OrderRequestId == id);
                // Check if the order exists
                if (order == null)
                {
                    response.Message = "Order not found.";
                    response.Status = false;
                    return response;
                }

                //Get the Format message
                var orderItems = order.OrderMenuItems?.Select(omi => new
                {
                    omi.MenuItem?.MenuItemID,
                    omi.MenuItem?.Name,
                    omi.MenuItem?.Price,
                    omi.MenuItem?.IsExtra,
                    omi.MenuItem?.Type
                }).ToList();

                
                _context.OrderRequests.Remove(order);
                await _context.SaveChangesAsync();

                response.Message = $"Order deleted successfully: Name: {order.Name},  Menu Items Selected by {order.Name}: {string.Join(", ", orderItems?.Select(item => item.Name)?? [])}";
                response.Status = true;
                response.Data = new
                {
                    order.OrderRequestId,
                    order.Name,
                    Items = orderItems
                };
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"Internal server error: {ex.Message}";
            }

            return response;
        }
    }
}
