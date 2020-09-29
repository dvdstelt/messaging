using System;

namespace Billing
{
    using System.Threading.Tasks;
    using NServiceBus;
    using Shared.Configuration;

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Billing";

            var endpointConfiguration = new EndpointConfiguration("Billing")
                .ApplyDefaultConfiguration();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
