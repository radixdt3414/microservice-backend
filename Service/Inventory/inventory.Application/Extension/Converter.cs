using inventory.Domain.DTOs;
using inventory.Domain.Models;

namespace inventory.Application.Extension
{
    public static class Converter
    {
        public static List<WarehouseDTO> WarehouseModelToDTO( List<Warehouse> lstWarehouses)
        {
            return lstWarehouses.Select(x => new WarehouseDTO()
            {
                Id = x.Id.Value,
                Landmark = x.Address.Landmark,
                Country = x.Address.Country,
                State = x.Address.State,
                City = x.Address.City,
                PostalCode = x.Address.PostalCode,
                Description = x.Address.Description,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                IsActive = x.IsActive,
                CurrentCapacityUnitUsed = x.CurrentCapacityUnitUsed,
                CapacityUnit = x.CapacityUnit,
                InventoryItem = x.InventoryItem.Select(y => new InventoryItemDTO() {
                    Id = y.Id.Value,
                    ProductId = y.ProductId.Value,
                    WarehouseId = x.Id.Value,
                    QuantityOnHand = y.QuantityOnHand,
                    QuantityReserved = y.QuantityReserved,
                    QuantityAvailable = y.QuantityAvailable,

                }).ToList()
            }).ToList();
        }

    }
}
