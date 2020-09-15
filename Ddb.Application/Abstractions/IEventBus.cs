using System;
using System.Threading.Tasks;

namespace Ddb.Application.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T obj) where T : class;
        void Publish<T>(T obj) where T : class;
    }
}
