using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GOOD_HAMBURGER.Services.OrderItem
{
    public class OrderService : IOrderService
    {
        private readonly AppDBContext _context;

        public OrderService(AppDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public decimal CalculateDiscount(List<MenuItemModel> items)
        {
            bool hasSandwich = items.Any(item => item.Type == ItemType.Sandwich);
            bool hasFries = items.Any(item => item.Type == ItemType.Fries);
            bool hasDrink = items.Any(item => item.Type == ItemType.Drink);

            decimal discount = 0;

            // options pattern
            discount = (hasSandwich, hasFries, hasDrink) switch
            {
                (true, true, true) => items.Sum(item => item.Price) * 0.20m,
                (true, false, true) => items.Sum(item => item.Price) * 0.15m,
                (true, true, false) => items.Sum(item => item.Price) * 0.10m,
                _ => 0,
            };
            return discount;
        }

        public async Task<ResponseModel<List<OrderRequestModel>>> GetOrdersAsync()
        {
            var response = new ResponseModel<List<OrderRequestModel>>();

            try
            {
                var orders = await _context.OrderRequests
                    .Include(ordered => ordered.MenuItems)
                    .ToListAsync();

                response.Data = orders;
                response.Status = true;
                response.Message = "Order items retrieved successfully";

                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"Internal server error: {ex.Message}";
                return response;
            }
        }

        public async Task<ResponseModel<OrderRequestModel>> GetOrderByIdAsync(int id)
        {
            var response = new ResponseModel<OrderRequestModel>();

            try
            {
                var order = await _context.OrderRequests
                    .Include(o => o.MenuItems)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    response.Status = false;
                    response.Message = "Order not found.";
                    return response;
                }

                response.Data = order;
                response.Status = true;
                response.Message = "Order retrieved successfully";

                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"Internal server error: {ex.Message}";
                return response;
            }
        }

        public async Task<ResponseModel<OrderRequestModel>> CreateOrderAsync(CreateOrderRequestDTO orderRequestDto)
        {
            var response = new ResponseModel<OrderRequestModel>();

            try
            {
                var menuItems = await _context.MenuItems.Where(item => orderRequestDto.MenuItemIds.Contains(item.Id)).ToListAsync();

                // Verificação de itens duplicados
                if (menuItems.Count(item => item.Type == ItemType.Sandwich) > 1 ||
                    menuItems.Count(item => item.Type == ItemType.Fries) > 1 ||
                    menuItems.Count(item => item.Type == ItemType.Drink) > 1)
                {
                    throw new InvalidOperationException("Each order cannot contain more than one sandwich, fries, or soda.");
                }

                // Calcular o preço total e o desconto
                await orderRequestDto.CalculateTotalPriceAsync(_context, this);

                var orderRequest = new OrderRequestModel
                {
                    Name = orderRequestDto.Name,
                    MenuItems = menuItems,
                    TotalPrice = orderRequestDto.TotalPrice,
                    Discount = orderRequestDto.Discount
                };

                _context.OrderRequests.Add(orderRequest);
                await _context.SaveChangesAsync();

                response.Data = orderRequest;
                response.Status = true;
                response.Message = "Order created successfully";
            }
            catch (InvalidOperationException ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"Internal server error: {ex.Message}";
            }

            return response;
        }
        public async Task<ResponseModel<OrderRequestModel>> UpdateOrderAsync(UpdateOrderRequestDTO orderRequest)
        {
            var response = new ResponseModel<OrderRequestModel>();

            try
            {
                var existingOrder = await _context.OrderRequests
                    .Include(o => o.MenuItems)
                    .FirstOrDefaultAsync(o => o.Id == orderRequest.Id);

                if (existingOrder == null)
                {
                    response.Status = false;
                    response.Message = "Order not found.";
                    return response;
                }

                existingOrder.Name = orderRequest.Name;

                // Atualizar os itens do menu
                var menuItems = await _context.MenuItems.Where(item => orderRequest.MenuItemIds.Contains(item.Id)).ToListAsync();
                existingOrder.MenuItems = menuItems;

                // Recalcular o preço total e o desconto
                await orderRequest.CalculateTotalPriceAsync(_context, this);
                existingOrder.TotalPrice = orderRequest.TotalPrice;
                existingOrder.Discount = orderRequest.Discount;

                _context.OrderRequests.Update(existingOrder);
                await _context.SaveChangesAsync();

                response.Status = true;
                response.Data = existingOrder;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public async Task<ResponseModel<bool>> DeleteOrderAsync(int id)
        {
            var response = new ResponseModel<bool>();

            try
            {
                var order = await _context.OrderRequests
                    .Include(o => o.MenuItems)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    response.Status = false;
                    response.Message = "Order not found.";
                    return response;
                }

                _context.OrderRequests.Remove(order);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Status = true;
                response.Message = "Order deleted successfully";

                return response;
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"Internal server error: {ex.Message}";
                return response;
            }
        }
    }
}
