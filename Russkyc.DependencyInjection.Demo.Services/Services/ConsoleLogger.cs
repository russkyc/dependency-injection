using System;
using System.Diagnostics;
using Russkyc.DependencyInjection.Attributes;
using Russkyc.DependencyInjection.Demo.Services.Interfaces;
using Russkyc.DependencyInjection.Enums;

namespace Russkyc.DependencyInjection.Demo.Services.Services;

// Defined scope and Registration Type
[Service(Scope.Singleton, Registration.AsInterfaces)]
public class ConsoleLogger : ILogger
{
    private string? _name;

    public void setName(string name)
    {
        _name = name;
    }

    public void Log(string message)
    {
        Console.WriteLine(_name + message);
        Debug.WriteLine(_name + message);
    }
}