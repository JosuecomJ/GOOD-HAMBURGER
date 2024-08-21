using GOOD_HAMBURGER.Model;

namespace GOOD_HAMBURGER.Services.MenuItem
{
    public interface IMenuItem
    {
        Task<ResponseModel<List<MenuItemModel>>> GETMenuItems();
        Task<ResponseModel<MenuItemModel>> GETMenuItemById(int Id);
        Task<ResponseModel<List<MenuItemModel>>> GETExtraItemsONLY();
        Task<ResponseModel<List<MenuItemModel>>> GETSandwichesONLY();
        Task AddMenuItem(MenuItemModel newItem);
    }
}
