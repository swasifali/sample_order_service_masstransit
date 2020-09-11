using MassTransit;
using Microsoft.Extensions.Logging;
using OrderSample.Contracts;
using System.Threading.Tasks;

//This consumer could be in a completely different assembly and hosted in another process.
//For simplicity its kept in the same project as Requests/Commands
namespace OrderSample.OrderServices
{
    /// <summary>
    /// Order Submitted Event Consumer 
    /// </summary>
    public class OrderSubmittedEventConsumer : IConsumer<OrderSubmittedEvent>
    {
        private readonly ILogger<OrderSubmittedEventConsumer> _logger;

        public OrderSubmittedEventConsumer(ILogger<OrderSubmittedEventConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
        {
            _logger.LogInformation("Received event: OrderSubmittedEvent {OrderId}/{CustomerNumber}", context.Message.OrderId, context.Message.CustomerNumber);

            // await do something with order submitted event e.g. send an email to customer with order confirmation
        }
    }
}