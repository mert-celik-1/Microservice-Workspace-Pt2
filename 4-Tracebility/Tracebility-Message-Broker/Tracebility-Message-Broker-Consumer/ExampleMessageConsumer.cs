using System.Diagnostics;
using MassTransit;
using Tracebility_Message_Broker_Shared;

namespace Tracebility_Message_Broker_Consumer;

public class ExampleMessageConsumer(ILogger<ExampleMessageConsumer> logger) : IConsumer<ExampleMessage>
{
    public Task Consume(ConsumeContext<ExampleMessage> context)
    {
        var correlationId = Guid.NewGuid();
        if (context.Headers.TryGetHeader("CorrelationId", out var _correlationId))
            correlationId = Guid.Parse(_correlationId.ToString());

        Trace.CorrelationManager.ActivityId = correlationId;
        logger.LogDebug("Consumer log");

        Console.WriteLine($"Gelen mesaj : {context.Message.Text} - Correlation Id : {(correlationId)}");
        return Task.CompletedTask;
    }
}