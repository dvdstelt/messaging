using System;

namespace Shared.Events
{
    using NServiceBus;

    public class OrderBilled : IEvent
    {
        public Guid OrderId { get; set; }
    }
}
