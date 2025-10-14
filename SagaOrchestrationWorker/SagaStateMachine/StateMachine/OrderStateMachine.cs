using buildingBlock.Messaging.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine.Data;
using SagaStateMachine.StateInstance;

namespace SagaStateMachine.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderInstance>
    {
        #region state
        public State CheckOut { get; set; }
        public State OrderPlacedSuccessful { get; set; }
        public State OrderPlacedFail { get; set; }
        public State InventoryReservedSuccessful { get; set; }
        public State InventoryReservedFail { get; set; }
        #endregion

        #region Event
        public Event<BasketCheckoutEvent> BasketCheckoutEvent { get; set; }
        public Event<OrderPlacedSuccessfullEvent> OrderPlacedSuccessfullEvent { get; set; }
        public Event<OrderPlacedRollbackEvent> OrderPlacedRollbackEvent { get; set; }
        public Event<StockeReservedSuccessfullEvent> StockeReservedSuccessfullEvent { get; set; }
        public Event<StockeReservedRollbackEvent> StockeReservedRollbackEvent { get; set; }

        #endregion

        public readonly SagaContext context;

        public OrderStateMachine(ILogger<OrderStateMachine> _logger, SagaContext _context)
        {
            context = _context;

            InstanceState(x => x.CurrentState);

            Event(() => BasketCheckoutEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => OrderPlacedSuccessfullEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => OrderPlacedRollbackEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => StockeReservedSuccessfullEvent, e => e.CorrelateById(m => m.Message.CorrelationId));
            Event(() => StockeReservedRollbackEvent, e => e.CorrelateById(m => m.Message.CorrelationId));


            Initially(

                When(BasketCheckoutEvent)
                .Then(x => {
                    _logger.LogInformation($"------------------------------> Saga Orchestration: started with correlationId {x.Saga.CorrelationId}");
                })
                .Then(x => { _logger.LogInformation("---------------------------------------------> BasketCheckoutEvent: Event received in SEC."); })
                .Then(x =>
                {
                    x.Saga.CorrelationId = x.Message.CorrelationId;
                    x.Saga.UserName = x.Message.UserName;
                    x.Saga.CustomerId = x.Message.CustomerId;
                    x.Saga.CurrentState = nameof(CheckOut);
                })
                .TransitionTo(CheckOut)
                .ThenAsync(async ctx =>
                {
                    ctx.Saga.CurrentState = nameof(CheckOut);
                    await ctx.Publish(new OrderEvent()
                    {
                        CustomerId = ctx.Message.CustomerId,
                        TotalPrice = ctx.Message.TotalPrice,
                        UserName = ctx.Message.UserName,
                        OrderName = ctx.Message.OrderName,
                        CardNumber = ctx.Message.CardNumber,
                        Cvv = ctx.Message.Cvv,
                        ExpiryDate = ctx.Message.ExpiryDate,
                        PaymentType = ctx.Message.PaymentType,
                        CardMemberName = ctx.Message.CardMemberName,
                        Shipping_FirstName = ctx.Message.Shipping_FirstName,
                        Shipping_LastName = ctx.Message.Shipping_LastName,
                        Shipping_Country = ctx.Message.Shipping_Country,
                        Shipping_Landmark = ctx.Message.Shipping_Landmark,
                        Shipping_State = ctx.Message.Shipping_State,
                        Shipping_City = ctx.Message.Shipping_City,
                        Shipping_PostalCode = ctx.Message.Shipping_PostalCode,
                        Shipping_Description = ctx.Message.Shipping_Description,
                        Order_FirstName = ctx.Message.Order_FirstName,
                        Order_LastName = ctx.Message.Order_LastName,
                        Order_Country = ctx.Message.Order_Country,
                        Order_Landmark = ctx.Message.Order_Landmark,
                        Order_State = ctx.Message.Order_State,
                        Order_City = ctx.Message.Order_City,
                        Order_PostalCode = ctx.Message.Order_PostalCode,
                        Order_Description = ctx.Message.Order_Description,
                        items = ctx.Message.items,
                        CorrelationId = ctx.Message.CorrelationId,
                    });
                })
                .Then(x => { _logger.LogInformation("---------------------------------------------> OrderEvent: state change to Checkout."); })
                .Then(x => { _logger.LogInformation("---------------------------------------------> OrderEvent: Event publish from the SEC."); })

                );


            During(
                CheckOut,

                When(OrderPlacedSuccessfullEvent)
                .Then(x => { _logger.LogInformation($"------------------------------> OrderPlacedSuccessfullEvent: Event received in SEC. correlationId: {x.Saga.CorrelationId}"); })
                .Then(x => {
                    _logger.LogInformation($"\"------------------------------> OrderPlacedSuccessfullEvent:  Published event type: {nameof(OrderPlacedSuccessfullEvent)} - {typeof(OrderPlacedSuccessfullEvent).AssemblyQualifiedName}");
                })
                .Then(x => { _logger.LogInformation("---------------------------------------------> OrderPlacedSuccessfullEvent: Order record saved with pending status successfully."); })
                .TransitionTo(OrderPlacedSuccessful)
                .Then(x => { _logger.LogInformation("---------------------------------------------> OrderPlacedSuccessfullEvent: state change to OrderPlacedSuccessful."); })
                .ThenAsync(async ctx =>
                {
                    ctx.Saga.CurrentState = nameof(OrderPlacedSuccessful);
                    
                    await ctx.Publish(new OrderPlacedEvent()
                    {

                        OrderId = ctx.Message.OrderId,
                        CustomerId = ctx.Message.CustomerId,
                        CorrelationId = ctx.Message.CorrelationId,
                        Shipping_Country = ctx.Message.Shipping_Country,
                        Shipping_Landmark = ctx.Message.Shipping_Landmark,
                        Shipping_State = ctx.Message.Shipping_State,
                        Shipping_City = ctx.Message.Shipping_City,
                        Shipping_PostalCode = ctx.Message.Shipping_PostalCode,
                        Shipping_Description = ctx.Message.Shipping_Description,
                        items = ctx.Message.items
                    });

                })
                .Then(x => { _logger.LogInformation("---------------------------------------------> OrderPlacedEvent: Event publish from the SEC."); }),

                When(OrderPlacedRollbackEvent)
                .Then(x => { _logger.LogInformation($"------------------------------> OrderPlacedRollbackEvent: Event received in SEC. correlationId: {x.Saga.CorrelationId}"); })
                .Then(x => { _logger.LogInformation("---------------------------------------------> OrderPlacedRollbackEvent: While saving order record encounter error."); })
                .TransitionTo(OrderPlacedFail)
                .Then(x => { _logger.LogInformation("---------------------------------------------> OrderPlacedRollbackEvent: state change to orderplacedfail."); })
                .ThenAsync(async ctx =>
                {
                    ctx.Saga.CurrentState = nameof(OrderPlacedFail);
                    await ctx.Publish(new OrderFailedEvent
                    {
                        CorrelationId = ctx.Message.CorrelationId,
                        UserName = ctx.Saga.UserName,
                        Items = ctx.Message.Items,
                        reason = ctx.Message.reason
                    });
                })
                .Then(x => { _logger.LogInformation("---------------------------------------------> Saga Rollback started. <--------------- "); })
                .Then(x => { _logger.LogInformation("---------------------------------------------> OrderFailedEvent: Event publish from the SEC."); })
            );

            During(
                OrderPlacedSuccessful,

                When(StockeReservedSuccessfullEvent)
                .Then(x => { _logger.LogInformation($"------------------------------> StockeReservedSuccessfullEvent: Event received in SEC. correlationId: {x.Saga.CorrelationId}"); })
                .Then(x => { _logger.LogInformation("---------------------------------------------> StockeReservedSuccessfullEvent: Stock reserved successfully."); })
                .TransitionTo(InventoryReservedSuccessful)
                 .Then(x => { _logger.LogInformation("---------------------------------------------> StockeReservedSuccessfullEvent: state change to InventoryReservedSuccessful."); })
                 .ThenAsync(async ctx =>
                 {
                     ctx.Saga.CurrentState = nameof(InventoryReservedSuccessful);
                     await ctx.Publish(new StockeReservedEvent()
                     {
                         CorrelationId = ctx.Message.CorrelationId,
                         orderId = ctx.Message.orderId
                     });
                 })
                .Then(x => { _logger.LogInformation("---------------------------------------------> StockeReservedEvent: Event publish from the SEC."); })
                .Then(x => { _logger.LogInformation($"------------------------------> Saga Orchestration: Ended with correlationId {x.Saga.CorrelationId}"); })
                .Finalize(),

                When(StockeReservedRollbackEvent)
                .Then(x => { _logger.LogInformation($"------------------------------> StockeReservedRollbackEvent: Event received in SEC. correlationId: {x.Saga.CorrelationId}"); })
                .Then(x => { _logger.LogInformation("---------------------------------------------> StockeReservedRollbackEvent: While Reserving product from inventory encounter error."); })
                .Then(x => { _logger.LogInformation("---------------------------------------------> Saga Rollback started. <--------------- "); })
                .TransitionTo(InventoryReservedFail)
                .Then(x => { _logger.LogInformation("---------------------------------------------> StockeReservedRollbackEvent: state change to InventoryReservedFail."); })
                .ThenAsync(async ctx =>
                {
                    ctx.Saga.CurrentState = nameof(InventoryReservedFail);
                    await ctx.Publish(new StockeReserveFailedEvent()
                    {
                        CorrelationId = ctx.Message.CorrelationId,
                        OrderId = ctx.Message.orderId
                    });
                })
                .Then(x => { _logger.LogInformation("---------------------------------------------> StockeReserveFailedEvent: Event publish from the SEC."); })
                .ThenAsync(async ctx =>
                {
                    ctx.Saga.CurrentState = nameof(InventoryReservedFail);
                    await ctx.Publish(new OrderFailedEvent
                    {
                        CorrelationId = ctx.Message.CorrelationId,
                        UserName = ctx.Saga.UserName,
                        Items = ctx.Message.Items,
                        reason = ctx.Message.reason
                    });
                })
                .Then(x => { _logger.LogInformation("---------------------------------------------> OrderFailedEvent: Event publish from the SEC."); })
                .Finalize()
                );
        }
    }
}
