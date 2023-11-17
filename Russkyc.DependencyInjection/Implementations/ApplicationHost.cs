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
using Russkyc.DependencyInjection.Helpers;
using Russkyc.DependencyInjection.Interfaces;

namespace Russkyc.DependencyInjection.Implementations
{
    public class ApplicationHost<T>
    {
        private readonly IServicesCollection _collection;

        public IServicesContainer Services => _collection.Build();
        public T Root => Services.Resolve<T>();

        private ApplicationHost()
        {
           _collection = new ServicesCollection();
        }
        
        public static ApplicationHost<T> CreateDefault()
        {
            var applicationHost = new ApplicationHost<T>();
            applicationHost._collection
                .AddSingleton<T>()
                .AddServices()
                .AddServicesFromReferenceAssemblies();
            return applicationHost;
        }
        
        public static ApplicationHost<T> CreateDefault(T instance)
        {
            var applicationHost = new ApplicationHost<T>();
            applicationHost._collection
                .Add(instance)
                .AddServices()
                .AddServicesFromReferenceAssemblies();
            return applicationHost;
        }

        public ApplicationHost<T> ConfigureServices(Action<IServicesCollection> transform)
        {
            transform.Invoke(_collection);
            return this;
        }
    }
}