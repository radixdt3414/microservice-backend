using inventory.Domain.Models;
using inventory.Domain.ValueObjects;

namespace inventory.Infrastructure.Data.SeedData
{
    public static class InitialData
    {
        public static List<Warehouse> lstWarehouses() {

            var wareHouse1 = Warehouse.Create(
                WarehouseId.Of(new Guid("363fd635-8bd7-4f76-b142-60f5f8eb9b39")),
                Address.of("India", "Near City Mall", "Maharashtra", "Mumbai", "400001", "Main warehouse in Mumbai"),
                null,
                null,
                13,
                20
                );
            var wareHouse2 = Warehouse.Create(
                WarehouseId.Of(new Guid("fff86e39-d7f9-408d-9646-27c6aa940ea1")),
                Address.of("USA", "Opposite Central Park", "New York", "New York City", "10001", "East Coast distribution hub"),
                null,
                null,
                13,
                20
                );
            //wareHouse1.AddInventoryItem(ProductId.Of(new Guid("019933d8-d5cb-463f-911d-1ce871e00627")),50, 0);
            //wareHouse1.AddInventoryItem(ProductId.Of(new Guid("01992e66-3ebd-4dc9-b411-3551d5f7f5a5")),50, 0);
            //wareHouse1.AddInventoryItem(ProductId.Of(new Guid("01992e66-3ebc-4490-aa56-67ae4ecc572d")),50, 0);
            //wareHouse2.AddInventoryItem(ProductId.Of(new Guid("019933d8-d5cb-463f-911d-1ce871e00627")), 150, 0);
            //wareHouse2.AddInventoryItem(ProductId.Of(new Guid("01992e66-3ebd-4dc9-b411-3551d5f7f5a5")), 150, 0);
            //wareHouse2.AddInventoryItem(ProductId.Of(new Guid("01992e66-3ebc-4490-aa56-67ae4ecc572d")), 150, 0);
            return new List<Warehouse>() {wareHouse1, wareHouse2};
        }
    }
}