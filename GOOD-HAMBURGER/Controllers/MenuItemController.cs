using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services.MealItem;
using Microsoft.AspNetCore.Mvc;

namespace GOOD_HAMBURGER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController(IMenuItem menuItem) : ControllerBase
    {
        private readonly IMenuItem _menuItem = menuItem;

        [HttpGet("GetMenuItemList")]
        public async Task<ActionResult<ResponseModel<List<MenuItemModel>>>> GetMenuItemList()
        {
            try
            {
                var mealItems = await _menuItem.GetMenuItemList();
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


        [HttpGet("GetExtraItems")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<ActionResult<ResponseModel<List<MenuItemModel>>>> GetExtraItems()
        {
            try
            {
                var response = await _menuItem.GetExtraItems();

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

        [HttpGet("GetMenuItem/{id}")]
        public async Task<ActionResult<ResponseModel<MenuItemModel>>> GetMenuItem(int MenuID)
        {
            try
            {
                // Tenta recuperar o item pelo ID
                var mealItem = await _menuItem.GetMenuItem(MenuID);

                // Verifica se o status da resposta é positivo
                if (mealItem.Status)
                {
                    mealItem.Message = "Meal item retrieved successfully"; // Define a mensagem de sucesso
                    return Ok(mealItem); // Retorna o item encontrado com sucesso
                }

                // Se o status for falso, retorna a mensagem de erro
                return NotFound(mealItem.Message); // Pode retornar 404 se o item não for encontrado
            }
            catch (Exception ex)
            {
                // Em caso de exceção, retorna um erro 500 com a mensagem
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

                return CreatedAtAction(nameof(GetMenuItem), new { MenuId = newItem.Id }, newItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }




    }
}
