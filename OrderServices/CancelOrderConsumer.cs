using MassTransit;
using Microsoft.Extensions.Logging;
using OrderSample.Contracts;
using System.Threading.Tasks;

namespace OrderSample.OrderServices
{
    public class CancelOrderConsumer : IConsumer<CancelOrderCommand>
    {
        private readonly ILogger<CancelOrderConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public CancelOrderConsumer(ILogger<CancelOrderConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<CancelOrderCommand> context)
        {
            _logger.LogInformation("Received cancellation request for Order {OrderId} at {CancelDate}", context.Message.OrderId, context.Message.CancelDateTime);

            //await {retrieve order details such as customer number, order total etc.}
            double orderTotal = 20.00;
            string customerNumber = "customer 1";

            //publish/broadcast an event that order submitted successfully
            await _publishEndpoint.Publish<OrderCancelledEvent>(new
            {

                OrderId = context.Message.OrderId,
                Reason = context.Message.Reason,
                CancelDateTime = context.Message.CancelDateTime,
                OrderTotal = orderTotal,
                CustomerNumber = customerNumber
            });
        }
    }
}