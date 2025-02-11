﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_DAL.Entity
{
    public class OrderBasket
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public Guid DishId { get; set; }

        [ForeignKey("DishId")]

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int TotalPrice { get; set; }

        [Required]
        public int Amount { get; set; }

        public string? Image { get; set; }
    }
}
