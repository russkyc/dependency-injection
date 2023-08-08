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
using System.Linq;
using System.Reflection;
using Russkyc.DependencyInjection.Enums;
using Russkyc.DependencyInjection.Exceptions;
using Russkyc.DependencyInjection.Interfaces;

namespace Russkyc.DependencyInjection.Implementations
{
    public class ServicesContainer : IServicesContainer,
        IServicesCollection
    {
        private readonly object _lock = new object();
        private readonly ICollection<IService> _services = new List<IService>();

        public IServicesCollection AddSingleton<RegisteredAs>(string name = null, Action<RegisteredAs> builder = null)
        {
            return AddSingleton<RegisteredAs, RegisteredAs>(name, builder);
        }

        public IServicesCollection AddSingleton<RegisteredAs, RegisteredTo>(string name = null, Action<RegisteredAs> builder = null)
        {
            var constructors = typeof(RegisteredTo).GetConstructors();
            var parameters = constructors.Length > 0 ? constructors.FirstOrDefault(constructor => constructor.GetParameters().Length > 0)?.GetParameters() : Array.Empty<ParameterInfo>();
            var services = (parameters ?? Array.Empty<ParameterInfo>()).Select(param => Resolve(param.ParameterType)).ToArray();
            var registerService = constructors.Length > 0
                ? Activator.CreateInstance(typeof(RegisteredTo), services)
                : Activator.CreateInstance(typeof(RegisteredTo));
            if (builder != null)
            {
                builder((RegisteredAs)registerService);
            }
            lock (_lock)
            {
                _services.Add(new Service
                {
                    RegisterAs = typeof(RegisteredAs),
                    RegisterTo = typeof(RegisteredTo),
                    RegisterService = registerService,
                    Name = name,
                    Type = ServiceType.Singleton
                });
            }
            return this;
        }

        public IServicesCollection AddTransient<RegisteredAs>(string name = null, Action<RegisteredAs> builder = null)
        {
            return AddTransient<RegisteredAs, RegisteredAs>(name, builder);
        }

        public IServicesCollection AddTransient<RegisteredAs, RegisteredTo>(string name = null, Action<RegisteredAs> builder = null)
        {
            lock (_lock)
            {
                _services.Add(new Service
                {
                    Name = name,
                    RegisterAs = typeof(RegisteredAs),
                    RegisterTo = typeof(RegisteredTo),
                    Type = ServiceType.Transient,
                    Builder = builder
                });
            }
            return this;
        }

        public RegisterAs Resolve<RegisterAs>(string name = null)
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
        
        public object Resolve(Type registerAs, string name = null)
        {
            try
            {
                lock (_lock)
                {
                    var resolved = _services.First(service => service.RegisterAs == registerAs && service.Name == name);

                    if (resolved.Type == ServiceType.Singleton)
                    {
                        return resolved.RegisterService;
                    }
                    
                    return Activator.CreateInstance(resolved.RegisterTo,
                        resolved.RegisterTo
                            .GetConstructors()[0]
                            .GetParameters()
                            .Select(p => Resolve(p.ParameterType))
                            .ToArray());
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
            return this;
        }


    }
}