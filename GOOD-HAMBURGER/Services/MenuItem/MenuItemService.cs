using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.DTOs;
using GOOD_HAMBURGER.Model;
using Microsoft.EntityFrameworkCore;

namespace GOOD_HAMBURGER.Services.MenuItem
{
    // This class is used to implement the IMenuItem interface
    public class MenuItemService(AppDBContext context) : IMenuItem
    {
        // Create a private readonly variable to hold the context
        private readonly AppDBContext _context = context;

        // Method to get all the menu items from the database
        public async Task<ResponseModel<List<MenuItemModel>>> GETMenuItems()
        {
            ResponseModel<List<MenuItemModel>> response = new();
            try
            {
                // Get all the menu items from the database
                var mealItems = await _context.MenuItems.ToListAsync();
                response.Data = mealItems;
                response.Message = mealItems.Count > 0 ? "Menu items retrieved successfully" : "Despite the lack of data, the method is working";
                response.Status = true;
            }
            catch (Exception error)
            {
                response.Message = error.Message;
                response.Status = false;
            }
            return response;
        }


        // Method to get all the extra items from the database
        public async Task<ResponseModel<List<MenuItemModel>>> GETExtraItemsONLY()
        {
            var response = new ResponseModel<List<MenuItemModel>>();
            try
            {
                // Get all the extra items from the database checking the IsExtra property
                var extraItems = await _context.MenuItems
                                               .Where(item => item.IsExtra)
                                               .ToListAsync();
                response.Data = extraItems;
                response.Status = true;
                response.Message = extraItems.Count > 0 ? "Extra items retrieved successfully" : "Despite the lack of data, the method is working";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }


        // Method to get all the sandwich items from the database | Other way to implement the method could be checking the Type property
        public async Task<ResponseModel<List<MenuItemModel>>> GETSandwichesONLY()
        {
            var response = new ResponseModel<List<MenuItemModel>>();
            try
            {
                // Get all the sandwich items from the database checking the IsExtra property
                var sandwiche = await _context.MenuItems
                                               .Where(item => !item.IsExtra)
                                               .ToListAsync();
                response.Data = sandwiche;
                response.Status = true;
                response.Message = sandwiche.Count > 0 ? "Sandwiches retrieved successfully" : "Despite the lack of data, the method is working";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = ex.Message;
            }
            return response;
        }


        // Method to get a menu item by its id 
        public async Task<ResponseModel<MenuItemModel>> GETMenuItemById(int MenuId)
        {
            ResponseModel<MenuItemModel> response = new();
            try
            {
                // Get the menu item by its id
                var menuItem = await _context.MenuItems.FindAsync(MenuId);
                // Check if the menu item exists
                if (menuItem == null)
                {
                    response.Status = false;
                    response.Message = "Item not found, please check if the item id exists";
                }
                else
                {
                    response.Data = menuItem;
                    response.Status = true;
                    response.Message = "Menu item retrieved successfully";
                }
            }
            catch (Exception error)
            {
                response.Message = error.Message;
                response.Status = false;
            }
            return response;
        }


        // Method to add a new menu item to the database
        public async Task<ResponseModel<MenuItemModel>> AddMenuItem(CreateMenuItemDTO newItemDto)
        {
            var response = new ResponseModel<MenuItemModel>();
            try
            {
                // Verificar se a entidade já está sendo rastreada
                var existingItem = await _context.MenuItems
                                                 .AsNoTracking()
                                                 .FirstOrDefaultAsync(item => item.Name == newItemDto.Name);

                if (existingItem != null)
                {
                    response.Status = false;
                    response.Message = "Item already exists.";
                    return response;
                }

                // Criar um novo item de menu
                var newItem = new MenuItemModel
                {
                    Type = newItemDto.Type,
                    Name = newItemDto.Name,
                    Price = newItemDto.Price,
                    IsExtra = newItemDto.IsExtra
                };

                // Adicionar o novo item de menu ao banco de dados
                _context.MenuItems.Add(newItem);
                await _context.SaveChangesAsync();

                response.Data = newItem;
                response.Status = true;
                response.Message = "Menu item added successfully";
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"Internal server error: {ex.Message}";
            }
            return response;
        }


    }

}
