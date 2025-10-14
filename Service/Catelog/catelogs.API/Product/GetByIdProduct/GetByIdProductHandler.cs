using catelogs.API.Exceptions;

namespace catelogs.API.Product.GetByIdProduct
{
    public record GetByIdProductQuery(Guid Id) : IQuery<GetByIdProductResponse>;
    public record GetByIdProductResponse(Guid Id, string Name, string Description, decimal Price, string Image, List<string> Categories);

    public class GetByIdProductHandler(IDocumentSession session) : IQueryHandler<GetByIdProductQuery, GetByIdProductResponse>
    {
        public async Task<GetByIdProductResponse> Handle(GetByIdProductQuery query, CancellationToken cancellationToken)
        {
            var response = await session.Query<ProductModel>().FirstOrDefaultAsync(x => x.Id == query.Id);
            if (response == null)
            {
                throw new ProductNotFoundException(query.Id);
            }
            return response.Adapt<GetByIdProductResponse>();
        }
    }
}