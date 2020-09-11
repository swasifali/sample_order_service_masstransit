namespace OrderSample.OrderServices
{
    //marker interface for order services
    //this can be used to register all consumers within a namespace that implement this market interface
    // by calling AddConsumersFromNamespaceContaining<IOrderServices>() method while configuring MassTransit middleware
    public interface IOrderServices { }
}
