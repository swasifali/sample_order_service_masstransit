using MassTransit;
using Microsoft.Extensions.Logging;
using OrderSample.Contracts;
using System.Threading.Tasks;

//This consumer could be in a completely different assembly and hosted in another process.
//For simplicity its kept in the same project as Requests/Commands
namespace OrderSample.OrderServices
{
    /// <summary>
    /// Order Cancelled Event Consumer 
    /// </summary>
    public class OrderCancelledEventConsumer : IConsumer<OrderCancelledEvent>
    {
        private readonly ILogger<OrderCancelledEventConsumer> _logger;

        public OrderCancelledEventConsumer(ILogger<OrderCancelledEventConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderCancelledEvent> context)
        {
            _logger.LogInformation("Received event: OrderCancelledEvent {OrderId}/{Reason}", context.Message.OrderId, context.Message.Reason);

            // await do something with order cancel event e.g. send an email to customer with cancellation confirmation
        }
    }
}