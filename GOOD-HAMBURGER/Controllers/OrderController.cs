using GOOD_HAMBURGER.Data;
using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var response = await _orderService.GetOrdersAsync();
            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }

            var orderResponses = response.Data.Select(order => new
            {
                order.Id,
                order.Name,
                order.TotalPrice,
                order.Discount,
                order.FormattedTotalPrice,
                order.FormattedDiscount,
                MenuItems = order.MenuItems.Select(menuItem => new
                {
                    menuItem.Id,
                    menuItem.Name,
                    menuItem.Price,
                    menuItem.IsExtra,
                    menuItem.Type
                }).ToList()
            }).ToList();

            return Ok(new
            {
                data = orderResponses,
                message = "Order items retrieved successfully",
                status = true
            });
        }

        [HttpGet("GetOrder/{id:int}")]
        public async Task<ActionResult<OrderRequestModel>> GetOrderById(int id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);

            if (!response.Status)
            {
                return NotFound(response.Message);
            }

            var order = response.Data;
            var orderResponse = new
            {
                order.Id,
                order.Name,
                order.TotalPrice,
                order.Discount,
                FormattedTotalPrice = order.FormattedTotalPrice,
                FormattedDiscount = order.FormattedDiscount,
                MenuItems = order.MenuItems.Select(omi => new
                {
                    omi.Id,
                    omi.Name,
                    omi.Price,
                    omi.IsExtra,
                    omi.Type
                }).ToList()
            };

            return Ok(orderResponse);
        }


        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDTO createOrderRequestDTO)
        {
            if (createOrderRequestDTO == null || !createOrderRequestDTO.MenuItemIds.Any())
            {
                return BadRequest("Invalid order data.");
            }

            var response = await _orderService.CreateOrderAsync(createOrderRequestDTO);

            if (!response.Status)
            {
                return BadRequest(response.Message);
            }

            var order = response.Data;
            var orderResponse = new
            {
                order.Id,
                order.Name,
                order.TotalPrice,
                order.Discount,
                order.FormattedTotalPrice,
                order.FormattedDiscount,
                MenuItems = order.MenuItems.Select(omi => new
                {
                    omi.Id,
                    omi.Name,
                    omi.Price,
                    omi.IsExtra,
                    omi.Type
                }).ToList()
            };

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, orderResponse);
        }




        [HttpPut("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequestDTO orderRequest)
        {
            if (id != orderRequest.Id)
            {
                return BadRequest("Order ID mismatch.");
            }

            var response = await _orderService.UpdateOrderAsync(orderRequest);

            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }

            var updatedOrder = response.Data;
            var orderResponse = new
            {
                updatedOrder.Id,
                updatedOrder.Name,
                updatedOrder.TotalPrice,
                updatedOrder.Discount,
                FormattedTotalPrice = updatedOrder.FormattedTotalPrice,
                FormattedDiscount = updatedOrder.FormattedDiscount,
                MenuItems = updatedOrder.MenuItems.Select(omi => new
                {
                    omi.Id,
                    omi.Name,
                    omi.Price,
                    omi.IsExtra,
                    omi.Type
                }).ToList()
            };

            return Ok(orderResponse);
        }



        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = await _orderService.DeleteOrderAsync(id);

            if (!response.Status)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }

            return NoContent();
        }
    }
}
