using Delivery_DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_DAL.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime DeliveryTime { get; set; }

        [Required]
        public DateTime OrderTime { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public double Price { get; set; }
        public List<DishBasketDto> Dishes { get; set; }

        [Required]
        [MinLength(1)]
        public string Address { get; set; }
    }
}
