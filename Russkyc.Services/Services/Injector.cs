using System;
using System.Linq;
using Russkyc.Services.Entities;
using Russkyc.Services.Enums;
using Russkyc.Services.Interfaces;

namespace Russkyc.Services.Services
{
    public class Injector
    {
        private static object _lock;
        private static IContainer _container;
        private static Injector _instance;

        public Injector()
        {
            _lock = new object();
        }

        public void Use(IContainer container)
        {
            lock (_lock)
            {
                _container = container;
            }
        }

        public static Injector GetInstance()
        {
            if (_instance is null)
            {
                _instance = new Injector();
            }
                
            return _instance;
        }

        public void Register<TType, TObject>() where TObject : new()
        {
            lock (_lock)
            {
                _container.Services
                    .Add(
                    new Dependency
                    {
                        Id = Guid.NewGuid(),
                        Name = nameof(TObject),
                        RegisterAs = typeof(TType),
                        Type = typeof(TObject),
                        Service = new TObject(),
                        Scope = Scope.Multiple
                    });
            }
        }
        
        public void Register<TType, TObject>(Scope scope) where TObject : new()
        {
            lock (_lock)
            {
                _container.Services
                    .Add(
                        new Dependency
                        {
                            Id = Guid.NewGuid(),
                            Name = nameof(TObject),
                            RegisterAs = typeof(TType),
                            Type = typeof(TObject),
                            Service = new TObject(),
                            Scope = scope
                        });
            }
        }

        public void Register<TType, TObject>(string name) where TObject : new()
        {
            lock (_lock)
            {
                _container.Services
                    .Add(
                        new Dependency
                        {
                            Id = Guid.NewGuid(),
                            Name = name,
                            RegisterAs = typeof(TType),
                            Type = typeof(TObject),
                            Service = new TObject(),
                            Scope = Scope.Multiple
                        });
            }
        }
        
        public void Register<TType, TObject>(string name, Scope scope) where TObject : new()
        {
            lock (_lock)
            {
                _container.Services
                    .Add(
                        new Dependency
                        {
                            Id = Guid.NewGuid(),
                            Name = name,
                            RegisterAs = typeof(TType),
                            Type = typeof(TObject),
                            Service = new TObject(),
                            Scope = scope
                        });
            }
        }

        public TType Resolve<TType>()
        {
            lock (_lock)
            {
                var resolved =  _container.Services
                    .First(service => service.RegisterAs == typeof(TType));
                if (resolved.Scope == Scope.Multiple)
                {
                    return (TType)Activator.CreateInstance(resolved.Type);
                }
                else
                {
                    return (TType)resolved.Service;
                }
            }
        }
        
        public TType Resolve<TType>(string name) where TType : class
        {
            lock (_lock)
            {
                var resolved =  _container.Services
                    .First(service => service.Name.Equals(name));
                if (resolved.Scope == Scope.Multiple)
                {
                    return (TType)Activator.CreateInstance(resolved.Type);
                }
                else
                {
                    return (TType)resolved.Service;
                }
            }
        }

        public IContainer GetContainer()
        {
            lock (_lock)
            {
                return _container;
            }
        }

    }
}