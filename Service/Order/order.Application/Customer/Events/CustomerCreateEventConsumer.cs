using buildingBlock.Messaging.Events;
using Mapster;
using MassTransit;
using order.Application.Customer.Commands;

namespace order.Application.Customer.Events
{
    public class CustomerCreateEventConsumer(ISender sender) : IConsumer<CustomerCreateEvent>
    {
        public async Task Consume(ConsumeContext<CustomerCreateEvent> context)
        {
            var customerCreateEventObj = context.Message;
            var command = customerCreateEventObj.Adapt<CreateCustomerCommand>();
            var result = await sender.Send(command);
            if (!result.IsSuccess)
            {
                throw new Exception("Encounter error while creating customer");
            }
        }
    }
}
