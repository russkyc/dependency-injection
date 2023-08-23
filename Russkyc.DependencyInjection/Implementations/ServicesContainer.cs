using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Russkyc.DependencyInjection.Attributes;
using Russkyc.DependencyInjection.Enums;
using Russkyc.DependencyInjection.Exceptions;
using Russkyc.DependencyInjection.Interfaces;

namespace Russkyc.DependencyInjection.Implementations
{
    public class ServicesContainer : IServicesContainer, IServicesCollection
    {
        private readonly object _syncLock = new object();
        private readonly ICollection<IRegisteredService> _registeredServices = new List<IRegisteredService>();

        public ServicesContainer()
        {
            lock (_syncLock)
            {
                _registeredServices.Add(new SingletonService
                {
                    RegisterAs = typeof(IServicesContainer),
                    RegisterTo = typeof(ServicesContainer),
                    Service = this
                });
            }
        }

        // Method to add a service as a singleton
        public IServicesCollection AddSingleton<RegisteredAs>(string name = null)
        {
            return AddSingleton<RegisteredAs, RegisteredAs>(name);
        }

        // Helper method to create an instance of a registered type
        private object ConstructRegisteredType(Type registeredToType)
        {
            // Attempt to get the first constructor. If no constructor is found, default to parameter-less instance creation.
            var constructors = registeredToType.GetConstructors();
            if (constructors.Length == 0)
            {
                return Activator.CreateInstance(registeredToType);
            }

            var neededServices = constructors[0].GetParameters()
                .Select(param => Resolve(param.ParameterType)).ToArray();
            return Activator.CreateInstance(registeredToType, neededServices);
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
            }

            return this;
        }

        // Method to add a service as a singleton with option to specify a different registered type
        public IServicesCollection AddSingleton<RegisteredAs, RegisteredTo>(string name = null)
        {
            return AddSingleton(typeof(RegisteredAs), typeof(RegisteredTo), name);
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
            }

            return this;
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
            }

            return this;
        }

        // Helper method to get an instance of a service
        private object ResolveServiceInstance(IRegisteredService service)
        {
            // If service is registered as Singleton, return the instance created at registration
            // If service is registered as Transient, create a new instance.

            if (service.GetType() == typeof(SingletonService))
            {
                if (service.Service is null)
                {
                    var instanceOfRegisteredTo = ConstructRegisteredType(service.RegisterTo);
                    service.Service = instanceOfRegisteredTo;
                }
                return service.Service;
            }
            return ConstructRegisteredType(service.RegisterTo);
        }

        // Method to resolve a service instance by generic type
        public RegisteredAs Resolve<RegisteredAs>(string name = null)
        {
            return (RegisteredAs) Resolve(typeof(RegisteredAs), name);
        }

        // Method to resolve a service instance by type
        public object Resolve(Type registeredAs, string name = null)
        {
            try
            {
                if (name is null)
                {
                    lock (_syncLock)
                    {
                        return ResolveServiceInstance(
                            _registeredServices.First(service => service.RegisterAs == registeredAs));
                    }
                }

                lock (_syncLock)
                {
                    return ResolveServiceInstance(
                        _registeredServices.First(service => service.RegisterAs == registeredAs && service.Identifier == name));
                }
            }
            catch (InvalidOperationException)
            {
                throw new MissingDependencyException($"No registered dependency for type {registeredAs}");
            }
        }
        
        public IServicesContainer AddServicesFromAssembly(Assembly assembly)
        {
            var assemblyTypes = assembly.DefinedTypes
                .Where(type => Attribute.IsDefined(type, typeof(ServiceAttribute)));
            foreach (var type in assemblyTypes)
                    {
                        var t = type;
                        var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>();
        
                        if (serviceAttribute == null)
                        {
                            continue;
                        }
                        
                        var registration = serviceAttribute.Registration;
                        
                        if (registration == Registration.AsSelf)
                        {
                            if (serviceAttribute.Scope == Scope.Singleton)
                            {
                                AddSingleton(type, type);
                                continue;
                            }
                            AddTransient(type, type);
                            continue;
                        }
        
                        if (registration == Registration.AsInterfaces)
                        {
                            foreach (var serviceType in type.ImplementedInterfaces)
                            {
                                if (serviceAttribute.Scope == Scope.Singleton)
                                {
                                    AddSingleton(serviceType, type);
                                    continue;
                                }
                                AddTransient(serviceType, type);
                            }
                            continue;
                        }
                        
                        if (serviceAttribute.Scope == Scope.Singleton)
                        {
                            AddSingleton(type, type);
                            continue;
                        }
                        AddTransient(type, type);
                        
                        foreach (var serviceType in type.ImplementedInterfaces)
                        {
                            if (serviceAttribute.Scope == Scope.Singleton)
                            {
                                AddSingleton(serviceType, type);
                                continue;
                            }
                            AddTransient(serviceType, type);
                        }
                    }
            return this;
        }

        // Method to complete the build
        public IServicesContainer Build()
        {
            return this;
        }
    }
}