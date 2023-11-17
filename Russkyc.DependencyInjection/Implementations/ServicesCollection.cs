using System;
using System.Collections.Generic;
using System.Linq;
using Russkyc.DependencyInjection.Interfaces;

namespace Russkyc.DependencyInjection.Implementations
{
    public class ServicesCollection : IServicesCollection
    {
        private readonly object _syncLock = new object();
        private readonly IList<IRegisteredService> _registeredServices = new List<IRegisteredService>();

        // Method to add a service as a singleton
        public IServicesCollection AddSingleton<RegisteredAs>(string name = null)
        {
            return AddSingleton<RegisteredAs, RegisteredAs>(name);
        }

        public IServicesCollection AddSingleton(Type registeredAs, Type registeredTo, string name = null)
        {
            var singletonService = new SingletonService
            {
                Identifier = name,
                RegisterAs = registeredAs,
                RegisterTo = registeredTo
            };
            lock (_syncLock)
            {
                _registeredServices.Add(singletonService);
                return this;
            }
        }

        // Method to add a service as a singleton with option to specify a different registered type
        public IServicesCollection AddSingleton<RegisteredAs, RegisteredTo>(string name = null)
        {
            return AddSingleton(typeof(RegisteredAs), typeof(RegisteredTo), name);
        }

        public IServicesCollection Add<T>(T instance, string name = null)
        {
            var singletonService = new SingletonService
            {
                Identifier = name,
                RegisterAs = typeof(T),
                RegisterTo = typeof(T),
                Service = instance
            };
            lock (_syncLock)
            {
                _registeredServices.Add(singletonService);
                return this;
            }
        }
        
        public IServicesCollection AddTransient(Type registeredAs, Type registeredTo, string name = null)
        {
            var transientService = new TransientService
            {
                Identifier = name,
                RegisterAs = registeredAs,
                RegisterTo = registeredTo
            };
            lock (_syncLock)
            {
                _registeredServices.Add(transientService);
                return this;
            }
        }

        // Method to add a service as a transient
        public IServicesCollection AddTransient<RegisteredAs>(string name = null, Action<RegisteredAs> serviceBuilder = null)
        {
            return AddTransient<RegisteredAs, RegisteredAs>(name, serviceBuilder);
        }

        // Method to add a service as a transient with option to specify a different registered type
        public IServicesCollection AddTransient<RegisteredAs, RegisteredTo>(string name = null, Action<RegisteredAs> serviceBuilder = null)
        {
            var transientService = new TransientService
            {
                Identifier = name,
                RegisterAs = typeof(RegisteredAs),
                RegisterTo = typeof(RegisteredTo)
            };

            lock (_syncLock) 
            {
                _registeredServices.Add(transientService);
                return this;
            }
        }

        // Method to complete the build
        public IServicesContainer Build()
        {
            lock (_syncLock)
            {
                return new ServicesContainer(this);
            }
        }

        public IRegisteredService Get(Type registeredAs, string name = null)
        {
            lock (_syncLock)
            {
                if (name is null)
                {
                    return _registeredServices.First(service => service.RegisterAs == registeredAs);
                }

                return _registeredServices.First(service =>
                    service.RegisterAs == registeredAs && service.Identifier == name);
            }
        }
    }
}