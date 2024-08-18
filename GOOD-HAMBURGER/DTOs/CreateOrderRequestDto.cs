using GOOD_HAMBURGER.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class CreateOrderRequestDTO
{
    
    public string? Name { get; set; }


    // Navigation Properties
    [JsonIgnore]
    public ICollection<MenuItemModel>? MenuItems { get; set; }

    [ForeignKey("MenuItemModel")]
    public int MenuItemModelId { get; set; }
}


