using Services.Core.DataOperation.InvalidationHandlers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.DataOperation.InvalidationHandlers
{


    public class InMemoryEventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new();

        public void Publish<T>(T @event)
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
            {
                foreach (var handler in _handlers[type])
                {
                    ((Action<T>)handler)(@event);
                }
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type))
            {
                _handlers[type] = new List<Delegate>();
            }

            _handlers[type].Add(handler);
        }
    }

}
