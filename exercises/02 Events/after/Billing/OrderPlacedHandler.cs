namespace Billing
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using Shared.Events;

    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();
        
        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");

            var orderBilled = new OrderBilled
            {
                OrderId = message.OrderId
            };
            return context.Publish(orderBilled);
        }
    }
}
