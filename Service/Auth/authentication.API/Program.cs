using authentication.API.Data;
using authentication.API.Services;
using authentication.API.User.Login;
using authentication.API.User.SignUp;
using buildingBlock.Behaviour;
using buildingBlock.JWT;
using Carter;
using Microsoft.EntityFrameworkCore;
using buildingBlock.Messaging.Extension;

var builder = WebApplication.CreateBuilder(args);


#region Registration carter
builder.Services.AddCarter(null, config => config.WithModule<SignUpEndpoint>());
builder.Services.AddCarter(null, config => config.WithModule<LoginEndpoint>());
#endregion

#region Registration mediatR
builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));
});
#endregion

#region Registration services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtTokenGenerator>();
#endregion


#region Registration EFcore
builder.Services.AddDbContext<UserContext>(config => config.UseSqlite(builder.Configuration.GetConnectionString("database")));
#endregion

#region Registration message broker
builder.Services.AddMessageBroker(builder.Configuration);
#endregion



var app = builder.Build();

var scope = app.Services.CreateScope();
var context =scope.ServiceProvider.GetRequiredService<UserContext>();
context.Database.Migrate();

app.MapCarter();
app.Run();