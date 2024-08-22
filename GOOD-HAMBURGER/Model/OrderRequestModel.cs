using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using GOOD_HAMBURGER.Services.OrderItem;

namespace GOOD_HAMBURGER.Model
{
    // persistent model for order request
    public class OrderRequestModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? OrderRequestId { get; set; }
        public string? Name { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }



        // navigation property for menu items ignore this property when serializing to JSON to avoid circular reference
        [JsonIgnore]
        public ICollection<OrderMenuItem> OrderMenuItems { get; set; } = [];

        // formatted properties for display not mapped to database
        [JsonIgnore]
        [NotMapped]
        public string FormattedTotalPrice => $"$: {TotalPrice:F2}";

        [JsonIgnore]
        [NotMapped]
        public string FormattedDiscount => $"$: {Discount:F2}";

        // calculate total price of order request based on menu items and discount from order service 
        public async Task CalculateTotalPriceAsync(IOrderService orderService)
        {
            await orderService.CalculateAndSaveOrderTotalAsync(this);
        }

        
    }
}
