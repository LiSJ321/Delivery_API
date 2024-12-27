using Delivery_BLL.Exceptions;
using Delivery_BLL.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Delivery_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return Ok(await _basketService.GetBasket(userId));
        }

        [HttpPost("dish/{dishId}")]
        public async Task<IActionResult> AddBasket(Guid dishId)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                await _basketService.AddBasket(dishId, userId);
                return Ok("Dish added successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }

        }

        [HttpDelete("dish/{dishId}")]
        public async Task<IActionResult> DeleteBasket(Guid dishId, bool increase)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                await _basketService.DeleteBasket(dishId, userId, increase);
                return Ok("Dish deleted successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
