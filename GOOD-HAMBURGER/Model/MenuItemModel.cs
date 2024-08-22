using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GOOD_HAMBURGER.Model
{
    // persistent model for menu items
    public class MenuItemModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuItemID { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }

        //will be used to filter if the is a sandwich or an extra on endpoints
        public bool IsExtra { get; set; }

        //will be used to determine the type of item and aply discounts logic
        public ItemType Type { get; set; }

        // navigation property for order menu items
        [JsonIgnore]
        public ICollection<OrderMenuItem> OrderMenuItems { get; set; } = [];


    }
}