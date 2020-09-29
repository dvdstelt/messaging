namespace Shared.Configuration
{
    using System;
    using NServiceBus;
    using NServiceBus.Logging;

    public static class EndpointConfigurationExtensions
    {
        static readonly ILog Log = LogManager.GetLogger(typeof(EndpointConfigurationExtensions));

        public static EndpointConfiguration ApplyDefaultConfiguration(this EndpointConfiguration endpointConfiguration, Action<RoutingSettings<LearningTransport>> configureRouting = null)
        {
            Log.Info("Configuring endpoint...");

            endpointConfiguration.UsePersistence<LearningPersistence>();
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var routing = transport.Routing();

            endpointConfiguration.UseSerialization<XmlSerializer>();
            endpointConfiguration.Recoverability().Immediate(c => c.NumberOfRetries(2));
            endpointConfiguration.Recoverability().Delayed(c => c.NumberOfRetries(0));

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            configureRouting?.Invoke(routing);

            return endpointConfiguration;
        }

    }
}
