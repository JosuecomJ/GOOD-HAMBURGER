using GOOD_HAMBURGER.Model;

namespace GOOD_HAMBURGER.Services.MealItem
{
    public interface IMenuItem
    {
        Task<ResponseModel<List<MenuItemModel>>> GetMenuItemList();
        Task<ResponseModel<MenuItemModel>> GetMenuItem(int MenuId);
        Task<ResponseModel<List<MenuItemModel>>> GetExtraItems();
        Task AddMenuItem(MenuItemModel newItem);
    }
}
