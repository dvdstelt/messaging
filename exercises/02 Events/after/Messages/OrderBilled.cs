namespace Messages
{
    using System;
    using NServiceBus;

    public class OrderBilled : IEvent
    {
        public Guid OrderId { get; set; }
    }
}