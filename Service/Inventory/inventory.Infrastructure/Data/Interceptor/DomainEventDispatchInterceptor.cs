using inventory.Domain.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace inventory.Infrastructure.Data.Interceptor
{
    public class DomainEventDispatchInterceptor(IMediator _mediatr) : SaveChangesInterceptor
    {
        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            DispatchDomainEvent(eventData.Context);
            return base.SavedChanges(eventData, result);
        }

        public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            DispatchDomainEvent(eventData.Context);
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        public async Task DispatchDomainEvent(DbContext context)
        {
            var lstAggregate = context.ChangeTracker.Entries<IAggregate>();
            var lstDomainEvents = lstAggregate.SelectMany(x => x.Entity.DomainEvents).ToList();
            foreach (var item in lstAggregate)
            {
                item.Entity.CleardomainEvent();
            }
            foreach (var item in lstDomainEvents)
            {
                await _mediatr.Publish(item);
            }
        }
    }
}