﻿using Delivery_DAL.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_DAL.Entity
{
    public class Dish
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public int Price { get; set; }

        public string? Image { get; set; }

        public bool Vegetarian { get; set; }

        public double Rating { get; set; }

        public DishCategory Category { get; set; }
    }
}
