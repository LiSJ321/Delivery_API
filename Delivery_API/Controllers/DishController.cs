using Delivery_BLL.Exceptions;
using Delivery_BLL.Services.IServices;
using Delivery_DAL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Delivery_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }


        [HttpGet]
        public async Task<IActionResult> GetDish([FromQuery] DishCategory[] categories, [FromQuery] DishSorting sorting, bool vegetarian, int page = 1)
        {
            return Ok(await _dishService.GetDish(categories, sorting, vegetarian, page));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDishDetails(Guid id)
        {
            try
            {
                return Ok(await _dishService.GetDishDetails(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        [HttpGet("{id}/rating/check")]
        public async Task<IActionResult> CheckRating(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                return Ok(await _dishService.CheckRating(id, userId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("{id}/rating")]
        [Authorize]
        public async Task<IActionResult> SetRating(Guid id, [Range(0, 10)] int ratingScore)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                await _dishService.SetRating(id, ratingScore, userId);
                return Ok("Rating set successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
