using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services.MenuItem;
using Microsoft.EntityFrameworkCore;

namespace GOOD_HAMBURGER.Services.MenuItem
{
    public class MenuItemService(AppDBContext context) : IMenuItem
    {
        private readonly AppDBContext _context = context;

        public async Task<ResponseModel<MenuItemModel>> GETMenuItemById(int MenuId)
        {
            ResponseModel<MenuItemModel> response = new();
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(MenuId);
                if (menuItem == null)
                {
                    response.Status = false;
                    response.Message = "Item not found, plese check if the item id exists";
                }
                else
                {
                    response.Data = menuItem;
                    response.Status = true;
                }
            }
            catch (Exception error)
            {
                response.Message = error.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<MenuItemModel>>> GETMenuItems()
        {
            ResponseModel<List<MenuItemModel>> response = new();
            try
            {
                var mealItems = await _context.MenuItems.ToListAsync();
                response.Data = mealItems;
                if (response.Data.Count <= 0)
                {
                    response.Message = "No data yet, but work";
                }
                else
                {
                response.Message= "success";
                }
                response.Status = true;
            }
            catch (Exception error)
            {
                response.Message = error.Message;
                response.Status = false;
            }
            return response;
        }

        public async Task<ResponseModel<List<MenuItemModel>>> GETExtraItemsONLY()
        {
            var response = new ResponseModel<List<MenuItemModel>>();
            try
            {
                var extraItems = await _context.MenuItems
                                               .Where(item => item.IsExtra)
                                               .ToListAsync();
                response.Data = extraItems;
                response.Status = true;
                response.Message = "success";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel<List<MenuItemModel>>> GETSandwichesONLY()
        {
            var response = new ResponseModel<List<MenuItemModel>>();
            try
            {
                var sandwiche = await _context.MenuItems
                                               .Where(item => !item.IsExtra)
                                               .ToListAsync();
                response.Data = sandwiche;
                response.Status = true;
                response.Message = "success";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task AddMenuItem(MenuItemModel newItem)
        {
        _context.MenuItems.Add(newItem);
            await _context.SaveChangesAsync();
        }
    }

}
