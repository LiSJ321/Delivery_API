using Delivery_DAL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_BLL.Services.IServices
{
    public interface IBasketService
    {
        Task<IEnumerable<DishBasketDto>> GetBasket(Guid userId);
        Task AddBasket(Guid dishId, Guid userId);
        Task DeleteBasket(Guid dishId, Guid userId, bool increase = false);
    }
}
