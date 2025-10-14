using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using order.Domain.Abstraction;

namespace order.Infrastructure.Data.Interceptor
{
    public class DomainEventDispatchInterceptor(IMediator _mediat) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispathcEvents(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            DispathcEvents(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public  async Task DispathcEvents(DbContext context)
        {
            if(context == null) return;

            var aggregate = context.ChangeTracker.Entries<IAggregate>();
            aggregate = aggregate.Where(x => x.Entity.DomainEvents.Any()).ToList();
            var domainEvents = aggregate.SelectMany(x => x.Entity.DomainEvents).ToList();
            foreach (var item in aggregate.Select(x => x.Entity))
            {
                item.ClearDomainEvents();
            }

            foreach (var e in domainEvents)
            {
                await _mediat.Publish(e);
            }
        }
    }
}