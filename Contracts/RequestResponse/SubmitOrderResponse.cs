using System;
using System.Collections.Generic;
using System.Text;

namespace OrderSample.Contracts
{
    public interface SubmitOrderResponse
    {
        bool Ack { get; set; }
        string OrderStatus { get; set; }
    }
}
