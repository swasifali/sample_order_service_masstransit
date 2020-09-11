using System;

namespace OrderSample.Contracts 
{
    public interface OrderSubmittedEvent
    {
        Guid OrderId { get; }
        DateTime OrderDate { get; }
        string CustomerNumber { get; }
        decimal OrderTotal { get; }
        string OrderStatus { get; }

    }
}