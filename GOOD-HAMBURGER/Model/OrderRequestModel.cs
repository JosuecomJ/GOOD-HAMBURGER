using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class OrderRequestModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    public string? Name { get; set; }

    public decimal TotalPrice { get; set; }
    public decimal Discount { get; set; }

    [JsonIgnore]
    public List<MenuItemModel> MenuItems { get; set; } = new List<MenuItemModel>();

    [JsonIgnore]
    [NotMapped]
    public string FormattedTotalPrice => $"$: {TotalPrice:F2}";
    [JsonIgnore]
    [NotMapped]
    public string FormattedDiscount => $"$: {Discount:F2}";

    public void CalculateTotalPrice(IOrderService orderService)
    {
        decimal discount = orderService.CalculateDiscount(MenuItems);
        decimal total = MenuItems.Sum(item => item.Price);

        Discount = discount;
        TotalPrice = total - discount;
    }
}
