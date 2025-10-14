using buildingBlock.DTO;

namespace catelogs.API.Product.GetProduct
{
    public record GetProductQuery(PaginationDTO pageDetails) : IQuery<PageResultDTO<GetProductResponse>>;
    public record GetProductResponse(IReadOnlyList<ProductModel> lstProducts);

    public class GetProductHandler(IDocumentSession session) : IQueryHandler<GetProductQuery, PageResultDTO<GetProductResponse>>
    {
        public async Task<PageResultDTO<GetProductResponse>> Handle(GetProductQuery query, CancellationToken cancellationToken)
        {

            var qry = session.Query<ProductModel>().AsQueryable();
            PageResultDTO<GetProductResponse> response = new PageResultDTO<GetProductResponse>();
            if (string.IsNullOrEmpty(query.pageDetails.SortBy))
            {
                qry = query.pageDetails.SortBy switch
                {
                    "Name" => query.pageDetails.SortOrder == "asc" ? qry.OrderBy(x => x.Name).AsQueryable() : qry.OrderByDescending(x => x.Name).AsQueryable(),
                    "Description" => query.pageDetails.SortOrder == "asc" ? qry.OrderBy(x => x.Description).AsQueryable() : qry.OrderByDescending(x => x.Name).AsQueryable(),
                    "Price" => query.pageDetails.SortOrder == "asc" ? qry.OrderBy(x => x.Price).AsQueryable() : qry.OrderByDescending(x => x.Name).AsQueryable(),
                    "Image" => query.pageDetails.SortOrder == "asc" ? qry.OrderBy(x => x.Image).AsQueryable() : qry.OrderByDescending(x => x.Name).AsQueryable(),
                    _ => query.pageDetails.SortOrder == "asc" ? qry.OrderBy(x => x.Name).AsQueryable() : qry.OrderBy(x => x.Name).AsQueryable(),
                };
            }
            response.PageLimit = query.pageDetails.PageLimit;
            response.TotalRecords = await qry.CountAsync();
            response.PageCount = query.pageDetails.PageLimit  > 0 ?( (long)Math.Ceiling((decimal)(response.TotalRecords > 0 ? response.TotalRecords > query.pageDetails.PageLimit ? (float) ((float)response.TotalRecords / (float)query.pageDetails.PageLimit) : 1 : 0))) : 1;
            if(response.PageCount < query.pageDetails.PageIndex || query.pageDetails.PageIndex < 1 || query.pageDetails.PageLimit == 0)
            {
                response.PageIndex = 1;
                response.PageCount = 1;
            }
            else { response.PageIndex = query.pageDetails.PageIndex; }

            if (response.TotalRecords > 0 && query.pageDetails.PageLimit > 0)
            {
                int previousPageCount = (int)(query.pageDetails.PageLimit * (response.PageIndex - 1));
                int currentPageCount = (int)(response.TotalRecords - previousPageCount >= query.pageDetails.PageLimit ? query.pageDetails.PageLimit : response.TotalRecords - previousPageCount);
                response.Data = new GetProductResponse(qry.Skip(previousPageCount).Take(currentPageCount).ToList());
            }
            else
            {
                if(query.pageDetails.PageLimit == 0)
                {
                    response.Data = new GetProductResponse(qry.ToList());
                }
            }
                return response;
        }
    }
}