using System;

namespace OrderSample.Contracts
{
    public interface OrderCancelledEvent
    {
        Guid OrderId { get; set; }

        string Reason { get; set; }

        DateTime CancelDateTime { get; set; }

        double OrderTotal { get; set; }

        string CustomerNumber { get; set; }
    }
}
