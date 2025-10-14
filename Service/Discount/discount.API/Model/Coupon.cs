namespace discount.API.Model
{

    public class Coupon
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public long DiscountPrice { get; set; } = default;
        public string Description { get; set; } = string.Empty;
    }
}