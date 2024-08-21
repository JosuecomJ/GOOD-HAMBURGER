using GOOD_HAMBURGER.Model;

public interface IOrderService
{
    decimal CalculateDiscount(List<MenuItemModel> items);
    Task<ResponseModel<List<OrderRequestModel>>> GetOrdersAsync();
    Task<ResponseModel<OrderRequestModel>> GetOrderByIdAsync(int id);
    Task<ResponseModel<OrderRequestModel>> CreateOrderAsync(CreateOrderRequestDTO orderRequestDto);
    Task<ResponseModel<OrderRequestModel>> UpdateOrderAsync(UpdateOrderRequestDTO orderRequest);
    Task<ResponseModel<bool>> DeleteOrderAsync(int id);
}
