using System.Diagnostics;
using System.Text.Json;
using MassTransit;
using Tracebility_Message_Broker_Shared;

namespace Tracebility_Message_Broker_Publisher;

public class PublishMessageService(IPublishEndpoint publishEndpoint, ILogger<PublishMessageService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var correlationId = Guid.NewGuid();

        int i = 0;
        while (true)
        {
            ExampleMessage message = new()
            {
                Text = $"{++i}. mesaj"
            };

            Trace.CorrelationManager.ActivityId = correlationId;
            logger.LogDebug("Publisher log");

            await Console.Out.WriteLineAsync($"{JsonSerializer.Serialize(message)} - Correlation Id : {correlationId}");
            await publishEndpoint.Publish(message,
                async context => { context.Headers.Set("CorrelationId", correlationId); });
            await Task.Delay(750);
        }
    }
}