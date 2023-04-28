// MIT License
// 
// Copyright (c) 2023 Russell Camo (Russkyc)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Russkyc.DependencyInjection.Enums;
using Russkyc.DependencyInjection.Exceptions;
using Russkyc.DependencyInjection.Interfaces;

namespace Russkyc.DependencyInjection.Implementations
{
    public class ServicesCollection : IServicesCollection
    {
        private readonly object _lock;
        private readonly ICollection<IService> _services;

        public ServicesCollection()
        {
            _lock = new object();
            _services = new List<IService>();
        }

        public IServicesCollection AddSingleton<RegisterAs>()
        {
            return AddSingleton<RegisterAs, RegisterAs>();
        }

        public IServicesCollection AddSingleton<RegisterAs>(string name)
        {
            return AddSingleton<RegisterAs, RegisterAs>(name);
        }

        public IServicesCollection AddSingleton<RegisterAs, RegisteredTo>()
        {
            var constructors = typeof(RegisteredTo).GetConstructors();
            var parameters = constructors.Length > 0 ? constructors[0].GetParameters() : Array.Empty<ParameterInfo>();
            var services = parameters.Select(param => Resolve(param.ParameterType)).ToArray();
            var registerService = constructors.Length > 0
                ? Activator.CreateInstance(typeof(RegisteredTo), services)
                : Activator.CreateInstance(typeof(RegisteredTo));
            lock (_lock)
            {
                _services.Add(new Service
                {
                    RegisterAs = typeof(RegisterAs),
                    RegisterTo = typeof(RegisteredTo),
                    RegisterService = registerService,
                    Type = ServiceType.Singleton
                });
            }
            return this;
        }

        public IServicesCollection AddSingleton<RegisterAs, RegisteredTo>(string name)
        {
            var constructors = typeof(RegisteredTo).GetConstructors();
            var parameters = constructors.Length > 0 ? constructors[0].GetParameters() : Array.Empty<ParameterInfo>();
            var services = parameters.Select(param => Resolve(param.ParameterType)).ToArray();
            var registerService = constructors.Length > 0
                ? Activator.CreateInstance(typeof(RegisteredTo), services)
                : Activator.CreateInstance(typeof(RegisteredTo));
            lock (_lock)
            {
                _services.Add(new Service
                {
                    RegisterAs = typeof(RegisterAs),
                    RegisterTo = typeof(RegisteredTo),
                    RegisterService = registerService,
                    Type = ServiceType.Singleton
                });
            }
            return this;
        }

        public IServicesCollection AddTransient<RegisterAs>()
        {
            return AddTransient<RegisterAs,RegisterAs>();
        }

        public IServicesCollection AddTransient<RegisterAs>(string name)
        {
            return AddTransient<RegisterAs,RegisterAs>(name);
        }

        public IServicesCollection AddTransient<RegisterAs, RegisteredTo>()
        {
            lock (_lock)
            {
                _services.Add(new Service
                {
                    RegisterAs = typeof(RegisterAs),
                    RegisterTo = typeof(RegisteredTo),
                    Type = ServiceType.Transient
                });
            }
            return this;
        }

        public IServicesCollection AddTransient<RegisterAs, RegisteredTo>(string name)
        {
            lock (_lock)
            {
                _services.Add(new Service
                {
                    Name = name,
                    RegisterAs = typeof(RegisterAs),
                    RegisterTo = typeof(RegisteredTo),
                    Type = ServiceType.Transient
                });
            }
            return this;
        }

        public IServicesCollection AddScoped<RegisterAs>()
        {
            return AddScoped<RegisterAs,RegisterAs>();
        }

        public IServicesCollection AddScoped<RegisterAs>(string name)
        {
            return AddScoped<RegisterAs,RegisterAs>(name);
        }

        public IServicesCollection AddScoped<RegisterAs, RegisteredTo>()
        {
            lock (_lock)
            {
                _services.Add(new Service
                {
                    RegisterAs = typeof(RegisterAs),
                    RegisterTo = typeof(RegisteredTo),
                    Type = ServiceType.Scoped
                });
            }
            return this;
        }

        public IServicesCollection AddScoped<RegisterAs, RegisteredTo>(string name)
        {
            lock (_lock)
            {
                _services.Add(new Service
                {
                    Name = name,
                    RegisterAs = typeof(RegisterAs),
                    RegisterTo = typeof(RegisteredTo),
                    Type = ServiceType.Scoped
                });
            }
            return this;
        }

