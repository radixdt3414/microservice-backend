using basket.API.Cart.AddItem;
using basket.API.Cart.DeleteCart;
using basket.API.Cart.GetCart;
using basket.API.Data.CacheCartRepository;
using basket.API.Data.CartCacheStore;
using buildingBlock.Behaviour;
using discount.API.Protos;
using FluentValidation;
using buildingBlock.Messaging.Extension;
using basket.API.Cart.Checkout;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


#region Registration carter
builder.Services.AddCarter(null, config => config.WithModule<AddItemEndpoint>());
builder.Services.AddCarter(null, config => config.WithModule<GetCartEndpoint>());
builder.Services.AddCarter(null, config => config.WithModule<DeleteCartEndpoint>());
builder.Services.AddCarter(null, config => config.WithModule<CheckoutEndpoint>());
#endregion

#region Registration mediatR
builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
#endregion

#region Registration redis
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.InstanceName = "basket";
    option.Configuration = builder.Configuration.GetConnectionString("RedisStore");
});
builder.Services.AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));
#endregion

#region Registration services
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
//builder.Services.AddScoped<ICartItemRepository, CacheCartRepository>(); // 1. might throw error or will be overide 



//builder.Services.AddScoped(typeof( ICartItemRepository), (provider) =>
//{
//    return new CacheCartRepository(provider.GetRequiredService<CartItemRepository>(), provider.GetRequiredService<ICacheService<CartModel>>());
//}); // 2. register extended functionality manually with ICartItemRepository.


builder.Services.Decorate<ICartItemRepository, CacheCartRepository>(); // 3. equivalent as second registration
#endregion

#region Registration marten
builder.Services.AddMarten(
    config => {
        config.Connection(builder.Configuration.GetConnectionString("PostgresDb") ?? string.Empty);
        config.AutoCreateSchemaObjects = JasperFx.AutoCreate.All;
    }
    ).UseLightweightSessions();
#endregion

#region Registration Fluent validator
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
#endregion

#region Registration GRPCClient
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(config =>
{
    config.Address = new Uri(builder.Configuration.GetSection("GRPCClient").GetValue<string>("DiscountAddress")!);
});
#endregion

#region Registration RabbitMq
builder.Services.AddMessageBroker(builder.Configuration,Assembly.GetExecutingAssembly());
#endregion

#region Registration Httpclient
builder.Services.AddHttpClient();
#endregion

var app = builder.Build();

//app.UseHttpsRedirection();
app.MapCarter();
app.Run();