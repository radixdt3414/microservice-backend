using catelogs.API.Product.CreateProduct;
using Marten.Schema;

namespace catelogs.API.SeedData
{
    public class InitialCatalogData : IInitialData
    {
        public List<ProductModel> LstProduct;
        public InitialCatalogData(List<ProductModel> _LstProduct)
        {
            LstProduct = _LstProduct;
        }

        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            var session = store.LightweightSession();
            if (session.Query<ProductModel>().Any())
            {
                return;
            }
            session.Store(LstProduct.ToArray());
            await session.SaveChangesAsync();
        }

    }

    public static class ProductSeed
    {
        public static List<ProductModel> Products = new List<ProductModel>
    {
        new ProductModel
        {
            Name = "Dell Inspiron 15",
            Description = "Affordable laptop with AMD Ryzen 5 processor, 8GB RAM, and 512GB SSD.",
            Price = 42000.00m,
            Image = "dell-inspiron.jpg",
            Categories = new List<string> { "Electronics", "Laptop", "Dell" }
        },
        new ProductModel
        {
            Name = "HP Pavilion",
            Description = "Lightweight laptop with Intel i7 processor and 16GB RAM.",
            Price = 55000.00m,
            Image = "hp-pavilion.jpg",
            Categories = new List<string> { "Electronics", "Laptop", "HP" }
        },
        new ProductModel
        {
            Name = "Asus VivoBook S14",
            Description = "Slim and portable ultrabook with Intel Core i5 and 512GB SSD.",
            Price = 48000.00m,
            Image = "asus-vivobook.jpg",
            Categories = new List<string> { "Electronics", "Laptop", "Asus" }
        },
        new ProductModel
        {
            Name = "Apple MacBook Air M2",
            Description = "Premium ultrabook with Apple M2 chip, 8GB RAM, and 256GB SSD.",
            Price = 99900.00m,
            Image = "macbook-air-m2.jpg",
            Categories = new List<string> { "Electronics", "Laptop", "Apple" }
        },
        new ProductModel
        {
            Name = "Lenovo ThinkPad X1 Carbon",
            Description = "Business laptop with Intel i7, 16GB RAM, and 1TB SSD.",
            Price = 120000.00m,
            Image = "lenovo-thinkpad.jpg",
            Categories = new List<string> { "Electronics", "Laptop", "Lenovo", "Business" }
        }
    };
    }
}