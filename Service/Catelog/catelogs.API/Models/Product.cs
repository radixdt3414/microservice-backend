﻿namespace catelogs.API.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public string? Image { get; set; } = null;
        public List<string> Categories { get; set; } = new List<string>();
    }
}