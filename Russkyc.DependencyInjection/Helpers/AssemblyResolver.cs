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
using System.Linq;
using System.Reflection;
using Russkyc.DependencyInjection.Attributes;
using Russkyc.DependencyInjection.Enums;
using Russkyc.DependencyInjection.Interfaces;

namespace Russkyc.DependencyInjection.Helpers
{
    public static class AssemblyResolver
    {

        public static IServicesCollection AddServices(this IServicesCollection collection)
        {
            var assembly = Assembly.GetEntryAssembly();
            return collection.AddServicesFromAssembly(assembly);
        }
        
        public static IServicesCollection AddServicesFromAssembly(this IServicesCollection collection, Assembly assembly)
        {
            var assemblyTypes = assembly.DefinedTypes
                .Where(type => Attribute.IsDefined(type, typeof(ServiceAttribute)))
                .ToList();

            foreach (var type in assemblyTypes)
            {
                var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>();
                if (serviceAttribute == null)
                {
                    continue;
                }

                if (serviceAttribute.Registration == Registration.AsSelf || serviceAttribute.Registration == Registration.AsInterfaces)
                {
                    if (serviceAttribute.Scope == Scope.Singleton)
                    {
                        collection.AddSingleton(type, type);
                    }
                    else 
                    {
                        collection.AddTransient(type, type);
                    }
                }

                if (serviceAttribute.Registration == Registration.AsInterfaces || serviceAttribute.Registration != Registration.AsSelf)
                {
                    foreach (var serviceType in type.ImplementedInterfaces)
                    {
                        if (serviceAttribute.Scope == Scope.Singleton)
                        {
                            collection.AddSingleton(serviceType, type);
                        }
                        else 
                        {
                            collection.AddTransient(serviceType, type);
                        }
                    }
                }
            }
            return collection;
        }
        
        public static IServicesCollection AddServicesFromReferenceAssemblies(this IServicesCollection collection)
        {
            var assembly = Assembly.GetEntryAssembly();
            return collection.AddServicesFromReferenceAssemblies(assembly);
        }
        
        public static IServicesCollection AddServicesFromReferenceAssemblies(this IServicesCollection collection, Assembly assembly)
        {
            foreach (var referenceAssembly in assembly.GetReferencedAssemblies())
            {
                collection.AddServicesFromAssembly(Assembly.Load(referenceAssembly));
            }
            return collection;
        }

    }
}