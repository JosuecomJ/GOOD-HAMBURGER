using GOOD_HAMBURGER.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class MenuItemModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public bool IsExtra { get; set; }
    public ItemType Type { get; set; }

   // [JsonIgnore]
    //public List<OrderMenuItem> OrderedMenuItem { get; set; } = new List<OrderMenuItem>();
}