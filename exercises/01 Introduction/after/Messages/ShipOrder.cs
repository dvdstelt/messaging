
namespace Messages
{
    using System;
    using NServiceBus;

    public class ShipOrder : ICommand
    {
        public Guid OrderId { get; set; }
    }
}