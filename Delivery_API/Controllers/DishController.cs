using Delivery_BLL.Exceptions;
using Delivery_BLL.Services.IServices;
using Delivery_DAL.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
