﻿// MIT License
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
using Russkyc.DependencyInjection.Exceptions;
using Russkyc.DependencyInjection.Helpers;
using Russkyc.DependencyInjection.Interfaces;

namespace Russkyc.DependencyInjection.Implementations
{
    public class ServicesContainer : IServicesContainer
    {
        private static IServicesCollection _servicesCollection;

        public ServicesContainer(IServicesCollection servicesCollection)
        {
            _servicesCollection = servicesCollection;
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
                if (registeredAs == typeof(IServicesContainer))
                {
                    return this;
                }
                if (name is null)
                {
                    return this.ResolveServiceInstance(_servicesCollection.Get(registeredAs));
                }

                return this.ResolveServiceInstance(_servicesCollection.Get(registeredAs, name));

            }
            catch (InvalidOperationException)
            {
                throw new MissingDependencyException($"No registered dependency for type {registeredAs}");
            }
        }

    }
}