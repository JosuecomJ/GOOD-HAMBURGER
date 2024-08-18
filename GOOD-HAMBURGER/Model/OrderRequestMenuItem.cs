namespace GOOD_HAMBURGER.Model
{
    public class OrderRequestMenuItem
    {
        public int? OrderRequestId { get; set; }
        public OrderRequestModel OrderRequest { get; set; }
        public int? MenuItemId { get; set; }
        public MenuItemModel MenuItem { get; set; }

        // Construtor que inicializa as propriedades não anuláveis
        public OrderRequestMenuItem()
        {
            OrderRequest = new OrderRequestModel();
            MenuItem = new MenuItemModel();
        }
    }
}
