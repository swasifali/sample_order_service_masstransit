using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using OrderSample.Contracts;

namespace OrderSample.Api.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {       
        private readonly ILogger<OrderController> _logger;
        private readonly IRequestClient<SubmitOrderRequest> _requestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public OrderController(ILogger<OrderController> logger, IRequestClient<SubmitOrderRequest> requestClient, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _requestClient = requestClient;
            _sendEndpointProvider = sendEndpointProvider;
        }

        /// <summary>
        /// This showcases a Request/Reply pattern of communication. This is generally not recommended in distributed applications
        /// but may be desirable if a response is required from the consumer of the request.
        /// </summary>
        /// <param name="customerNumber">Customer Number</param>
        /// <param name="orderTotal">Order Total</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SubmitOrder(string customerNumber, decimal orderTotal)
        {
            _logger.LogInformation("OrderController:SubmitOrder for customer {customerNumber}", customerNumber);
            //try/catch to show that with Req/Response model, a timeout can occur if the consumer is not running.
            try
            {
                var response = await _requestClient.GetResponse<SubmitOrderResponse>(new
                {
                    OrderId = InVar.Id,
                    OrderDate = InVar.Timestamp,
                    CustomerNumber = customerNumber,
                    OrderTotal = orderTotal
                });
                _logger.LogInformation("OrderController:SubmitOrder for customer {customerNumber}, Result: {Ack}", customerNumber, response.Message.Ack);
                return Accepted(response);

            }
            catch (RequestTimeoutException rte)
            {
                _logger.LogError(rte, "OrderController:SubmitOrder for customer {customerNumber}, Timeout occurred", customerNumber);
            }

            return Accepted();
        }

        /// <summary>
        /// This showcases a Send Command pattern of communication. This is generally recommended in distributed applications
        /// when there is a specific and single target consumer, but waiting for a response in not required. 
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <returns></returns>
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelOrder(string orderId, string reason)
        {
            _logger.LogInformation("OrderController:CancelOrder for order {orderId}, Reason {CancelReason}", orderId, reason);

            //either get a sendendpoint using explicit address OR use endpoint conventions
            //for endpoint address schemes see https://masstransit-project.com/usage/producers.html#short-addresses
            var _serviceAddress = "queue:cancel-order";
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri(_serviceAddress));

            //even if the processor of order cancellations is not running when this command is sent, the method completes successfully
            //this provides temporal decoupling but if there is any issue with order cancellation with downstream system then that should be handled separately
            await endpoint.Send<CancelOrderCommand>(new
            {
                OrderId = orderId,
                Reason = reason,
                CancelDateTime = InVar.Timestamp
            });

            _logger.LogInformation($"OrderController:CancelOrder for order {orderId} completed", orderId);
            return Accepted();
        }
    }
}
