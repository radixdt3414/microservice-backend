using buildingBlock.Behaviour;
using catelogs.API.Product.CreateProduct;
using catelogs.API.Product.GetByIdProduct;
using catelogs.API.Product.GetProduct;
using catelogs.API.Product.GetProductByCategory;
using catelogs.API.Product.UpdateProduct;
using catelogs.API.SeedData;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using buildingBlock.Messaging.Extension;
using catelogs.API.Product.DeleteProduct;

var builder = WebApplication.CreateBuilder(args);

#region Registration mediatR
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
#endregion

#region Registration carter
builder.Services.AddCarter(null, config => config.WithModule<CreateProductEndpoint>());
builder.Services.AddCarter(null, config => config.WithModule<GetProductEndpoint>());
builder.Services.AddCarter(null, config => config.WithModule<GetByIdProductEndpoint>());
builder.Services.AddCarter(null, config => config.WithModule<GetProductByCategoryEndpoint>());
builder.Services.AddCarter(null, config => config.WithModule<UpdateProductEndpoint>());
builder.Services.AddCarter(null, config => config.WithModule<DeleteProductEndpoint>());
#endregion

#region Registration marten
builder.Services.AddMarten(config =>
{
    config.Connection(builder.Configuration.GetConnectionString("PostgresDb") ?? string.Empty);
    config.AutoCreateSchemaObjects = JasperFx.AutoCreate.All;

}).UseLightweightSessions();//.InitializeWith(new InitialCatalogData(ProductSeed.Products));
#endregion

#region Registration Fluent validator
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
#endregion

#region Registration Exception handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
#endregion

#region Registration health
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("PostgresDb") ?? string.Empty);
#endregion

#region Registration rabbitmq
builder.Services.AddMessageBroker(builder.Configuration);
#endregion


var app = builder.Build();

app.UseExceptionHandler(option => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseHealthChecks("/health", new HealthCheckOptions() 
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapCarter();
app.Run();
