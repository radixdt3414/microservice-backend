namespace inventory.Domain.DTOs
{
    public record OrderedProduct(ProductId ProductId, int quantity)
    {
    }
}