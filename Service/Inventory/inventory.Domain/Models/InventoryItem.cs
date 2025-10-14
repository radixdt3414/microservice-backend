namespace inventory.Domain.Models
{
    public class InventoryItem : Entity<InventoryItemId>
    {
        public ProductId ProductId { get; private set; } = default!;
        public WarehouseId WarehouseId { get; private set; } = default!;
        public int QuantityOnHand { get; private set; } = default!;
        public int QuantityReserved { get; private set; } = default!;
        public int QuantityAvailable
        {
            get => QuantityOnHand - QuantityReserved;
            set => value = QuantityOnHand - QuantityReserved;
        }

        public static InventoryItem Create(InventoryItemId inventoryItemId, ProductId productId, WarehouseId warehouseId, int quantityOnHand, int quantityReserved)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(quantityOnHand);
            ArgumentOutOfRangeException.ThrowIfNegative(quantityReserved);
            ArgumentOutOfRangeException.ThrowIfLessThan(quantityOnHand, quantityReserved);
            
            var obj = new InventoryItem()
            {
                Id = inventoryItemId,
                ProductId = productId,
                WarehouseId = warehouseId,
                QuantityOnHand = quantityOnHand,
                QuantityReserved = quantityReserved,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
            };
            return obj;
        }

        public void UpdateQuantity(int quantity)
        {
            ModifiedOn = DateTime.UtcNow;
            QuantityOnHand = quantity;
        }

        //use case -  orders microservice
        public void PurchasedQantity(int quantity) {
            ModifiedOn = DateTime.UtcNow;
            QuantityReserved += quantity;
        }
        
        //use case -  shipment microservice
        public void ShipStock(int quantity) { /* ... */ }


        //public void ReserveStock(int quantity) { /* ... */ }
        //public void ReleaseStock(int quantity) { /* ... */ }
    }
}


//inventory - aggregate
//    warehouse - entity 
//        - warehouseId
//        - location
        
//    product - entity
//        - ProductId 
//        - Sku 
//        - otherProductRelatedDetails
//    InventoryItem - entity(Aggregate root)
//        - productId
//        - quentity
//        - QuantityAvailable 
//        - QuantityReserved 
//        - warehouseId
//        - LastUpdated
//        - AddStock()
//        - AdjustStock()


// AddStock() 
    //-  Will check productId available 
    //    ? (verify wareHouseid ? update quentity : throw ErrorEventArgs WareHouseNotFoundException)
    //    : (verify wareHouseid ? create new inventoryItem : throw ErrorEventArgs WareHouseNotFoundException)


// OrderCreatedEventHandler()
    //- get the list of inventoryItem for each productId
    //- match the wareHouse address
    //- check the quentity ? (update the stock) : (throw CancelOrderException)
    

    