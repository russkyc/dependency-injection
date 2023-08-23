using System;
using System.Diagnostics;
using DependencyInjectionDemo.Interfaces;
using Russkyc.DependencyInjection.Attributes;
using Russkyc.DependencyInjection.Enums;

namespace DependencyInjectionDemo.Models;

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