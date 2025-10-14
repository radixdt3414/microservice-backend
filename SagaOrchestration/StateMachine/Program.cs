using MassTransit;
using Microsoft.EntityFrameworkCore;
using StateMachine.Data;
using StateMachine.StateInstance;
using StateMachine.StateMachine;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SagaContext>(config => config.UseSqlServer(builder.Configuration.GetConnectionString("SagaStateDb")));


builder.Services.AddMassTransit(config =>
{
    config.AddSagaStateMachine<OrderStateMachine, OrderInstance>().EntityFrameworkRepository(repoConfig =>
    {
        repoConfig.ExistingDbContext<SagaContext>();
        repoConfig.UseSqlServer();
        repoConfig.ConcurrencyMode = ConcurrencyMode.Optimistic;
        
        //repoConfig.AddDbContext<DbContext, SagaContext>((provider, option) =>
        //{

        //    option.UseSqlServer(
        //        builder.Configuration.GetConnectionString("SagaStateDb")
        //        , x => {
        //            x.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        //        }
        //        );
        //});
    });

    config.AddConsumers(Assembly.GetExecutingAssembly());

    config.UsingRabbitMq((context, busConfig) =>
    {
        busConfig.Host(new Uri(builder.Configuration["MessageBroker:Host"] ?? string.Empty), hostConfig =>
        {
            hostConfig.Username(builder.Configuration["MessageBroker:UserName"] ?? string.Empty);
            hostConfig.Password(builder.Configuration["MessageBroker:Password"] ?? string.Empty);
        });
        busConfig.UseInMemoryOutbox(context);

        busConfig.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<SagaContext>();
context.Database.Migrate();

app.Run();