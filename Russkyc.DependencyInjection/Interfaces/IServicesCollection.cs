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
using System.Reflection;

namespace Russkyc.DependencyInjection.Interfaces
{
    public interface IServicesCollection
    {
        IServicesCollection AddSingleton<RegisteredAs>(string name = null);
        IServicesCollection AddSingleton(Type registeredAs, Type registeredTo, string name = null);
        IServicesCollection AddSingleton<RegisteredAs, RegisteredTo>(string name = null);
        IServicesCollection Add<T>(T instance, string name = null);
        IServicesCollection AddTransient(Type registeredAs, Type registeredTo, string name = null);
        IServicesCollection AddTransient<RegisteredAs>(string name = null, Action<RegisteredAs> serviceBuilder = null);
        IServicesCollection AddTransient<RegisteredAs, RegisteredTo>(string name = null, Action<RegisteredAs> serviceBuilder = null);
        IServicesContainer Build();
        IRegisteredService Get(Type registeredAs, string name = null);
    }
}