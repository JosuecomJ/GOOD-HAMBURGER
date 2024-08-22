using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services.OrderItem;
using System.Text.Json.Serialization;

namespace GOOD_HAMBURGER.DTOs
{
    //DTO to create a new order
    public class CreateOrderRequestDTO
    {
        public string? Name { get; set; }
        public List<int> MenuItemIds { get; set; } = [];

        [JsonIgnore]
        public ICollection<MenuItemModel> MenuItems { get; set; } = [];

        [JsonIgnore]
        public ICollection<OrderMenuItemDTO> OrderMenuItems { get; set; } = [];

        [JsonIgnore]
        public decimal TotalPrice { get; private set; }

        [JsonIgnore]
        public decimal Discount { get; private set; }

        [JsonIgnore]
        public string FormattedTotalPrice => $"$: {TotalPrice:F2}";

        [JsonIgnore]
        public string FormattedDiscount => $"$: {Discount:F2}";

        // Method to calculate the total price of the order
        public async Task CalculateTotalPriceAsync(IOrderService orderService)
        {
            var orderRequestModel = ToOrderRequestModel();
            await orderService.CalculateAndSaveOrderTotalAsync(orderRequestModel);
            TotalPrice = orderRequestModel.TotalPrice;
            Discount = orderRequestModel.Discount;
        }

        // method to convert the DTO to an OrderRequestModel
        public OrderRequestModel ToOrderRequestModel()
        {
            var orderRequest = new OrderRequestModel
            {
                Name = Name
            };

            //Iterate over the MenuItemIds and add the corresponding MenuItem to the OrderRequest
            orderRequest.OrderMenuItems = MenuItems.Select(mi => new OrderMenuItem
            {
                MenuItemId = mi.MenuItemID,
                MenuItem = mi,
                OrderRequest = orderRequest 
            }).ToList();

            return orderRequest;
        }
    }
}