namespace Shipping
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Logging;
    using Shared.Commands;

    public class ShipOrderHandler : IHandleMessages<ShipOrder>
    {
        static ILog log = LogManager.GetLogger<ShipOrderHandler>();

        public Task Handle(ShipOrder message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Let's ship");
            return Task.CompletedTask;
        }
    }
}