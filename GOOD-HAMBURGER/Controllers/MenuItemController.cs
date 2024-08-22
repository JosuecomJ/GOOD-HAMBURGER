using GOOD_HAMBURGER.DTOs;
using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services.MenuItem;
using Microsoft.AspNetCore.Mvc;

namespace GOOD_HAMBURGER.Controllers
{
    //Controller for MenuItems 
    [Route("api/[controller]")]
    [ApiController]

    //Inherit from ControllerBase
    public class MenuItemController(IMenuItem menuItem) : ControllerBase
    {
        //Private field of type IMenuItem
        private readonly IMenuItem _menuItem = menuItem;

        //GET: api/MenuItem/GETMenuItems returns all menu items
        [HttpGet("GETMenuItems")]
        public async Task<IActionResult> GETMenuItems()
        {
            try
            {
                var mealItems = await _menuItem.GETMenuItems();
                if (mealItems.Status)
                {
                    return Ok(new
                    {
                        mealItems.Message,
                        mealItems.Data
                    });
                }
                return BadRequest(new
                {
                    mealItems.Message,
                    mealItems.Data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }


        //GET: api/MenuItem/GETExtraItemsONLY returns all extra items
        [HttpGet("GetExtraItemsONLY")]
        [ProducesResponseType(typeof(ResponseModel<List<MenuItemModel>>), 200)]
        public async Task<IActionResult> GetExtraItemsONLY()
        {
            try
            {
                var result = await _menuItem.GETExtraItemsONLY();

                if (result.Status)
                {
                    return Ok(new
                    {
                        result.Message,
                        result.Data
                    });
                }

                return NotFound(new
                {
                    result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }


        //GET: api/MenuItem/GETSandwichesONLY returns all sandwiches
        [HttpGet("GetSandwichesONLY")]
        [ProducesResponseType(typeof(ResponseModel<List<MenuItemModel>>), 200)]
        public async Task<IActionResult> GetSandwichesONLY()
        {
            try
            {
                var result = await _menuItem.GETSandwichesONLY();

                if (result.Status)
                {
                    return Ok(new
                    {
                        result.Message,
                        result.Data
                    });
                }

                return NotFound(new
                {
                    result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        //GET: api/MenuItem/GETDrinksONLY returns all drinks
        [HttpGet("GetMenuItem/{id:int}")]
        [ProducesResponseType(typeof(ResponseModel<MenuItemModel>), 200)]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            try
            {
                var result = await _menuItem.GETMenuItemById(id);

                if (result.Status)
                {
                    return Ok(new
                    {
                        result.Message,
                        result.Data
                    });
                }

                return NotFound(new
                {
                    result.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        //POST: api/MenuItem/AddMenuItem adds a new menu item
        [HttpPost("AddMenuItem")]
        [ProducesResponseType(typeof(ResponseModel<MenuItemModel>), 201)]
        public async Task<IActionResult> AddMenuItem([FromBody] CreateMenuItemDTO newItemDto)
        {
            if (newItemDto == null)
            {
                return BadRequest("Invalid item data.");
            }

            try
            {
                var result = await _menuItem.AddMenuItem(newItemDto);

                if (result.Status)
                {
                    return CreatedAtAction(nameof(GetMenuItemById), new { id = result?.Data?.MenuItemID }, new
                    {
                        result?.Message,
                        result?.Data
                    });
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

    }
}
