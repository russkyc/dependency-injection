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
using Russkyc.DependencyInjection.Enums;
using Russkyc.DependencyInjection.Interfaces;

namespace Russkyc.DependencyInjection.Implementations
{
    public class Service : IService
    {
        public string Name { get; set; }
        public Type RegisterAs { get; set; }
        public Type RegisterTo { get; set; }
        public string RegisterScope { get; set; }
        public ServiceType Type { get; set; }
        public object RegisterService { get; set; }
        public object RegisterContext { get; set; }
        public object Builder { get; set; }

        public Service()
        {
            
        }

        public Service(IService service)
        {
            Name = service.Name;
            Type = service.Type;
            RegisterAs = service.RegisterAs;
            RegisterTo = service.RegisterTo;
            RegisterScope = service.RegisterScope;
            RegisterService = service.RegisterService;
            RegisterContext = service.RegisterContext;
            Builder = service.Builder;
        }
    }
}