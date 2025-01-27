namespace Components.OrderManagement;

using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;


public class ProcessOrderConsumerDup :
    IConsumer<ProcessOrder>
{
    readonly ILogger<ProcessOrderConsumerDup> _logger;

    public ProcessOrderConsumerDup(ILogger<ProcessOrderConsumerDup> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ProcessOrder> context)
    {
        _logger.LogInformation("Dupe for fun {CustomerNumber,-20} Order {OrderId}", context.Message.CustomerNumber, context.Message.OrderId);

        return Task.CompletedTask;
    }
}