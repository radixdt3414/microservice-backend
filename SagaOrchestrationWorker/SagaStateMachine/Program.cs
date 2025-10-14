using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine;
using SagaStateMachine.Data;
using SagaStateMachine.StateInstance;
using SagaStateMachine.StateMachine;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

// register services
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddSagaStateMachine<OrderStateMachine, OrderInstance>()
        .EntityFrameworkRepository(opt =>   
        {
            opt.AddDbContext<DbContext, SagaContext>((provider, dbBuilder) =>
            {
                dbBuilder.UseSqlServer(
                builder.Configuration.GetConnectionString("SagaStateDb")
                , x => {
                    x.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                }
                );
            });

            opt.ConcurrencyMode = ConcurrencyMode.Optimistic;
        });

    cfg.AddConsumers(Assembly.GetExecutingAssembly());

    cfg.UsingRabbitMq((context, busConfig) =>
    {
        busConfig.Host(new Uri(builder.Configuration["MessageBroker:Host"] ?? string.Empty), hostConfig =>
        {
            hostConfig.Username(builder.Configuration["MessageBroker:UserName"] ?? string.Empty);
            hostConfig.Password(builder.Configuration["MessageBroker:Password"] ?? string.Empty);
        });
        busConfig.ConfigureEndpoints(context);
    });
});
builder.Services.AddDbContext<SagaContext>(config => config.UseSqlServer(builder.Configuration.GetConnectionString("SagaStateDb")));

// worker service
builder.Services.AddHostedService<Worker>();

var host = builder.Build();

// run migrations on startup
using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SagaContext>();
    db.Database.Migrate();
}
host.Run();
