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
using Russkyc.DependencyInjection.Interfaces;

namespace Russkyc.DependencyInjection.Implementations
{
    public class ServicesContainer : IServicesContainer
    {
        private readonly IServicesCollection _serviceses;

        public ServicesContainer(IServicesCollection serviceses)
        {
            _serviceses = serviceses;
        }
        
        public IServicesCollection AddSingleton<RegisteredAs, RegisteredTo>()
            where RegisteredTo : new()
        {
            _serviceses.AddSingleton<RegisteredAs,RegisteredTo>();
            return _serviceses;
        }

        public IServicesCollection AddTransient<RegisteredAs, RegisteredTo>()
            where RegisteredTo : new()
        {
            _serviceses.AddTransient<RegisteredAs, RegisteredTo>();
            return _serviceses;
        }

        public IServicesCollection AddScoped<RegisteredAs, RegisteredTo>()
            where RegisteredTo : new()
        {
            _serviceses.AddScoped<RegisteredAs, RegisteredTo>();
            return _serviceses;
        }

        public RegisteredAs Resolve<RegisteredAs>()
        {
            return _serviceses.Resolve<RegisteredAs>();
        }

        public object Resolve(Type registeredAs)
        {
            return _serviceses.Resolve(registeredAs);
        }

        public RegisteredAs Resolve<RegisteredAs>(string name)
        {
            return _serviceses.Resolve<RegisteredAs>(name);
        }

        public object Resolve(Type registeredAs, string name)
        {
            return _serviceses.Resolve(registeredAs, name);
        }

        public IServicesContainer Build()
        {
            return this;
        }
    }
}