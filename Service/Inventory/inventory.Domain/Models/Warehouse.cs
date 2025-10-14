using inventory.Domain.DTOs;
using inventory.Domain.Events;
using inventory.Domain.Exceptions;
using InventoryItemModel = inventory.Domain.Models.InventoryItem;


namespace inventory.Domain.Models
{
    public class Warehouse : Aggregate<WarehouseId>
    {

        private List<InventoryItem> _inventoryItem = new List<InventoryItem>();
        public IReadOnlyList<InventoryItem> InventoryItem => _inventoryItem.AsReadOnly();

        public Address Address { get; private set; } = default!;
        public decimal? Latitude { get; private set; } = default!;
        public decimal? Longitude { get; private set; } = default!;
        public bool IsActive { get; private set; } = default!;
        public int CurrentCapacityUnitUsed { get; private set; } = default!;
        public int CapacityUnit { get; private set; } = default!;

        private Warehouse() { }

        public static Warehouse Create(WarehouseId warehouseId, Address address , decimal? latitude, decimal? longitude, int currentCapacityUnitUsed, int capacityUnit)
        {
            ArgumentNullException.ThrowIfNull(address);
            ArgumentNullException.ThrowIfNull(currentCapacityUnitUsed);
            ArgumentNullException.ThrowIfNull(capacityUnit);

            var obj = new Warehouse()
            {
                Id = warehouseId,
                Address = address,
                Latitude = latitude,
                Longitude = longitude,
                IsActive = true,
                CurrentCapacityUnitUsed = currentCapacityUnitUsed,
                CapacityUnit = capacityUnit,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
            };
            return obj;
        }

        public void AddInventoryItem(ProductId productId,int quantityOnHand,int quantityReserved)
        {
            _inventoryItem.Add(InventoryItemModel.Create(InventoryItemId.Of(Guid.NewGuid()), productId, Id, quantityOnHand, quantityReserved));
        }

        public void UpdateInventoryItem(ProductId productId, int quantityOnHand)
        {
            var product = _inventoryItem.Where(x => x.ProductId.Value == productId.Value).FirstOrDefault();
            if(product == null) { throw new DomainNullException("Updated product not found."); }
            product.UpdateQuantity(quantityOnHand);
        }

        public void RemoveInventoryItem(ProductId productId)
        {
            var product = _inventoryItem.Where(x => x.ProductId.Value == productId.Value).FirstOrDefault();
            if (product != null) { _inventoryItem.Remove(product); }
        }

        public void OrderPlaced(List<OrderedProduct> orderedList, Guid orderId)
        {
            foreach (var item in orderedList)
            {
                var inventoryObj = _inventoryItem.Where(x => x.ProductId.Value == item.ProductId.Value).FirstOrDefault();
                if(inventoryObj == null)
                {
                    throw new DomainNullException("Product not found in selected warehouse");
                }
                inventoryObj.PurchasedQantity(item.quantity);
            }

            AddDomainEvent(new StockReservedDomainEvent(orderId));
           
        }
    }
}