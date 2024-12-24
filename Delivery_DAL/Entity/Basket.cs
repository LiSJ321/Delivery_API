using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_DAL.Entity
{
    public class Basket
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid DishId { get; set; }

        [ForeignKey("DishId")]
        public Dish Dish { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
