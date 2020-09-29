# Exercise 01: Sending messages between endpoints

In this exercise we will use NServiceBus to reliably send messages between endpoints. We will simulate a client user-interface that will send a message. Instead of processing everything at once, we will break up the processing and send a 2nd message to another endpoint.

**Note:** Not everything has been written out completely. Links to NServiceBus documentation will be provided where possible/necessary.

## Start-up projects

There are several console windows serving as a host for the NServiceBus endpoint. Next to that we'll have a shared project for messages. The console windows all have to be started when debugging.

- ClientUI
- Sales
- Shipping

## Exercise 1.1

In this exercise we'll set up the client user-interface to be able to send messages.

### Step 1

Add a NuGet package reference to the [latest version](https://www.nuget.org/packages/NServiceBus/) of NServiceBus.

**Note:** This can be added immediately to every project in the solution!

### Step 2

- Open the file`Program.cs` in the `ClientUI` project.
- Instantiate a new `EndpointConfiguration` object to provide [configuration](https://docs.particular.net/nservicebus/hosting/#self-hosting) for NServiceBus. Name your endpoint `ClientUI`.
- In the `Shared` project there is a class `EndpointConfigurationExtensions`. Call it on your `endpointConfiguration` object from the `ClientUI` project.
- In the `EndpointConfigurationExtensions` class, set the `LearningTransport` [as its used transport](https://docs.particular.net/transports/learning/).
- The `EndpointConfigurationExtensions` should return the `EndpointConfiguration` object again.
- The following step is not required. But if you want to be more explicit in your configuration, also try to understand and set the following settings:
  - For [recoverability](https://docs.particular.net/nservicebus/recoverability/), set the [error queue](https://docs.particular.net/nservicebus/recoverability/configure-error-handling).
  - For recoverability, set the [immediate retries](https://docs.particular.net/nservicebus/recoverability/configure-immediate-retries) and [delayed retries](https://docs.particular.net/nservicebus/recoverability/configure-delayed-retries).
  - Set [serialization](https://docs.particular.net/nservicebus/serialization/) to the [XML Serializer](https://docs.particular.net/nservicebus/serialization/xml). This is build into .NET and NServiceBus. For a [JSON serializer](https://docs.particular.net/nservicebus/serialization/newtonsoft), you need the `NServiceBus.Newtonsoft.Json` [NuGet package](https://www.nuget.org/packages/NServiceBus.Newtonsoft.Json/).
- In the `ClientUI` project, [start the endpoint](https://docs.particular.net/samples/endpoint-configuration/#start-the-endpoint) and create a variable to hold the started endpoint instance. This will be used to send messages.
  - Don't forget to `await` the creation of the endpoint!
- After the loop, make sure the NServiceBus endpoint instance is correctly stopped.

**Note:** At the bottom of this page is the full `EndpointConfigurationExtensions` class. You can copy&paste this. Have a look at what is all configured.

### Step 3

We will now create a message to send.

#### Ownership

So we need to process a customer its order. In our system, _Sales_ is initially responsible for processing it. This makes _Sales_ the owner of this message. The result is that only _Sales_ can process the message, but multiple components can send it though. For this reason, NServiceBus cannot be configured to send (command) messages to more than one endpoint.

- Open the `Shared` project and add a new class called `PlaceOrder` in the `Commands` folder.
- Make sure this class implements the `ICommand` interface, provided by NServiceBus
- Add a single property called `OrderId` of type `Guid`

### Step 4

In a next step, we'll create a message handler in the `Sales` project to process this message. For now we'll start sending the message from the client user-interface.

- Open the file `Program.cs` in the `ClientUI` project again.
- When the key `P` is pressed, the `PlaceOrder` message class should be initialized and the `OrderId` should be set as followed: `OrderId = Guid.NewGuid()`.
  **Note:** The code for pressing the key `P` is already provided.
- Write a message to console to show which Guid was created, so we can check on the receiving side later. If you're up for it, you can [introduce logging](https://docs.particular.net/nservicebus/logging/usage) into your endpoint. Otherwise you can simply use `Console.WriteLine()`
- Send the message using the endpoint instance.

### Step 5

Your client user-interface has no knowledge of where to send this message. Instead of providing the queue to the receiving endpoint, we provide a logical name of the endpoint that owns this message. In our case this is `Sales`. NServiceBus will then take care of technical routing.

The logical name is provided by naming your endpoint. We'll get back to this later.

- Add routing for the type `PlaceOrder` to the `Sales` endpoint.  
  You could add it to `endpointConfiguration` just before starting the endpoint. But the `ApplyDefaultConfiguration()` method also accepts a lambda to configure the routing.
  Documentation [can be found here](https://docs.particular.net/nservicebus/messaging/routing#command-routing).

## Exercise 1.2

In this exercise we'll make sure the `Sales` endpoint will process the message.

### Step 1

- Open `Program.cs` from the `Sales` project.
- Initialize a new `EndpointConfiguration` and name it `Sales`.
- The Learning Transport (and other default configuration type) has been configured in the `EndpointConfigurationExtensions`, let's use that again.
- Start the endpoint
- After a press on the 'enter' key, stop the endpoint instance.

Most of the code above looks exactly like the `ClientUI` configuration.

### Step 2

We'll create the code to process the message.

- Create a new class `PlaceOrderHandler`
- Inherit the `IHandleMessages<T>` interface and implement it for the `PlaceOrder` message.
- Log/Write to the console the received Guid for the `OrderId`.

## Exercise 1.3

In this exercise, we'll process the second step by shipping the product.

### Step 1

- Create a new message called `ShipOrder`.
- Add the `OrderId` property to it.

### Step 2

- Initialize and start the endpoint, but this time name it "Shipping".
- Add a new class called `ShipOrderHandler` for the `ShipOrder` message.
- Log/Write to the console the received Guid for the `OrderId`.

### Step 3

- Send the `ShipOrder` command from the `PlaceOrderHandler` so that `Shipping` will be able to process it.



## Conclusion

You have now learned how to send messages using NServiceBus. More importantly though you've learned how to reliably send a message and break apart the business process by splitting up two different parts of it into different endpoints.

## NServiceBus default configuration

```
public static class EndpointConfigurationExtensions
{
    static readonly ILog Log = LogManager.GetLogger(typeof(EndpointConfigurationExtensions));

    public static EndpointConfiguration ApplyDefaultConfiguration(this EndpointConfiguration endpointConfiguration, Action<RoutingSettings<LearningTransport>> configureRouting = null)
    {
        Log.Info("Configuring endpoint...");

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();

        endpointConfiguration.UsePersistence<LearningPersistence>();

        endpointConfiguration.UseSerialization<XmlSerializer>();
        endpointConfiguration.Recoverability().Immediate(c => c.NumberOfRetries(2));
        endpointConfiguration.Recoverability().Delayed(c => c.NumberOfRetries(0));

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        configureRouting?.Invoke(routing);

        return endpointConfiguration;
    }

}
```