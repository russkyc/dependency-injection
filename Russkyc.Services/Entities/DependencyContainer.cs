using System;
using System.Collections.Generic;
using Russkyc.Services.Interfaces;

namespace Russkyc.Services.Entities
{
    public class DependencyContainer : IContainer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<IService> Services { get; set; }

        public DependencyContainer()
        {
            Id = Guid.NewGuid();
            Name = Id.ToString();
            Services = new List<IService>();
        }
        public DependencyContainer(string Name)
        {
            this.Name = Name;
            Id = Guid.NewGuid();
            Services = new List<IService>();
        }
    }
}