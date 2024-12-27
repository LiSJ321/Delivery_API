using Delivery_DAL.Dto;
using Delivery_DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_BLL.Services.IServices
{
    public interface IDishService
    {
        Task<DishPagedListDto> GetDish(DishCategory[] category, DishSorting sorting, bool vegetarian, int page);
        Task<DishDto> GetDishDetails(Guid id);
    }
}
