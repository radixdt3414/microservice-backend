namespace basket.API.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalPrice { get; set; }
        public Cart(string _UserName) 
        {
            UserName = _UserName;
        }
        public Cart() { }
    }
}