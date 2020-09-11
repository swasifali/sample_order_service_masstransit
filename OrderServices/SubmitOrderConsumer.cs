using MassTransit;
using Microsoft.Extensions.Logging;
using OrderSample.Contracts;
using System.Threading.Tasks;

namespace OrderSample.OrderServices
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrderRequest>
    {
        private readonly ILogger<SubmitOrderConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<SubmitOrderRequest> context)
        {
            _logger.LogInformation("Received Order {OrderId} at {OrderDate}", context.Message.OrderId, context.Message.OrderDate);

            // await generate/submit order

            //publish/broadcast an event that order submitted successfully
            await _publishEndpoint.Publish<OrderSubmittedEvent>(new
            {
                context.Message.OrderId,
                context.Message.OrderDate,
                context.Message.CustomerNumber,
                context.Message.OrderTotal,
                OrderStatus = "Submitted"
            });

            //caller expects a response
            //mass transit can do both patterns i.e fire and forget and respond, however it is generally not recommended to do both in a consumer
            if (context.RequestId != null) 
            {
                await context.RespondAsync<SubmitOrderResponse>(new
                {
                    Ack = true,
                    OrderStatus = "Submitted"
                });
            }
        }
    }
}
