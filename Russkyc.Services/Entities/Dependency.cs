using System;
using Russkyc.Services.Enums;
using Russkyc.Services.Interfaces;

namespace Russkyc.Services.Entities
{
    public class Dependency : IService
    {
        private Type _type;
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Type RegisterAs { get; set; }
        public Type Type { get; set; }
        public object Service { get; set; }
        public Scope Scope { get; set; }
    }
}