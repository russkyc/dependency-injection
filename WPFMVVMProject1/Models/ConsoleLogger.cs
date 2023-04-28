using System;
using System.Diagnostics;
using WPFMVVMProject1.Interfaces;

namespace WPFMVVMProject1.Models;

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