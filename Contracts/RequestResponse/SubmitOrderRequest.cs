using System;

namespace OrderSample.Contracts
{
    public interface SubmitOrderRequest
    {
        Guid OrderId { get; }
	    DateTime OrderDate { get; }
	    string CustomerNumber { get; }
	    decimal OrderTotal { get; }
    }
}
