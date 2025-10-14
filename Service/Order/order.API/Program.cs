using order.API;
using order.Application;
using order.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices().AddApplicationServices(builder.Configuration).AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseApiServices();
//if (app.Environment.IsDevelopment())
//{
   await app.UseInfrastructureServices();
//}
app.Run();