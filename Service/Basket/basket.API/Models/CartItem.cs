namespace basket.API.Models
{
    public class CartItem
    {
        public long Quentity { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; 
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; } 
    }
}
