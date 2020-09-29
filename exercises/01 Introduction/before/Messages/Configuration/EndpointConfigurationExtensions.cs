namespace Shared.Configuration
{
    using System;
    using NServiceBus.Logging;

    public static class EndpointConfigurationExtensions
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(EndpointConfigurationExtensions));

        public static EndpointConfiguration ApplyDefaultConfiguration(this EndpointConfiguration endpointConfiguration, Action<RoutingSettings<LearningTransport>> configureRouting = null)
        {
            Log.Info("Configuring endpoint...");
        }

    }
}
