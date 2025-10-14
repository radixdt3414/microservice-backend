using buildingBlock.JWT;
using pizza_shop.gateway.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region Registration reverse proxy
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
#endregion

#region Registration 
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy => {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
        });
});
#endregion
builder.Services.AddTransient<AuthMiddleware>();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var app = builder.Build();
app.UseCors();
app.UseMiddleware<AuthMiddleware>();
app.MapReverseProxy();

app.Run();