        public RegisterAs Resolve<RegisterAs>()
        {
            try
            {
                lock (_lock)
                {
                    var resolved = _services.First(service => service.RegisterAs == typeof(RegisterAs));
                    switch (resolved.Type) {
                        case ServiceType.Singleton:
                            return (RegisterAs)resolved.RegisterService;
                        case ServiceType.Transient:
                            return (RegisterAs)Activator.CreateInstance(resolved.RegisterTo, resolved.RegisterTo.GetConstructors()[0].GetParameters().Select(p => Resolve(p.ParameterType)).ToArray());
                        case ServiceType.Scoped:
                            var scope = new StackFrame(3).GetMethod().DeclaringType?.Name;
                            var scoped = _services.FirstOrDefault(s => s.RegisterAs == typeof(RegisterAs) && scope == s.RegisterScope);
                            if (scoped == null) {
                                var service = new Service(resolved) { RegisterScope = scope, RegisterService = Activator.CreateInstance(resolved.RegisterTo) };
                                _services.Add(service);
                                scoped = service;
                            }
                            return (RegisterAs)scoped.RegisterService;
                        default:
                            return (RegisterAs)Activator.CreateInstance(resolved.RegisterTo);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw new MissingDependencyException($"No registered dependency for type {typeof(RegisterAs)}");
            }
        }
        public object Resolve(Type registerAs)
        {
            try
            {
                lock (_lock)
                {
                    var resolved = _services.First(service => service.RegisterAs == registerAs);
                    switch (resolved.Type) {
                        case ServiceType.Singleton:
                            return resolved.RegisterService;
                        case ServiceType.Transient:
                            return Activator.CreateInstance(resolved.RegisterTo, resolved.RegisterTo.GetConstructors()[0].GetParameters().Select(p => Resolve(p.ParameterType)).ToArray());
                        case ServiceType.Scoped:
                            var scope = new StackFrame(3).GetMethod().DeclaringType?.Name;
                            var scoped = _services.FirstOrDefault(s => s.RegisterAs == registerAs && scope == s.RegisterScope);
                            if (scoped == null) {
                                var service = new Service(resolved) { RegisterScope = scope, RegisterService = Activator.CreateInstance(resolved.RegisterTo) };
                                _services.Add(service);
                                scoped = service;
                            }
                            return scoped.RegisterService;
                        default:
                            return Activator.CreateInstance(resolved.RegisterTo);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw new MissingDependencyException($"No registered dependency for type {registerAs}");
            }
        }

        public RegisterAs Resolve<RegisterAs>(string name)
        {
            try
            {
                lock (_lock)
                {
                    var resolved = _services.First(service => service.RegisterAs == typeof(RegisterAs) && service.Name == name);
                    switch (resolved.Type) {
                        case ServiceType.Singleton:
                            return (RegisterAs)resolved.RegisterService;
                        case ServiceType.Transient:
                            return (RegisterAs)Activator.CreateInstance(resolved.RegisterTo, resolved.RegisterTo.GetConstructors()[0].GetParameters().Select(p => Resolve(p.ParameterType)).ToArray());
                        case ServiceType.Scoped:
                            var scope = new StackFrame(3).GetMethod().DeclaringType?.Name;
                            var scoped = _services.FirstOrDefault(s => s.RegisterAs == typeof(RegisterAs) && scope == s.RegisterScope);
                            if (scoped == null) {
                                var service = new Service(resolved) { RegisterScope = scope, RegisterService = Activator.CreateInstance(resolved.RegisterTo) };
                                _services.Add(service);
                                scoped = service;
                            }
                            return (RegisterAs)scoped.RegisterService;
                        default:
                            return (RegisterAs)Activator.CreateInstance(resolved.RegisterTo);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw new MissingDependencyException($"No registered dependency for type {typeof(RegisterAs)}");
            }
        }
        
        public object Resolve(Type registerAs, string name)
        {
            try
            {
                lock (_lock)
                {
                    var resolved = _services.First(service => service.RegisterAs == registerAs && service.Name == name);
                    switch (resolved.Type) {
                        case ServiceType.Singleton:
                            return resolved.RegisterService;
                        case ServiceType.Transient:
                            return Activator.CreateInstance(resolved.RegisterTo, resolved.RegisterTo.GetConstructors()[0].GetParameters().Select(p => Resolve(p.ParameterType)).ToArray());
                        case ServiceType.Scoped:
                            var scope = new StackFrame(3).GetMethod().DeclaringType?.Name;
                            var scoped = _services.FirstOrDefault(s => s.RegisterAs == registerAs && scope == s.RegisterScope);
                            if (scoped == null) {
                                var service = new Service(resolved) { RegisterScope = scope, RegisterService = Activator.CreateInstance(resolved.RegisterTo) };
                                _services.Add(service);
                                scoped = service;
                            }
                            return scoped.RegisterService;
                        default:
                            return Activator.CreateInstance(resolved.RegisterTo);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw new MissingDependencyException($"No registered dependency for type {registerAs}");
            }
        }

        public ICollection<IService> GetServices()
        {
            lock (_lock)
            {
                return _services;
            }
        }

        public IServicesContainer Build()
        {
            return new ServicesContainer(this)
                .Build();
        }
    }
}