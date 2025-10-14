using inventory.API;
using inventory.Application;
using inventory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddAPI().AddApplication(builder.Configuration).AddInfrastructure(builder.Configuration);

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
await app.UseInfrastructureServices();
//}
app.UseApiServices();
app.Run();