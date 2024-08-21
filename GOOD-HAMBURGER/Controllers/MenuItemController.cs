using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services.MenuItem;
using Microsoft.AspNetCore.Mvc;

namespace GOOD_HAMBURGER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController(IMenuItem menuItem) : ControllerBase
    {
        private readonly IMenuItem _menuItem = menuItem;

        [HttpGet("GETMenuItems")]
        public async Task<ActionResult<ResponseModel<List<MenuItemModel>>>> GETMenuItems()
        {
            try
            {
                var mealItems = await _menuItem.GETMenuItems();
                if (mealItems.Status)
                {
                    mealItems.Message = "Meal items retrieved successfully";
                    return Ok(mealItems);
                }
                return BadRequest(mealItems.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

        }


        [HttpGet("GETExtraItemsONLY")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<ActionResult<ResponseModel<List<MenuItemModel>>>> GETExtraONLY()
        {
            try
            {
                var response = await _menuItem.GETExtraItemsONLY();

                if (response.Status)
                {
                    return Ok(response);
                }
                return BadRequest(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GETSandwichesONLY")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<ActionResult<ResponseModel<List<MenuItemModel>>>> GETSandwichesONLY()
        {
            try
            {
                var response = await _menuItem.GETSandwichesONLY();

                if (response.Status)
                {
                    return Ok(response);
                }
                return BadRequest(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }




        [HttpGet("GetMenuItem/{id:int}")]
        [ProducesResponseType(typeof(ResponseModel<MenuItemModel>), 200)]
          
        public async Task<ActionResult<ResponseModel<MenuItemModel>>> GetMenuItemById(int id)
        {
            try
            {
                var mealItem = await _menuItem.GETMenuItemById(id);
                if (mealItem.Status)
                {
                    mealItem.Message = "Meal item retrieved successfully";
                    return Ok(mealItem);
                }

                return NotFound(mealItem.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("AddMenuItem")]
        public async Task<IActionResult> AddMenuItem([FromBody] CreateMenuItemDTO newItemDto)
        {
            if (newItemDto == null)
            {
                return BadRequest("Invalid item data.");
            }

            try
            {
                var newItem = new MenuItemModel
                {
                    Type = newItemDto.Type,
                    Name = newItemDto.Name,
                    Price = newItemDto.Price,
                    IsExtra = newItemDto.IsExtra
                };

                await _menuItem.AddMenuItem(newItem);

                return CreatedAtAction(nameof(GetMenuItemById), new { id = newItem.Id }, newItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }





    }
}
