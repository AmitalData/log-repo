using System;
using System.Collections.Generic;
using Telerik.JustMock;

namespace Logitude.Test.Utilities
{
    /// <summary>
    /// Lightweight test doubles registry to simulate Unity registrations without touching production containers.
    /// </summary>
    public sealed class JustMockRegistry
    {
        private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        public T RegisterInstance<T>(T instance) where T : class
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            _instances[typeof(T)] = instance;
            return instance;
        }

        public T RegisterMock<T>() where T : class
        {
            var mock = Mock.Create<T>();
            RegisterInstance(mock);
            return mock;
        }

        public T Resolve<T>() where T : class
        {
            if (_instances.TryGetValue(typeof(T), out var instance))
            {
                return (T)instance;
            }

            throw new InvalidOperationException($"Type {typeof(T).FullName} was not registered in JustMockRegistry.");
        }

        public bool TryResolve<T>(out T instance) where T : class
        {
            if (_instances.TryGetValue(typeof(T), out var stored))
            {
                instance = (T)stored;
                return true;
            }

            instance = null;
            return false;
        }

        public void Clear()
        {
            _instances.Clear();
        }
    }
}

