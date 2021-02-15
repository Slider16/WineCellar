using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WineCellar.API.Helpers
{
    public class MongoEventsLogger : IEventSubscriber
    {
        // Trying MongoEvents Logging -- See "Using the MongoDB C# v2 Driver" 
        // by Wes Higbee on Pluralsight - Module 3 - Diagnosing Database Interactions 

        private readonly ILogger<CommandStartedEvent> _CommandStartedLogger;

        private ReflectionEventSubscriber _Subscriber;

        public MongoEventsLogger(ILogger<CommandStartedEvent> commandStartedLogger)
        {
            _CommandStartedLogger = commandStartedLogger;
            _Subscriber = new ReflectionEventSubscriber(this);
        }

        public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
        {
            return _Subscriber.TryGetEventHandler(out handler);
        }

        public void Handle(CommandStartedEvent started)
        {
            var itemsToLog = new
            {
                started.Command,
                started.CommandName,
                started.ConnectionId,
                started.DatabaseNamespace,
                started.OperationId,
                started.RequestId
            };

            

            _CommandStartedLogger.LogInformation($"Start up information: {itemsToLog}");

        }

        public void Handle(CommandSucceededEvent succeeded)
        {

        }

        public void Handle(CommandFailedEvent failed)
        {

        }
    }


}
