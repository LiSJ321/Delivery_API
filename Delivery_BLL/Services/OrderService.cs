using Delivery_BLL.Exceptions;
using Delivery_BLL.Services.IServices;
using Delivery_DAL.Data;
using Delivery_DAL.Dto;
using Delivery_DAL.Entity;
using Delivery_DAL.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> GetOrderDetails(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderBaskets)
                .FirstOrDefaultAsync(o => o.Id == orderId) ?? throw new NotFoundException($"Order with id = {orderId} doesn't exist in the database");
            var dishBasketDtos = order.OrderBaskets.Select(item => new DishBasketDto
            {
                Id = item.DishId,
                Name = item.Name,
                Price = item.Price,
                TotalPrice = item.TotalPrice,
                Amount = item.Amount,
                Image = item.Image
            }).ToList();

            var orderDto = new OrderDto
            {
                Id = order.Id,
                DeliveryTime = order.DeliveryTime,
                OrderTime = order.OrderTime,
                Status = order.Status,
                Price = order.OrderBaskets.Sum(b => b.TotalPrice),
                Dishes = dishBasketDtos,
                Address = order.Address
            };

            return orderDto;
        }

        public async Task<List<OrderInfoDto>> GetOrders(Guid userId)
        {
            List<OrderInfoDto> orderInfoDtos = new();
            var orders = await _context.Orders.Where(w => w.UserId == userId).ToListAsync();
            foreach (var item in orders)
            {
                OrderInfoDto orderInfoDto = new()
                {
                    Id = item.Id,
                    DeliveryTime = item.DeliveryTime,
                    OrderTime = item.OrderTime,
                    Price = item.Price,
                    Status = item.Status
                };
                orderInfoDtos.Add(orderInfoDto);
            }

            return orderInfoDtos;
        }

        public async Task CreateOrder(OrderCreateDto orderCreateDto, Guid userId)
        {
            var baskets = await _context.Baskets.Where(b => b.UserId == userId).Include(b => b.Dish).ToListAsync();
            if (baskets.Any())
            {
                Order order = new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Address = orderCreateDto.Address,
                    DeliveryTime = orderCreateDto.DeliveryTime
                };

                int allTotalPrice = 0;

                List<OrderBasket> carts = new();

                foreach (var basketItem in baskets)
                {
                    allTotalPrice += basketItem.Dish.Price * basketItem.Amount;
                    OrderBasket cart = new()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        DishId = basketItem.Dish.Id,
                        Name = basketItem.Dish.Name,
                        Price = basketItem.Dish.Price,
                        TotalPrice = basketItem.Dish.Price * basketItem.Amount,
                        Amount = basketItem.Amount,
                        Image = basketItem.Dish.Image
                    };
                    carts.Add(cart);
                }

                order.Price = allTotalPrice;
                order.OrderTime = DateTime.Now;
                order.Status = OrderStatus.InProcess;

                await _context.AddRangeAsync(carts);
                await _context.AddAsync(order);
                _context.RemoveRange(baskets);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new BadRequestException($"Empty basket for user with id={userId}");
            }

        }

        public async Task ConfirmDelivery(Guid orderId, Guid userId)
        {
            var order = await _context.Orders.FindAsync(orderId) ?? throw new NotFoundException($"Order with id = {orderId} don't in database");
            if (order.Status == OrderStatus.Delivered)
            {
                throw new BadRequestException($"Can't update status for order with id = {orderId}");
            }
            else
            {
                order.Status = OrderStatus.Delivered;
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
