using GOOD_HAMBURGER.Model;

public class CreateMenuItemDTO
{
    public CreateMenuItemDTO(ItemType type, string? name, decimal price, bool isExtra)
    {
        Type = type;
        Name = name;
        Price = price;
        IsExtra = isExtra;
    }

    public ItemType Type { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public bool IsExtra { get; set; }
   // public int? OrderRequestId { get; set; } 

   // public OrderRequestModel? OrderRequest { get; set; }
}
