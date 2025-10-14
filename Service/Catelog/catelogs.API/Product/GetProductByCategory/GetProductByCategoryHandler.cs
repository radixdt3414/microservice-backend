namespace catelogs.API.Product.GetProductByCategory
{
    public record GetProductByCategoryQuery(string category) : IQuery<GetProductByCategoryResponse>;
    public record GetProductByCategoryResponse(IEnumerable<ProductModel> LstProduct);

    public class GetProductByCategoryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResponse>
    {
        public async Task<GetProductByCategoryResponse> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            var temp = await session.Query<ProductModel>().ToListAsync();
            var response = await session.Query<ProductModel>().Where(x => x.Categories.Contains(query.category.ToString())).ToListAsync(cancellationToken);
            return new GetProductByCategoryResponse(response);
        }
    }
}