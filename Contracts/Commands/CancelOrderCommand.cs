using System;
using System.Collections.Generic;
using System.Text;

namespace OrderSample.Contracts
{
    public interface CancelOrderCommand
    {
        Guid OrderId { get; set; }

        string Reason { get; set; }

        DateTime CancelDateTime { get; set; }
    }
}
