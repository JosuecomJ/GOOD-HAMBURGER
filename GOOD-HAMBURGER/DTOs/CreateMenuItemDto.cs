using GOOD_HAMBURGER.Model;

public class CreateMenuItemDTO
{
    public ItemType Type { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public bool IsExtra { get; set; }
    public int? OrderRequestId { get; set; } // Torne a chave estrangeira nullable

    public OrderRequestModel? OrderRequest { get; set; }
}
