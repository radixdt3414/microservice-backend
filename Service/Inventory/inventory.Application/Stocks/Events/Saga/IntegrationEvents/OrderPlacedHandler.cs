using buildingBlock.Messaging.Events;
using inventory.Application.Data;
using inventory.Application.Exceptions;
using inventory.Domain.DTOs;
using inventory.Domain.Models;
using inventory.Domain.ValueObjects;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace inventory.Application.Stocks.Events.Saga.IntegrationEvents
{
    public class OrderPlacedHandler(IApplicationDbContext dbContext, ICorrelationContext correlationContext, IPublishEndpoint publisher, ILogger<OrderPlacedHandler> logger) : IConsumer<OrderPlacedEvent>
    {
        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            logger.LogInformation($"--------------------inventory------> OrderPlacedHandler: StockReservedDomainEventHandler invoked.");

            correlationContext.CorrelationId = context.Message.CorrelationId;
            OrderPlacedEvent eventObj = context.Message;
            var lstWarehouse = new List<Warehouse>();
            lstWarehouse = await dbContext.Warehouses
                                          .Where(x => x.Address.City == eventObj.Shipping_City
                                                    && x.Address.State == eventObj.Shipping_State
                                                    && x.Address.Country == eventObj.Shipping_Country
                                                    )
                                          .Include(x => x.InventoryItem)
                                          .ToListAsync();
            

            if (lstWarehouse == null)
            {
                lstWarehouse = await dbContext.Warehouses
                                              .Where(x => x.Address.State == eventObj.Shipping_State
                                                    && x.Address.Country == eventObj.Shipping_Country)
                                              .Include(x => x.InventoryItem)
                                              .ToListAsync();

                if (lstWarehouse == null)
                {
                    lstWarehouse = await dbContext.Warehouses
                                                  .Where(x => x.Address.Country == eventObj.Shipping_Country)
                                                  .Include(x => x.InventoryItem)
                                                  .ToListAsync();


                }
            }

            if (lstWarehouse.Count == 0) { await OrderReservedFailed(new DeliveryNotPossibleException(), eventObj); }
            else
            {
                logger.LogInformation($"--------------------inventory------> OrderPlacedHandler: Found list of warehouse and will check quantity.");
                var allocatedWarehouse = FindWarehouseWithAdequantProductQuantity(lstWarehouse, eventObj.items);
                if (allocatedWarehouse == null) { await OrderReservedFailed(new ProductOutOfStock(), eventObj); }
                else
                {
                    logger.LogInformation($"--------------------inventory------> OrderPlacedHandler: Found warehouse with ordered items.");
                    var lstOrderedProduct = new List<OrderedProduct>();
                    eventObj.items.ForEach(x =>
                    {
                        lstOrderedProduct.Add(new OrderedProduct(ProductId.Of(x.ProductId), (int)x.Quentity));
                    });
                    logger.LogInformation($"--------------------inventory------> OrderPlacedHandler: ordered quantity reserved.");
                    allocatedWarehouse!.OrderPlaced(lstOrderedProduct, eventObj.OrderId);
                    dbContext.Warehouses.Update(allocatedWarehouse!);
                    logger.LogInformation($"--------------------inventory------> OrderPlacedHandler: inventory record updated.");
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation($"--------------------inventory------> OrderPlacedHandler: inventory record saveds.");
                }
            }   
        }

        public Warehouse? FindWarehouseWithAdequantProductQuantity(List<Warehouse> warehouses, List<BaskteItem> orderedItems)
        {

            return warehouses.Find(x =>
            {
                return orderedItems.All(y =>
                {
                    return x.InventoryItem.Any(z => (y.ProductId == z.ProductId.Value && z.QuantityAvailable >= y.Quentity));
                });
            });
        }

        public async Task OrderReservedFailed(Exception ex, OrderPlacedEvent payload)
        {
            logger.LogError($"--------------------inventory------> OrderPlacedHandler: OrderReservedFailed due to {ex.Message}.");
            logger.LogError($"--------------------inventory------> OrderPlacedHandler: StockeReservedRollbackEvent dispatch.");
            await publisher.Publish(new StockeReservedRollbackEvent()
            {
                CorrelationId = correlationContext.CorrelationId,
                Items = payload.items.Select(x => new OrderFailedCartItem
                {
                    Quentity = x.Quentity,
                    ProductId = x.ProductId
                    
                }).ToList(),
                reason = ex.Message,
                orderId = payload.OrderId
            });
            
        }
    }
}