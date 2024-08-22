using GOOD_HAMBURGER.DTOs;
using GOOD_HAMBURGER.Model;

namespace GOOD_HAMBURGER.Services.OrderItem
{
    // Interface for Order Service
    public interface IOrderService
    {
        Task<ResponseModel<object>> GetOrdersAsync();
        Task<ResponseModel<object>> GetOrderByIdAsync(int id);
        Task<ResponseModel<object>> CreateOrderAsync(CreateOrderRequestDTO orderRequestDto);
        Task<ResponseModel<object>> UpdateOrderAsync(int id, CreateOrderRequestDTO orderRequest);
        Task<ResponseModel<object>> DeleteOrderAsync(int id);
        Task CalculateAndSaveOrderTotalAsync(OrderRequestModel orderRequestModel);
    }
}
