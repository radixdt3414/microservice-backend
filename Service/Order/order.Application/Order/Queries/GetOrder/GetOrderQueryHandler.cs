using buildingBlock.DTO;

namespace order.Application.Order.Queries.GetOrder
{
    public class GetOrderQueryHandler(IApplicationDbContext DbContext) : IQueryHandler<GetOrderQuery, GetOrderResponse>
    {
        public async Task<GetOrderResponse> Handle(GetOrderQuery query, CancellationToken cancellationToken)
        {
            var qry = DbContext.Orders.Include(x => x.OrderItems).AsNoTracking().ToList();
            PageResultDTO<IEnumerable<OrderDTO>> response = new PageResultDTO<IEnumerable<OrderDTO>>();
            response.PageLimit = query.PageDetails.PageLimit;
            response.TotalRecords = qry.Count();
            response.PageCount = (long)Math.Ceiling((decimal)(response.TotalRecords > 0 ? response.TotalRecords > query.PageDetails.PageLimit ? (float)((float)response.TotalRecords / (float)query.PageDetails.PageLimit) : 1 : 0));
            if (response.PageCount < query.PageDetails.PageIndex || query.PageDetails.PageIndex < 1)
            {
                response.PageIndex = 1;
            }
            else { response.PageIndex = query.PageDetails.PageIndex; }

            if (response.TotalRecords > 0)
            {
                int previousPageCount = (int)(query.PageDetails.PageLimit * (response.PageIndex - 1));
                int currentPageCount = (int)(response.TotalRecords - previousPageCount >= query.PageDetails.PageLimit ? query.PageDetails.PageLimit : response.TotalRecords - previousPageCount);
                
                response.Data = Converter.ModelToDTOConverter(qry.Skip(previousPageCount).Take(currentPageCount).ToList());
            }
            return new GetOrderResponse(response);
        }
    }
}
