namespace order.Application.Order.Queries.GetOrderByCustomer
{
    public class GetOrderByCustomerQueryHandler(IApplicationDbContext applicationDbContext) : IQueryHandler<GetOrderByCustomerQuery, GetOrderByCustomerResponse>
    {
        public async Task<GetOrderByCustomerResponse> Handle(GetOrderByCustomerQuery query, CancellationToken cancellationToken)
        {

            var lstOrder = await applicationDbContext.Orders.AsNoTracking().Include(x => x.OrderItems).ToListAsync();
            lstOrder = lstOrder.Where(x => x.CustomerId.Value == query.CustomerId)
                .OrderBy(x => x.Name.Value).ToList();
            var lstProduct = await applicationDbContext.Products.AsNoTracking().ToListAsync();

            var result = Converter.ModelToDTOConverter(lstOrder);
            foreach (var item in result)
            {
                foreach (var item1 in item.OrderItems)
                {
                    item1.ProductName = lstProduct.Where(x => x.Id.Value == item1.ProductId).Select(x => x.ProductName).FirstOrDefault();
                }
            }
            return new GetOrderByCustomerResponse(result);
        }
    }
}