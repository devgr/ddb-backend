using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Ddb.Application.Abstractions;

namespace Ddb.Infrastructure.EventBus
{
    public class DummyEventBus : IEventBus
    {
        private readonly ILogger<DummyEventBus> _logger;

        public DummyEventBus(ILogger<DummyEventBus> logger)
        {
            _logger = logger;
        }

        public async Task PublishAsync<T>(T obj) where T : class
        {
            // This just wraps the syncronous version. In a real implementation,
            // this might asyncronously send the obj to a message bus on the network.
            await Task.Run(() => Publish<T>(obj));
        }

        public void Publish<T>(T obj) where T: class
        {
            // Dummy implementation. This writes the obj to the logger.
            var messageString = JsonSerializer.Serialize(obj);
            _logger.LogInformation($"Event: {messageString}");
        }
    }
}
