using GOOD_HAMBURGER.DTOs;
using GOOD_HAMBURGER.Model;

namespace GOOD_HAMBURGER.Services.MenuItem
{
    // Interface for MenuItem Service
    public interface IMenuItem
    {
        Task<ResponseModel<List<MenuItemModel>>> GETMenuItems();
        Task<ResponseModel<MenuItemModel>> GETMenuItemById(int Id);
        Task<ResponseModel<List<MenuItemModel>>> GETExtraItemsONLY();
        Task<ResponseModel<List<MenuItemModel>>> GETSandwichesONLY();
        Task<ResponseModel<MenuItemModel>> AddMenuItem(CreateMenuItemDTO newItemDto);
    }
}
