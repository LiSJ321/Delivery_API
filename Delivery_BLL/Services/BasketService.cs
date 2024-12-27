using Delivery_BLL.Exceptions;
using Delivery_BLL.Services.IServices;
using Delivery_DAL.Data;
using Delivery_DAL.Dto;
using Delivery_DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_BLL.Services
{
    public class BasketService : IBasketService
    {
        private readonly ApplicationDbContext _context;
        public BasketService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<DishBasketDto>> GetBasket(Guid userId)
        {
            var baskets = await _context.Baskets.Where(b => b.UserId == userId).Include(w => w.Dish).ToListAsync();

            List<DishBasketDto> basketsDtos = new();
            foreach (var item in baskets)
            {
                DishBasketDto basketsDto = new()
                {
                    Id = item.DishId,
                    Name = item.Dish.Name,
                    Price = item.Dish.Price,
                    TotalPrice = item.Dish.Price * item.Amount,
                    Amount = item.Amount,
                    Image = item.Dish.Image
                };
                basketsDtos.Add(basketsDto);
            }
            return basketsDtos;
        }

        public async Task AddBasket(Guid dishId, Guid userId)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId) ?? throw new NotFoundException($"Dish with id = {dishId} don't in basket");
            var baskets = await _context.Baskets.FirstOrDefaultAsync(x => x.UserId == userId && x.DishId == dishId);
            if (baskets == null)
            {
                Basket basket = new()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DishId = dish.Id
                };
                basket.Amount += 1;

                await _context.AddAsync(basket);
                await _context.SaveChangesAsync();
            }
            else
            {
                baskets.Amount += 1;
                _context.Update(baskets);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBasket(Guid dishId, Guid userId, bool increase)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId) ?? throw new NotFoundException($"Dish with id = {dishId} don't in database");
            var baskets = await _context.Baskets.FirstOrDefaultAsync(x => x.UserId == userId && x.DishId == dishId) ?? throw new NotFoundException($"Dish with id = {dishId} don't in basket");
            if (baskets.Amount == 1 || !increase)
            {
                _context.Baskets.Remove(baskets);
                await _context.SaveChangesAsync();
            }
            else
            {
                baskets.Amount -= 1;
                _context.Update(baskets);
                await _context.SaveChangesAsync();
            }
        }
    }
}
