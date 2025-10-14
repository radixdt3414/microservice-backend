using basket.API.Cart.AddItem;
using basket.API.Dtos;
using basket.API.Models;
using buildingBlock.DTO;
using buildingBlock.Messaging.Events;
using MassTransit;
using System.Net.Http;
using System.Text.Json;

namespace basket.API.Cart.OrderFailed
{
    public class OrderFailedEventConsumer(HttpClient httpClient, ILogger<OrderFailedEventConsumer> logger, ISender sender, IConfiguration configuration) : IConsumer<OrderFailedEvent>
    {
        public async Task Consume(ConsumeContext<OrderFailedEvent> context)
        {
            try
            {
                logger.LogInformation($"--------------------basket------> OrderFailedEventConsumer: OrderFailedEventConsumer invoked.");

                var checkoutDetails = context.Message;
                var cart = await GetCartDetails(checkoutDetails);

                var result = await sender.Send(new AddItemCommand(cart));
            }
            catch (Exception ex)
            {
                logger.LogError($"OrderFailedEventConsumer: While create cart encuonter error:{context.Message.CorrelationId}");
                //throw new Exception($"OrderFailedEventConsumer: While fetching product list encounter error:{context.Message.CorrelationId}");
            }
        }

        public async Task<CartModel> GetCartDetails(OrderFailedEvent deletedCheckoutDetails)
        {
            var lstProduct = await GetProductList();
            var cartDetails = new CartModel()
            {
                Id = Guid.Empty,
                UserName = deletedCheckoutDetails.UserName,
                TotalPrice = 0
            };
            var lstCartItem = new List<CartItem>();
            foreach (var cartItem in deletedCheckoutDetails.Items)
            {
                var product = lstProduct.Find(x => x.Id == cartItem.ProductId);
                lstCartItem.Add(new CartItem()
                {
                    Quentity = cartItem.Quentity,
                    ProductId = cartItem.ProductId,
                    ProductName = product.Name,
                    Color = string.Empty,
                    Price = product.Price
                });
            }
            cartDetails.Items = lstCartItem;
            return cartDetails;
        }

        public async Task<List<ProductDTO>> GetProductList()
        {
            var lstProduct = new List<ProductDTO>();
            PaginationDTO paginationDTO = new PaginationDTO()
            {
                PageLimit = 0,
                PageIndex = 1,
                SortBy = "Name",
                SortOrder = "asc"
            };
            logger.LogInformation($"--------------------basket------> OrderFailedEventConsumer: Trying to fetch product list.");
            //var response = await (new HttpClient()).PostAsJsonAsync<PaginationDTO>(new Uri("http://localhost:5000/products"), paginationDTO);
            var url = configuration["Client:Product"];
            logger.LogInformation($"--------------------basket------> OrderFailedEventConsumer: url = {url}");
            var response = await httpClient.PostAsJsonAsync<PaginationDTO>(new Uri($"{url}/products"), paginationDTO);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                logger.LogInformation($"GetProductList: Product list fetched successfully.");
                var streamedResult = await response.Content.ReadAsStreamAsync();
                var result = JsonSerializer.Deserialize<PageResultDTO<GetProductResponse>>(streamedResult, options);
                if(result.Data == null)
                {
                    throw new Exception("-----basket------> OrderFailedEventConsumer: Product list not found");
                }
                lstProduct = result.Data?.lstProducts;
            }
            else
            {
                logger.LogError($"--------------------basket------> OrderFailedEventConsumer: While fetching product list encounter error:{response.StatusCode}");
                throw new Exception($"--------------------basket------> OrderFailedEventConsumer: While fetching product list encounter error:{response.StatusCode}");
            }
            return lstProduct;
        }
    }
}