using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.DataOperation.InvalidationHandlers.Interfaces
{
    public interface IEventBus
    {
        void Publish<T>(T @event);
        void Subscribe<T>(Action<T> handler);
    }
}
