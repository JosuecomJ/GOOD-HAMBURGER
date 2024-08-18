using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GOOD_HAMBURGER.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDBContext _context;

        public OrderService(AppDBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<ResponseModel<List<OrderRequestModel>>> GetOrdersAsync()
        {
            var response = new ResponseModel<List<OrderRequestModel>>();

            try
            {
                var orders = await _context.OrderRequests
                    .Include(o => o.MenuItems) // Inclua as propriedades de navegação
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

    }
}