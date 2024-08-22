using GOOD_HAMBURGER.Model;

namespace GOOD_HAMBURGER.DTOs
{
    //This DTO is used to create a new MenuItem
    public class CreateMenuItemDTO
    {
        public ItemType Type { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public bool IsExtra { get; set; }
    }
}
