using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services;
using GOOD_HAMBURGER.Services.MealItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GOOD_HAMBURGER.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly AppDBContext _context;

        public OrderController(IOrderService orderService, AppDBContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        [HttpGet("GetOrders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ResponseModel<List<OrderRequestModel>>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<OrderRequestModel>>> GetOrdersAsync()
        {
            try
            {
                var orders = await _context.OrderRequests
                    .Include(o => o.MenuItems)
                    .Select(o => new
                    {
                        Id = o.Id,
                        Name = o.Name,
                        MenuItems = o.MenuItems.Select(m => new
                        {
                            Id = m.Id,
                            Type = m.Type,
                            Name = m.Name,
                            Price = m.Price,
                            IsExtra = m.IsExtra
                        }).ToList()
                    }).ToListAsync();

                return Ok(new
                {
                    data = orders,
                    message = "Order items retrieved successfully",
                    status = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}


