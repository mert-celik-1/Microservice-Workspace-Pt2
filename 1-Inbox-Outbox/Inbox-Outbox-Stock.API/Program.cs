using Inbox_Outbox_Stock.API;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<StockDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLServer")));

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration["RabbitMQ"]);

        _configure.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEvent, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
    });
});
var host = builder.Build();
host.Run();