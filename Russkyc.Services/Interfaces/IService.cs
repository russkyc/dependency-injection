using System;
using Russkyc.Services.Enums;

namespace Russkyc.Services.Interfaces
{
    public interface IService
    {
        Guid Id { get; set; }
        String Name { get; set; }
        Type RegisterAs { get; set; }
        Type Type { get; set; }
        object Service { get; set; }
        Scope Scope { get; set; }
    }
}