﻿using System.ComponentModel.DataAnnotations;

namespace tai_shop.Dtos.Item
{
    public class UpdateItemRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
