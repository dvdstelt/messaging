# Exercise 02: Events and recoverability

In this exercise we will see the benefits of events. How a very old publish/subscribe pattern can empower and decouple our applications. After that we'll see how recoverability can save our bacon.

**Note:** Not everything has been written out completely. Links to NServiceBus documentation will be provided where possible/necessary.

## Start-up projects

There are several console windows serving as a host for the NServiceBus endpoint. Next to that we'll have a shared project for messages. The console windows all have to be started when debugging.

- ClientUI
- Sales
- Shipping
- Finance

## Exercise 2.1

In this exercise we'll move away from sending just commands and switch to events. The `ShipOrder` class and corresponding handler have been removed.

### Step 1

Verify the `ClientUI` project and the loop. Nothing has changed here.

### Step 2

In the `Sales` endpoint we no longer send the `ShipOrder` command. Instead we are going to change this to publishing an event.

- Create a new message in the `Messages` project and call it `OrderPlaced`. 
- Instead of implementing `ICommand`, now implement `IEvent`. This tells NServiceBus to treat this differently. We'll get back later how.
- Add a property `OrderId` of type `Guid` again.

### Step 3

In the `Sales` endpoint, [publish](https://docs.particular.net/nservicebus/messaging/publish-subscribe/publish-handle-event#publishing-an-event) the `OrderPlaced` event.

## Exercise 2.2

We now successfully published an event. But there are definitely no subscribers yet. Let's create the subscribers. In the previous module we created the `ShipOrder` command and only `Shipping` received it.

We now want both `Shipping` and `Finance` to receive the order. They both depend on the event being published, but it doesn't really matter to them where it's coming from. NServiceBus will take care of the routing again. However `Sales` is responsible and the **owner** of the event. It should publish it, no matter if anyone is listening.

### Step 1

We already worked with `Shipping`. We'll create a handler there to decide whether or not we can execute the shipment.

- Create a handler in `Shipping` that accepts the `OrderPlaced` event.
- Use the static `log` variable to log information about the `OrderId`.

### Step 2

- Verify it the event is received.

### Explanation

What happened now is that `Shipping` got scanned by NServiceBus. It notices the handler that accepts the `OrderPlaced` event. It notifies the queuing technology about the fact that it wants to subscribe to this event. With the NServiceBus Learning Transport, this is done automatically and a subscription is stored on disk.

The Learning Transport mimics a [brokered transport](https://docs.particular.net/transports/types#broker-transports). Whenever the `OrderPlaced` event is published, NServiceBus sends this message to the queuing technology and it figures out who is subscribed. With the Learning Transport, this behavior is mimicked.

With a [federated transport](https://docs.particular.net/transports/types#unicast-only-transports) like MSMQ, this works a little bit differently and NServiceBus actually needs to keep track of subscriptions. These subscriptions need to be stored, using a persister like SQL Server.

## Exercise 2.3

We'll introduce the `Billing` component.

The idea is that shipping can't take place, until the order has been paid for by the customer. We won't implement a full payment solution. But while `Shipping` is made aware of the order being accepted, we'll also make `Billing` aware.  After the payment has succeeded, it should publish another event. Then `Shipping` can check if the order has been paid for and actually ship the order.

### Step 1

Create a handler in `Billing` to process the `OrderPlaced` event.

### Step 2

Create a new event in `Messages` called `OrderBilled`.

### Step 3

In the newly created handler in `Billing`, publish the `OrderBilled` event and copy the `OrderId` from the incoming `OrderPlaced` event.

### Step 4

In `Shipping` create an additional handler that processes the `OrderBilled` event and log the `OrderId`, so we can verify it in the console window.

### Step 5

Verify if both the `OrderPlaced` and `OrderBilled` event arrive in the `Shipping` endpoint.

## Exercise 2.4

In this exercise will see how recoverability works.

### Step 1

Open the `PlaceOrderHandler` class in the `Sales` endpoint. Throw an exception in it.

Verify recoverability by sending an order to it from the `ClientUI`. The message should be retried a couple of times and then go into delayed retries. After a while it should be sent off to the error queue. To properly be able to test, modify the recoverability options for [immediate](https://docs.particular.net/nservicebus/recoverability/configure-immediate-retries)- and [delayed](https://docs.particular.net/nservicebus/recoverability/configure-delayed-retries) retries.

This is a systemic exception. It cannot be solved without modifying the handler and fixing a potential bug.

### Step 2

- Remove the throwing of the exception.
- Add a static `Random` variable to the class, so we can test throwing a transient exception. It should look like this: `static Random random = new Random();`
- Throw an exception at random, using the random variable. Every time the result of the `random.Next(0,5)` function equals `0`, we throw an exception.

Now verify if the transient exception throws by sending a couple of orders from the `ClientUI`. Only sometimes the exception should occur, but it's probably fixed on the next retry.



## Conclusion

We've taken a look at the most important messaging patterns in action. But we've only seen a couple of very important features every messaging solution should have. There are many more that need to be implemented, if you build your own library. Like timeouts, versioning, manipulating message headers, outbox, routing, databus, etc. Not to mention test, document, etc. all this.

Focus on adding business value, instead of creating your own messaging library. Use a servicebus. Preferably NServiceBus. :-)