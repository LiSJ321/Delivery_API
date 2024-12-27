using Delivery_DAL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_BLL.Services.IServices
{
    public interface IOrderService
    {
        Task<OrderDto> GetOrderDetails(Guid orderId);
        Task<List<OrderInfoDto>> GetOrders(Guid userId);
        Task CreateOrder(OrderCreateDto orderCreateDto, Guid userId);
        Task ConfirmDelivery(Guid orderId, Guid userId);
    }
}
