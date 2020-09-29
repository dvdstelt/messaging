namespace Sales
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using Shared.Commands;
    using Shared.Configuration;

    class Program
    {
        static async Task Main()
        {
            Console.Title = "Sales";

            var endpointConfiguration = new EndpointConfiguration("Sales")
                .ApplyDefaultConfiguration(s => s.RouteToEndpoint(typeof(ShipOrder), "Shipping"));

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}