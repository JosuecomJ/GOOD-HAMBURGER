using GOOD_HAMBURGER.DTOs;
using GOOD_HAMBURGER.Model;
using GOOD_HAMBURGER.Services.OrderItem;
using Microsoft.AspNetCore.Mvc;

namespace GOOD_HAMBURGER.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        // GET: api/Order/GetOrders returns all orders
        [HttpGet("GetOrders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ResponseModel<object>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var result = await _orderService.GetOrdersAsync();

            if (result.Status)
            {
                return Ok(new
                {
                    result.Message,
                    result.Data
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
        // GET: api/Order/GetOrder/{id} returns order by id
        [HttpGet("GetOrder/{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);

            if (result.Status)
            {
                return Ok(new
                {
                    result.Message,
                    result.Data
                });
            }

            return NotFound(result.Message);
        }

        // POST: api/Order/CreateOrder creates a new order 
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDTO orderRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _orderService.CreateOrderAsync(orderRequestDto);

            if (!result.Status)
            {
                return StatusCode(500, result.Message);
            }

            return Ok(result.Data);
        }

        // PUT: api/Order/UpdateOrder/{id} updates order by id
        [HttpPut("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] CreateOrderRequestDTO orderRequest)
        {
            var result = await _orderService.UpdateOrderAsync(id, orderRequest);

            if (result.Status)
            {
                return Ok(new
                {
                    result.Message,
                    result.Data
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        // DELETE: api/Order/DeleteOrder/{id} deletes order by id
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);

            if (result.Status)
            {
                return Ok(new
                {
                    result.Message
                });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
    }
}
