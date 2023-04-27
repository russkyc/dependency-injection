using System;
using System.Collections.Generic;

namespace Russkyc.Services.Interfaces
{
    public interface IContainer
    {
        Guid Id { get; set; }
        string Name { get; set; }
        List<IService> Services { get; set; }
        
    }
}