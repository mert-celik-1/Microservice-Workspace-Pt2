using MassTransit;
using NLog.Extensions.Logging;
using Tracebility_Message_Broker_Publisher;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host("");
    });
});

builder.Logging.ClearProviders();
builder.Logging.AddNLog();

builder.Services.AddHostedService<PublishMessageService>(provider =>
{
    using IServiceScope scope = provider.CreateScope();
    IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();
    var logger = scope.ServiceProvider.GetService<ILogger<PublishMessageService>>();
    return new(publishEndpoint, logger);
});

var host = builder.Build();
host.Run();