using System;
using System.Diagnostics;
using DependencyInjectionDemo.Services.Interfaces;
using Russkyc.DependencyInjection.Attributes;
using Russkyc.DependencyInjection.Enums;

namespace DependencyInjectionDemo.Services.Services;

// Defined scope and Registration Type
[Service(Scope.Singleton, Registration.AsInterfaces)]
public class ConsoleLogger : ILogger
{
    private string? _name;

    public void SetName(string name)
    {
        _name = name;
    }

    public void Log(string message)
    {
        Console.WriteLine(_name + message);
        Debug.WriteLine(_name + message);
    }
}