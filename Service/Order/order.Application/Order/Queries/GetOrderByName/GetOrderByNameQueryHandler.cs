namespace order.Application.Order.Queries.GetOrderByName
{
    public class GetOrderByNameQueryHandler(IApplicationDbContext applicationDbContext) : IQueryHandler<GetOrderByNameQuery, GetOrderByNameResponse>
    {
        public async Task<GetOrderByNameResponse> Handle(GetOrderByNameQuery query, CancellationToken cancellationToken)
        {
            var lstOrder = await applicationDbContext.Orders.AsNoTracking().Include(x => x.OrderItems).ToListAsync();
            lstOrder = lstOrder.Where(x => x.Name.Value.Equals(query.name, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Name.Value).ToList();

            var result = Converter.ModelToDTOConverter(lstOrder);
            return new GetOrderByNameResponse(result);
        }
    }
}