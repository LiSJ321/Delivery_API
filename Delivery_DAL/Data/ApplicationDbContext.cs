
using Delivery_DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Delivery_DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Basket> Baskets { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderBasket> OrderBaskets { get; set; }

        public DbSet<Rating> Ratings { get; set; }
    }
}
