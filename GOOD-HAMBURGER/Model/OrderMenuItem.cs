namespace GOOD_HAMBURGER.Model
{
    // persistent model for order menu items relationship table 
    public class OrderMenuItem
    {
        public int OrderMenuItemID { get; set; } 

        public int OrderRequestId { get; set; }
        public OrderRequestModel? OrderRequest { get; set; }

        public int MenuItemId { get; set; } 
        public MenuItemModel? MenuItem { get; set; }
    }
}
