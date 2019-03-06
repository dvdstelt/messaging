using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Shipping
{
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