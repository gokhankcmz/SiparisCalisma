﻿using Entities.Models;

namespace Entities.RequestModels
{
    public class UpdateOrderDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = "Placed.";
        public Product Product { get; set; }
    }
}