﻿using AutoMapper;
using Delivery_BLL.Exceptions;
using Delivery_BLL.Services.IServices;
using Delivery_DAL.Data;
using Delivery_DAL.Dto;
using Delivery_DAL.Entity;
using Delivery_DAL.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_BLL.Services
{
    public class DishService: IDishService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private const int PageSize = 8;

        public DishService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DishPagedListDto> GetDish(DishCategory[] category, DishSorting sorting, bool vegetarian, int page)
        {
            IQueryable<Dish> dishQueryable = _context.Dishes;

            //filter by category
            dishQueryable = !category.IsNullOrEmpty() ? dishQueryable.Where(x => category != null && category.Contains(x.Category)) : dishQueryable;
            //filter by vagetarian
            dishQueryable = vegetarian ? dishQueryable.Where(x => x.Vegetarian) : dishQueryable;
            //filter by sort
            dishQueryable = Sort(dishQueryable, sorting);
            //pagination
            int dishCount = await dishQueryable.CountAsync();
            int pageTotal = (int)Math.Ceiling(dishCount / (double)PageSize);

            if (page < 1)
            {
                page = 1;
            }
            if (pageTotal == 0)
            {
                pageTotal = 1;
                page = 1;
            }
            else
            {
                if (page > pageTotal)
                {
                    page = pageTotal;
                }
            }
            var dishes = dishQueryable.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            PageInfoModel paginationModel = new()
            {
                Size = PageSize,
                Count = pageTotal,
                Current = page
            };
            List<DishDto> DishDtos = new();

            foreach (var item in dishes)
            {
                DishDto DishDto = new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Image = item.Image,
                    Vegetarian = item.Vegetarian,
                    Rating = item.Rating,
                    Category = item.Category
                };

                DishDtos.Add(DishDto);
            }
            DishPagedListDto dishListDto = new()
            {
                Dishes = DishDtos,
                Pagination = paginationModel
            };
            return dishListDto;
        }

        private static IQueryable<Dish> Sort(IQueryable<Dish> dishes, DishSorting? sorting = null)
        {
            return sorting switch
            {
                DishSorting.NameAsc => dishes.OrderBy(d => d.Name),
                DishSorting.NameDesc => dishes.OrderByDescending(d => d.Name),
                DishSorting.PriceAsc => dishes.OrderBy(d => d.Price),
                DishSorting.PriceDesc => dishes.OrderByDescending(d => d.Price),
                DishSorting.RatingAsc => dishes.OrderBy(d => d.Rating),
                DishSorting.RatingDesc => dishes.OrderByDescending(d => d.Rating),
                _ => throw new Exception()
            };
        }
        public async Task<DishDto> GetDishDetails(Guid id)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == id);

            return dish == null ? throw new NotFoundException($"Dish with id = {id} don't in database") : _mapper.Map<DishDto>(dish);
        }
        public async Task<bool> CheckRating(Guid dishId, Guid userId)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId) ?? throw new NotFoundException($"Dish with id = {dishId} don't in database");
            foreach (var order in _context.Orders)
            {
                if (order.UserId == userId && order.Status == OrderStatus.Delivered)
                {
                    foreach (var cart in _context.OrderBaskets)
                    {
                        if (cart.OrderId == order.Id && cart.DishId == dishId)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public async Task SetRating(Guid dishId, int ratingScore, Guid userId)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId) ?? throw new NotFoundException($"Dish with id = {dishId} don't in database");
            if (!await CheckRating(dishId, userId))
            {
                throw new BadRequestException("User can't set rating on dish that wasn't ordered");
            }

            var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.UserId == userId && r.DishId == dishId);
            if (rating != null)
            {
                rating.RatingScore = ratingScore;
                _context.Ratings.Update(rating);
            }
            else
            {
                _context.Ratings.Add(new Rating
                {
                    Id = Guid.NewGuid(),
                    DishId = dishId,
                    UserId = userId,
                    RatingScore = ratingScore
                });

            }
            await _context.SaveChangesAsync();

            double dishRating = _context.Ratings
            .Where(rating => rating.DishId == dishId)
            .Select(rating => rating.RatingScore)
            .ToList()
            .Average();

            dish.Rating = dishRating;
            _context.Dishes.Update(dish);

            await _context.SaveChangesAsync();
        }
    }
}
