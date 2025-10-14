namespace basket.API.Dtos
{
    public record GetProductResponse(List<ProductDTO> lstProducts);

    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; } = default!;
        public string Image { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
}